using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Modules.HumanResource.AttendanceManagement.Dtos;
using ERP.Modules.HumanResource.EmployeeManagement;
using ERP.Modules.HumanResource.GazettedHoliday;
using ERP.Modules.HumanResource.LookUps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.HumanResource.AttendanceManagement
{
    [AbpAuthorize(PermissionNames.LookUps_Attendance)]
    public class AttendanceManagementAppService : ApplicationService
    {
        public IRepository<AttendanceInfo, long> Attendance_Repo { get; set; }
        public IRepository<EmployeeInfo, long> Employee_Repo { get; set; }
        public IRepository<DesignationInfo, long> Designation_Repo { get; set; }
        public IRepository<GazettedHolidayInfo, long> GazettedHoliday_Repo { get; set; }

        [AbpAuthorize(PermissionNames.LookUps_Attendance_Submit)]
        public async Task<string> SubmitAttendance(SubmitAttendanceDto input)
        {
            if (input.EmployeeIds == null || !input.EmployeeIds.Any())
                throw new UserFriendlyException("EmployeeIds list cannot be empty.");

            var employees = Employee_Repo.GetAll(this).Where(i => input.EmployeeIds.Contains(i.Id)).Select(i => i.Id).Future();
            var existing_attendances = Attendance_Repo.GetAll(this).Where(i => input.EmployeeIds.Contains(i.EmployeeId) && i.AttendanceDate.Date == input.AttendanceDate.Date).Select(i => i.EmployeeId).Future();
            _ = await existing_attendances.ToListAsync();

            var new_attendances = new List<AttendanceInfo>();
            var invalid_employees_ids = input.EmployeeIds.Except(employees).ToList();

            if (invalid_employees_ids.Any())
                throw new UserFriendlyException($"Invalid EmployeeIds: {string.Join(", ", invalid_employees_ids)}");

            foreach (var emp_id in input.EmployeeIds)
            {
                if (!existing_attendances.Contains(emp_id))
                {
                    new_attendances.Add(new AttendanceInfo
                    {
                        EmployeeId = emp_id,
                        AttendanceDate = input.AttendanceDate,
                        CheckIn_Time = input.CheckIn_Time,
                        CheckOut_Time = input.CheckOut_Time,
                        TenantId = AbpSession.TenantId
                    });
                }
            }

            if (new_attendances.Count > 0)
            {
                await Attendance_Repo.InsertRangeAsync(new_attendances);
                await CurrentUnitOfWork.SaveChangesAsync();
                return "Attendance marked successfully for selected Employees.";
            }

            return "Attendance was already marked for all provided EmployeeIds on the given date.";
        }

        public async Task<PagedResultDto<GetAllAttendanceDto>> GetAllPresentEmployees(PagedResultRequestDto input, string EmployeeId, string DesignationId, DateTime? StartDate, DateTime? EndDate)
        {
            StartDate ??= DateTime.Now;
            EndDate ??= DateTime.Now;

            long.TryParse(EmployeeId, out var employee_id);
            long.TryParse(DesignationId, out var designation_id);

            var attendance_query = Attendance_Repo.GetAll(this);
            if (StartDate != null)
                attendance_query = attendance_query.Where(i => i.AttendanceDate.Date >= StartDate.Value.Date);
            if (EndDate != null)
                attendance_query = attendance_query.Where(i => i.AttendanceDate.Date <= EndDate.Value.Date);
            var employee_ids = await attendance_query.Select(i => i.EmployeeId).Distinct().ToListAsync();

            var employees_query = Employee_Repo.GetAll(this, i => employee_ids.Contains(i.Id));
            if (!string.IsNullOrWhiteSpace(EmployeeId) && employee_id != 0)
                employees_query = employees_query.Where(i => i.Id == employee_id);
            if (!string.IsNullOrWhiteSpace(DesignationId) && designation_id != 0)
                employees_query = employees_query.Where(i => i.DesignationId == designation_id);
            var filtered_employee_ids = await employees_query.Select(i => i.Id).Distinct().ToListAsync();

            var final_query = attendance_query.Where(i => filtered_employee_ids.Contains(i.EmployeeId)).OrderByDescending(i => i.CreationTime);
            var paged_query = final_query.Skip(input.SkipCount).Take(input.MaxResultCount);
            var paged_employee_ids = await paged_query.Select(i => i.EmployeeId).Distinct().ToListAsync();

            var total_count = final_query.Count();
            var employees = await employees_query.Where(i => paged_employee_ids.Contains(i.Id)).Select(i => new { i.Id, i.ErpId, i.Name }).ToListAsync();
            var attendance = paged_query.ToList();

            var dict_employees = employees.ToDictionary(i => i.Id);

            var output = new List<GetAllAttendanceDto>();
            foreach (var item in attendance)
            {
                dict_employees.TryGetValue(item.EmployeeId, out var employee);

                var dto = new GetAllAttendanceDto();
                dto.Id = item.Id;
                dto.EmployeeId = employee?.Id ?? 0;
                dto.EmployeeErpId = employee?.ErpId ?? "";
                dto.EmployeeName = employee?.Name ?? "";
                dto.CheckIn_Time = item.CheckIn_Time?.ToString("hh:mm:ss tt");
                dto.CheckOut_Time = item.CheckOut_Time?.ToString("hh:mm:ss tt") ?? "";
                dto.AttendanceDate = item.AttendanceDate.ToString("yyyy-MM-dd");
                output.Add(dto);
            }

            return new PagedResultDto<GetAllAttendanceDto>(total_count, output);
        }

        public async Task<PagedResultDto<GetAllEmployeesAttendanceDto>> GetAllAbsentEmployees(PagedResultRequestDto input, string EmployeeId, string DesignationId, DateTime? StartDate, DateTime? EndDate)
        {
            StartDate ??= DateTime.Now;
            EndDate ??= DateTime.Now;

            long.TryParse(EmployeeId, out var employee_id);
            long.TryParse(DesignationId, out var designation_id);

            var employees_query = Employee_Repo.GetAll(this);
            if (!string.IsNullOrWhiteSpace(EmployeeId) && employee_id != 0)
                employees_query = employees_query.Where(i => i.Id == employee_id);
            if (!string.IsNullOrWhiteSpace(DesignationId) && designation_id != 0)
                employees_query = employees_query.Where(i => i.DesignationId == designation_id);
            var employees = await employees_query.Select(i => new { i.Id, i.RestDays }).ToListAsync();

            var attendance_query = Attendance_Repo.GetAll(this);
            if (StartDate != null && EndDate != null)
                attendance_query = attendance_query.Where(i => i.AttendanceDate.Date >= StartDate.Value.Date && i.AttendanceDate.Date <= EndDate.Value.Date);
            var attendance = await attendance_query.Select(i => new { i.EmployeeId, i.AttendanceDate.Date }).ToListAsync();

            var absentees = new List<(long EmployeeId, DateTime DateTime)>();
            var gazetted_holidays = await GazettedHoliday_Repo.GetAll(this, i => (!i.IsRecurring && i.EventStartDate <= EndDate && i.EventEndDate >= StartDate) || (i.IsRecurring && (i.EventStartDate.Month == StartDate.Value.Month || i.EventEndDate.Month == EndDate.Value.Month))).ToListAsync();
            var gazetted_days = gazetted_holidays.SelectMany(i => EachDay(i.EventStartDate < StartDate ? StartDate.Value : i.EventStartDate, i.EventEndDate > EndDate ? EndDate.Value : i.EventEndDate)).Select(i => i.Date).ToHashSet();

            foreach (var day in EachDay(StartDate.Value.Date, EndDate.Value.Date))
            {
                if (gazetted_days.Contains(day))
                    continue;

                foreach (var employee in employees)
                {
                    if (employee.RestDays != null && employee.RestDays.Contains((int)day.DayOfWeek))
                        continue;

                    if (!attendance.Exists(i => i.Date == day && i.EmployeeId == employee.Id))
                        absentees.Add((employee.Id, day));
                }
            }

            var absent_employees_ids = absentees.Select(i => i.EmployeeId);
            employees_query = employees_query.Where(i => absent_employees_ids.Contains(i.Id)).OrderByDescending(i => i.CreationTime);

            var total_count = employees_query.Count();
            var paged_result = await employees_query.Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync();

            var output = ObjectMapper.Map<List<GetAllEmployeesAttendanceDto>>(paged_result);
            foreach (var employee in output)
            {
                var employee_absentees = absentees.Where(i => i.EmployeeId == employee.Id);
                employee.AbsenteesCount = employee_absentees.Count().ToString();
                employee.AbsenteesDates = employee_absentees.Select(i => i.DateTime.ToString("yyyy-MM-dd"));
            }

            return new PagedResultDto<GetAllEmployeesAttendanceDto>(total_count, output);
        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
