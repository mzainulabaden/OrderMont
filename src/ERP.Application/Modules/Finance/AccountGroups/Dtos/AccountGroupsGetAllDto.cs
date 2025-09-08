using Abp.Application.Services.Dto;
using ERP.Modules.Suggestion.Dtos;
using System.Collections.Generic;

namespace ERP.Modules.Finance.AccountGroups.Dtos
{
    public class AccountGroupsGetAllDto : EntityDto<long>
    {
        public string Name { get; set; }
        public List<SuggestionDto> AccountTypes { get; set; }
    }
}
