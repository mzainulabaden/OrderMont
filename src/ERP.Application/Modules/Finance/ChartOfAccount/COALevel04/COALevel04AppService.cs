using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Modules.Finance.ChartOfAccount.COALevel03;
using ERP.Modules.Finance.LookUps;
using ERP.Modules.Finance.GeneralLedger;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;
using System;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel04
{
    [AbpAuthorize(PermissionNames.LookUps_COALevel04)]
    public class COALevel04AppService : ApplicationService
    {
        public IRepository<COALevel04Info, long> COALevel04_Repo { get; set; }
        public IRepository<COALevel03Info, long> COALevel03_Repo { get; set; }
        public IRepository<AccountTypeInfo, long> AccountType_Repo { get; set; }
        public IRepository<CurrencyInfo, long> Currency_Repo { get; set; }
        public IRepository<LinkWithInfo, long> LinkWith_Repo { get; set; }
        public IRepository<GeneralLedgerInfo, long> GeneralLedger_Repo { get; set; }

        public async Task<PagedResultDto<COALevel04GetAllDto>> GetAll(COALevel04FiltersDto filters)
        {
            var coa_level04_query = COALevel04_Repo.GetAll(this).Where(i => i.IsDeleted == false).ApplyBaseFilters(filters);
            if (!string.IsNullOrWhiteSpace(filters.COALevel03Id))
                coa_level04_query = coa_level04_query.Where(i => i.COALevel03Id == filters.COALevel03Id.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.AccountTypeId))
                coa_level04_query = coa_level04_query.Where(i => i.AccountTypeId == filters.AccountTypeId.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.CurrencyId))
                coa_level04_query = coa_level04_query.Where(i => i.CurrencyId == filters.CurrencyId.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.LinkWithId))
                coa_level04_query = coa_level04_query.Where(i => i.LinkWithId == filters.LinkWithId.TryToLong());
            if (filters.NatureOfAccounts?.Any() ?? default)
                coa_level04_query = coa_level04_query.Where(i => filters.NatureOfAccounts.Contains(i.NatureOfAccount));
            var coa_level04s = await coa_level04_query.ToPagedListAsync(filters);

            var coa_level03_ids = coa_level04s.Select(i => i.COALevel03Id).ToList();
            var account_type_ids = coa_level04s.Select(i => i.AccountTypeId).ToList();
            var currency_ids = coa_level04s.Select(i => i.CurrencyId).ToList();
            var link_with_ids = coa_level04s.Select(i => i.LinkWithId).ToList();

            var total_count = coa_level04_query.DeferredCount().FutureValue();
            var coa_level03s = COALevel03_Repo.GetAll(this, i => coa_level03_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name, i.SerialNumber }).Future();
            var account_types = AccountType_Repo.GetAll(this, i => account_type_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            var currencies = Currency_Repo.GetAll(this, i => currency_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            var link_withs = LinkWith_Repo.GetAll(this, i => link_with_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            await link_withs.ToListAsync();

            var dict_coa_level03s = coa_level03s.ToDictionary(i => i.Id);
            var dict_account_types = account_types.ToDictionary(i => i.Id);
            var dict_currencies = currencies.ToDictionary(i => i.Id);
            var dict_link_withs = link_withs.ToDictionary(i => i.Id);

            var output = new List<COALevel04GetAllDto>();
            foreach (var coa_level04 in coa_level04s)
            {
                dict_coa_level03s.TryGetValue(coa_level04.COALevel03Id, out var coa_level03);
                dict_account_types.TryGetValue(coa_level04.AccountTypeId, out var account_type);
                dict_currencies.TryGetValue(coa_level04.CurrencyId, out var currency);
                dict_link_withs.TryGetValue(coa_level04.LinkWithId, out var link_with);

                var dto = ObjectMapper.Map<COALevel04GetAllDto>(coa_level04);
                dto.COALevel03Name = coa_level03?.Name ?? "";
                dto.COALevel03SerialNumber = coa_level03?.SerialNumber ?? "";
                dto.AccountTypeName = account_type?.Name ?? "";
                dto.CurrencyName = currency?.Name ?? "";
                dto.LinkWithName = link_with?.Name ?? "";
                output.Add(dto);
            }

            return new PagedResultDto<COALevel04GetAllDto>(total_count.Value, output);
        }

        [AbpAuthorize(PermissionNames.LookUps_COALevel04_Create)]
        public async Task<string> Create(COALevel04Dto input)
        {
            var entity = ObjectMapper.Map<COALevel04Info>(input);
            entity.TenantId = AbpSession.TenantId;
            await COALevel04_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            if (entity.OpeningBalance.HasValue && entity.OpeningBalance.Value > 0)
            {
                var ledger = new GeneralLedgerInfo
                {
                    IssueDate = DateTime.Now,
                    VoucherNumber = $"OB-{DateTime.Now:yyyyMMddHHmmss}",
                    ChartOfAccountId = entity.Id,
                    Credit = entity.OpeningBalance ?? 0,
                    Debit = 0,
                    IsAdjustmentEntry = false,
                    Status = "PENDING",
                    EmployeeId = 0,
                    LinkedDocumentId = 0,
                    LinkedDocument = 0,
                    ReferenceDocumentId = 0,
                    ReferenceVoucherNumber = null,
                    ReferenceDocument = null,
                    Remarks = "Opening Balance",
                    TenantId = AbpSession.TenantId
                };

                await GeneralLedger_Repo.InsertAsync(ledger);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            return "COALevel04 Created Successfully.";
        }

        private async Task<COALevel04Info> GetById(long Id)
        {
            var coa_level04 = await COALevel04_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (coa_level04 != null)
                return coa_level04;
            else
                throw new UserFriendlyException($"COALevel04Id: '{Id}' is invalid.");
        }

        public async Task<COALevel04GetAllDto> Get(long Id)
        {
            var coa_level04 = await GetById(Id);
            var coa_level03 = COALevel03_Repo.GetAll(this, i => i.Id == coa_level04.COALevel03Id).DeferredFirstOrDefault().FutureValue();
            var account_type = AccountType_Repo.GetAll(this, i => i.Id == coa_level04.AccountTypeId).DeferredFirstOrDefault().FutureValue();
            var currency = Currency_Repo.GetAll(this, i => i.Id == coa_level04.CurrencyId).DeferredFirstOrDefault().FutureValue();
            var link_with = LinkWith_Repo.GetAll(this, i => i.Id == coa_level04.LinkWithId).DeferredFirstOrDefault().FutureValue();
            await link_with.ValueAsync();

            var output = ObjectMapper.Map<COALevel04GetAllDto>(coa_level04);
            output.COALevel03Name = coa_level03?.Value?.Name ?? "";
            output.COALevel03SerialNumber = coa_level03?.Value?.SerialNumber ?? "";
            output.AccountTypeName = account_type?.Value?.Name ?? "";
            output.CurrencyName = currency?.Value?.Name ?? "";
            output.LinkWithName = link_with?.Value?.Name ?? "";
            return output;
        }

        [AbpAuthorize(PermissionNames.LookUps_COALevel04_Update)]
        public async Task<string> Update(COALevel04Dto input)
        {
            var old_coa_level_04 = await GetById(input.Id);
            var entity = ObjectMapper.Map(input, old_coa_level_04);
            await COALevel04_Repo.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            // Remove existing opening balance entries for this COA to avoid duplicates
            var existingOpeningEntries = await GeneralLedger_Repo
                .Where(this, i => i.ChartOfAccountId == entity.Id && i.Remarks == "Opening Balance")
                .ToListAsync();
            if (existingOpeningEntries.Any())
            {
                await GeneralLedger_Repo.DeleteRangeAsync(existingOpeningEntries);
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            // Insert opening balance as Credit with Debit = 0 if provided
            if (entity.OpeningBalance.HasValue && entity.OpeningBalance.Value > 0)
            {
                var ledger = new GeneralLedgerInfo
                {
                    IssueDate = DateTime.Now,
                    VoucherNumber = $"OB-{DateTime.Now:yyyyMMddHHmmss}",
                    ChartOfAccountId = entity.Id,
                    Credit = entity.OpeningBalance ?? 0,
                    Debit = 0,
                    IsAdjustmentEntry = false,
                    Status = "PENDING",
                    EmployeeId = 0,
                    LinkedDocumentId = 0,
                    LinkedDocument = 0,
                    ReferenceDocumentId = 0,
                    ReferenceVoucherNumber = null,
                    ReferenceDocument = null,
                    Remarks = "Opening Balance",
                    TenantId = AbpSession.TenantId
                };

                await GeneralLedger_Repo.InsertAsync(ledger);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            return "COALevel04 Updated Successfully.";
        }

        [AbpAuthorize(PermissionNames.LookUps_COALevel04_Delete)]
        public async Task<string> Delete(long Id)
        {
            var coa_level04 = await GetById(Id);
            await COALevel04_Repo.DeleteAsync(coa_level04);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "COALevel04 Deleted Successfully.";
        }

        public async Task<string> GetSerialNumber(long COALevel03Id)
        {
            var count = await COALevel04_Repo.GetAll(this, i => i.COALevel03Id == COALevel03Id).CountAsync() + 1;
            if (count > 9999)
                throw new UserFriendlyException($"Cannot create a new COALevel04 entry. The maximum allowed limit of 9999 entries has been reached for COALevel03 with Id {COALevel03Id}.");

            return count.ToString().PadLeft(4, '0');
        }
        
        [AbpAuthorize(PermissionNames.LookUps_COALevel04_Create)]
        public async Task<COALevel04BulkUploadResultDto> BulkUpload(COALevel04BulkUploadRequestDto input)
        {
            var result = new COALevel04BulkUploadResultDto
            {
                TotalItems = input.Items?.Count ?? 0
            };

            if (input.Items == null || input.Items.Count == 0)
            {
                result.Errors.Add("No items provided for bulk upload");
                return result;
            }

            var accountTypesQuery = await AccountType_Repo.GetAll(this)
                .Select(at => new { at.Id, at.Name })
                .ToListAsync();
                
            var accountTypes = accountTypesQuery
                .GroupBy(at => at.Name.ToLower())
                .ToDictionary(g => g.Key, g => g.First().Id);

            var coaLevel03sQuery = await COALevel03_Repo.GetAll(this)
                .Select(c => new { c.Id, c.Name })
                .ToListAsync();
                
            var coaLevel03s = coaLevel03sQuery
                .GroupBy(c => c.Name.ToLower())
                .ToDictionary(g => g.Key, g => g.First().Id);

            var currenciesQuery = await Currency_Repo.GetAll(this)
                .Select(c => new { c.Id, c.Name })
                .ToListAsync();
                
            var currencies = currenciesQuery
                .GroupBy(c => c.Name.ToLower())
                .ToDictionary(g => g.Key, g => g.First().Id);

            var linkWithsQuery = await LinkWith_Repo.GetAll(this)
                .Select(lw => new { lw.Id, lw.Name })
                .ToListAsync();
                
            var linkWiths = linkWithsQuery
                .GroupBy(lw => lw.Name.ToLower())
                .ToDictionary(g => g.Key, g => g.First().Id);

            foreach (var item in input.Items)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(item.Name))
                    {
                        result.Errors.Add($"Item with SerialNumber '{item.SerialNumber}' has no Name");
                        result.FailureCount++;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.COALevel03Name))
                    {
                        result.Errors.Add($"Item '{item.Name}' has no COALevel03Name");
                        result.FailureCount++;
                        continue;
                    }

                    if (!coaLevel03s.TryGetValue(item.COALevel03Name.ToLower(), out var coaLevel03Id))
                    {
                        result.Errors.Add($"COALevel03 '{item.COALevel03Name}' not found for item '{item.Name}'");
                        result.FailureCount++;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.AccountTypeName))
                    {
                        result.Errors.Add($"Item '{item.Name}' has no AccountTypeName");
                        result.FailureCount++;
                        continue;
                    }

                    if (!accountTypes.TryGetValue(item.AccountTypeName.ToLower(), out var accountTypeId))
                    {
                        result.Errors.Add($"AccountType '{item.AccountTypeName}' not found for item '{item.Name}'");
                        result.FailureCount++;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.CurrencyName))
                    {
                        result.Errors.Add($"Item '{item.Name}' has no CurrencyName");
                        result.FailureCount++;
                        continue;
                    }

                    if (!currencies.TryGetValue(item.CurrencyName.ToLower(), out var currencyId))
                    {
                        result.Errors.Add($"Currency '{item.CurrencyName}' not found for item '{item.Name}'");
                        result.FailureCount++;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.LinkWithName))
                    {
                        result.Errors.Add($"Item '{item.Name}' has no LinkWithName");
                        result.FailureCount++;
                        continue;
                    }

                    if (!linkWiths.TryGetValue(item.LinkWithName.ToLower(), out var linkWithId))
                    {
                        result.Errors.Add($"LinkWith '{item.LinkWithName}' not found for item '{item.Name}'");
                        result.FailureCount++;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.NatureOfAccount))
                    {
                        result.Errors.Add($"Item '{item.Name}' has no NatureOfAccount");
                        result.FailureCount++;
                        continue;
                    }
                    if (!Enum.TryParse<ERP.Enums.NatureOfAccount>(item.NatureOfAccount, true, out var natureOfAccountEnum))
                    {
                        result.Errors.Add($"NatureOfAccount '{item.NatureOfAccount}' is invalid for item '{item.Name}'");
                        result.FailureCount++;
                        continue;
                    }

                    var serialNumber = string.IsNullOrWhiteSpace(item.SerialNumber) 
                        ? await GetSerialNumber(coaLevel03Id) 
                        : item.SerialNumber;

                    var entity = new COALevel04Info
                    {
                        Name = item.Name,
                        SerialNumber = serialNumber,
                        COALevel03Id = coaLevel03Id,
                        AccountTypeId = accountTypeId,
                        CurrencyId = currencyId,
                        LinkWithId = linkWithId,
                        NatureOfAccount = natureOfAccountEnum,
                        CNIC = item.CNIC,
                        EmailAddress = item.EmailAddress,
                        ContactNumber = item.ContactNumber,
                        PhysicalAddress = item.PhysicalAddress,
                        SalesTaxNumber = item.SalesTaxNumber,
                        NationalTaxNumber = item.NationalTaxNumber,
                        TenantId = AbpSession.TenantId
                    };

                    await COALevel04_Repo.InsertAsync(entity);
                    result.SuccessCount++;
                }
                catch (System.Exception ex)
                {
                    result.Errors.Add($"Error processing item '{item.Name}': {ex.Message}");
                    result.FailureCount++;
                }
            }

            await CurrentUnitOfWork.SaveChangesAsync();
            return result;
        }
    }
}
