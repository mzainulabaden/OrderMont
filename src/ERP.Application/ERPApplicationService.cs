using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using ERP.Authorization.Users;
using ERP.MultiTenancy;
using ERP.Users.Dto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ERP
{
    public abstract class ERPApplicationService : ApplicationService
    {
        public TenantManager TenantManager { get; set; }
        public IRepository<User, long> User_Repo { get; set; }
        public UserManager UserManager { get; set; }

        protected ERPApplicationService()
        {
            LocalizationSourceName = ERPConsts.LocalizationSourceName;
        }

        protected virtual async Task<UserDto> GetCurrentUserAsync()
        {
            var user = await User_Repo.FirstOrDefaultAsync(i => i.Id == AbpSession.UserId.Value && i.TenantId == AbpSession.TenantId);
            if (user == null)
                throw new Exception("There is no current user!");

            var user_dto = ObjectMapper.Map<UserDto>(user);
            var role_names = await UserManager.GetRolesAsync(user);
            user_dto.RoleNames = role_names.ToArray();

            return user_dto;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
