using Microsoft.AspNetCore.Http;
using Sny.Api.Services;
using Sny.Core.AccountsAggregate.Exceptions;
using Sny.Core.Interfaces.Core;
using Sny.Core.Interfaces.Infrastructure;
using System.Security.Claims;

namespace Sny.Api.Middlewares
{
    public class CurrentLoggedContextMiddleware
    {
        private readonly RequestDelegate _next;

        public CurrentLoggedContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ICurrentAccountContext icac)
        {
            var user = context.User;
            if (user?.Identity == null || !user.Identity.IsAuthenticated) 
                goto next;
            var userId = GetUserId(user);
            if (userId == null)
                goto next;

            icac.CurrentAccountId = userId;

            next:
            await _next.Invoke(context);
        }

        public static Guid? GetUserId(ClaimsPrincipal principal)
        {
            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (claim?.Value == null) return null;
            return new Guid(claim.Value);
        }
    }
}
