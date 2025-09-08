using Abp.Application.Services;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using ERP.Enums;
using ERP.Extensions.Common;
using ERP.Generics;
using ERP.Modules.Finance.GeneralLedger;
using ERP.Modules.InventoryManagement.StockLedger;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ERP;

public static class IQueryableExtensions
{
    public static IQueryable<TEntity> GetAll<TEntity>(this IRepository<TEntity, long> repository, IEnumerable<long> Ids) where TEntity : ERPProjBaseEntity
    {
        return repository.GetAll().Where(i => Ids.Contains(i.Id));
    }

    public static IQueryable<TEntity> GetAll<TEntity>(this IRepository<TEntity, long> repository, IEnumerable<long> Ids, Expression<Func<TEntity, bool>> predicate = null) where TEntity : ERPProjBaseEntity
    {
        var output = repository.GetAll().Where(i => Ids.Contains(i.Id));
        if (predicate != null)
            output = output.Where(predicate);
        return output;
    }

    public static IQueryable<TEntity> GetAll<TEntity>(this IRepository<TEntity, long> repository, ApplicationService service, Expression<Func<TEntity, bool>> predicate = null) where TEntity : ERPProjBaseEntity
    {
        var output = repository.GetAll().Where(i => i.TenantId == service.AbpSession.TenantId);
        if (predicate != null)
            output = output.Where(predicate);
        return output;
    }

    public static IQueryable<TEntity> Where<TEntity>(this IRepository<TEntity, long> repository, ApplicationService service, Expression<Func<TEntity, bool>> predicate = null) where TEntity : ERPProjBaseEntity
    {
        var output = repository.GetAll().Where(i => i.TenantId == service.AbpSession.TenantId);
        if (predicate != null)
            output = output.Where(predicate);
        return output;
    }

    public static async Task UpdateRangeAsync<TEntity>(this IRepository<TEntity, long> repository, List<TEntity> entities) where TEntity : Entity<long>
    {
        foreach (var entity in entities)
        {
            await repository.UpdateAsync(entity);
        }
    }

    public static async Task DeleteRangeAsync<TEntity>(this IRepository<TEntity, long> repository, List<TEntity> entities) where TEntity : Entity<long>
    {
        foreach (var entity in entities)
        {
            await repository.DeleteAsync(entity);
        }
    }

    public static async Task DeleteRangeAsync<TEntity>(this IRepository<TEntity, long> repository, List<long> ids) where TEntity : Entity<long>
    {
        foreach (var id in ids)
        {
            await repository.DeleteAsync(id);
        }
    }

    public static async Task<List<T>> ToPagedListAsync<T>(this IQueryable<T> query, BaseFiltersDto filters) where T : class
    {
        query = query
            .OrderByDescending(x => EF.Property<DateTime>(x, "CreationTime"))
            .Skip(filters.SkipCount)
            .Take(filters.MaxResultCount);

        return await query.ToListAsync();
    }

    public static IQueryable<T> ApplyBaseFilters<T>(this IQueryable<T> query, BaseFiltersDto filters)
    {
        if (!string.IsNullOrWhiteSpace(filters.Id))
            query = query.Where(x => EF.Property<long>(x, "Id") == filters.Id.TryToLong());

        return query;
    }

    public static IQueryable<T> ApplyDocumentFilters<T>(this IQueryable<T> query, BaseDocumentFiltersDto filters) where T : class
    {
        query = query.ApplyBaseFilters(filters);

        if (filters.IssueDate != null)
            query = query.Where(x => EF.Property<DateTime>(x, "IssueDate") == filters.IssueDate);
        if (!string.IsNullOrWhiteSpace(filters.VoucherNumber))
            query = query.Where(x => EF.Property<string>(x, "VoucherNumber").Contains(filters.VoucherNumber));
        if (!string.IsNullOrWhiteSpace(filters.Status))
            query = query.Where(x => EF.Property<string>(x, "Status").Contains(filters.Status));

        return query;
    }

