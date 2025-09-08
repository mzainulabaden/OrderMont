using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ERP.Generics.Simple
{
    public abstract class GenericSimpleAppService<TDto, TEntity, TSearchDto> : GenericAppService<TDto, TDto, TDto, TDto, TEntity> where TDto : SimpleDtoBase where TEntity : SimpleEntityBase where TSearchDto : SimpleSearchDtoBase
    {
        public GenericSimpleAppService() 
        {
            CheckForPermissions = true;
        }

        public virtual PagedResultDto<TDto> GetAll(TSearchDto search)
        {
            var query = MainRepository.GetAll().Where(i => i.TenantId == AbpSession.TenantId);
            query = ApplyFilters(query, search);
            query = query.OrderByDescending(i => i.CreationTime);
            var paged = query.Skip(search.SkipCount).Take(search.MaxResultCount).ToList();
            return new PagedResultDto<TDto>(query.Count(), ObjectMapper.Map<List<TDto>>(paged));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual IQueryable<TEntity> ApplyFilters(IQueryable<TEntity> queryable, TSearchDto search)
        {
            long.TryParse(search.Id, out long entity_id);

            if (!string.IsNullOrWhiteSpace(search.Id) && entity_id != 0)
                queryable = queryable.Where(i => i.Id == entity_id);
            if (!string.IsNullOrWhiteSpace(search.Name))
                queryable = queryable.Where(i => i.Name.ToLower().StartsWith(search.Name.ToLower()));

            return queryable;
        }

        /* --------------------------------------------------------------------------------------- */

        public override List<string> CheckDbDuplication(TDto input, bool updating, int? index)
        {
            var errors = new List<string>();

            var duplicate = MainRepository.GetAll().Where(x => (updating ? x.Id != input.Id : true) && x.TenantId == AbpSession.TenantId).Where(DbDuplicates(input)).FirstOrDefault();
            if (duplicate != null)
                errors.Add($"Cannot create duplicate {GetName()}.");

            return errors;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual Expression<Func<TEntity, bool>> DbDuplicates(TDto dto) => i => i.Name.ToLower() == dto.Name.ToLower();
        public override dynamic InFileDuplicates(KeyValuePair<int, TEntity> dict) => new { dict.Value.Name };
    }
}
