using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Authorization.Users;
using System.Threading.Tasks;

namespace ERP.Modules.Finance.LookUps
{
    public interface IUserInfoAppService : IApplicationService
    {
        Task<UserIdDto> GetUserIdByUsername(string username);
    }

    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class UserInfoAppService : ERPApplicationService, IUserInfoAppService
    {
        private readonly IRepository<User, long> _userRepository;

        public UserInfoAppService(IRepository<User, long> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserIdDto> GetUserIdByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new UserFriendlyException("Username cannot be empty.");
            }

            var user = await _userRepository.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                throw new UserFriendlyException($"User with username '{username}' not found.");
            }

            return new UserIdDto
            {
                UserId = user.Id,
                Username = user.UserName
            };
        }
    }

    public class UserIdDto
    {
        public long UserId { get; set; }
        public string Username { get; set; }
    }
}
