using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Extensions.Common;
using ERP.Modules.Finance.ChartOfAccount.COALevel04;
using ERP.Modules.Finance.GeneralLedger;
using ERP.Modules.InventoryManagement.Item;
using ERP.Modules.InventoryManagement.LookUps;
using ERP.Modules.InventoryManagement.StockLedger;
using ERP.Modules.SalesManagement.SalesInvoice;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.SalesManagement.SalesReturn
{
    [AbpAuthorize(PermissionNames.LookUps_SalesReturn)]
    public class SalesReturnAppService : ERPDocumentService<SalesReturnInfo>
    {
        public IRepository<SalesReturnInfo, long> SalesReturn_Repo { get; set; }
        public IRepository<COALevel04Info, long> CustomerCOALevel04_Repo { get; set; }
        public IRepository<ItemInfo, long> Item_Repo { get; set; }
        public IRepository<UnitInfo, long> Unit_Repo { get; set; }
        public IRepository<SalesInvoiceInfo, long> SalesInvoice_Repo { get; set; }
        public IRepository<WarehouseInfo, long> Warehouse_Repo { get; set; }
        public IRepository<GeneralLedgerInfo, long> GeneralLedger_Repo { get; set; }
        public IRepository<WarehouseStockLedgerInfo, long> WarehouseStockLedger_Repo { get; set; }

        public async Task<PagedResultDto<SalesReturnGetAllDto>> GetAll(SalesReturnFiltersDto filters)
        {
            var sales_return_query = SalesReturn_Repo.GetAll(this).ApplyDocumentFilters(filters);
            if (!string.IsNullOrWhiteSpace(filters.CustomerCOALevel04Id))
                sales_return_query = sales_return_query.Where(i => i.CustomerCOALevel04Id == filters.CustomerCOALevel04Id.TryToLong());
            var sales_returns = await sales_return_query.Include(x => x.SalesReturnDetails).ToListAsync();
            if (!string.IsNullOrWhiteSpace(filters.WarehouseId))
            {
                var warehouseIdLong = filters.WarehouseId.TryToLong();
                sales_returns = sales_returns.Where(sr => sr.SalesReturnDetails.Any(d => d.WarehouseId == warehouseIdLong)).ToList();
            }

            var customer_coa_level04_ids = sales_returns.Select(i => i.CustomerCOALevel04Id).ToList();
            var warehouse_ids = sales_returns.SelectMany(i => i.SalesReturnDetails.Select(d => d.WarehouseId)).Distinct().ToList();

            var total_count = sales_returns.Count;
            var customer_coa_level04s = CustomerCOALevel04_Repo.GetAll(this, i => customer_coa_level04_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var warehouses = Warehouse_Repo.GetAll(this, i => warehouse_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();

            var dict_customer_coa_level04s = customer_coa_level04s.ToDictionary(i => i.Id);
            var dict_warehouses = warehouses.ToDictionary(i => i.Id);

            var output = new List<SalesReturnGetAllDto>();
            foreach (var sales_return in sales_returns)
            {
                dict_customer_coa_level04s.TryGetValue(sales_return.CustomerCOALevel04Id, out var customer_coa_level04);
                var dto = ObjectMapper.Map<SalesReturnGetAllDto>(sales_return);
                dto.CustomerCOALevel04Name = customer_coa_level04?.Name ?? "";
                output.Add(dto);
            }

            return new PagedResultDto<SalesReturnGetAllDto>(total_count, output);
        }

        [AbpAuthorize(PermissionNames.LookUps_SalesReturn_Create)]
        public async Task<string> Create(SalesReturnDto input)
        {
            var warehouseIds = input.SalesReturnDetails.Select(d => d.WarehouseId).Distinct().ToList();
            var warehouses = Warehouse_Repo.GetAll(this, i => warehouseIds.Contains(i.Id)).Select(i => i.Id).ToList();
            foreach (var wid in warehouseIds)
            {
                if (!warehouses.Contains(wid))
                    throw new UserFriendlyException($"WarehouseId: '{wid}' is invalid in details.");
            }

            var entity = ObjectMapper.Map<SalesReturnInfo>(input);
            entity.Status = "PENDING";
            entity.VoucherNumber = await GetVoucherNumber("SR", input.IssueDate);

            var item_ids = input.SalesReturnDetails.Select(i => i.ItemId).ToList();
            var unit_ids = input.SalesReturnDetails.Select(i => i.UnitId).ToList();

            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Select(i => i.Id).ToList();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Select(i => i.Id).ToList();

            for (int i = 0; i < entity.SalesReturnDetails.Count; i++)
            {
                var detail = entity.SalesReturnDetails[i];

                if (!items.Contains(detail.ItemId))
                    throw new UserFriendlyException($"ItemId: '{detail.ItemId}' is invalid at Row: '{i + 1}'.");
                if (!units.Contains(detail.UnitId))
                    throw new UserFriendlyException($"UnitId: '{detail.UnitId}' is invalid at Row: '{i + 1}'.");
                if (!warehouses.Contains(detail.WarehouseId))
                    throw new UserFriendlyException($"WarehouseId: '{detail.WarehouseId}' is invalid at Row: '{i + 1}'.");
            }

            entity.TenantId = AbpSession.TenantId;
            await SalesReturn_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "SalesReturn Created Successfully.";
        }

        private async Task<SalesReturnInfo> Get(long Id)
        {
            var sales_return = await SalesReturn_Repo.GetAllIncluding(i => i.SalesReturnDetails).Where(i => i.Id == Id).FirstOrDefaultAsync();
            if (sales_return != null)
                return sales_return;
            else
                throw new UserFriendlyException($"SalesReturnId: '{Id}' is invalid.");
        }

        public async Task<SalesReturnGetForEditDto> GetForEdit(long Id)
        {
            var sales_return = await Get(Id);
            var customer_coa_level04 = CustomerCOALevel04_Repo.GetAll(this, i => i.Id == sales_return.CustomerCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            await customer_coa_level04.ValueAsync();

            var output = ObjectMapper.Map<SalesReturnGetForEditDto>(sales_return);
            output.CustomerCOALevel04Name = customer_coa_level04?.Value?.Name ?? "";

            var item_ids = sales_return.SalesReturnDetails.Select(i => i.ItemId).ToList();
            var unit_ids = sales_return.SalesReturnDetails.Select(i => i.UnitId).ToList();
            var warehouse_ids = sales_return.SalesReturnDetails.Select(i => i.WarehouseId).ToList();

            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var warehouses = Warehouse_Repo.GetAll(this, i => warehouse_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();

            var dict_items = items.ToDictionary(i => i.Id, i => i.Name);
            var dict_units = units.ToDictionary(i => i.Id, i => i.Name);
            var dict_warehouses = warehouses.ToDictionary(i => i.Id, i => i.Name);

            output.SalesReturnDetails = new();
            foreach (var detail in sales_return.SalesReturnDetails)
            {
                var mapped_detail = ObjectMapper.Map<SalesReturnDetailsGetForEditDto>(detail);
                mapped_detail.ItemName = dict_items.GetValueOrDefault(detail.ItemId);
                mapped_detail.UnitName = dict_units.GetValueOrDefault(detail.UnitId);
                mapped_detail.WarehouseName = dict_warehouses.GetValueOrDefault(detail.WarehouseId);
                output.SalesReturnDetails.Add(mapped_detail);
            }

            return output;
        }

        [AbpAuthorize(PermissionNames.LookUps_SalesReturn_Update)]
        public async Task<string> Update(SalesReturnDto input)
        {
            var old_salesreturn = await Get(input.Id);
            if (old_salesreturn.Status != "PENDING")
                throw new UserFriendlyException($"Only entries with a status of 'PENDING' can be edited. The current status is '{old_salesreturn.Status}'.");

            var warehouseIds = input.SalesReturnDetails.Select(d => d.WarehouseId).Distinct().ToList();
            var warehouses = Warehouse_Repo.GetAll(this, i => warehouseIds.Contains(i.Id)).Select(i => i.Id).ToList();
            foreach (var wid in warehouseIds)
            {
                if (!warehouses.Contains(wid))
                    throw new UserFriendlyException($"WarehouseId: '{wid}' is invalid in details.");
            }

            var entity = ObjectMapper.Map(input, old_salesreturn);
            entity.VoucherNumber = await GetVoucherNumber("SR", input.IssueDate, entity.VoucherNumber.GetVoucherIndex());

            var item_ids = input.SalesReturnDetails.Select(i => i.ItemId).ToList();
            var unit_ids = input.SalesReturnDetails.Select(i => i.UnitId).ToList();

            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Select(i => i.Id).ToList();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Select(i => i.Id).ToList();

            for (int i = 0; i < entity.SalesReturnDetails.Count; i++)
            {
                var detail = entity.SalesReturnDetails[i];

                if (!items.Contains(detail.ItemId))
                    throw new UserFriendlyException($"ItemId: '{detail.ItemId}' is invalid at Row: '{i + 1}'.");
                if (!units.Contains(detail.UnitId))
                    throw new UserFriendlyException($"UnitId: '{detail.UnitId}' is invalid at Row: '{i + 1}'.");
                if (!warehouses.Contains(detail.WarehouseId))
                    throw new UserFriendlyException($"WarehouseId: '{detail.WarehouseId}' is invalid at Row: '{i + 1}'.");
            }

            await SalesReturn_Repo.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "SalesReturn Updated Successfully.";
        }

        [AbpAuthorize(PermissionNames.LookUps_SalesReturn_Delete)]
        public async Task<string> Delete(long Id)
        {
            var sales_return = await SalesReturn_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (sales_return == null)
                throw new UserFriendlyException($"SalesReturnId: '{Id}' is invalid.");
            if (sales_return.Status != "PENDING")
                throw new UserFriendlyException($"Unable to delete SalesReturn: only records with a 'PENDING' status can be deleted.");

            await SalesReturn_Repo.DeleteAsync(sales_return);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "SalesReturn Deleted Successfully.";
        }

        public async override Task<string> ApproveDocument(long Id)
        {
            var document = await GetForEdit(Id);

            var item_ids = document.SalesReturnDetails.Select(i => i.ItemId).ToList();
            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).ToList();
            var customer = CustomerCOALevel04_Repo.GetAll(this, i => i.Id == document.CustomerCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            await customer.ValueAsync();

            var transaction_base = new GeneralLedgerTransactionBaseDto()
            {
                IssueDate = document.IssueDate,
                VoucherNumber = document.VoucherNumber,
                IsAdjustmentEntry = false,
                Remarks = document.Remarks,
                LinkedDocument = Enums.GeneralLedgerLinkedDocument.SalesReturn,
                LinkedDocumentId = document.Id,
                TenantId = AbpSession.TenantId,
            };

            foreach (var detail in document.SalesReturnDetails)
            {
                var item_coa = items.FirstOrDefault(i => i.Id == detail.ItemId)?.SalesCOALevel04Id ?? 0;

                await WarehouseStockLedger_Repo.AddLedgerTransactionAsync(
                    document.IssueDate,
                    document.VoucherNumber,
                    detail.ItemId,
                    detail.WarehouseId,
                    detail.ReturnedQty,
                    0,
                    detail.Rate,
                    detail.ReturnedQty,
                    detail.GrandTotal,
                    document.Remarks,
                    document.Id,
                    Enums.WarehouseStockLedgerLinkedDocument.SalesReturn,
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
                document.TotalAmount,
                document.CustomerCOALevel04Id,
                0);

            CurrentUnitOfWork.SaveChanges();

            return await base.ApproveDocument(Id);
        }

        //public async Task<decimal> GetLatestRate(long ItemId, long CustomerCOALevel04Id, long UnitId)
        //{
        //    var latest_sales_rate = await SalesReturn_Repo
        //        .GetAllIncluding(i => i.SalesReturnDetails)
        //        .Where(i => i.CustomerCOALevel04Id == CustomerCOALevel04Id)
        //        .OrderByDescending(i => i.IssueDate)
        //        .ThenByDescending(i => i.CreationTime)
        //        .SelectMany(i => i.SalesReturnDetails)
        //        .Where(detail => detail.ItemId == ItemId && detail.UnitId == UnitId)
        //        .Select(i => i.Rate)
        //        .FirstOrDefaultAsync();

        //    return latest_sales_rate;
        //}

        public async Task<decimal> GetLatestRate(long ItemId, long CustomerCOALevel04Id, long UnitId)
        {
            var latest_sales_rate = await SalesReturn_Repo
                .GetAllIncluding(sr => sr.SalesReturnDetails)
                .Where(sr => sr.CustomerCOALevel04Id == CustomerCOALevel04Id && sr.SalesReturnDetails.Any(d => d.ItemId == ItemId && d.UnitId == UnitId))
                .SelectMany(sr => sr.SalesReturnDetails.Where(d => d.ItemId == ItemId && d.UnitId == UnitId)
                                    .Select(d => new
                                    {
                                        sr.IssueDate,
                                        sr.CreationTime,
                                        d.Rate
                                    }))
                .OrderByDescending(x => x.IssueDate)
                .ThenByDescending(x => x.CreationTime)
                .Select(x => x.Rate)
                .FirstOrDefaultAsync();

            return latest_sales_rate;
        }

    }
}
