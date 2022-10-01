using Microsoft.AspNetCore.Mvc;
using Sny.Api.Dtos.Models.Accounts;
using Sny.Api.Dtos.Models.Goals;
using Sny.Core.AccountsAggregate.Exceptions;
using Sny.Core.Interfaces.Core;
using Sny.Api.Mappers;
using Microsoft.IdentityModel.Tokens;
using Sny.Api.Options;
using Sny.Core.AccountsAggregate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Sny.Api.Services;

namespace Sny.Api.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IAccountManager _accManager;
        private readonly IJwtService _jwtService;

        public AccountController(IAccountManager accManager, IJwtService jwtService)
        {
            this._accManager = accManager;
            this._jwtService = jwtService;
        }

        /// <summary>
        /// If login is sucess, return 200 OK. Otherwise, return 403 unauthorized.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(LoginResponseDto), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 403)]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
            try
            {
                var result = await _accManager.Login(new LoginModel(model.Email, model.Password));
                if (result.Result.Success)
                {
                    return Ok(new LoginResponseDto(_jwtService.CreateJWT(result.Account)));
                }
            }
            catch (LoginFailedException)
            {
                return StatusCode(403);
            }
            return StatusCode(403);
        }

      

        /// <summary>
        /// Register new user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(RegisterResponseDto), 200)]
        public async Task<IActionResult> Register(RegisterRequestDto model)
        {
            var result = await _accManager.Register(model.ToRegisterModel());
            return Ok(result.ToRegisterResponseModel());
        }
    }
}
