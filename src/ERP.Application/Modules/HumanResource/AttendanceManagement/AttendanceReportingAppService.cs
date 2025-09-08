using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Modules.HumanResource.EmployeeManagement;
using ERP.Modules.HumanResource.GazettedHoliday;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.HumanResource.AttendanceManagement
{
    [AbpAuthorize]
    public class AttendanceReportingAppService : ApplicationService
    {
        public IRepository<AttendanceInfo, long> Attendance_Repo { get; set; }
        public IRepository<EmployeeInfo, long> Employee_Repo { get; set; }
        public IRepository<GazettedHolidayInfo, long> GazettedHoliday_Repo { get; set; }

        private void AddHeading(ExcelWorksheet worksheet, string cell, double width, string text)
        {
            worksheet.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells[cell].Style.Font.Bold = true;
            worksheet.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[cell].Value = text;
            worksheet.Cells[cell].EntireColumn.Width = width;
        }

        public async Task<string> GenerateAttendanceSummary(DateTime? StartDate, DateTime? EndDate)
        {
            StartDate ??= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            EndDate ??= new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1).AddDays(-1);

            var employees = Employee_Repo.GetAll(this, i => i.IsActive).Future();
            var attendance_records = Attendance_Repo.GetAll(this, i => i.AttendanceDate >= StartDate && i.AttendanceDate <= EndDate).Future();
            var gazetted_holidays = GazettedHoliday_Repo.GetAll(this, i => (!i.IsRecurring && i.EventStartDate <= EndDate && i.EventEndDate >= StartDate) || (i.IsRecurring && (i.EventStartDate.Month == StartDate.Value.Month || i.EventEndDate.Month == EndDate.Value.Month))).Future();
            _ = await attendance_records.ToListAsync();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Attendance Summary");

            AddHeading(worksheet, "A1", 15, "ERP ID");
            AddHeading(worksheet, "B1", 25, "Employee Name");

            int col = 3;
            foreach (var date in EachDay(StartDate.Value, EndDate.Value))
            {
                AddHeading(worksheet, worksheet.Cells[1, col].Address, 10, date.Day.ToString());
                col++;
            }

            AddHeading(worksheet, worksheet.Cells[1, col].Address, 15, "Total Present");
            AddHeading(worksheet, worksheet.Cells[1, col + 1].Address, 15, "Total Absent");
            AddHeading(worksheet, worksheet.Cells[1, col + 2].Address, 15, "Total Late");
            AddHeading(worksheet, worksheet.Cells[1, col + 3].Address, 15, "Total RestDay");
            AddHeading(worksheet, worksheet.Cells[1, col + 4].Address, 15, "Total Gazetted");

            int row = 2;

            foreach (var employee in employees)
            {
                var emp_attendance = attendance_records.Where(i => i.EmployeeId == employee.Id).ToList();
                var gazetted_days = gazetted_holidays.SelectMany(i => EachDay(i.EventStartDate < StartDate ? StartDate.Value : i.EventStartDate, i.EventEndDate > EndDate ? EndDate.Value : i.EventEndDate)).Select(day => day.Date).ToHashSet();

                worksheet.Cells[$"A{row}"].Value = employee.ErpId;
                worksheet.Cells[$"B{row}"].Value = employee.Name;

                col = 3;
                int restday_count = 0, gazetted_count = 0;

                foreach (var date in EachDay(StartDate.Value, EndDate.Value))
                {
                    var attendance = emp_attendance.FirstOrDefault(i => i.AttendanceDate.Date == date.Date);
                    var attendance_status = attendance != null ? "P" : employee.RestDays?.Contains((int)date.DayOfWeek) == true ? "R" : gazetted_days.Contains(date.Date) ? "G" : "A";
                    worksheet.Cells[row, col++].Value = attendance_status;

                    restday_count += attendance_status == "R" ? 1 : 0;
                    gazetted_count += attendance_status == "G" ? 1 : 0;
                }

                int present_count = emp_attendance.Count();
                int late_count = emp_attendance.Count(i => i.CheckIn_Time.HasValue && i.CheckIn_Time.Value.TimeOfDay > new TimeSpan(9, 0, 0));
                int absent_count = (EndDate.Value - StartDate.Value).Days + 1 - restday_count - gazetted_count - present_count;

                worksheet.Cells[row, col].Value = present_count;
                worksheet.Cells[row, col + 1].Value = absent_count;
                worksheet.Cells[row, col + 2].Value = late_count;
                worksheet.Cells[row, col + 3].Value = restday_count;
                worksheet.Cells[row, col + 4].Value = gazetted_count;

                row++;
            }

            string file_path = $"AttendanceReports/AttendanceSummary_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            Directory.CreateDirectory(Path.GetDirectoryName("wwwroot/" + file_path));
            package.SaveAs(new FileInfo("wwwroot/" + file_path));
            return file_path;
        }

        public async Task<string> GenerateEmployeeMonthlyReport(long EmployeeId, int? Year, int? Month)
        {
            Year ??= DateTime.Now.Year;
            Month ??= DateTime.Now.Month;

            var employee = await Employee_Repo.GetAll(this).FirstOrDefaultAsync(i => i.Id == EmployeeId);
            if (employee == null)
                throw new UserFriendlyException($"EmployeeId: '{EmployeeId}' is invalid.");

            var start_date = new DateTime(Year.Value, Month.Value, 1);
            var end_date = start_date.AddMonths(1).AddDays(-1);

            var attendance_records = Attendance_Repo.GetAll(this, i => i.EmployeeId == EmployeeId && i.AttendanceDate >= start_date && i.AttendanceDate <= end_date).Future();
            var gazetted_holidays = GazettedHoliday_Repo.GetAll(this, i => (!i.IsRecurring && i.EventStartDate <= end_date && i.EventEndDate >= start_date) || (i.IsRecurring && (i.EventStartDate.Month == start_date.Month || i.EventEndDate.Month == end_date.Month))).Future();
            _ = await gazetted_holidays.ToListAsync();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Employee Attendance");

            AddHeading(worksheet, "A1", 15, "Date");
            AddHeading(worksheet, "B1", 15, "Check-In");
            AddHeading(worksheet, "C1", 15, "Check-Out");
            AddHeading(worksheet, "D1", 15, "Status");

            int row = 2;

            foreach (var date in EachDay(start_date, end_date))
            {
                var attendance = attendance_records.FirstOrDefault(i => i.AttendanceDate.Date == date.Date);
                var gazetted_days = gazetted_holidays.SelectMany(i => EachDay(i.EventStartDate < start_date ? start_date : i.EventStartDate, i.EventEndDate > end_date ? end_date : i.EventEndDate)).Select(i => i.Date).ToHashSet();
                string attendance_status = attendance != null ? (attendance.CheckIn_Time?.TimeOfDay > new TimeSpan(9, 0, 0) ? "Late" : "Present") : (employee.RestDays.Contains((int)date.DayOfWeek) ? "RestDay" : (gazetted_days.Contains(date.Date) ? "GazettedHoliday" : "Absent"));

                worksheet.Cells[$"A{row}"].Value = date.ToString("yyyy-MM-dd");
                worksheet.Cells[$"B{row}"].Value = attendance?.CheckIn_Time?.ToString("HH:mm") ?? "-";
                worksheet.Cells[$"C{row}"].Value = attendance?.CheckOut_Time?.ToString("HH:mm") ?? "-";
                worksheet.Cells[$"D{row}"].Value = attendance_status;

                row++;
            }

            string file_path = $"AttendanceReports/Employee_{EmployeeId}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            Directory.CreateDirectory(Path.GetDirectoryName("wwwroot/" + file_path));
            package.SaveAs(new FileInfo("wwwroot/" + file_path));
            return file_path;
        }

        private static IEnumerable<DateTime> EachDay(DateTime from, DateTime to)
        {
            for (var day = from; day <= to; day = day.AddDays(1))
                yield return day;
        }
    }
}