    public static async Task AddLedgerTransactionAsync(this IRepository<GeneralLedgerInfo, long> repository, GeneralLedgerTransactionBaseDto transaction_base, decimal credit, decimal debit, long chart_of_account_id, long employee_id)
    {
        if ((credit + debit) != 0)
        {
            var ledger_transaction = new GeneralLedgerInfo
            {
                IssueDate = transaction_base.IssueDate,
                VoucherNumber = transaction_base.VoucherNumber,
                Credit = credit,
                Debit = debit,
                IsAdjustmentEntry = transaction_base.IsAdjustmentEntry,
                ChartOfAccountId = chart_of_account_id,
                EmployeeId = employee_id,
                LinkedDocumentId = transaction_base.LinkedDocumentId,
                LinkedDocument = transaction_base.LinkedDocument,
                Status = "PENDING",
                Remarks = transaction_base.Remarks,
                TenantId = transaction_base.TenantId
            };

            await repository.InsertAsync(ledger_transaction);
        }
    }

    public static async Task AddLedgerTransactionAsync(this IRepository<GeneralLedgerInfo, long> repository, GeneralLedgerTransactionBaseDto transaction_base, decimal credit, decimal debit, long chart_of_account_id, long employee_id, long reference_document_id, string reference_voucher_number, GeneralLedgerLinkedDocument? reference_document)
    {
        if ((credit + debit) != 0)
        {
            var ledgerTransaction = new GeneralLedgerInfo
            {
                VoucherNumber = transaction_base.VoucherNumber,
                IssueDate = transaction_base.IssueDate,
                Credit = credit,
                Debit = debit,
                IsAdjustmentEntry = transaction_base.IsAdjustmentEntry,
                ChartOfAccountId = chart_of_account_id,
                EmployeeId = employee_id,
                LinkedDocumentId = transaction_base.LinkedDocumentId,
                LinkedDocument = transaction_base.LinkedDocument,
                ReferenceDocumentId = reference_document_id,
                ReferenceVoucherNumber = reference_voucher_number,
                ReferenceDocument = reference_document,
                Remarks = transaction_base.Remarks,
                Status = "PENDING",
                TenantId = transaction_base.TenantId
            };

            await repository.InsertAsync(ledgerTransaction);
        }
    }

    public static async Task AddLedgerTransactionAsync(this IRepository<WarehouseStockLedgerInfo, long> repository, DateTime issue_date, string voucher_number, long item_id, long warehouse_id, decimal credit, decimal debit, decimal rate, decimal actual_qty, decimal TotalAmount, string remarks, long document_id, WarehouseStockLedgerLinkedDocument warehouse_stock_ledger_linked_document, int? tenant_id)
    {
        var ledger_entry = new WarehouseStockLedgerInfo
        {
            IssueDate = issue_date,
            VoucherNumber = voucher_number,
            ItemId = item_id,
            Credit = credit,
            Debit = debit,
            Rate = rate,
            ActualQty = actual_qty,
            TotalAmount = TotalAmount,
            Remarks = remarks,
            WarehouseId = warehouse_id,
            DocumentId = document_id,
            WarehouseStockLedgerLinkedDocument = warehouse_stock_ledger_linked_document,
            TenantId = tenant_id,
        };

        await repository.InsertAsync(ledger_entry);
    }

    public static async Task AddLedgerTransactionAsync(this IRepository<WarehouseStockLedgerInfo, long> repository, DateTime issue_date, string voucher_number, long item_id, long warehouse_id, decimal credit, decimal debit, decimal rate, decimal actual_qty, decimal TotalAmount, long? coa_level04_id, string remarks, long document_id, WarehouseStockLedgerLinkedDocument warehouse_stock_ledger_linked_document, int? tenant_id)
    {
        var ledger_entry = new WarehouseStockLedgerInfo
        {
            IssueDate = issue_date,
            VoucherNumber = voucher_number,
            ItemId = item_id,
            Credit = credit,
            Debit = debit,
            Rate = rate,
            ActualQty = actual_qty,
            TotalAmount = TotalAmount,
            COALevel04Id = coa_level04_id,
            Remarks = remarks,
            WarehouseId = warehouse_id,
            DocumentId = document_id,
            WarehouseStockLedgerLinkedDocument = warehouse_stock_ledger_linked_document,
            TenantId = tenant_id,
        };

        await repository.InsertAsync(ledger_entry);
    }
}
