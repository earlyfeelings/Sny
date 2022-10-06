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
using Sny.Api.Middlewares;

namespace Sny.Api.Controllers
{
    [ApiController]
   
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

        [HttpGet]
        [Route("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var acc = await _accManager.CurrentAccount();
                await _accManager.SetValidSecurityRefreshTokenId(acc, Guid.NewGuid().ToString());
            }
            catch (AccountNotFoundException)
            {
                return Ok();
            }
            return Ok();
        }
        

        [HttpGet]
        [Route("refresh-token")]
        [ProducesResponseType(typeof(LoginResponseDto), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(string jwt, string refreshToken)
        {
            var validated = _jwtService.ValidateJwtAndGetPrincipal(refreshToken);
            if (validated == null) goto response401;

            var principal = _jwtService.GetPrincipalFromExpiredToken(jwt);
            if (principal == null) goto response401;

            var userId = CurrentLoggedContextMiddleware.GetUserId(principal);
            if (userId == null) goto response401;

            var acc = await _accManager.GetAccountById(userId.Value);
            var validRefreshTokenId = await _accManager.GetValidSecurityRefreshTokenId(acc);
            if (validRefreshTokenId == null) goto response401;

            var refreshTokenId = validated.Claims.SingleOrDefault(d => d.Type == ClaimTypes.Sid);
            if (refreshTokenId == null) goto response401;

            if (refreshTokenId.Value != validRefreshTokenId) goto response401;

            var newRefreshToken = _jwtService.CreateRefreshJwt();
            var newJwtToken = _jwtService.CreateJWT(acc);

            //persist new token
            await _accManager.SetValidSecurityRefreshTokenId(acc, newRefreshToken.RefreshTokenId);

            return Ok(new LoginResponseDto(newJwtToken.Jwt, newRefreshToken.Jwt, newJwtToken.expiryAt));

        response401:
            return StatusCode(401);
        }


        /// <summary>
        /// If login is sucess, return 200 OK. Otherwise, return 401 unauthorized.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(LoginResponseDto), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
            try
            {
                var result = await _accManager.Login(new LoginModel(model.Email, model.Password));
                if (result.Result.Success)
                {
                    var refreshToken = _jwtService.CreateRefreshJwt();
                    var jwtToken = _jwtService.CreateJWT(result.Account);

                    //persist new token
                    await _accManager.SetValidSecurityRefreshTokenId(result.Account, refreshToken.RefreshTokenId);

                    return Ok(new LoginResponseDto(jwtToken.Jwt, refreshToken.Jwt, jwtToken.expiryAt));
                }
            }
            catch (LoginFailedException)
            {
                return StatusCode(401);
            }
            return StatusCode(401);
        }


        /// <summary>
        /// Register new user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(RegisterResponseDto), 200)]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequestDto model)
        {
            var result = await _accManager.Register(model.ToRegisterModel());
            return Ok(result.ToRegisterResponseModel());
        }


        /// <summary>
        /// If user is logged in, return info about Account and User. Otherwise, return 401 unauthorized.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("myinfo")]
        [ProducesResponseType(typeof(MyInfoResponseDto), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 403)]
        [Authorize]
        public async Task<IActionResult> MyInfo()
        {
            var currAcc = await _accManager.CurrentAccount();
            return Ok(new MyInfoResponseDto(currAcc.Email));
        }
    }
}
