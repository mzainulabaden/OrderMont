using ERP.Controllers;
using ERP.Modules.Finance.LookUps;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.Web.Host.Controllers
{
    [Route("api/services/app/[controller]")]
    public class UserInfoController : ERPControllerBase
    {
        private readonly IUserInfoAppService _userInfoAppService;

        public UserInfoController(IUserInfoAppService userInfoAppService)
        {
            _userInfoAppService = userInfoAppService;
        }

        /// <summary>
        /// Gets user ID by username
        /// </summary>
        /// <param name="username">The username to search for</param>
        /// <returns>User ID and username</returns>
        [HttpGet("get-user-id-by-username")]
        public async Task<UserIdDto> GetUserIdByUsername([FromQuery] string username)
        {
            return await _userInfoAppService.GetUserIdByUsername(username);
        }
    }
} 