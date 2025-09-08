using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Authorization;
using Abp.Application.Services.Dto;
using Abp.UI;
using ERP.Authorization;
using ERP.Generics;
using ERP.Generics.Simple;
using System.Text.Json.Serialization;

namespace ERP.Modules.Finance.ProfitLoseNote
{
    [AbpAuthorize(PermissionNames.LookUps_FINANCE_ProfitLoseNote)]
    public class ProfitLoseNoteAppService : GenericSimpleAppService<FINANCE_ProfitLoseNoteDto, ProfitLoseNoteInfo, SimpleSearchDtoBase>
    {
        
        [AbpAuthorize(PermissionNames.LookUps_FINANCE_ProfitLoseNote)]
        public override PagedResultDto<FINANCE_ProfitLoseNoteDto> GetAll(SimpleSearchDtoBase search)
        {
            var query = MainRepository.GetAllIncluding(i => i.ProfitLoseNoteDetails).Where(i => i.TenantId == AbpSession.TenantId);

            query = ApplyFilters(query, search);
            query = query.OrderByDescending(i => i.CreationTime);

            var pagedEntities = query
                .Skip(search.SkipCount)
                .Take(search.MaxResultCount)
                .ToList();

            var items = ObjectMapper.Map<List<FINANCE_ProfitLoseNoteDto>>(pagedEntities) ?? new List<FINANCE_ProfitLoseNoteDto>();
            return new PagedResultDto<FINANCE_ProfitLoseNoteDto>(query.Count(), items);
        }

        [AbpAuthorize(PermissionNames.LookUps_FINANCE_ProfitLoseNote)]
        public override FINANCE_ProfitLoseNoteDto Get(long Id)
        {
            var entity = MainRepository
                .GetAllIncluding(i => i.ProfitLoseNoteDetails)
                .FirstOrDefault(i => i.Id == Id && i.TenantId == AbpSession.TenantId);

            if (entity == null)
                throw new UserFriendlyException($"Could not find {GetName()} with ID: '{Id}'.");

            var dto = ObjectMapper.Map<FINANCE_ProfitLoseNoteDto>(entity);
            return dto;
        }

        [AbpAuthorize(PermissionNames.LookUps_FINANCE_ProfitLoseNote_Create)]
        public override async Task<FINANCE_ProfitLoseNoteDto> Create(FINANCE_ProfitLoseNoteDto input)
        {
            var entity = ObjectMapper.Map<ProfitLoseNoteInfo>(input);
            if (input.ProfitLoseNoteDetails != null)
            {
                entity.ProfitLoseNoteDetails = input.ProfitLoseNoteDetails.Select(d => ObjectMapper.Map<ProfitLoseNoteDetailsInfo>(d)).ToList();
            }
            entity.TenantId = AbpSession.TenantId;
            var created = await MainRepository.InsertAsync(entity);
            return ObjectMapper.Map<FINANCE_ProfitLoseNoteDto>(created);
        }

        [AbpAuthorize(PermissionNames.LookUps_FINANCE_ProfitLoseNote_Edit)]
        public override async Task<FINANCE_ProfitLoseNoteDto> Update(FINANCE_ProfitLoseNoteDto input)
        {
            var entity = await MainRepository.GetAsync(input.Id);
            ObjectMapper.Map(input, entity);
            if (input.ProfitLoseNoteDetails != null)
            {
                entity.ProfitLoseNoteDetails = input.ProfitLoseNoteDetails.Select(d => ObjectMapper.Map<ProfitLoseNoteDetailsInfo>(d)).ToList();
            }
            entity.TenantId = AbpSession.TenantId;
            var updated = await MainRepository.UpdateAsync(entity);
            return ObjectMapper.Map<FINANCE_ProfitLoseNoteDto>(updated);
        }

        [AbpAuthorize(PermissionNames.LookUps_FINANCE_ProfitLoseNote_Delete)]
        public override async Task<string> Delete(EntityDto<long> input)
        {
            return await base.Delete(input);
        }
    }

    [AutoMap(typeof(ProfitLoseNoteInfo))]
    public class FINANCE_ProfitLoseNoteDto : SimpleDtoBase
    {
        public string NoteNumber { get; set; }
        public int AccountType { get; set; }
        public List<FINANCE_ProfitLoseNoteDetailsDto> ProfitLoseNoteDetails { get; set; }
    }

    [AutoMap(typeof(ProfitLoseNoteDetailsInfo))]
    public class FINANCE_ProfitLoseNoteDetailsDto
    {
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public long? Id { get; set; }
        public long COALevel03Id { get; set; }
        public string COAlevel03Name { get; set; }
    }
}
