using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Modules.Finance.AccountGroups;
using ERP.Modules.Finance.ChartOfAccount.COALevel01;
using ERP.Modules.Finance.ChartOfAccount.COALevel02;
using ERP.Modules.Finance.ChartOfAccount.COALevel03;
using ERP.Modules.Finance.ChartOfAccount.COALevel04;
using ERP.Modules.Finance.GeneralNote.Dtos;
using ERP.Modules.Finance.LookUps;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.Finance.GeneralNote
{
    public class GeneralNoteAppService : ERPDocumentService<GeneralNoteInfo>
    {
        public IRepository<GeneralNoteInfo, long> FINANCE_GeneralNote_Repo { get; set; }
        public IRepository<AccountGroupsInfo, long> AccountGroups_Repo { get; set; }
        public IRepository<COALevel01Info, long> COALvl1_Repo { get; set; }
        public IRepository<COALevel02Info, long> COALvl2_Repo { get; set; }
        public IRepository<COALevel03Info, long> COALvl3_Repo { get; set; }
        public IRepository<COALevel04Info, long> COALvl4_Repo { get; set; }
        public IRepository<AccountTypeInfo, long> AccountType_Repo { get; set; }

        public async Task<PagedResultDto<FINANCE_GeneralNoteGetAllDto>> GetAll(FINANCE_GeneralNoteFiltersDto filters)
        {
            var f_inance_general_note_query = FINANCE_GeneralNote_Repo.GetAll(this);
            if (!string.IsNullOrWhiteSpace(filters.Id))
                f_inance_general_note_query = f_inance_general_note_query.Where(i => i.Id == filters.Id.TryToLong());
            var f_inance_general_notes = await f_inance_general_note_query.ToPagedListAsync(filters);

            var total_count = f_inance_general_note_query.DeferredCount().FutureValue();
            var output = new List<FINANCE_GeneralNoteGetAllDto>();
            foreach (var f_inance_general_note in f_inance_general_notes)
            {
                var dto = ObjectMapper.Map<FINANCE_GeneralNoteGetAllDto>(f_inance_general_note);
                output.Add(dto);
            }

            return new PagedResultDto<FINANCE_GeneralNoteGetAllDto>(total_count.Value, output);
        }

        public async Task<string> Create(FINANCE_GeneralNoteDto input)
        {
            var entity = ObjectMapper.Map<GeneralNoteInfo>(input);
            entity.Status = "PENDING";
            entity.VoucherNumber = await GetVoucherNumber("UA", input.IssueDate ?? DateTime.Now);

            if (input.GeneralNoteDetails != null)
            {
                var c_oa_lvl1_ids = input.GeneralNoteDetails.Where(i => i.COALvl1Id.HasValue).Select(i => i.COALvl1Id.Value).ToList();
                var c_oa_lvl2_ids = input.GeneralNoteDetails.Where(i => i.COALvl2Id.HasValue).Select(i => i.COALvl2Id.Value).ToList();
                var c_oa_lvl3_ids = input.GeneralNoteDetails.Where(i => i.COALvl3Id.HasValue).Select(i => i.COALvl3Id.Value).ToList();
                var c_oa_lvl4_ids = input.GeneralNoteDetails.Where(i => i.COALvl4Id.HasValue).Select(i => i.COALvl4Id.Value).ToList();

                var c_oa_lvl1s = COALvl1_Repo.GetAll(this, i => c_oa_lvl1_ids.Contains(i.Id)).Select(i => i.Id).Future();
                var c_oa_lvl2s = COALvl2_Repo.GetAll(this, i => c_oa_lvl2_ids.Contains(i.Id)).Select(i => i.Id).Future();
                var c_oa_lvl3s = COALvl3_Repo.GetAll(this, i => c_oa_lvl3_ids.Contains(i.Id)).Select(i => i.Id).Future();
                var c_oa_lvl4s = COALvl4_Repo.GetAll(this, i => c_oa_lvl4_ids.Contains(i.Id)).Select(i => i.Id).Future();
                _ = await c_oa_lvl4s.ToListAsync();

                for (int i = 0; i < entity.GeneralNoteDetails.Count; i++)
                {
                    var detail = entity.GeneralNoteDetails[i];

                    if (detail.COALvl1Id.HasValue && !c_oa_lvl1s.Contains(detail.COALvl1Id.Value))
                        throw new UserFriendlyException($"COALvl1Id: '{detail.COALvl1Id}' is invalid at Row: '{i + 1}'.");
                    if (detail.COALvl2Id.HasValue && !c_oa_lvl2s.Contains(detail.COALvl2Id.Value))
                        throw new UserFriendlyException($"COALvl2Id: '{detail.COALvl2Id}' is invalid at Row: '{i + 1}'.");
                    if (detail.COALvl3Id.HasValue && !c_oa_lvl3s.Contains(detail.COALvl3Id.Value))
                        throw new UserFriendlyException($"COALvl3Id: '{detail.COALvl3Id}' is invalid at Row: '{i + 1}'.");
                    if (detail.COALvl4Id.HasValue && !c_oa_lvl4s.Contains(detail.COALvl4Id.Value))
                        throw new UserFriendlyException($"COALvl4Id: '{detail.COALvl4Id}' is invalid at Row: '{i + 1}'.");

                    entity.GeneralNoteDetails[i].VoucherNumber = $"{entity.VoucherNumber}/{i + 1}";
                }
            }

            entity.TenantId = AbpSession.TenantId;
            await FINANCE_GeneralNote_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "FINANCE_GeneralNote Created Successfully.";
        }

        private async Task<GeneralNoteInfo> Get(long Id)
        {
            var f_inance_general_note = await FINANCE_GeneralNote_Repo.GetAllIncluding(i => i.GeneralNoteDetails).Where(i => i.Id == Id).FirstOrDefaultAsync();
            if (f_inance_general_note != null)
                return f_inance_general_note;
            else
                throw new UserFriendlyException($"FINANCE_GeneralNoteId: '{Id}' is invalid.");
        }

        public async Task<FINANCE_GeneralNoteGetForEditDto> GetForEdit(long Id)
        {
            var f_inance_general_note = await Get(Id);
            var output = ObjectMapper.Map<FINANCE_GeneralNoteGetForEditDto>(f_inance_general_note);
            
            if (f_inance_general_note.GeneralNoteDetails != null)
            {
                var c_oa_lvl1_ids = f_inance_general_note.GeneralNoteDetails.Where(i => i.COALvl1Id.HasValue).Select(i => i.COALvl1Id.Value).ToList();
                var c_oa_lvl2_ids = f_inance_general_note.GeneralNoteDetails.Where(i => i.COALvl2Id.HasValue).Select(i => i.COALvl2Id.Value).ToList();
                var c_oa_lvl3_ids = f_inance_general_note.GeneralNoteDetails.Where(i => i.COALvl3Id.HasValue).Select(i => i.COALvl3Id.Value).ToList();
                var c_oa_lvl4_ids = f_inance_general_note.GeneralNoteDetails.Where(i => i.COALvl4Id.HasValue).Select(i => i.COALvl4Id.Value).ToList();

                var c_oa_lvl1s = COALvl1_Repo.GetAll(this, i => c_oa_lvl1_ids.Contains(i.Id)).Future();
                var c_oa_lvl2s = COALvl2_Repo.GetAll(this, i => c_oa_lvl2_ids.Contains(i.Id)).Future();
                var c_oa_lvl3s = COALvl3_Repo.GetAll(this, i => c_oa_lvl3_ids.Contains(i.Id)).Future();
                var c_oa_lvl4s = COALvl4_Repo.GetAll(this, i => c_oa_lvl4_ids.Contains(i.Id)).Future();

                var dict_c_oa_lvl1s = c_oa_lvl1s.ToDictionary(i => i.Id, i => i.Name);
                var dict_c_oa_lvl2s = c_oa_lvl2s.ToDictionary(i => i.Id, i => i.Name);
                var dict_c_oa_lvl3s = c_oa_lvl3s.ToDictionary(i => i.Id, i => i.Name);
                var dict_c_oa_lvl4s = c_oa_lvl4s.ToDictionary(i => i.Id, i => i.Name);

                output.GeneralNoteDetails = new();
                foreach (var detail in f_inance_general_note.GeneralNoteDetails)
                {
                    var mapped_detail = ObjectMapper.Map<GeneralNoteDetailsGetForEditDto>(detail);
                    mapped_detail.COALvl1Name = detail.COALvl1Id.HasValue ? dict_c_oa_lvl1s.GetValueOrDefault(detail.COALvl1Id.Value) : null;
                    mapped_detail.COALvl2Name = detail.COALvl2Id.HasValue ? dict_c_oa_lvl2s.GetValueOrDefault(detail.COALvl2Id.Value) : null;
                    mapped_detail.COALvl3Name = detail.COALvl3Id.HasValue ? dict_c_oa_lvl3s.GetValueOrDefault(detail.COALvl3Id.Value) : null;
                    mapped_detail.COALvl4Name = detail.COALvl4Id.HasValue ? dict_c_oa_lvl4s.GetValueOrDefault(detail.COALvl4Id.Value) : null;
                    output.GeneralNoteDetails.Add(mapped_detail);
                }
            }

            return output;
        }

        public async Task<string>Update(FINANCE_GeneralNoteDto input)
        {
            var old_finance_generalnote = await Get(input.Id);
            if (old_finance_generalnote.Status != "PENDING")
                throw new UserFriendlyException($"Only entries with a status of 'PENDING' can be edited. The current status is '" + old_finance_generalnote.Status + "'.");

            var entity = ObjectMapper.Map(input, old_finance_generalnote);
            entity.VoucherNumber = await GetVoucherNumber("UA", input.IssueDate ?? DateTime.Now, entity.VoucherNumber.GetVoucherIndex());

            if (input.GeneralNoteDetails != null)
            {
                var c_oa_lvl1_ids = input.GeneralNoteDetails.Where(i => i.COALvl1Id.HasValue).Select(i => i.COALvl1Id.Value).ToList();
                var c_oa_lvl2_ids = input.GeneralNoteDetails.Where(i => i.COALvl2Id.HasValue).Select(i => i.COALvl2Id.Value).ToList();
                var c_oa_lvl3_ids = input.GeneralNoteDetails.Where(i => i.COALvl3Id.HasValue).Select(i => i.COALvl3Id.Value).ToList();
                var c_oa_lvl4_ids = input.GeneralNoteDetails.Where(i => i.COALvl4Id.HasValue).Select(i => i.COALvl4Id.Value).ToList();

                var c_oa_lvl1s = COALvl1_Repo.GetAll(this, i => c_oa_lvl1_ids.Contains(i.Id)).Select(i => i.Id).Future();
                var c_oa_lvl2s = COALvl2_Repo.GetAll(this, i => c_oa_lvl2_ids.Contains(i.Id)).Select(i => i.Id).Future();
                var c_oa_lvl3s = COALvl3_Repo.GetAll(this, i => c_oa_lvl3_ids.Contains(i.Id)).Select(i => i.Id).Future();
                var c_oa_lvl4s = COALvl4_Repo.GetAll(this, i => c_oa_lvl4_ids.Contains(i.Id)).Select(i => i.Id).Future();
                _ = await c_oa_lvl4s.ToListAsync();

                for (int i = 0; i < entity.GeneralNoteDetails.Count; i++)
                {
                    var detail = entity.GeneralNoteDetails[i];

                    if (detail.COALvl1Id.HasValue && !c_oa_lvl1s.Contains(detail.COALvl1Id.Value))
                        throw new UserFriendlyException($"COALvl1Id: '{detail.COALvl1Id}' is invalid at Row: '{i + 1}'.");
                    if (detail.COALvl2Id.HasValue && !c_oa_lvl2s.Contains(detail.COALvl2Id.Value))
                        throw new UserFriendlyException($"COALvl2Id: '{detail.COALvl2Id}' is invalid at Row: '{i + 1}'.");
                    if (detail.COALvl3Id.HasValue && !c_oa_lvl3s.Contains(detail.COALvl3Id.Value))
                        throw new UserFriendlyException($"COALvl3Id: '{detail.COALvl3Id}' is invalid at Row: '{i + 1}'.");
                    if (detail.COALvl4Id.HasValue && !c_oa_lvl4s.Contains(detail.COALvl4Id.Value))
                        throw new UserFriendlyException($"COALvl4Id: '{detail.COALvl4Id}' is invalid at Row: '{i + 1}'.");

                    entity.GeneralNoteDetails[i].VoucherNumber = $"{entity.VoucherNumber}/{i + 1}";
                }
            }

            await FINANCE_GeneralNote_Repo.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "FINANCE_GeneralNote Updated Successfully.";
        }

        public async Task<string> Delete(long Id)
        {
            var f_inance_general_note = await FINANCE_GeneralNote_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (f_inance_general_note == null)
                throw new UserFriendlyException($"FINANCE_GeneralNoteId: '{Id}' is invalid.");
            if (f_inance_general_note.Status != "PENDING")
                throw new UserFriendlyException($"Unable to delete FINANCE_GeneralNote: only records with a 'PENDING' status can be deleted.");

            await FINANCE_GeneralNote_Repo.DeleteAsync(f_inance_general_note);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "FINANCE_GeneralNote Deleted Successfully.";
        }

        public async Task<List<COALvl3ByAccountClassDto>> GetCOALvl3ByAccountClass(string accountClass)
        {

            var accountGroup = await AccountGroups_Repo.GetAll(this).Where(ag => ag.Name == accountClass).FirstOrDefaultAsync();
            if (accountGroup == null)
                throw new UserFriendlyException($"Account class '{accountClass}' not found.");
            var accountTypes = await AccountType_Repo.GetAll(this).Where(at => at.AccountGroupId == accountGroup.Id).ToListAsync();
            if (!accountTypes.Any())
                throw new UserFriendlyException($"No account types found for account class '{accountClass}'.");

            var accountTypeIds = accountTypes.Select(at => at.Id).ToList();
            var coaLvl3Entries = await COALvl3_Repo.GetAll(this).Where(coa => accountTypeIds.Contains(coa.AccountTypeId)).ToListAsync();

            var result = new List<COALvl3ByAccountClassDto>();
            foreach (var coa in coaLvl3Entries)
            {
                var accountType = accountTypes.FirstOrDefault(at => at.Id == coa.AccountTypeId);
                var dto = ObjectMapper.Map<COALvl3ByAccountClassDto>(coa);
                dto.AccountTypeName = accountType?.Name;
                dto.AccountTypeShortName = accountType?.ShortName;
                result.Add(dto);
            }

            return result;
        }
    }
}
