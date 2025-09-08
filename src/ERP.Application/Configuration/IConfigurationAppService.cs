using ERP.Configuration.Dto;
using System.Threading.Tasks;

namespace ERP.Configuration;

public interface IConfigurationAppService
{
    Task ChangeUiTheme(ChangeUiThemeInput input);
}
