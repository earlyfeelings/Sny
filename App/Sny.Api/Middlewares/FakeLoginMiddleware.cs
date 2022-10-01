using Microsoft.AspNetCore.Http;
using Sny.Api.Services;
using Sny.Core.AccountsAggregate.Exceptions;
using Sny.Core.Interfaces.Core;

namespace Sny.Api.Middlewares
{
    public class FakeLoginMiddleware
    {
        private readonly RequestDelegate _next;

        public FakeLoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAccountManager accManager, IJwtService jwtService)
        {
            var headers = context.Request.Headers;
            if (!headers.Values.Contains("Authorization"))
            {
                try
                {
                    var result = await accManager.Login(new LoginModel("dev@dev.cz", "1234"));
                    if (result.Result.Success)
                    {
                       var jwt = jwtService.CreateJWT(result.Account);
                       headers.Add("Authorization", $"bearer {jwt}");
                    }
                }
                catch (LoginFailedException)
                {
                    goto next;
                }
                goto next;
            }

            next:
            await _next.Invoke(context);
        }
    }
}
