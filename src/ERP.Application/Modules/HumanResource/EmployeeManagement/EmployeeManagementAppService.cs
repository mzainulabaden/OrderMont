using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Authorization.Users;
using ERP.Modules.HumanResource.AttendanceManagement;
using ERP.Modules.HumanResource.CommisionPolicy;
using ERP.Modules.HumanResource.EmployeeManagement.Dtos;
using ERP.Modules.HumanResource.LookUps;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.HumanResource.EmployeeManagement
{
    [AbpAuthorize(PermissionNames.LookUps_Employee)]
    public class EmployeeManagementAppService : ApplicationService
    {
        public IRepository<AttendanceInfo, long> Attendance_Repo { get; set; }
        public IRepository<EmployeeInfo, long> Employee_Repo { get; set; }
        public IRepository<DesignationInfo, long> Designation_Repo { get; set; }
        public IRepository<User, long> User_Repository { get; set; }
        public IRepository<CommissionPolicyInfo, long> CommissionPolicy_Repo { get; set; }

        public async Task<PagedResultDto<EmployeeGetAllDto>> GetAll(EmployeeFiltersDto filters)
        {
            var employee_query = Employee_Repo.GetAll(this).ApplyBaseFilters(filters);
            if (!string.IsNullOrWhiteSpace(filters.ErpId))
                employee_query = employee_query.Where(i => i.ErpId == filters.ErpId);
            if (filters.IsActive.HasValue)
                employee_query = employee_query.Where(i => i.IsActive == filters.IsActive.Value);
            if (!string.IsNullOrWhiteSpace(filters.DesignationId))
                employee_query = employee_query.Where(i => i.DesignationId == filters.DesignationId.TryToLong());
            var employees = await employee_query.ToPagedListAsync(filters);

            var designation_ids = employees.Select(i => i.DesignationId).ToList();
            var commission_policy_ids = employees.Where(e => e.CommissionPolicyId > 0).Select(i => i.CommissionPolicyId).ToList();

            var total_count = employee_query.DeferredCount().FutureValue();
            var designations = Designation_Repo.GetAll(this, i => designation_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            var commissionPolicies = CommissionPolicy_Repo.GetAll(this, i => commission_policy_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            
            _ = await designations.ToListAsync();
            _ = await commissionPolicies.ToListAsync();

            var dict_designations = designations.ToDictionary(i => i.Id);
            var dict_commissionPolicies = commissionPolicies.ToDictionary(i => i.Id);

            var output = new List<EmployeeGetAllDto>();
            foreach (var employee in employees)
            {
                dict_designations.TryGetValue(employee.DesignationId, out var designation);
                dict_commissionPolicies.TryGetValue(employee.CommissionPolicyId, out var commissionPolicy);

                var dto = ObjectMapper.Map<EmployeeGetAllDto>(employee);
                dto.JobDuration = employee.JoiningDate.GetValueOrDefault().CalculateJobDuration();
                dto.Age = (DateTime.Now.Year - employee.DateOfBirth.GetValueOrDefault().Year);
                dto.DesignationName = designation?.Name ?? "";
                dto.CommissionPolicy = commissionPolicy?.Name ?? "";
                
                output.Add(dto);
            }

            return new PagedResultDto<EmployeeGetAllDto>(total_count.Value, output);
        }

        [AbpAuthorize(PermissionNames.LookUps_Employee_Create)]
        public async Task<string> Create(EmployeeDto input)
        {
            var entity = ObjectMapper.Map<EmployeeInfo>(input);
            entity.ErpId = await GetErpId();
            entity.TenantId = AbpSession.TenantId;
            await Employee_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "Employee Created Successfully.";
        }

        private async Task<EmployeeInfo> GetById(long Id)
        {
            var employee = await Employee_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (employee != null)
                return employee;
            else
                throw new UserFriendlyException($"EmployeeId: '{Id}' is invalid.");
        }

        public async Task<EmployeeGetAllDto> Get(long Id)
        {
            var employee = await GetById(Id);
            var designation = Designation_Repo.GetAll(this, i => i.Id == employee.DesignationId).DeferredFirstOrDefault().FutureValue();
            var commissionPolicy = CommissionPolicy_Repo.GetAll(this, i => i.Id == employee.CommissionPolicyId).DeferredFirstOrDefault().FutureValue();
            
            _ = await designation.ValueAsync();
            _ = await commissionPolicy.ValueAsync();

            var output = ObjectMapper.Map<EmployeeGetAllDto>(employee);
            output.JobDuration = employee.JoiningDate.GetValueOrDefault().CalculateJobDuration();
            output.Age = (DateTime.Now.Year - employee.DateOfBirth.GetValueOrDefault().Year);
            output.DesignationName = designation?.Value?.Name ?? "";
            output.CommissionPolicy = commissionPolicy?.Value?.Name ?? "";
            
            return output;
        }

        [AbpAuthorize(PermissionNames.LookUps_Employee_Update)]
        public async Task<string> Update(EmployeeDto input)
        {
            var old_employee = await GetById(input.Id);
            var entity = ObjectMapper.Map(input, old_employee);
            await Employee_Repo.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "Employee Updated Successfully.";
        }

        [AbpAuthorize(PermissionNames.LookUps_Employee_Delete)]
        public async Task<string> Delete(long Id)
        {
            var employee = await GetById(Id);

            var has_attendance = await Attendance_Repo.GetAll(this, i => i.EmployeeId == Id).AnyAsync();
            if (has_attendance)
                throw new UserFriendlyException("Employee cannot be deleted as they have attendance recorded.");

            await Employee_Repo.DeleteAsync(employee);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "Employee Deleted Successfully.";
        }

        public async Task<string> GetErpId()
        {
            string latest_erp = (await Employee_Repo.GetAll(this).OrderByDescending(i => i.ErpId).FirstOrDefaultAsync())?.ErpId ?? "1001000";
            var new_erp = ulong.Parse(latest_erp) + 1;
            return new_erp.ToString();
        }
        public async Task<List<ViewUsersGetAllDto>> ViewUsersDetails()
        {
            var users = await User_Repository.GetAll()
                .Select(u => new ViewUsersGetAllDto
                {
                    UserName = u.UserName,
                    EmailAddress = u.EmailAddress,
                    Name = u.Name,
                    SurName = u.Surname,
                    CommissionPolicyId = u.CommissionPolicyId,
                })
                .ToListAsync();

            return users;
        }
    }
}
