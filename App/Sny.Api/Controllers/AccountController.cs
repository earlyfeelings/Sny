using Microsoft.AspNetCore.Mvc;
using Sny.Api.Dtos.Models.Accounts;
using Sny.Api.Dtos.Models.Goals;
using Sny.Core.AccountsAggregate.Exceptions;
using Sny.Core.Interfaces.Core;
using Sny.Api.Mappers;

namespace Sny.Api.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IAccountManager _accManager;

        public AccountController(IAccountManager accManager)
        {
            this._accManager = accManager;
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
                return Ok(new LoginResponseDto(result.Jwt));
            }
            catch (LoginFailedException)
            {
                return StatusCode(403);
            }
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
