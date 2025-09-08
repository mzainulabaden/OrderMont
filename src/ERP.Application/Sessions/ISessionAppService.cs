using Abp.Application.Services;
using ERP.Sessions.Dto;
using System.Threading.Tasks;

namespace ERP.Sessions;

public interface ISessionAppService : IApplicationService
{
    Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
}
