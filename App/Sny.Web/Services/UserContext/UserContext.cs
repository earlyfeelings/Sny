using Sny.Api.Dtos.Models.Accounts;
using Sny.Web.Model;
using Sny.Web.Services.BackendProvider;
using Sny.Web.Services.LocalStorageService;
using System.Net;

namespace Sny.Web.Services.UserContext
{
    public class UserContext : IUserContext
    {
        private readonly IBackendProvider _ibp;
        private ILocalStorageService _localStorageService;

        public UserContext(IBackendProvider ibp, ILocalStorageService localStorageService)
        {
            this._ibp = ibp;
            this._localStorageService = localStorageService;
        }

        public bool IsLoggedIn { get; private set; } = false;
        public string Email { get; private set; } = String.Empty;

        public async Task Initialize()
        {
            var jwt = await _localStorageService.GetItem<string>(LocalStorageKeys.Jwt);
            var refresh = await _localStorageService.GetItem<string>(LocalStorageKeys.RefreshToken);
            if (jwt == null || refresh == null) 
                return;
            await Login(new BackendApiCredentials(jwt, refresh, DateTime.MinValue));
        }

        public async Task Login(BackendApiCredentials credentials)
        {
            await _ibp.SetCredentials(credentials);

            try
            {
                var res = await _ibp.GetMyInfo();
                if (res.Response.IsSuccessStatusCode)
                {
                    IsLoggedIn = true;
                    Email = res.Data.Email;
                }
                else
                {
                    await Logout();
                    return;
                }
            }
            catch (HttpRequestException)
            {
                //unauthorized
                await Logout();
                return;
            }
            finally
            {
                NotifyStateChanged();
            }
        }

        public event Action? StateChanged;

        private void NotifyStateChanged()
        {
            StateChanged?.Invoke();
        }

        public async Task Logout()
        {
            try
            {
                var currCreds = _ibp.CurrentCredentials;

                if (currCreds != null)
                {
                    //invalidate refresh token on server side
                    var logoutRequest = new LogoutRequestDto(currCreds.RefreshToken);
                    (await _ibp.Logout(logoutRequest)).ThrowWhenUnsuccessful();
                }

                await _ibp.ClearCredentials();
                IsLoggedIn = false;
                Email = String.Empty;
            }
            finally
            {
                NotifyStateChanged();
            }
        }
    }
}
