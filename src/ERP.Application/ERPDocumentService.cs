using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ERP;

public class ERPDocumentService<TEntity> : ERPApplicationService where TEntity : ERPDocumentBaseEntity
{
    public IRepository<TEntity, long> MainRepository { get; set; }

    [ApiExplorerSettings(IgnoreApi = true)]
    public virtual async Task<int> GetMaxCount(DateTime IssueDate)
    {
        using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
        {
            var count = await MainRepository
            .GetAll(this, i =>
                i.IssueDate.Month == IssueDate.Month &&
                i.IssueDate.Year == IssueDate.Year  )
            .CountAsync();

            return count + 1;
        }
    }

    public virtual async Task<string> GetVoucherNumber(string Prefix, DateTime IssueDate, int Count = 0)
    {
        int count = Count is 0
         ? await GetMaxCount(IssueDate) : Count;

        var voucher_number = $"{Prefix}-{IssueDate.Year}-{IssueDate.Month}-{count + 1}";
        return voucher_number;
    }

    public virtual async Task<string> ApproveDocument(long Id)
    {
        IsDocumentApprovalPermitted();

        var document = await MainRepository.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
        if (document == null)
            throw new UserFriendlyException($"Could not find {GetName()} with ID: '{Id}'.");
        if (document.Status != "PENDING")
            throw new UserFriendlyException($"Before Approving Document; Status must be 'PENDING'");

        document.Status = "APPROVED";
        await MainRepository.UpdateAsync(document);
        CurrentUnitOfWork.SaveChanges();
        return $"{GetName()} Status 'APPROVED' Successfully.";
    }

    /* -------------------------------------------------------------------------------------------- */

    [ApiExplorerSettings(IgnoreApi = true)]
    public string GetName() => typeof(TEntity).Name.Replace("Info", "");

    [ApiExplorerSettings(IgnoreApi = true)]
    public void IsDocumentApprovalPermitted()
    {
        if (CheckForPermissions && !PermissionChecker.IsGranted(APPROVE_DOCUMENT_PERMISSION))
            throw new UserFriendlyException($"Don't have permission to APPROVE DOCUMENT '{GetName()}'.");
    }

    public virtual string APPROVE_DOCUMENT_PERMISSION =>
        string.Format(ERPConsts.APPROVE_DOCUMENT_PERMISSION, GetName());

    public bool CheckForPermissions { get; set; }
}
