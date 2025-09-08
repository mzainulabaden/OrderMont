using Abp.Authorization.Users;
using Abp.Extensions;
using System;
using System.Collections.Generic;
namespace ERP.Authorization.Users;
public class User : AbpUser<User>
{
    public const string DefaultPassword = "123qwe";

    public long CommissionPolicyId { get; set; }

    public static string CreateRandomPassword()
    {
        return Guid.NewGuid().ToString("N").Truncate(16);
    }
    public static User CreateTenantAdminUser(int tenantId, string emailAddress)
    {
        var user = new User
        {
            TenantId = tenantId,
            UserName = AdminUserName,
            Name = AdminUserName,
            Surname = AdminUserName,
            EmailAddress = emailAddress,
            Roles = new List<UserRole>()
            // You don't have to initialize CommissionPolicy here if you don't want to
        };
        user.SetNormalizedNames();
        return user;
    }
}