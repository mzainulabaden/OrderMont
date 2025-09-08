using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Enums;
using ERP.Modules.HumanResource.AttendanceManagement;
using ERP.Modules.HumanResource.EmployeeManagement;
using ERP.Modules.HumanResource.GazettedHoliday;
using ERP.Modules.HumanResource.LookUps;
using ERP.Modules.HumanResource.PayrollAdministration.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;
using ERP.Modules.Finance.GeneralLedger;
using ERP.Modules.Finance.ChartOfAccount.COALevel04;

namespace ERP.Modules.HumanResource.PayrollAdministration;

[AbpAuthorize(PermissionNames.LookUps_EmployeeSalary)]
public class EmployeeSalaryAppService : ERPDocumentService<EmployeeSalaryInfo>
{
    public IRepository<AttendanceInfo, long> Attendance_Repo { get; set; }
    public IRepository<EmployeeInfo, long> Employee_Repo { get; set; }
    public IRepository<EmployeeSalaryInfo, long> EmployeeSalary_Repo { get; set; }
    public IRepository<DesignationInfo, long> Designation_Repo { get; set; }
    public IRepository<GazettedHolidayInfo, long> GazettedHoliday_Repo { get; set; }
    public IRepository<GeneralLedgerInfo, long> GeneralLedger_Repo { get; set; }
    public IRepository<COALevel04Info, long> COALevel04_Repo { get; set; }

    [HttpGet]
    public async Task<PagedResultDto<EmployeeSalaryGetAllDto>> GetAll(EmployeeSalaryFiltersDto filters)
    {
        var employee_salary_query = EmployeeSalary_Repo.GetAll(this).ApplyDocumentFilters(filters);
        var employee_salaries = await employee_salary_query.ToPagedListAsync(filters);

        var total_count = employee_salary_query.DeferredCount().FutureValue();

        var output = new List<EmployeeSalaryGetAllDto>();
        foreach (var employee_salary in employee_salaries)
        {
            var dto = ObjectMapper.Map<EmployeeSalaryGetAllDto>(employee_salary);
            output.Add(dto);
        }

        return new PagedResultDto<EmployeeSalaryGetAllDto>(total_count.Value, output);
    }

    [HttpGet]
    public async Task<List<EmployeeSalaryDetailsGetForEditDto>> GenerateSalary(DateTime StartDate, DateTime EndDate, EmployeeType EmployeeType)
    {
        var employees = Employee_Repo.GetAll(this, i => i.EmployeeType == EmployeeType).Select(i => new { i.Id, i.ErpId, i.Name, i.RestDays, i.DailyWageRate, i.MonthlySalary }).Future();
        var attendance_data = Attendance_Repo.GetAll(this, i => i.AttendanceDate >= StartDate && i.AttendanceDate <= EndDate).GroupBy(i => i.EmployeeId).Select(g => new { EmployeeId = g.Key, AttendanceDays = g.Count() }).Future();
        var gazetted_holidays = GazettedHoliday_Repo.GetAll(this, i => (i.IsRecurring && (i.EventStartDate.Month == StartDate.Month || i.EventEndDate.Month == EndDate.Month)) || (!i.IsRecurring && i.EventStartDate <= EndDate && i.EventEndDate >= StartDate)).Future();
        _ = await gazetted_holidays.ToListAsync();

        int total_days = (EndDate - StartDate).Days + 1;
        var unique_gazetted_days = new HashSet<DateTime>();
        var employee_salary_details = new List<EmployeeSalaryDetailsGetForEditDto>();

        foreach (var holiday in gazetted_holidays)
        {
            DateTime adjusted_start = holiday.EventStartDate < StartDate ? StartDate : holiday.EventStartDate;
            DateTime adjusted_end = holiday.EventEndDate > EndDate ? EndDate : holiday.EventEndDate;

            if (adjusted_start > adjusted_end)
                continue;

            foreach (var day in EachDay(adjusted_start, adjusted_end))
            {
                unique_gazetted_days.Add(day);
            }
        }

        foreach (var employee in employees)
        {
            decimal net_payable;
            int total_gazetted_days = unique_gazetted_days.Count;
            int total_rest_days = GetTotalRestDays(StartDate, EndDate, employee.RestDays);
            var attendance_days = attendance_data.FirstOrDefault(i => i.EmployeeId == employee.Id)?.AttendanceDays ?? 0;

            if (EmployeeType == EmployeeType.Monthly)
            {
                var daily_rate_for_monthly = employee.MonthlySalary / 30;
                var payable_days = attendance_days + total_gazetted_days + total_rest_days;
                net_payable = daily_rate_for_monthly * payable_days;
            }
            else
            {
                net_payable = employee.DailyWageRate * attendance_days;
            }

            employee_salary_details.Add(new EmployeeSalaryDetailsGetForEditDto
            {
                EmployeeId = employee.Id,
                EmployeeErpId = employee.ErpId,
                EmployeeName = employee.Name,
                AttendanceDays = attendance_days,
                RestDays = total_rest_days,
                GazettedHolidays = total_gazetted_days,
                LeaveDays = total_days - (attendance_days + total_gazetted_days + total_rest_days),
                PayableDays = attendance_days + total_gazetted_days + total_rest_days,
                DailyWageRate = employee.DailyWageRate,
                NetPayable = net_payable.SetPrecision(0)
            });
        }

        return employee_salary_details;
    }

