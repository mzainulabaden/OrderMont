using Abp.Application.Services.Dto;

namespace ERP.Modules.Suggestion.Dtos;

public class SuggestionFilterDto : PagedResultRequestDto
{
    public string Id { get; set; }
    public string Name { get; set; }

    public override int MaxResultCount { get; set; } = 1000;
}
