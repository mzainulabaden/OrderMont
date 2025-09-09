using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using ERP.Authorization.Users;
using ERP.Enums;
using ERP.Generics;
using ERP.Modules.Finance.ChartOfAccount.COALevel01;
using ERP.Modules.Finance.ChartOfAccount.COALevel02;
using ERP.Modules.Finance.ChartOfAccount.COALevel03;
using ERP.Modules.Finance.ChartOfAccount.COALevel04;
using ERP.Modules.Finance.LookUps;
using ERP.Modules.InventoryManagement.Item;
using ERP.Modules.InventoryManagement.LookUps;
using ERP.Modules.Suggestion.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ERP.Modules.Suggestion
{
    [AbpAuthorize]
    public class SuggestionAppService : ApplicationService
    {
        public IRepository<AccountTypeInfo, long> AccountType_Repo { get; set; }
        public IRepository<COALevel01Info, long> COALevel01_Repo { get; set; }
        public IRepository<COALevel02Info, long> COALevel02_Repo { get; set; }
        public IRepository<COALevel03Info, long> COALevel03_Repo { get; set; }
        public IRepository<COALevel04Info, long> COALevel04_Repo { get; set; }
        public IRepository<CurrencyInfo, long> Currency_Repo { get; set; }

        public IRepository<ItemInfo, long> Item_Repo { get; set; }
        public IRepository<ItemCategoryInfo, long> ItemCategory_Repo { get; set; }
        public IRepository<LinkWithInfo, long> LinkWith_Repo { get; set; }
        public IRepository<PaymentModeInfo, long> PaymentMode_Repo { get; set; }
        public IRepository<User, long> User_Repo { get; set; }


        public async Task<PagedResultDto<SuggestionDto>> GetSuggestions(string Target, SuggestionFilterDto filters)
        {
            var output = new PagedResultDto<SuggestionDto>();

            switch (Target)
            {
                case "AccountType": output = await GetSuggestions(AccountType_Repo, filters); break;
                case "Broker": output = await GetCOALevel04Suggestions(filters, i => i.NatureOfAccount == NatureOfAccount.Broker); break;
                case "Client": output = await GetCOALevel04Suggestions(filters, i => i.NatureOfAccount == NatureOfAccount.Client); break;
                case "Bank": output = await GetCOALevel04Suggestions(filters, i => i.NatureOfAccount == NatureOfAccount.Bank); break;
                case "COALevel01": output = await GetCOALevel01Suggestions(filters); break;
                case "COALevel02": output = await GetCOALevel02Suggestions(filters); break;
                case "COALevel03": output = await GetCOALevel03Suggestions(filters); break;
                case "COALevel04": output = await GetCOALevel04Suggestions(filters); break;
                case "Currency": output = await GetSuggestions(Currency_Repo, filters); break;
                case "Item": output = await GetSuggestions(Item_Repo, filters); break;
                case "ItemCategory": output = await GetSuggestions(ItemCategory_Repo, filters); break;
            
                case "LinkWith": output = await GetSuggestions(LinkWith_Repo, filters); break;
                case "PaymentMode": output = await GetSuggestions(PaymentMode_Repo, filters); break;
                case "Users": output = await GetUserSuggestions(filters); break;
                case "Supplier": output = await GetCOALevel04Suggestions(filters, i => i.NatureOfAccount == NatureOfAccount.Supplier); break;
                case "Expense": output = await GetCOALevel04Suggestions(filters, i => i.NatureOfAccount == NatureOfAccount.Expense); break;
                case "Tax": output = await GetCOALevel04Suggestions(filters, i => i.NatureOfAccount == NatureOfAccount.Tax); break;
                
                default: throw new ArgumentException($"Invalid Target: {Target}");
            }

            return output;
        }

        private async Task<PagedResultDto<SuggestionDto>> GetCOALevel01Suggestions(SuggestionFilterDto filters)
        {
            var query = COALevel01_Repo.GetAll(this);

            if (!string.IsNullOrEmpty(filters.Id))
                query = query.Where(i => i.Id == filters.Id.TryToLong());
            if (!string.IsNullOrEmpty(filters.Name))
                query = query.Where(i => i.Name.Contains(filters.Name));
            var result = await query.Skip(filters.SkipCount).Take(filters.MaxResultCount).OrderByDescending(i => i.CreationTime).Select(i => new SuggestionDto { Id = i.Id, Name = i.Name, Additional = i.SerialNumber }).ToListAsync();

            return new PagedResultDto<SuggestionDto>(query.Count(), result);
        }

        private async Task<PagedResultDto<SuggestionDto>> GetCOALevel02Suggestions(SuggestionFilterDto filters)
        {
            var query = COALevel02_Repo.GetAll(this);

            if (!string.IsNullOrEmpty(filters.Id))
                query = query.Where(i => i.Id == filters.Id.TryToLong());
            if (!string.IsNullOrEmpty(filters.Name))
                query = query.Where(i => i.Name.Contains(filters.Name));
            var result = await query.Skip(filters.SkipCount).Take(filters.MaxResultCount).OrderByDescending(i => i.CreationTime).Select(i => new SuggestionDto { Id = i.Id, Name = i.Name, Additional = i.SerialNumber }).ToListAsync();

            return new PagedResultDto<SuggestionDto>(query.Count(), result);
        }

        private async Task<PagedResultDto<SuggestionDto>> GetCOALevel03Suggestions(SuggestionFilterDto filters)
        {
            var query = COALevel03_Repo.GetAll(this);

            if (!string.IsNullOrEmpty(filters.Id))
                query = query.Where(i => i.Id == filters.Id.TryToLong());
            if (!string.IsNullOrEmpty(filters.Name))
                query = query.Where(i => i.Name.Contains(filters.Name));
            var result = await query.Skip(filters.SkipCount).Take(filters.MaxResultCount).OrderByDescending(i => i.CreationTime).Select(i => new SuggestionDto { Id = i.Id, Name = i.Name, Additional = i.SerialNumber }).ToListAsync();

            return new PagedResultDto<SuggestionDto>(query.Count(), result);
        }

        private async Task<PagedResultDto<SuggestionDto>> GetCOALevel04Suggestions(SuggestionFilterDto filters, Expression<Func<COALevel04Info, bool>> custom_expression = null)
        {
            var query = COALevel04_Repo.GetAll(this);

            if (!string.IsNullOrEmpty(filters.Id))
                query = query.Where(i => i.Id == filters.Id.TryToLong());
            if (!string.IsNullOrEmpty(filters.Name))
                query = query.Where(i => i.Name.Contains(filters.Name));
            if (custom_expression != null)
                query = query.Where(custom_expression);
            var result = await query.Skip(filters.SkipCount).Take(filters.MaxResultCount).OrderByDescending(i => i.CreationTime).Select(i => new SuggestionDto { Id = i.Id, Name = i.Name, Additional = i.SerialNumber }).ToListAsync();

            return new PagedResultDto<SuggestionDto>(query.Count(), result);
        }

        private async Task<PagedResultDto<SuggestionDto>> GetSuggestions<TEntity>(IRepository<TEntity, long> repository, SuggestionFilterDto filters, Expression<Func<TEntity, bool>> custom_expression = null) where TEntity : SimpleEntityBase
        {
            var query = repository.GetAll(this);

            if (!string.IsNullOrEmpty(filters.Id))
                query = query.Where(i => i.Id == filters.Id.TryToLong());
            if (!string.IsNullOrEmpty(filters.Name))
                query = query.Where(i => i.Name.Contains(filters.Name));
            if (custom_expression != null)
                query = query.Where(custom_expression);
            var result = await query.Skip(filters.SkipCount).Take(filters.MaxResultCount).OrderByDescending(i => i.CreationTime).Select(i => new SuggestionDto { Id = i.Id, Name = i.Name,}).ToListAsync();

            return new PagedResultDto<SuggestionDto>(query.Count(), result);
        }
    
        private async Task<PagedResultDto<SuggestionDto>> GetUserSuggestions(SuggestionFilterDto filters)
        {
            var query = User_Repo.GetAll();

            if (!string.IsNullOrEmpty(filters.Id))
                query = query.Where(i => i.Id == filters.Id.TryToLong());
            if (!string.IsNullOrEmpty(filters.Name))
                query = query.Where(i => i.Name.Contains(filters.Name) || i.UserName.Contains(filters.Name));

            var result = await query
                .Skip(filters.SkipCount)
                .Take(filters.MaxResultCount)
                .OrderByDescending(i => i.CreationTime)
                .Select(i => new SuggestionDto { Id = i.Id, Name = i.Name, Additional = i.UserName })
                .ToListAsync();

            return new PagedResultDto<SuggestionDto>(await query.CountAsync(), result);
        }
    }
}