    [AbpAuthorize(PermissionNames.LookUps_EmployeeSalary_Create)]
    public async Task<string> Create(EmployeeSalaryDto input)
    {
        var entity = ObjectMapper.Map<EmployeeSalaryInfo>(input);
        entity.Status = "PENDING";
        entity.VoucherNumber = await GetVoucherNumber("ES", input.IssueDate);
        entity.TenantId = AbpSession.TenantId;

        var employee_ids = input.EmployeeSalaryDetails.Select(i => i.EmployeeId).ToList();

        var has_overlapping_salaries = EmployeeSalary_Repo.GetAll(this, i => i.EmployeeType == entity.EmployeeType && i.StartDate <= entity.EndDate && i.EndDate >= entity.StartDate).DeferredAny().FutureValue();
        var employees = Employee_Repo.GetAll(this, i => employee_ids.Contains(i.Id)).Select(i => i.Id).Future();
        _ = await employees.ToListAsync();

        if (has_overlapping_salaries.Value)
            throw new UserFriendlyException($"A salary record for Employee Type '{entity.EmployeeType.ToString()}' already exists for the selected date range ({entity.StartDate:dd MMM yyyy} to {entity.EndDate:dd MMM yyyy}). Please select a different date range to avoid conflicts.");

        for (int i = 0; i < entity.EmployeeSalaryDetails.Count; i++)
        {
            var detail = entity.EmployeeSalaryDetails[i];
            if (!employees.Contains(detail.EmployeeId))
                throw new UserFriendlyException($"EmployeeId: '{detail.EmployeeId}' is invalid at Row: '{i + 1}'.");
        }

        await EmployeeSalary_Repo.InsertAsync(entity);
        await CurrentUnitOfWork.SaveChangesAsync();
        return "EmployeeSalary Created Successfully.";
    }

    private async Task<EmployeeSalaryInfo> GetById(long Id)
    {
        var employee_salary = await EmployeeSalary_Repo.GetAllIncluding(i => i.EmployeeSalaryDetails).Where(i => i.Id == Id).FirstOrDefaultAsync();
        if (employee_salary != null)
            return employee_salary;
        else
            throw new UserFriendlyException($"EmployeeSalaryId: '{Id}' is invalid.");
    }

    public async Task<EmployeeSalaryGetForEditDto> Get(long Id)
    {
        var employee_salary = await GetById(Id);

        var output = ObjectMapper.Map<EmployeeSalaryGetForEditDto>(employee_salary);
        var employee_ids = employee_salary.EmployeeSalaryDetails.Select(i => i.EmployeeId).ToList();

        var employees = Employee_Repo.GetAll(this, i => employee_ids.Contains(i.Id)).Select(i => new { i.Id, i.ErpId, i.Name }).Future();
        _ = await employees.ToListAsync();

        var dict_employees = employees.ToDictionary(i => i.Id);

        output.EmployeeSalaryDetails = new();
        foreach (var detail in employee_salary.EmployeeSalaryDetails)
        {
            dict_employees.TryGetValue(detail.EmployeeId, out var employee);

            var mapped_detail = ObjectMapper.Map<EmployeeSalaryDetailsGetForEditDto>(detail);
            mapped_detail.EmployeeErpId = employee.ErpId;
            mapped_detail.EmployeeName = employee.Name;
            output.EmployeeSalaryDetails.Add(mapped_detail);
        }

        return output;
    }

    [AbpAuthorize(PermissionNames.LookUps_EmployeeSalary_Update)]
    public async Task<string> Update(EmployeeSalaryDto input)
    {
        var old_employee_salary = await GetById(input.Id);
        if (old_employee_salary.Status != "PENDING")
            throw new UserFriendlyException($"Only entries with a status of 'PENDING' can be edited. The current status is '" + old_employee_salary.Status + "'.");

        var entity = ObjectMapper.Map(input, old_employee_salary);
        entity.VoucherNumber = await GetVoucherNumber("ES", input.IssueDate, entity.VoucherNumber.GetVoucherIndex());

        var employee_ids = input.EmployeeSalaryDetails.Select(i => i.EmployeeId).ToList();

        var has_overlapping_salaries = EmployeeSalary_Repo.GetAll(this, i => i.Id != input.Id && i.EmployeeType == entity.EmployeeType && i.StartDate <= entity.EndDate && i.EndDate >= entity.StartDate).DeferredAny().FutureValue();
        var employees = Employee_Repo.GetAll(this, i => employee_ids.Contains(i.Id)).Select(i => i.Id).Future();
        _ = await employees.ToListAsync();

        if (has_overlapping_salaries.Value)
            throw new UserFriendlyException($"A salary record for Employee Type '{entity.EmployeeType.ToString()}' already exists for the selected date range ({entity.StartDate:dd MMM yyyy} to {entity.EndDate:dd MMM yyyy}). Please select a different date range to avoid conflicts.");

        for (int i = 0; i < entity.EmployeeSalaryDetails.Count; i++)
        {
            var detail = entity.EmployeeSalaryDetails[i];
            if (!employees.Contains(detail.EmployeeId))
                throw new UserFriendlyException($"EmployeeId: '{detail.EmployeeId}' is invalid at Row: '{i + 1}'.");
        }

        await EmployeeSalary_Repo.UpdateAsync(entity);
        await CurrentUnitOfWork.SaveChangesAsync();
        return "EmployeeSalary Updated Successfully.";
    }

