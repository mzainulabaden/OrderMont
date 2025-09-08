using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using Abp.UI;
using ERP.Authorization;
using ERP.Generics;
using ERP.Generics.Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Modules.InventoryManagement.DayBook
{
    [AbpAuthorize(PermissionNames.LookUps_DayBook)]
    public class DayBookAppService : GenericSimpleAppService<DayBookDto, DayBookInfo, SimpleSearchDtoBase>
    {
        public override PagedResultDto<DayBookDto> GetAll(SimpleSearchDtoBase search)
        {
            var query = MainRepository
                .GetAllIncluding(i => i.DayBookDetails)
                .Where(i => i.TenantId == AbpSession.TenantId);

            query = ApplyFilters(query, search);
            query = query.OrderByDescending(i => i.CreationTime);

            var pagedEntities = query
                .Skip(search.SkipCount)
                .Take(search.MaxResultCount)
                .ToList();

            var items = ObjectMapper.Map<List<DayBookDto>>(pagedEntities) ?? new List<DayBookDto>();
            foreach (var dto in items)
                dto.Total = dto.DayBookDetails?.Sum(d => (decimal)d.Amount) ?? 0m;

            return new PagedResultDto<DayBookDto>(query.Count(), items);
        }

        public override DayBookDto Get(long Id)
        {
            var entity = MainRepository
                .GetAllIncluding(i => i.DayBookDetails)
                .FirstOrDefault(i => i.Id == Id && i.TenantId == AbpSession.TenantId);

            if (entity == null)
                throw new UserFriendlyException($"Could not find {GetName()} with ID: '{Id}'.");

            var dto = ObjectMapper.Map<DayBookDto>(entity);
            dto.Total = dto.DayBookDetails?.Sum(d => (decimal)d.Amount) ?? 0m;
            return dto;
        }

        public override async Task<DayBookDto> Create(DayBookDto input)
        {
            var entity = ObjectMapper.Map<DayBookInfo>(input);
            if (input.DayBookDetails != null)
            {
                entity.DayBookDetails = input.DayBookDetails.Select(d => ObjectMapper.Map<DayBookDetailsInfo>(d)).ToList();
            }
            entity.TenantId = AbpSession.TenantId;
            var created = await MainRepository.InsertAsync(entity);
            return ObjectMapper.Map<DayBookDto>(created);
        }

        public override async Task<DayBookDto> Update(DayBookDto input)
        {
            var entity = await MainRepository.GetAsync(input.Id);
            ObjectMapper.Map(input, entity);
            if (input.DayBookDetails != null)
            {
                entity.DayBookDetails = input.DayBookDetails.Select(d => ObjectMapper.Map<DayBookDetailsInfo>(d)).ToList();
            }
            entity.TenantId = AbpSession.TenantId;
            var updated = await MainRepository.UpdateAsync(entity);
            return ObjectMapper.Map<DayBookDto>(updated);
        }
    }

    [AutoMap(typeof(DayBookInfo))]
    public class DayBookDto : SimpleDtoBase
    {
        public DateTime IssueDate { get; set; }
        public decimal Total { get; set; }
        public List<DayBookDetailsDto> DayBookDetails { get; set; }
    }

    [AutoMap(typeof(DayBookDetailsInfo))]
    public class DayBookDetailsDto
    {
        public long Id { get; set; }
        public string COAName { get; set; }
        public long COAlevel04Id { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
    }
}
