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

namespace Sny.Api.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IAccountManager _accManager;
        private readonly JwtOptions _jwtOptions;

        public AccountController(IAccountManager accManager, IOptions<JwtOptions> jwtOptions)
        {
            this._accManager = accManager;
            this._jwtOptions = jwtOptions.Value;
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
                    return Ok(new LoginResponseDto(CreateJWT(result.Account)));
                }
            }
            catch (LoginFailedException)
            {
                return StatusCode(403);
            }
            return StatusCode(403);
        }

        private string CreateJWT(Account acc)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtOptions.Secret));
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, acc.Email),
                new Claim(JwtRegisteredClaimNames.Sub, acc.Email),
                new Claim(JwtRegisteredClaimNames.Email, acc.Email),
                new Claim(JwtRegisteredClaimNames.Jti, acc.Id.ToString())
            };

            var token = new JwtSecurityToken(issuer: _jwtOptions.Issuer, audience: _jwtOptions.Audience, claims: claims, expires: DateTime.Now.AddMinutes(60), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
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
