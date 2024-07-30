using Data.Mongo.Collections;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Account;
using Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILoginLogService _loginLogService;
        public AccountController(IAccountService accountService, ILoginLogService loginLogService)
        {
            _accountService = accountService;
            _loginLogService = loginLogService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            //auth
            var result = await _accountService.AuthenticateAsync(request);
            if (result.Errors == null || !result.Errors.Any())
            {
                //mongo usage example
                LoginLog log = new LoginLog()
                {
                    LoginTime = DateTime.Now,
                    UserEmail = request.Email
                };
                await _loginLogService.Add(log);
            }
            return Ok(result);
        }

        [HttpPost("refreshtoken")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequest request)
        {
            return Ok(await _accountService.RefreshTokenAsync(request));
        }
        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
