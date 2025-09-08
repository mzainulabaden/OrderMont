using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using ERP.Authorization;
using ERP.Modules.InventoryManagement.Item;
using ERP.Modules.InventoryManagement.LookUps;
using ERP.Modules.InventoryManagement.StockLedger;
using ERP.Modules.InventoryManagement.WarehouseStockLedger.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Modules.InventoryManagement.WarehouseStockLedger
{
    [AbpAuthorize(PermissionNames.LookUps_IMS_WarehouseStockLedger)]
    public class WarehouseStockLedgerAppService : ERPApplicationService
    {
        public IRepository<WarehouseStockLedgerInfo, long> WarehouseStockLedger_Repo { get; set; }
        public IRepository<ItemInfo, long> Item_Repo { get; set; }
        public IRepository<WarehouseInfo, long> Warehouse_Repo { get; set; }

        [AbpAuthorize(PermissionNames.LookUps_IMS_WarehouseStockLedger_Create)]
        public async Task<WarehouseStockLedgerBulkUploadResultDto> BulkUpload(WarehouseStockLedgerBulkUploadRequestDto input)
        {
            var result = new WarehouseStockLedgerBulkUploadResultDto
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
                        if (string.IsNullOrWhiteSpace(item.ItemName))
                            throw new Exception("ItemName is required");
                        var itemId = await Item_Repo.GetAll(this).Where(i => i.Name.ToLower() == item.ItemName.ToLower()).Select(i => (long?)i.Id).FirstOrDefaultAsync();
                        if (itemId == null)
                            throw new Exception($"Item '{item.ItemName}' not found");
                        if (string.IsNullOrWhiteSpace(item.WarehouseName))
                            throw new Exception("WarehouseName is required");
                        var warehouseId = await Warehouse_Repo.GetAll(this).Where(w => w.Name.ToLower() == item.WarehouseName.ToLower()).Select(w => (long?)w.Id).FirstOrDefaultAsync();
                        if (warehouseId == null)
                            throw new Exception($"Warehouse '{item.WarehouseName}' not found");
                        var ledger = new WarehouseStockLedgerInfo
                        {
                            IssueDate = issueDate,
                            VoucherNumber = string.IsNullOrWhiteSpace(item.VoucherNumber)
                                ? await GetVoucherNumber("Opening Warehouse", issueDate)
                                : item.VoucherNumber,
                            ItemId = itemId.Value,
                            Credit = item.Credit,
                            Debit = item.Debit,
                            ActualQty = 0,
                            Rate = item.Rate,
                            TotalAmount = 0,
                            Remarks = item.Remarks,
                            WarehouseId = warehouseId.Value,
                            DocumentId = 0,
                            IsDeleted= false,
                            WarehouseStockLedgerLinkedDocument = Enums.WarehouseStockLedgerLinkedDocument.OpeningStock,
                            TenantId = AbpSession.TenantId
                        };
                        await WarehouseStockLedger_Repo.InsertAsync(ledger);
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
            var count = await WarehouseStockLedger_Repo
                .GetAll(this, i => i.IssueDate.Month == issueDate.Month && i.IssueDate.Year == issueDate.Year)
                .CountAsync();
            var voucherNumber = $"{prefix}-{issueDate.Year}-{issueDate.Month}-{count + 1}";
            return voucherNumber;
        }
        private static async Task WriteErrorsToFileAsync(WarehouseStockLedgerBulkUploadResultDto result)
        {
            try
            {
                var baseDirectory = Directory.GetCurrentDirectory();
                var errorsDirectory = Path.Combine(baseDirectory, "wwwroot", "bulk-upload-errors");
                if (!Directory.Exists(errorsDirectory))
                    Directory.CreateDirectory(errorsDirectory);
                var fileName = $"warehouse-stock-ledger-errors-{DateTime.Now:yyyyMMddHHmmssfff}.txt";
                var fullPath = Path.Combine(errorsDirectory, fileName);
                await File.WriteAllLinesAsync(fullPath, result.Errors);
                result.ErrorFilePath = $"/bulk-upload-errors/{fileName}";
            }
            catch
            {
            }
        }
    }
}
