using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoName.Core.Service;
using NoName.Model.Account;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace NoName.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Route("Token")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<TokenResponseModel> Token([FromBody]TokenRequestModel model)
        {
            return await _accountService.Token(model);
        }

        [Route("GetUser")]
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public long GetUser()
        {
            var userId = Convert.ToInt64(User.Claims.First(f => f.Type == Common.Foundation.Constants.Claim.UserId).Value);

            return userId;
        }
    }
}