    [AbpAuthorize(PermissionNames.LookUps_EmployeeSalary_Delete)]
    public async Task<string> Delete(long Id)
    {
        var employee_salary = await EmployeeSalary_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
        if (employee_salary == null)
            throw new UserFriendlyException($"EmployeeSalaryId: '{Id}' is invalid.");
        if (employee_salary.Status != "PENDING")
            throw new UserFriendlyException($"Unable to delete EmployeeSalary: only records with a 'PENDING' status can be deleted.");

        await EmployeeSalary_Repo.DeleteAsync(employee_salary);
        await CurrentUnitOfWork.SaveChangesAsync();
        return "EmployeeSalary Deleted Successfully.";
    }

    private int GetTotalRestDays(DateTime start_date, DateTime end_date, List<int> rest_days)
    {
        int rest_days_count = 0;
        int total_days = (end_date - start_date).Days + 1;

        for (int i = 0; i < total_days; i++)
        {
            var current_day = start_date.AddDays(i).DayOfWeek;
            if (rest_days.Contains((int)current_day))
                rest_days_count++;
        }

        return rest_days_count;
    }

    private static IEnumerable<DateTime> EachDay(DateTime from, DateTime to)
    {
        for (var day = from.Date; day <= to.Date; day = day.AddDays(1))
            yield return day;
    }

    //[AbpAuthorize(PermissionNames.LookUps_EmployeeSalary_ApproveDocument)]
    public async override Task<string> ApproveDocument(long Id)
    {
        var employee_salary = await GetById(Id);
        
        if (employee_salary.Status != "PENDING")
            throw new UserFriendlyException($"Only entries with a status of 'PENDING' can be approved. The current status is '{employee_salary.Status}'.");
        
       
        employee_salary.Status = "APPROVED";

        var general_ledger_entries = new List<GeneralLedgerInfo>();

        //Fixed It later dependent at the Default integrations

        const long Salary = 1; 
        const long Salary_Payable = 2;
        
        foreach (var detail in employee_salary.EmployeeSalaryDetails)
        {
            var employee = await Employee_Repo.GetAsync(detail.EmployeeId);
            
            var expense_entry = new GeneralLedgerInfo
            {
                IssueDate = employee_salary.IssueDate,
                VoucherNumber = employee_salary.VoucherNumber,
                ChartOfAccountId = Salary,
                Debit = detail.NetPayable,
                Credit = 0,
                IsAdjustmentEntry = false,
                Status = "PENDING",
                EmployeeId = detail.EmployeeId,
                LinkedDocumentId = employee_salary.Id,
                LinkedDocument = GeneralLedgerLinkedDocument.EmployeeSalary,
                Remarks = $"Salary expense for {employee.Name} for period {employee_salary.StartDate:dd MMM yyyy} to {employee_salary.EndDate:dd MMM yyyy}",
                TenantId = AbpSession.TenantId
            };
            
            general_ledger_entries.Add(expense_entry);
            
            var payable_entry = new GeneralLedgerInfo
            {
                IssueDate = employee_salary.IssueDate,
                VoucherNumber = employee_salary.VoucherNumber,
                ChartOfAccountId = Salary_Payable,
                Debit = 0,
                Credit = detail.NetPayable,
                IsAdjustmentEntry = false,
                Status = "PENDING",
                EmployeeId = detail.EmployeeId,
                LinkedDocumentId = employee_salary.Id,
                LinkedDocument = GeneralLedgerLinkedDocument.EmployeeSalary,
                Remarks = $"Salary payable for {employee.Name} for period {employee_salary.StartDate:dd MMM yyyy} to {employee_salary.EndDate:dd MMM yyyy}",
                TenantId = AbpSession.TenantId
            };
            
            general_ledger_entries.Add(payable_entry);
        }
        
        foreach (var entry in general_ledger_entries)
        {
            await GeneralLedger_Repo.InsertAsync(entry);
        }
        await EmployeeSalary_Repo.UpdateAsync(employee_salary);
        await CurrentUnitOfWork.SaveChangesAsync();
        return "Employee Salary approved successfully.";
    }
}
