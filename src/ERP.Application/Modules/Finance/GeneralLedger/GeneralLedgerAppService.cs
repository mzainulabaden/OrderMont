using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using ERP.Authorization;
using ERP.Modules.Finance.ChartOfAccount.COALevel04;
using ERP.Modules.Finance.GeneralLedger.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Modules.Finance.GeneralLedger
{
    [AbpAuthorize(PermissionNames.UNIVERSAL_Finance)]
    public class GeneralLedgerAppService : ERPApplicationService
    {
        public IRepository<GeneralLedgerInfo, long> GeneralLedger_Repo { get; set; }
        public IRepository<COALevel04Info, long> COALevel04_Repo { get; set; }

        public async Task<OpeningClientsBulkResultDto> BulkUpload(OpeningClientsBulkRequestDto input)
        {
            var result = new OpeningClientsBulkResultDto
            {
                TotalItems = input?.Items?.Count ?? 0
            };

            if (input?.Items == null || input.Items.Count == 0)
            {
                result.Errors.Add("No items provided for bulk upload");
                result.FailureCount = result.TotalItems;
                await WriteErrorsToFileAsync(result);
                return result;
            }

            using (var uow = UnitOfWorkManager.Begin())
            {
                for (int index = 0; index < input.Items.Count; index++)
                {
                    var item = input.Items[index];
                    var rowNumber = index + 1;
                    try
                    {
                        var issueDate = item.IssueDate == default ? DateTime.Now : item.IssueDate;
                        if (string.IsNullOrWhiteSpace(item.COALevel04Name))
                            throw new Exception("COALevel04Name is required");

                        var coaId = await COALevel04_Repo
                            .GetAll(this)
                            .Where(i => i.Name.ToLower().Trim() == item.COALevel04Name.ToLower().Trim())
                            .Select(i => (long?)i.Id)
                            .FirstOrDefaultAsync();

                        if (coaId == null)
                            throw new Exception($"COALevel04 '{item.COALevel04Name}' not found");

                        var ledger = new GeneralLedgerInfo
                        {
                            IssueDate = issueDate,
                            VoucherNumber = string.IsNullOrWhiteSpace(item.VoucherNumber) ? await GetVoucherNumber("Opening Clients and suppliers", issueDate) : item.VoucherNumber,
                            ChartOfAccountId = coaId.Value,
                            Credit = item.Credit ?? 0,
                            Debit = item.Debit ?? 0,
                            IsAdjustmentEntry = false,
                            Status = "PENDING",
                            EmployeeId = 0,
                            LinkedDocumentId = 0,
                            LinkedDocument = 0,
                            ReferenceDocumentId = 0,
                            ReferenceVoucherNumber = null,
                            ReferenceDocument = null,
                            Remarks = item.Remarks,
                            IsDeleted = false,
                            TenantId = AbpSession.TenantId
                        };

                        await GeneralLedger_Repo.InsertAsync(ledger);
                        result.SuccessCount++;
                    }
                    catch (Exception ex)
                    {
                        result.FailureCount++;
                        result.Errors.Add($"Row {rowNumber}: {ex.Message}");
                    }
                }

                await uow.CompleteAsync();
            }

            if (result.Errors.Count > 0)
            {
                await WriteErrorsToFileAsync(result);
            }

            return result;
        }

        private async Task<string> GetVoucherNumber(string prefix, DateTime issueDate)
        {
            var count = await GeneralLedger_Repo
                .GetAll(this, i => i.IssueDate.Month == issueDate.Month && i.IssueDate.Year == issueDate.Year)
                .CountAsync();

            var voucherNumber = $"{prefix}-{issueDate.Year}-{issueDate.Month}-{count + 1}";
            return voucherNumber;
        }

        private static async Task WriteErrorsToFileAsync(OpeningClientsBulkResultDto result)
        {
            try
            {
                var baseDirectory = Directory.GetCurrentDirectory();
                var errorsDirectory = Path.Combine(baseDirectory, "wwwroot", "bulk-upload-errors");
                if (!Directory.Exists(errorsDirectory))
                    Directory.CreateDirectory(errorsDirectory);

                var fileName = $"opening-clients-errors-{DateTime.Now:yyyyMMddHHmmssfff}.txt";
                var fullPath = Path.Combine(errorsDirectory, fileName);
                await File.WriteAllLinesAsync(fullPath, result.Errors);

                result.ErrorFilePath = $"/bulk-upload-errors/{fileName}";
            }
            catch
            {
                // If writing the error file fails, keep the errors in-memory
                // and leave ErrorFilePath as null.
            }
        }
    }
}


