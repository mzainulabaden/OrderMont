using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Modules.Finance.ChartOfAccount.COALevel02;
using ERP.Modules.Finance.LookUps;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;
using System;
using ERP.Modules.Finance.ChartOfAccount.COALevel03;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel01
{
    public class COALevel01AppService : ApplicationService
    {
        public IRepository<COALevel01Info, long> COALevel01_Repo { get; set; }
        public IRepository<COALevel02Info, long> COALevel02_Repo { get; set; }
        public IRepository<COALevel03Info, long> COALevel03_Repo { get; set; }
        public IRepository<AccountTypeInfo, long> AccountType_Repo { get; set; }

        public async Task<PagedResultDto<COALevel01GetAllDto>> GetAll(COALevel01FiltersDto filters)
        {
            var coa_level01_query = COALevel01_Repo.GetAll(this).ApplyBaseFilters(filters);
            if (!string.IsNullOrWhiteSpace(filters.AccountTypeId))
                coa_level01_query = coa_level01_query.Where(i => i.AccountTypeId == filters.AccountTypeId.TryToLong());
            var coa_level01s = await coa_level01_query.ToPagedListAsync(filters);

            var account_type_ids = coa_level01s.Select(i => i.AccountTypeId).ToList();

            var total_count = coa_level01_query.DeferredCount().FutureValue();
            var account_types = AccountType_Repo.GetAll(this, i => account_type_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            _ = await account_types.ToListAsync();

            var dict_account_types = account_types.ToDictionary(i => i.Id);

            var output = new List<COALevel01GetAllDto>();
            foreach (var coa_level01 in coa_level01s)
            {
                dict_account_types.TryGetValue(coa_level01.AccountTypeId, out var account_type);

                var dto = ObjectMapper.Map<COALevel01GetAllDto>(coa_level01);
                dto.AccountTypeName = account_type?.Name ?? "";
                output.Add(dto);
            }

            return new PagedResultDto<COALevel01GetAllDto>(total_count.Value, output);
        }

        public async Task<string> Create(COALevel01Dto input)
        {
            var account_type = AccountType_Repo.GetAll(this, i => i.Id == input.AccountTypeId).DeferredFirstOrDefault().FutureValue();
            _ = await account_type.ValueAsync();

            if (account_type.Value == null)
                throw new UserFriendlyException($"AccountTypeId: '{input.AccountTypeId}' is invalid.");

            var entity = ObjectMapper.Map<COALevel01Info>(input);
            entity.TenantId = AbpSession.TenantId;
            await COALevel01_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "COALevel01 Created Successfully.";
        }

        public async Task<COADumpHierarchyResultDto> DumpHierarchy(COADumpHierarchyRequestDto input)
        {
            var result = new COADumpHierarchyResultDto();
            if (input?.Items == null || input.Items.Count == 0)
            {
                result.Errors.Add("No items provided");
                result.FailureCount = 1;
                return result;
            }

            // Preload AccountTypes dictionary by name lower -> id
            var accountTypes = await AccountType_Repo.GetAll(this)
                .Select(at => new { at.Id, at.Name })
                .ToListAsync();
            var accountTypeByName = accountTypes
                .GroupBy(a => a.Name.ToLower())
                .ToDictionary(g => g.Key, g => g.First().Id);

            foreach (var l1 in input.Items)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(l1.Name) || string.IsNullOrWhiteSpace(l1.SerialNumber) || string.IsNullOrWhiteSpace(l1.AccountTypeName))
                        throw new Exception("Level01 requires Name, SerialNumber, AccountTypeName");

                    if (!accountTypeByName.TryGetValue(l1.AccountTypeName.ToLower(), out var l1AccountTypeId))
                        throw new Exception($"AccountType '{l1.AccountTypeName}' not found for Level01 '{l1.Name}'");

                    // Upsert Level01
                    var level01 = await COALevel01_Repo.GetAll(this)
                        .FirstOrDefaultAsync(x => x.SerialNumber == l1.SerialNumber);
                    if (level01 == null)
                    {
                        level01 = new COALevel01Info
                        {
                            Name = l1.Name,
                            SerialNumber = l1.SerialNumber,
                            AccountTypeId = l1AccountTypeId,
                            TenantId = AbpSession.TenantId
                        };
                        await COALevel01_Repo.InsertAsync(level01);
                        result.CreatedLevel01++;
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    else
                    {
                        level01.Name = l1.Name;
                        level01.AccountTypeId = l1AccountTypeId;
                        await COALevel01_Repo.UpdateAsync(level01);
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }

                    // Level02
                    if (l1.Level02Items != null)
                    {
                        foreach (var l2 in l1.Level02Items)
                        {
                            if (string.IsNullOrWhiteSpace(l2.Name) || string.IsNullOrWhiteSpace(l2.SerialNumber) || string.IsNullOrWhiteSpace(l2.AccountTypeName))
                            {
                                result.Errors.Add($"Level02 under '{l1.Name}' missing required fields");
                                result.FailureCount++;
                                continue;
                            }
                            if (!accountTypeByName.TryGetValue(l2.AccountTypeName.ToLower(), out var l2AccountTypeId))
                            {
                                result.Errors.Add($"AccountType '{l2.AccountTypeName}' not found for Level02 '{l2.Name}'");
                                result.FailureCount++;
                                continue;
                            }

                            var level02 = await COALevel02_Repo.GetAll(this)
                                .FirstOrDefaultAsync(x => x.SerialNumber == l2.SerialNumber && x.COALevel01Id == level01.Id);
                            if (level02 == null)
                            {
                                level02 = new COALevel02Info
                                {
                                    Name = l2.Name,
                                    SerialNumber = l2.SerialNumber,
                                    COALevel01Id = level01.Id,
                                    AccountTypeId = l2AccountTypeId,
                                    TenantId = AbpSession.TenantId
                                };
                                await COALevel02_Repo.InsertAsync(level02);
                                result.CreatedLevel02++;
                                await CurrentUnitOfWork.SaveChangesAsync();
                            }
                            else
                            {
                                level02.Name = l2.Name;
                                level02.AccountTypeId = l2AccountTypeId;
                                await COALevel02_Repo.UpdateAsync(level02);
                                await CurrentUnitOfWork.SaveChangesAsync();
                            }

                            // Level03
                            if (l2.Level03Items != null)
                            {
                                foreach (var l3 in l2.Level03Items)
                                {
                                    if (string.IsNullOrWhiteSpace(l3.Name) || string.IsNullOrWhiteSpace(l3.SerialNumber) || string.IsNullOrWhiteSpace(l3.AccountTypeName))
                                    {
                                        result.Errors.Add($"Level03 under '{l2.Name}' missing required fields");
                                        result.FailureCount++;
                                        continue;
                                    }
                                    if (!accountTypeByName.TryGetValue(l3.AccountTypeName.ToLower(), out var l3AccountTypeId))
                                    {
                                        result.Errors.Add($"AccountType '{l3.AccountTypeName}' not found for Level03 '{l3.Name}'");
                                        result.FailureCount++;
                                        continue;
                                    }

                                    var level03 = await COALevel03_Repo.GetAll(this)
                                        .FirstOrDefaultAsync(x => x.SerialNumber == l3.SerialNumber && x.COALevel02Id == level02.Id);
                                    if (level03 == null)
                                    {
                                        level03 = new COALevel03Info
                                        {
                                            Name = l3.Name,
                                            SerialNumber = l3.SerialNumber,
                                            COALevel02Id = level02.Id,
                                            AccountTypeId = l3AccountTypeId,
                                            TenantId = AbpSession.TenantId
                                        };
                                        await COALevel03_Repo.InsertAsync(level03);
                                        result.CreatedLevel03++;
                                        await CurrentUnitOfWork.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        level03.Name = l3.Name;
                                        level03.AccountTypeId = l3AccountTypeId;
                                        await COALevel03_Repo.UpdateAsync(level03);
                                        await CurrentUnitOfWork.SaveChangesAsync();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.FailureCount++;
                    result.Errors.Add(ex.Message);
                }
            }

            return result;
        }

        public async Task<COALevel01BulkUploadResultDto> BulkUpload(COALevel01BulkUploadRequestDto input)
        {
            var result = new COALevel01BulkUploadResultDto
            {
                TotalItems = input.Items?.Count ?? 0
            };

            if (input.Items == null || input.Items.Count == 0)
            {
                result.Errors.Add("No items provided for bulk upload");
                return result;
            }
            var accountTypes = await AccountType_Repo.GetAll(this)
                .Select(at => new { at.Id, at.Name })
                .ToDictionaryAsync(at => at.Name.ToLower(), at => at.Id);

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

                    if (string.IsNullOrWhiteSpace(item.SerialNumber))
                    {
                        result.Errors.Add($"Item with Name '{item.Name}' has no SerialNumber");
                        result.FailureCount++;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.AccountTypeName))
                    {
                        result.Errors.Add($"Item '{item.Name}' has no AccountTypeName");
                        result.FailureCount++;
                        continue;
                    }

                   
                    var accountTypeNameLower = item.AccountTypeName.ToLower();
                    if (!accountTypes.TryGetValue(accountTypeNameLower, out var accountTypeId))
                    {
                        result.Errors.Add($"AccountTypeName '{item.AccountTypeName}' for item '{item.Name}' does not exist");
                        result.FailureCount++;
                        continue;
                    }

                    var existingWithSameSerialNumber = await COALevel01_Repo.GetAll(this)
                        .AnyAsync(c => c.SerialNumber == item.SerialNumber);

                    if (existingWithSameSerialNumber)
                    {
                        result.Errors.Add($"SerialNumber '{item.SerialNumber}' for item '{item.Name}' already exists");
                        result.FailureCount++;
                        continue;
                    }

                    var entity = new COALevel01Info
                    {
                        Name = item.Name,
                        SerialNumber = item.SerialNumber,
                        AccountTypeId = accountTypeId,
                        TenantId = AbpSession.TenantId
                    };

                    await COALevel01_Repo.InsertAsync(entity);
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

        private async Task<COALevel01Info> GetById(long Id)
        {
            var coa_level01 = await COALevel01_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (coa_level01 != null)
                return coa_level01;
            else
                throw new UserFriendlyException($"COALevel01Id: '{Id}' is invalid.");
        }

        public async Task<COALevel01GetAllDto> Get(long Id)
        {
            var coa_level01 = await GetById(Id);
            var account_type = AccountType_Repo.GetAll(this, i => i.Id == coa_level01.AccountTypeId).DeferredFirstOrDefault().FutureValue();
            _ = await account_type.ValueAsync();

            var output = ObjectMapper.Map<COALevel01GetAllDto>(coa_level01);
            output.AccountTypeName = account_type?.Value?.Name ?? "";
            return output;
        }

        public async Task<string> Update(COALevel01Dto input)
        {
            var account_type = AccountType_Repo.GetAll(this, i => i.Id == input.AccountTypeId).DeferredFirstOrDefault().FutureValue();
            _ = await account_type.ValueAsync();

            if (account_type.Value == null)
                throw new UserFriendlyException($"AccountTypeId: '{input.AccountTypeId}' is invalid.");

            var old_coalevel01 = await GetById(input.Id);
            var entity = ObjectMapper.Map(input, old_coalevel01);
            await COALevel01_Repo.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "COALevel01 Updated Successfully.";
        }

        public async Task<string> Delete(long Id)
        {
            var coa_level01 = await GetById(Id);

            var has_linked = await COALevel02_Repo.GetAll(this).AnyAsync(i => i.COALevel01Id == Id);
            if (has_linked)
                throw new UserFriendlyException($"Cannot delete COALevel01 with SerialNumber '{coa_level01.SerialNumber}' because it has related COALevel02 records.");

            await COALevel01_Repo.DeleteAsync(coa_level01);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "COALevel01 Deleted Successfully.";
        }
    }
}
