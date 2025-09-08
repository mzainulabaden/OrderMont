using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;
using Abp.UI;
using ERP.Authorization;
using ERP.Modules.InventoryManagement.Item;
using ERP.Modules.InventoryManagement.LookUps;
using ERP.Modules.InventoryManagement.StockLedger;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.InventoryManagement.DemandBook
{
    [AbpAuthorize(PermissionNames.LookUps_DemandBook)]
    public class DemandBookAppService : ApplicationService
    {
        public IRepository<DemandBookInfo, long> DemandBook_Repo { get; set; }
        public IRepository<WarehouseInfo, long> Warehouse_Repo { get; set; }
        public IRepository<ItemInfo, long> Item_Repo { get; set; }
        public IRepository<WarehouseStockLedgerInfo, long> WarehouseStockLedger_Repo { get; set; }

        

        public async Task<PagedResultDto<DemandBookGetAllDto>> GetAll(DemandBookFiltersDto filters)
        {
            var demand_book_query = DemandBook_Repo.GetAll(this).ApplyBaseFilters(filters);
            if (!string.IsNullOrWhiteSpace(filters.WarehouseId))
                demand_book_query = demand_book_query.Where(i => i.WarehouseId == filters.WarehouseId.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.ItemId))
                demand_book_query = demand_book_query.Where(i => i.ItemId == filters.ItemId.TryToLong());

            var demand_books = await demand_book_query.ToPagedListAsync(filters);

            var warehouse_ids = demand_books.Select(i => i.WarehouseId).Distinct().ToList();
            var item_ids = demand_books.Select(i => i.ItemId).Distinct().ToList();

            var total_count = demand_book_query.DeferredCount().FutureValue();
            var warehouses = Warehouse_Repo.GetAll(this, i => warehouse_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            _ = await items.ToListAsync();

            var dict_warehouses = warehouses.ToDictionary(i => i.Id, i => i.Name);
            var dict_items = items.ToDictionary(i => i.Id, i => i.Name);

            var output = new List<DemandBookGetAllDto>();
            foreach (var demand_book in demand_books)
            {
                var dto = ObjectMapper.Map<DemandBookGetAllDto>(demand_book);
                dto.WarehouseName = dict_warehouses.GetValueOrDefault(demand_book.WarehouseId);
                dto.ItemName = dict_items.GetValueOrDefault(demand_book.ItemId);
                output.Add(dto);
            }

            var ledger_query = WarehouseStockLedger_Repo.GetAll(this);
            if (!string.IsNullOrWhiteSpace(filters.WarehouseId))
                ledger_query = ledger_query.Where(i => i.WarehouseId == filters.WarehouseId.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.ItemId))
                ledger_query = ledger_query.Where(i => i.ItemId == filters.ItemId.TryToLong());

            var stock_summaries = await ledger_query
                .GroupBy(i => new { i.ItemId, i.WarehouseId })
                .Select(g => new
                {
                    g.Key.ItemId,
                    g.Key.WarehouseId,
                    TotalDebit = g.Sum(x => x.Debit),
                    TotalCredit = g.Sum(x => x.Credit)
                })
                .ToListAsync();

            var stock_item_ids = stock_summaries.Select(i => i.ItemId).Distinct().ToList();
            var stock_warehouse_ids = stock_summaries.Select(i => i.WarehouseId).Distinct().ToList();

            var items_for_stock = Item_Repo.GetAll(this, i => stock_item_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name, i.ReOrderQty }).Future();
            var warehouses_for_stock = Warehouse_Repo.GetAll(this, i => stock_warehouse_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            _ = await warehouses_for_stock.ToListAsync();

            var dict_items_for_stock = items_for_stock.ToDictionary(i => i.Id);
            var dict_warehouses_for_stock = warehouses_for_stock.ToDictionary(i => i.Id, i => i.Name);

            var ledger_low_stock = new List<DemandBookGetAllDto>();
            foreach (var s in stock_summaries)
            {
                if (!dict_items_for_stock.TryGetValue(s.ItemId, out var item_info))
                    continue;
                var current_stock = s.TotalDebit - s.TotalCredit;
                if (current_stock < item_info.ReOrderQty)
                {
                    ledger_low_stock.Add(new DemandBookGetAllDto
                    {
                        ItemId = s.ItemId,
                        ItemName = item_info.Name,
                        WarehouseId = s.WarehouseId,
                        WarehouseName = dict_warehouses_for_stock.GetValueOrDefault(s.WarehouseId),
                        Qty = item_info.ReOrderQty - current_stock,
                        Name = ""
                    });
                }
            }

            var combined = new List<DemandBookGetAllDto>();
            combined.AddRange(output);
            combined.AddRange(ledger_low_stock);

            return new PagedResultDto<DemandBookGetAllDto>(total_count.Value + ledger_low_stock.Count, combined);
        }

        [AbpAuthorize(PermissionNames.LookUps_DemandBook_Create)]
        public async Task<string> Create(DemandBookDto input)
        {
            var warehouse = Warehouse_Repo.GetAll(this, i => i.Id == input.WarehouseId).DeferredFirstOrDefault().FutureValue();
            var item = Item_Repo.GetAll(this, i => i.Id == input.ItemId).DeferredFirstOrDefault().FutureValue();
            await item.ValueAsync();

            if (warehouse.Value == null)
                throw new UserFriendlyException($"WarehouseId: '{input.WarehouseId}' is invalid.");
            if (item.Value == null)
                throw new UserFriendlyException($"ItemId: '{input.ItemId}' is invalid.");

            var entity = ObjectMapper.Map<DemandBookInfo>(input);
            entity.TenantId = AbpSession.TenantId;
            await DemandBook_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "DemandBook Created Successfully.";
        }

        public async Task<DemandBookInfo> Get(long Id)
        {
            var demand_book = await DemandBook_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (demand_book != null)
                return demand_book;
            else
                throw new UserFriendlyException($"DemandBookId is '{Id}' invalid.");
        }

        public async Task<DemandBookGetAllDto> GetForEdit(long Id)
        {
            var demand_book = await Get(Id);
            var warehouse = Warehouse_Repo.GetAll(this, i => i.Id == demand_book.WarehouseId).DeferredFirstOrDefault().FutureValue();
            var item = Item_Repo.GetAll(this, i => i.Id == demand_book.ItemId).DeferredFirstOrDefault().FutureValue();
            await item.ValueAsync();

            var output = ObjectMapper.Map<DemandBookGetAllDto>(demand_book);
            output.WarehouseName = warehouse?.Value?.Name ?? "";
            output.ItemName = item?.Value?.Name ?? "";
            return output;
        }

        [AbpAuthorize(PermissionNames.LookUps_DemandBook_Edit)]
        public async Task<string> Edit(DemandBookDto input)
        {
            var warehouse = Warehouse_Repo.GetAll(this, i => i.Id == input.WarehouseId).DeferredFirstOrDefault().FutureValue();
            var item = Item_Repo.GetAll(this, i => i.Id == input.ItemId).DeferredFirstOrDefault().FutureValue();
            await item.ValueAsync();

            if (warehouse.Value == null)
                throw new UserFriendlyException($"WarehouseId: '{input.WarehouseId}' is invalid.");
            if (item.Value == null)
                throw new UserFriendlyException($"ItemId: '{input.ItemId}' is invalid.");

            var old_demandbook = await Get(input.Id);
            var entity = ObjectMapper.Map(input, old_demandbook);
            await DemandBook_Repo.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "DemandBook Updated Successfully.";
        }

        [AbpAuthorize(PermissionNames.LookUps_DemandBook_Delete)]
        public async Task<string> Delete(long Id)
        {
            var demand_book = await DemandBook_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (demand_book == null)
                throw new UserFriendlyException($"DemandBookId is '{Id}' invalid.");

            await DemandBook_Repo.DeleteAsync(demand_book);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "DemandBook Deleted Successfully.";
        }
    }
}
