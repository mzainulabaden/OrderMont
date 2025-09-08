using Abp.Application.Services.Dto;

namespace ERP.Modules.Suggestion.Dtos;

public class SuggestionDto : EntityDto<long>
{
    public string Name { get; set; }
    public string Additional { get; set; }


}
