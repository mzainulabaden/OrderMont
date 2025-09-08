using Abp.Authorization;
using Abp.Runtime.Session;
using ERP.Configuration.Dto;
using System.Threading.Tasks;

namespace ERP.Configuration;

[AbpAuthorize]
public class ConfigurationAppService : ERPApplicationService, IConfigurationAppService
{
    public async Task ChangeUiTheme(ChangeUiThemeInput input)
    {
        await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
    }
}
