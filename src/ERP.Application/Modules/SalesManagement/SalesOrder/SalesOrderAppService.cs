using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Authorization.Roles;
using ERP.Authorization.Users;
using ERP.Modules.Finance.ChartOfAccount.COALevel04;
using ERP.Modules.Finance.LookUps;
using ERP.Modules.HumanResource.EmployeeManagement;
using ERP.Modules.InventoryManagement.Item;
using ERP.Modules.InventoryManagement.LookUps;
using ERP.Modules.SalesManagement.SalesInvoice;
using ERP.Modules.SalesManagement.SalesOrder.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.SalesManagement.SalesOrder
{

    [AbpAuthorize(PermissionNames.LookUps_SalesOrder)]
    public class SalesOrderAppService : ERPDocumentService<SalesOrderInfo>
    {
        public IRepository<SalesOrderInfo, long> SalesOrder_Repo { get; set; }
        public IRepository<PaymentModeInfo, long> PaymentMode_Repo { get; set; }
        public IRepository<COALevel04Info, long> CustomerCOALevel04_Repo { get; set; }
        public IRepository<ItemInfo, long> Item_Repo { get; set; }
        public IRepository<UnitInfo, long> Unit_Repo { get; set; }
        public IRepository<WarehouseInfo, long> Warehouse_Repo { get; set; }
        public IRepository<EmployeeInfo, long> Employee_Repo { get; set; }
        public IRepository<SalesInvoiceInfo, long> SalesInvoice_Repo { get; set; }

        public async Task<PagedResultDto<SalesOrderGetAllDto>> GetAll(SalesOrderFiltersDto filters)
        {
            var sales_order_query = SalesOrder_Repo.GetAll(this).ApplyDocumentFilters(filters);
            if (!string.IsNullOrWhiteSpace(filters.PaymentModeId))
                sales_order_query = sales_order_query.Where(i => i.PaymentModeId == filters.PaymentModeId.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.CustomerCOALevel04Id))
                sales_order_query = sales_order_query.Where(i => i.CustomerCOALevel04Id == filters.CustomerCOALevel04Id.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.EmployeeId))
                sales_order_query = sales_order_query.Where(i => i.EmployeeId == filters.EmployeeId.TryToLong());
            // WarehouseId filter now applies to details
            var sales_orders = await sales_order_query.Include(i => i.SalesOrderDetails).ToPagedListAsync(filters);
            if (!string.IsNullOrWhiteSpace(filters.WarehouseId))
            {
                var warehouseId = filters.WarehouseId.TryToLong();
                sales_orders = sales_orders.Where(so => so.SalesOrderDetails.Any(d => d.WarehouseId == warehouseId)).ToList();
            }

            var payment_mode_ids = sales_orders.Select(i => i.PaymentModeId).ToList();
            var customer_coa_level04_ids = sales_orders.Select(i => i.CustomerCOALevel04Id).ToList();
            var warehouse_ids = sales_orders.SelectMany(o => o.SalesOrderDetails).Select(d => d.WarehouseId).Distinct().ToList();
            var creator_user_ids = sales_orders.Where(i => i.CreatorUserId.HasValue).Select(i => i.CreatorUserId.Value).ToList();
            var employee_ids = sales_orders.Select(i => i.EmployeeId).ToList();
            var item_ids = sales_orders.SelectMany(o => o.SalesOrderDetails).Select(d => d.ItemId).Distinct().ToList();
            var unit_ids = sales_orders.SelectMany(o => o.SalesOrderDetails).Select(d => d.UnitId).Distinct().ToList();

            var total_count = sales_orders.Count;
            var payment_modes = PaymentMode_Repo.GetAll().Where(i => payment_mode_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var customer_coa_level04s = CustomerCOALevel04_Repo.GetAll().Where(i => customer_coa_level04_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var warehouses = Warehouse_Repo.GetAll().Where(i => warehouse_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var users = User_Repo.GetAll().Where(i => creator_user_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var employees = User_Repo.GetAll().Where(i => employee_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var items = Item_Repo.GetAll().Where(i => item_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var units = Unit_Repo.GetAll().Where(i => unit_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();

            var dict_payment_modes = payment_modes.ToDictionary(i => i.Id);
            var dict_customer_coa_level04s = customer_coa_level04s.ToDictionary(i => i.Id);
            var dict_warehouses = warehouses.ToDictionary(i => i.Id);
            var dict_users = users.ToDictionary(i => i.Id);
            var dict_employees = employees.ToDictionary(i => i.Id);
            var dict_items = items.ToDictionary(i => i.Id);
            var dict_units = units.ToDictionary(i => i.Id);

            var output = new List<SalesOrderGetAllDto>();
            foreach (var sales_order in sales_orders)
            {
                dict_payment_modes.TryGetValue(sales_order.PaymentModeId, out var payment_mode);
                dict_customer_coa_level04s.TryGetValue(sales_order.CustomerCOALevel04Id, out var customer_coa_level04);
                dict_users.TryGetValue(sales_order.CreatorUserId ?? 0, out var creator);
                dict_employees.TryGetValue(sales_order.EmployeeId ?? 0, out var employee);

                var dto = ObjectMapper.Map<SalesOrderGetAllDto>(sales_order);
                dto.PaymentModeName = payment_mode?.Name ?? "";
                dto.CustomerCOALevel04Name = customer_coa_level04?.Name ?? "";
                dto.CreatorName = creator?.Name ?? "";
                dto.CreatorUserId = sales_order.CreatorUserId ?? 0;
                dto.EmployeeName = employee?.Name ?? "";

                dto.SalesOrderDetails = new List<SalesOrderDetailsGetAllDto>();
                foreach (var detail in sales_order.SalesOrderDetails)
                {
                    dict_items.TryGetValue(detail.ItemId, out var item);
                    dict_units.TryGetValue(detail.UnitId, out var unit);
                    dict_warehouses.TryGetValue(detail.WarehouseId, out var warehouse);

                    var detailDto = ObjectMapper.Map<SalesOrderDetailsGetAllDto>(detail);
                    detailDto.ItemName = item?.Name ?? "";
                    detailDto.UnitName = unit?.Name ?? "";
                    detailDto.WarehouseName = warehouse?.Name ?? "";
                    dto.SalesOrderDetails.Add(detailDto);
                }

                output.Add(dto);
            }

            return new PagedResultDto<SalesOrderGetAllDto>(total_count, output);
        }

        public async Task<PagedResultDto<SalesOrderGetAllDto>> GetAllSalesOrders(SalesOrderFiltersDto filters)
        {
            var sales_order_query = SalesOrder_Repo.GetAll(this).ApplyDocumentFilters(filters);
            if (!string.IsNullOrWhiteSpace(filters.PaymentModeId))
                sales_order_query = sales_order_query.Where(i => i.PaymentModeId == filters.PaymentModeId.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.CustomerCOALevel04Id))
                sales_order_query = sales_order_query.Where(i => i.CustomerCOALevel04Id == filters.CustomerCOALevel04Id.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.EmployeeId))
                sales_order_query = sales_order_query.Where(i => i.EmployeeId == filters.EmployeeId.TryToLong());

            var sales_orders = await sales_order_query.Include(i => i.SalesOrderDetails).ToPagedListAsync(filters);
            if (!string.IsNullOrWhiteSpace(filters.WarehouseId))
            {
                var warehouseId = filters.WarehouseId.TryToLong();
                sales_orders = sales_orders.Where(so => so.SalesOrderDetails.Any(d => d.WarehouseId == warehouseId)).ToList();
            }
            var allDetailIds = sales_orders.SelectMany(o => o.SalesOrderDetails).Select(d => d.Id).ToList();
            var allInvoiceDetails = SalesInvoice_Repo.GetAll()
                .SelectMany(i => i.SalesInvoiceDetails)
                .Where(d => allDetailIds.Contains(d.SalesOrderDetailId))
                .ToList();

            var invoiceQtyByDetailId = allInvoiceDetails
                .GroupBy(d => d.SalesOrderDetailId)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.InvoiceQty));

            var payment_mode_ids = sales_orders.Select(i => i.PaymentModeId).ToList();
            var customer_coa_level04_ids = sales_orders.Select(i => i.CustomerCOALevel04Id).ToList();
            var warehouse_ids = sales_orders.SelectMany(o => o.SalesOrderDetails).Select(d => d.WarehouseId).Distinct().ToList();
            var creator_user_ids = sales_orders.Where(i => i.CreatorUserId.HasValue).Select(i => i.CreatorUserId.Value).ToList();
            var employee_ids = sales_orders.Select(i => i.EmployeeId).ToList();
            var item_ids = sales_orders.SelectMany(o => o.SalesOrderDetails).Select(d => d.ItemId).Distinct().ToList();
            var unit_ids = sales_orders.SelectMany(o => o.SalesOrderDetails).Select(d => d.UnitId).Distinct().ToList();

            var total_count = sales_orders.Count;
            var payment_modes = PaymentMode_Repo.GetAll().Where(i => payment_mode_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var customer_coa_level04s = CustomerCOALevel04_Repo.GetAll().Where(i => customer_coa_level04_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var warehouses = Warehouse_Repo.GetAll().Where(i => warehouse_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var users = User_Repo.GetAll().Where(i => creator_user_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var employees = User_Repo.GetAll().Where(i => employee_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var items = Item_Repo.GetAll().Where(i => item_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var units = Unit_Repo.GetAll().Where(i => unit_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();

            var dict_payment_modes = payment_modes.ToDictionary(i => i.Id);
            var dict_customer_coa_level04s = customer_coa_level04s.ToDictionary(i => i.Id);
            var dict_warehouses = warehouses.ToDictionary(i => i.Id);
            var dict_users = users.ToDictionary(i => i.Id);
            var dict_employees = employees.ToDictionary(i => i.Id);
            var dict_items = items.ToDictionary(i => i.Id);
            var dict_units = units.ToDictionary(i => i.Id);

            var output = new List<SalesOrderGetAllDto>();
            foreach (var sales_order in sales_orders)
            {
                dict_payment_modes.TryGetValue(sales_order.PaymentModeId, out var payment_mode);
                dict_customer_coa_level04s.TryGetValue(sales_order.CustomerCOALevel04Id, out var customer_coa_level04);
                dict_users.TryGetValue(sales_order.CreatorUserId ?? 0, out var creator);
                dict_employees.TryGetValue(sales_order.EmployeeId ?? 0, out var employee);

                var dto = ObjectMapper.Map<SalesOrderGetAllDto>(sales_order);
                dto.PaymentModeName = payment_mode?.Name ?? "";
                dto.CustomerCOALevel04Name = customer_coa_level04?.Name ?? "";
                dto.CreatorName = creator?.Name ?? "";
                dto.CreatorUserId = sales_order.CreatorUserId ?? 0;
                dto.EmployeeName = employee?.Name ?? "";

                dto.SalesOrderDetails = new List<SalesOrderDetailsGetAllDto>();
                foreach (var detail in sales_order.SalesOrderDetails)
                {
                    var invoicedQty = invoiceQtyByDetailId.ContainsKey(detail.Id) ? invoiceQtyByDetailId[detail.Id] : 0;
                    var remainingQty = detail.OrderedQty - invoicedQty;
                    if (remainingQty > 0)
                    {
                        dict_items.TryGetValue(detail.ItemId, out var item);
                        dict_units.TryGetValue(detail.UnitId, out var unit);
                        dict_warehouses.TryGetValue(detail.WarehouseId, out var warehouse);

                        var detailDto = ObjectMapper.Map<SalesOrderDetailsGetAllDto>(detail);
                        detailDto.ItemName = item?.Name ?? "";
                        detailDto.UnitName = unit?.Name ?? "";
                        detailDto.WarehouseName = warehouse?.Name ?? "";
                        detailDto.InvoicedQty = invoicedQty;
                        detailDto.RemainingQty = remainingQty;
                        
                        // Get the last sales rate for this item, customer, and unit combination
                        var lastSaleRate = await GetLatestRate(detail.ItemId, sales_order.CustomerCOALevel04Id, detail.UnitId);
                        detailDto.LastSaleRate = lastSaleRate;
                        
                        dto.SalesOrderDetails.Add(detailDto);
                    }
                }
                if (dto.SalesOrderDetails.Count > 0) 
                    output.Add(dto);
            }

            return new PagedResultDto<SalesOrderGetAllDto>(output.Count, output);
        }

        [AbpAuthorize(PermissionNames.LookUps_SalesOrder_Create)]
        public async Task<string> Create(SalesOrderDto input)
        {
            var payment_mode = PaymentMode_Repo.GetAll().Where(i => i.Id == input.PaymentModeId).DeferredFirstOrDefault().FutureValue();
            await payment_mode.ValueAsync();
            if (payment_mode.Value == null)
                throw new UserFriendlyException($"PaymentModeId: '{input.PaymentModeId}' is invalid.");
            var item_ids = input.SalesOrderDetails.Select(i => i.ItemId).ToList();
            var unit_ids = input.SalesOrderDetails.Select(i => i.UnitId).ToList();
            var warehouse_ids = input.SalesOrderDetails.Select(i => i.WarehouseId).ToList();
            var items = Item_Repo.GetAll().Where(i => item_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var units = Unit_Repo.GetAll().Where(i => unit_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var warehouses = Warehouse_Repo.GetAll().Where(i => warehouse_ids.Contains(i.Id)).Select(i => i.Id).Future();
            _ = await units.ToListAsync();
            _ = await warehouses.ToListAsync();
            for (int i = 0; i < input.SalesOrderDetails.Count; i++)
            {
                var detail = input.SalesOrderDetails[i];
                if (!items.Contains(detail.ItemId))
                    throw new UserFriendlyException($"ItemId: '{detail.ItemId}' is invalid at Row: '{i + 1}'.");
                if (!units.Contains(detail.UnitId))
                    throw new UserFriendlyException($"UnitId: '{detail.UnitId}' is invalid at Row: '{i + 1}'.");
                if (!warehouses.Contains(detail.WarehouseId))
                    throw new UserFriendlyException($"WarehouseId: '{detail.WarehouseId}' is invalid at Row: '{i + 1}'.");
            }
            var entity = ObjectMapper.Map<SalesOrderInfo>(input);
            entity.Status = "PENDING";
            entity.VoucherNumber = await GetVoucherNumber("SO", input.IssueDate);
            entity.TenantId = AbpSession.TenantId;
            await SalesOrder_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "SalesOrder Created Successfully.";
        }

        private async Task<SalesOrderInfo> GetById(long Id)
        {
            var sales_order = await SalesOrder_Repo.GetAllIncluding(i => i.SalesOrderDetails).Where(i => i.Id == Id).FirstOrDefaultAsync();
            if (sales_order != null)
                return sales_order;
            else
                throw new UserFriendlyException($"SalesOrderId: '{Id}' is invalid.");
        }

        public async Task<SalesOrderGetForEditDto> Get(long Id)
        {
            var sales_order = await SalesOrder_Repo.GetAllIncluding(i => i.SalesOrderDetails).Where(i => i.Id == Id).FirstOrDefaultAsync();
            if (sales_order == null)
                throw new UserFriendlyException($"SalesOrderId: '{Id}' is invalid.");
            var payment_mode = PaymentMode_Repo.GetAll().Where(i => i.Id == sales_order.PaymentModeId).DeferredFirstOrDefault().FutureValue();
            var customer_coa_level04 = CustomerCOALevel04_Repo.GetAll().Where(i => i.Id == sales_order.CustomerCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            var creator = sales_order.CreatorUserId.HasValue
                ? User_Repo.GetAll().Where(i => i.Id == sales_order.CreatorUserId.Value).DeferredFirstOrDefault().FutureValue()
                : null;
            var employee = User_Repo.GetAll().Where(i => i.Id == sales_order.EmployeeId).DeferredFirstOrDefault().FutureValue();
            await payment_mode.ValueAsync();
            var output = ObjectMapper.Map<SalesOrderGetForEditDto>(sales_order);
            output.PaymentModeName = payment_mode?.Value?.Name ?? "";
            output.CustomerCOALevel04Name = customer_coa_level04?.Value?.Name ?? "";
            output.CreatorName = creator?.Value?.Name ?? "";
            output.CreatorUserId = sales_order.CreatorUserId ?? 0;
            output.EmployeeName = employee?.Value?.Name ?? "";
            var item_ids = sales_order.SalesOrderDetails.Select(i => i.ItemId).ToList();
            var unit_ids = sales_order.SalesOrderDetails.Select(i => i.UnitId).ToList();
            var warehouse_ids = sales_order.SalesOrderDetails.Select(i => i.WarehouseId).ToList();
            var items = Item_Repo.GetAll().Where(i => item_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var units = Unit_Repo.GetAll().Where(i => unit_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var warehouses = Warehouse_Repo.GetAll().Where(i => warehouse_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var dict_items = items.ToDictionary(i => i.Id, i => i.Name);
            var dict_units = units.ToDictionary(i => i.Id, i => i.Name);
            var dict_warehouses = warehouses.ToDictionary(i => i.Id, i => i.Name);
            output.SalesOrderDetails = new();
            foreach (var detail in sales_order.SalesOrderDetails)
            {
                var mapped_detail = ObjectMapper.Map<SalesOrderDetailsGetForEditDto>(detail);
                mapped_detail.ItemName = dict_items.GetValueOrDefault(detail.ItemId);
                mapped_detail.UnitName = dict_units.GetValueOrDefault(detail.UnitId);
                mapped_detail.WarehouseName = dict_warehouses.GetValueOrDefault(detail.WarehouseId);
                output.SalesOrderDetails.Add(mapped_detail);
            }
            return output;
        }

        [AbpAuthorize(PermissionNames.LookUps_SalesOrder_Update)]
        public async Task<string> Update(SalesOrderDto input)
        {
            var old_salesorder = await GetById(input.Id);
            if (old_salesorder.Status != "PENDING")
                throw new UserFriendlyException($"Only entries with a status of 'PENDING' can be edited. The current status is '{old_salesorder.Status}'.");
            var payment_mode = PaymentMode_Repo.GetAll().Where(i => i.Id == input.PaymentModeId).DeferredFirstOrDefault().FutureValue();
            await payment_mode.ValueAsync();
            if (payment_mode.Value == null)
                throw new UserFriendlyException($"PaymentModeId: '{input.PaymentModeId}' is invalid.");
            var item_ids = input.SalesOrderDetails.Select(i => i.ItemId).ToList();
            var unit_ids = input.SalesOrderDetails.Select(i => i.UnitId).ToList();
            var warehouse_ids = input.SalesOrderDetails.Select(i => i.WarehouseId).ToList();
            var items = Item_Repo.GetAll().Where(i => item_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var units = Unit_Repo.GetAll().Where(i => unit_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var warehouses = Warehouse_Repo.GetAll().Where(i => warehouse_ids.Contains(i.Id)).Select(i => i.Id).Future();
            _ = await units.ToListAsync();
            _ = await warehouses.ToListAsync();
            for (int i = 0; i < input.SalesOrderDetails.Count; i++)
            {
                var detail = input.SalesOrderDetails[i];
                if (!items.Contains(detail.ItemId))
                    throw new UserFriendlyException($"ItemId: '{detail.ItemId}' is invalid at Row: '{i + 1}'.");
                if (!units.Contains(detail.UnitId))
                    throw new UserFriendlyException($"UnitId: '{detail.UnitId}' is invalid at Row: '{i + 1}'.");
                if (!warehouses.Contains(detail.WarehouseId))
                    throw new UserFriendlyException($"WarehouseId: '{detail.WarehouseId}' is invalid at Row: '{i + 1}'.");
            }
            var entity = ObjectMapper.Map(input, old_salesorder);
            entity.VoucherNumber = await GetVoucherNumber("SO", input.IssueDate, entity.VoucherNumber.GetVoucherIndex());
            await SalesOrder_Repo.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "SalesOrder Updated Successfully.";
        }

        [AbpAuthorize(PermissionNames.LookUps_SalesOrder_Delete)]
        public async Task<string> Delete(long Id)
        {
            var sales_order = await SalesOrder_Repo.GetAll().Where(i => i.Id == Id).FirstOrDefaultAsync();
            if (sales_order == null)
                throw new UserFriendlyException($"SalesOrderId: '{Id}' is invalid.");
            if (sales_order.Status != "PENDING")
                throw new UserFriendlyException($"Unable to delete SalesOrder: only records with a 'PENDING' status can be deleted.");

            await SalesOrder_Repo.DeleteAsync(sales_order);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "SalesOrder Deleted Successfully.";
        }

        public async Task<ItemPricingInfoDto> GetItemPricingInfo(long itemId, long unitId)
        {
            var currentUser = await GetCurrentUserAsync();

            var itemDetails = await Item_Repo.GetAll().Where(i => i.Id == itemId)
                .SelectMany(i => i.ItemDetails)
                .Where(d => d.UnitId == unitId)
                .FirstOrDefaultAsync();
            if (itemDetails == null)
                throw new UserFriendlyException($"No pricing information found for ItemId: '{itemId}' and UnitId: '{unitId}'.");

            var lastSaleRate = await SalesOrder_Repo.GetAll().SelectMany(so => so.SalesOrderDetails)
                .Where(d => d.ItemId == itemId && d.UnitId == unitId)
                .Select(d => d.LastSaleRate)
                .FirstOrDefaultAsync();

            var perBagPrice = currentUser.RoleNames.Contains(StaticRoleNames.Roles.SaleMan) ? 0 : itemDetails.PerBagPrice;

            return new ItemPricingInfoDto
            {
                Rate = itemDetails.UnitPrice,
                LastSaleRate = lastSaleRate,
                MinPrice = itemDetails.MinSalePrice,
                MaxPrice = itemDetails.MaxSalePrice,
                MinStockLevel = itemDetails.MinStockLevel,
                PerBagPrice = perBagPrice,
            };
        }

        public async Task<decimal> GetLatestRate(long ItemId, long CustomerCOALevel04Id, long UnitId)
        {
            var latest_sales_rate = await SalesOrder_Repo
                .GetAllIncluding(i => i.SalesOrderDetails)
                .Where(i => i.CustomerCOALevel04Id == CustomerCOALevel04Id)
                .OrderByDescending(i => i.IssueDate)
                .ThenByDescending(i => i.CreationTime)
                .SelectMany(i => i.SalesOrderDetails)
                .Where(detail => detail.ItemId == ItemId && detail.UnitId == UnitId)
                .Select(i => i.Rate)
                .FirstOrDefaultAsync();

            return latest_sales_rate;
        }
    }
}
