using Microsoft.AspNetCore.Mvc;
using Sny.Api.Dtos.Models.Accounts;
using Sny.Api.Dtos.Models.Goals;

namespace Sny.Api.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : Controller
    {
        /// <summary>
        /// If login is sucess, return 200 OK. Otherwise, return 401 unauthorized.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(LoginResponseDto), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
            var resp = new LoginResponseDto("test_jwt");
            return Ok(resp);
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
            var resp = new RegisterResponseDto(RegisterStatus.Success);
            return Ok(resp);
        }
    }
}
