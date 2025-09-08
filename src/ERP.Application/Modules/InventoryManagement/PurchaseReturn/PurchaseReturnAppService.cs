using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Enums;
using ERP.Extensions.Common;
using ERP.Modules.Finance.ChartOfAccount.COALevel04;
using ERP.Modules.Finance.GeneralLedger;
using ERP.Modules.InventoryManagement.Item;
using ERP.Modules.InventoryManagement.LookUps;
using ERP.Modules.InventoryManagement.PurchaseInvoice;
using ERP.Modules.InventoryManagement.StockLedger;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.InventoryManagement.PurchaseReturn
{
    [AbpAuthorize(PermissionNames.LookUps_PurchaseReturn)]
    public class PurchaseReturnAppService : ERPDocumentService<PurchaseReturnInfo>
    {
        public IRepository<PurchaseReturnInfo, long> PurchaseReturn_Repo { get; set; }
        public IRepository<COALevel04Info, long> COALevel04_Repo { get; set; }
        public IRepository<ItemInfo, long> Item_Repo { get; set; }
        public IRepository<UnitInfo, long> Unit_Repo { get; set; }
        public IRepository<PurchaseInvoiceInfo, long> PurchaseInvoice_Repo { get; set; }
        public IRepository<WarehouseInfo, long> Warehouse_Repo { get; set; }
        public IRepository<GeneralLedgerInfo, long> GeneralLedger_Repo { get; set; }
        public IRepository<WarehouseStockLedgerInfo, long> WarehouseStockLedger_Repo { get; set; }

        public async Task<PagedResultDto<PurchaseReturnGetAllDto>> GetAll(PurchaseReturnFiltersDto filters)
        {
            var purchase_return_query = PurchaseReturn_Repo.GetAll(this).ApplyDocumentFilters(filters);
            if (!string.IsNullOrWhiteSpace(filters.SupplierCOALevel04Id))
                purchase_return_query = purchase_return_query.Where(i => i.SupplierCOALevel04Id == filters.SupplierCOALevel04Id.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.WarehouseId))
                purchase_return_query = purchase_return_query.Where(i => i.WarehouseId == filters.WarehouseId.TryToLong());
            var purchase_returns = await purchase_return_query.ToPagedListAsync(filters);

            var supplier_coa_level04_ids = purchase_returns.Select(i => i.SupplierCOALevel04Id).ToList();
            var warehouse_ids = purchase_returns.Select(i => i.WarehouseId).ToList();

            var total_count = purchase_return_query.DeferredCount().FutureValue();
            var supplier_coa_level04s = COALevel04_Repo.GetAll(this, i => supplier_coa_level04_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            var warehouses = Warehouse_Repo.GetAll(this, i => warehouse_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            await warehouses.ToListAsync();

            var dict_supplier_coa_level04s = supplier_coa_level04s.ToDictionary(i => i.Id);
            var dict_warehouses = warehouses.ToDictionary(i => i.Id);

            var output = new List<PurchaseReturnGetAllDto>();
            foreach (var purchase_return in purchase_returns)
            {
                dict_supplier_coa_level04s.TryGetValue(purchase_return.SupplierCOALevel04Id, out var supplier_coa_level04);
                dict_warehouses.TryGetValue(purchase_return.WarehouseId, out var warehouse);

                var dto = ObjectMapper.Map<PurchaseReturnGetAllDto>(purchase_return);
                dto.SupplierCOALevel04Name = supplier_coa_level04?.Name ?? "";
                dto.WarehouseName = warehouse?.Name ?? "";
                output.Add(dto);
            }

            return new PagedResultDto<PurchaseReturnGetAllDto>(total_count.Value, output);
        }

        [AbpAuthorize(PermissionNames.LookUps_PurchaseReturn_Create)]
        public async Task<string> Create(PurchaseReturnDto input)
        {
            var supplier_coa_level04 = COALevel04_Repo.GetAll(this, i => i.Id == input.SupplierCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            var warehouse = Warehouse_Repo.GetAll(this, i => i.Id == input.WarehouseId).DeferredFirstOrDefault().FutureValue();
            await warehouse.ValueAsync();

            if (supplier_coa_level04.Value == null)
                throw new UserFriendlyException($"SupplierCOALevel04Id: '{input.SupplierCOALevel04Id}' is invalid.");
            if (warehouse.Value == null)
                throw new UserFriendlyException($"WarehouseId: '{input.WarehouseId}' is invalid.");

            var entity = ObjectMapper.Map<PurchaseReturnInfo>(input);
            entity.Status = "PENDING";
            entity.VoucherNumber = await GetVoucherNumber("PR", input.IssueDate);

            var item_ids = input.PurchaseReturnDetails.Select(i => i.ItemId).ToList();
            var unit_ids = input.PurchaseReturnDetails.Select(i => i.UnitId).ToList();

            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Select(i => i.Id).Future();
            _ = await units.ToListAsync();

            for (int i = 0; i < entity.PurchaseReturnDetails.Count; i++)
            {
                var detail = entity.PurchaseReturnDetails[i];

                if (!items.Contains(detail.ItemId))
                    throw new UserFriendlyException($"ItemId: '{detail.ItemId}' is invalid at Row: '{i + 1}'.");
                if (!units.Contains(detail.UnitId))
                    throw new UserFriendlyException($"UnitId: '{detail.UnitId}' is invalid at Row: '{i + 1}'.");
            }

            entity.TenantId = AbpSession.TenantId;
            await PurchaseReturn_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "PurchaseReturn Created Successfully.";
        }

        private async Task<PurchaseReturnInfo> GetById(long Id)
        {
            var purchase_return = await PurchaseReturn_Repo.GetAllIncluding(i => i.PurchaseReturnDetails).Where(i => i.Id == Id).FirstOrDefaultAsync();
            if (purchase_return != null)
                return purchase_return;
            else
                throw new UserFriendlyException($"PurchaseReturnId: '{Id}' is invalid.");
        }

        public async Task<PurchaseReturnGetForEditDto> Get(long Id)
        {
            var purchase_return = await GetById(Id);
            var supplier_coa_level04 = COALevel04_Repo.GetAll(this, i => i.Id == purchase_return.SupplierCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            var warehouse = Warehouse_Repo.GetAll(this, i => i.Id == purchase_return.WarehouseId).DeferredFirstOrDefault().FutureValue();
            await warehouse.ValueAsync();

            var output = ObjectMapper.Map<PurchaseReturnGetForEditDto>(purchase_return);
            output.SupplierCOALevel04Name = supplier_coa_level04?.Value?.Name ?? "";
            output.WarehouseName = warehouse?.Value?.Name ?? "";

            var item_ids = purchase_return.PurchaseReturnDetails.Select(i => i.ItemId).ToList();
            var unit_ids = purchase_return.PurchaseReturnDetails.Select(i => i.UnitId).ToList();

            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            _ = await units.ToListAsync();

            var dict_items = items.ToDictionary(i => i.Id, i => i.Name);
            var dict_units = units.ToDictionary(i => i.Id, i => i.Name);

            output.PurchaseReturnDetails = new();
            foreach (var detail in purchase_return.PurchaseReturnDetails)
            {
                var mapped_detail = ObjectMapper.Map<PurchaseReturnDetailsGetForEditDto>(detail);
                mapped_detail.ItemName = dict_items.GetValueOrDefault(detail.ItemId);
                mapped_detail.UnitName = dict_units.GetValueOrDefault(detail.UnitId);
                output.PurchaseReturnDetails.Add(mapped_detail);
            }

            return output;
        }

        [AbpAuthorize(PermissionNames.LookUps_PurchaseReturn_Update)]
        public async Task<string> Update(PurchaseReturnDto input)
        {
            var old_purchase_return = await GetById(input.Id);
            if (old_purchase_return.Status != "PENDING")
                throw new UserFriendlyException($"Only entries with a status of 'PENDING' can be edited. The current status is '{old_purchase_return.Status}'.");

            var supplier_coa_level04 = COALevel04_Repo.GetAll(this, i => i.Id == input.SupplierCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            var warehouse = Warehouse_Repo.GetAll(this, i => i.Id == input.WarehouseId).DeferredFirstOrDefault().FutureValue();
            await warehouse.ValueAsync();

            if (supplier_coa_level04.Value == null)
                throw new UserFriendlyException($"SupplierCOALevel04Id: '{input.SupplierCOALevel04Id}' is invalid.");
            if (warehouse.Value == null)
                throw new UserFriendlyException($"WarehouseId: '{input.WarehouseId}' is invalid.");

            var entity = ObjectMapper.Map(input, old_purchase_return);
            entity.VoucherNumber = await GetVoucherNumber("PR", input.IssueDate, entity.VoucherNumber.GetVoucherIndex());

            var item_ids = input.PurchaseReturnDetails.Select(i => i.ItemId).ToList();
            var unit_ids = input.PurchaseReturnDetails.Select(i => i.UnitId).ToList();

            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Select(i => i.Id).Future();
            _ = await units.ToListAsync();

            for (int i = 0; i < entity.PurchaseReturnDetails.Count; i++)
            {
                var detail = entity.PurchaseReturnDetails[i];

                if (!items.Contains(detail.ItemId))
                    throw new UserFriendlyException($"ItemId: '{detail.ItemId}' is invalid at Row: '{i + 1}'.");
                if (!units.Contains(detail.UnitId))
                    throw new UserFriendlyException($"UnitId: '{detail.UnitId}' is invalid at Row: '{i + 1}'.");
            }

            await PurchaseReturn_Repo.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "PurchaseReturn Updated Successfully.";
        }

        [AbpAuthorize(PermissionNames.LookUps_PurchaseReturn_Delete)]
        public async Task<string> Delete(long Id)
        {
            var purchase_return = await PurchaseReturn_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (purchase_return == null)
                throw new UserFriendlyException($"PurchaseReturnId: '{Id}' is invalid.");
            if (purchase_return.Status != "PENDING")
                throw new UserFriendlyException($"Unable to delete PurchaseReturn: only records with a 'PENDING' status can be deleted.");

            await PurchaseReturn_Repo.DeleteAsync(purchase_return);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "PurchaseReturn Deleted Successfully.";
        }

        public async override Task<string> ApproveDocument(long Id)
        {
            var document = await Get(Id);
            var item_ids = document.PurchaseReturnDetails.Select(i => i.ItemId).ToList();
            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Future();
            var supplier = COALevel04_Repo.GetAll(this, i => i.Id == document.SupplierCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            _ = await supplier.ValueAsync();

            var transaction_base = new GeneralLedgerTransactionBaseDto()
            {
                IssueDate = document.IssueDate,
                VoucherNumber = document.VoucherNumber,
                IsAdjustmentEntry = false,
                Remarks = document.Remarks,
                LinkedDocument = GeneralLedgerLinkedDocument.PurchaseReturn,
                LinkedDocumentId = document.Id,
                TenantId = AbpSession.TenantId,
            };

            foreach (var detail in document.PurchaseReturnDetails)
            {
                var item_coa = items.FirstOrDefault(i => i.Id == detail.ItemId)?.PurchaseCOALevel04Id ?? 0;
               
                await WarehouseStockLedger_Repo.AddLedgerTransactionAsync(
                    document.IssueDate, 
                    document.VoucherNumber, 
                    detail.ItemId, 
                    document.WarehouseId, 
                    0,
                    detail.QuantityReturned, 
                    detail.LastPurchaseRate,
                    detail.QuantityReturned,
                    detail.GrandTotal,
                    document.Remarks,
                    document.Id,
                    WarehouseStockLedgerLinkedDocument.PurchaseReturn,
                    AbpSession.TenantId);

                await GeneralLedger_Repo.AddLedgerTransactionAsync(
                    transaction_base, 
                    detail.GrandTotal,
                    0,
                    item_coa,
                    0);
            }

            await GeneralLedger_Repo.AddLedgerTransactionAsync(
                transaction_base, 
                0,
                document.PurchaseReturnDetails.Sum(d => d.GrandTotal),
                document.SupplierCOALevel04Id, 
                0);

            CurrentUnitOfWork.SaveChanges();
            return await base.ApproveDocument(Id);
        }

        public async Task<decimal> GetLatestRate(long ItemId, long SupplierCOALevel04Id, long UnitId)
        {
            var latest_purchase_rate = await PurchaseReturn_Repo
                .GetAllIncluding(i => i.PurchaseReturnDetails)
                .Where(i => i.SupplierCOALevel04Id == SupplierCOALevel04Id)
                .OrderByDescending(i => i.IssueDate)
                .ThenByDescending(i => i.CreationTime)
                .SelectMany(i => i.PurchaseReturnDetails)
                .Where(detail => detail.ItemId == ItemId && detail.UnitId == UnitId)
                .Select(i => i.LastPurchaseRate)
                .FirstOrDefaultAsync();

            return latest_purchase_rate;
        }
    }
}
