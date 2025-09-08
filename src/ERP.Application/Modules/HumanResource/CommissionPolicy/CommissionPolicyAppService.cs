using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization.Users;
using ERP.Enums;
using ERP.Modules.HumanResource.CommisionPolicy;
using ERP.Modules.HumanResource.EmployeeManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Modules.HumanResource.CommissionPolicy
{
    public class CommissionPolicyAppService : ApplicationService
    {
        public IRepository<CommissionPolicyInfo, long> CommisionPolicy_Repo { get; set; }
        public IRepository<User, long> User_Repo { get; set; }
        public IRepository<EmployeeInfo, long> Employee_Repo { get; set; }

        public async Task<PagedResultDto<CommissionPolicyDto>> GetAll(PolicyType? policyType, decimal? commissionAmount)
        {
            var query = CommisionPolicy_Repo.GetAllIncluding(c => c.CommissionPolicyDetails);

            if (AbpSession.TenantId.HasValue)
                query = query.Where(i => i.TenantId == AbpSession.TenantId);

            if (policyType != null)
                query = query.Where(i => i.PolicyType == policyType);

            if (commissionAmount != null)
                query = query.Where(i => i.CommisionAmount == commissionAmount);

            var totalCount = await query.CountAsync();
            var data = await query.ToListAsync();
            var dtoList = ObjectMapper.Map<List<CommissionPolicyDto>>(data);

            return new PagedResultDto<CommissionPolicyDto>(totalCount, dtoList);
        }

        public async Task<CommissionPolicyDto> GetById(long id)
        {
            var entity = await CommisionPolicy_Repo.GetAllIncluding(i => i.CommissionPolicyDetails).FirstOrDefaultAsync(i => i.Id == id);
            if (entity == null)
                throw new UserFriendlyException("Commission Policy not found");

            return ObjectMapper.Map<CommissionPolicyDto>(entity);
        }

        public async Task<string> Create(CommissionPolicyDto input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input), "Input cannot be null");

            var entity = ObjectMapper.Map<CommissionPolicyInfo>(input);
            entity.TenantId = AbpSession.TenantId;
            await CommisionPolicy_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "Commission Policy created successfully.";
        }

        public async Task<string> Update(CommissionPolicyDto input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input), "Input cannot be null");
            var entity = await CommisionPolicy_Repo.GetAsync(input.Id);
            if (entity == null)
                throw new UserFriendlyException("Commission Policy not found");

            ObjectMapper.Map(input, entity);
            await CommisionPolicy_Repo.EnsureCollectionLoadedAsync(entity, e => e.CommissionPolicyDetails);

            if (input.CommissionPolicyDetails != null)
            {
                entity.CommissionPolicyDetails.Clear();
                foreach (var detailDto in input.CommissionPolicyDetails)
                {
                    var detailEntity = ObjectMapper.Map<CommissionPolicyDetailsInfo>(detailDto);
                    entity.CommissionPolicyDetails.Add(detailEntity);
                }
            }
            await CurrentUnitOfWork.SaveChangesAsync();
            return "Commission Policy updated successfully.";
        }

        public async Task<string> Delete(long id)
        {
            var entity = await CommisionPolicy_Repo.FirstOrDefaultAsync(id);
            if (entity == null)
                throw new UserFriendlyException("Commission Policy not found");

            await CommisionPolicy_Repo.DeleteAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "Commission Policy deleted successfully.";
        }

        public async Task<string> EmployeePolicyAllocation(long employeeId, long policyId,long UserId)
        {
            var employee = await Employee_Repo.GetAsync(employeeId);
            if (employee == null)
                throw new UserFriendlyException("Employee not found");

            employee.CommissionPolicyId = policyId;
            employee.UserId = UserId;
            await Employee_Repo.UpdateAsync(employee);
            await CurrentUnitOfWork.SaveChangesAsync();

            return "Policy allocated successfully";
        }
    }
}
