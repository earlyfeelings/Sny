using Sny.Api.Dtos.Models.Accounts;
using Sny.Web.Services.BackendProvider;
using Sny.Web.Services.LocalStorageService;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace Sny.Web.Services.UserContext
{
    public interface IUserContext
    {
        public bool IsLoggedIn { get; }
        public string Email { get; }
        public Task Login(string JwtToken);
        public Task Logout();

        /// <summary>
        /// Pokusí se načíst JWT token z User storage a provést přihlášení.
        /// Pokud se nepodaří uživatele přihlásit, je přesměrován na "/login".
        /// </summary>
        /// <returns></returns>
        public Task Initialize();

        public event Action? StateChanged;
    }

    public class UserContext : IUserContext
    {
        private readonly HttpClient _client;
        private readonly IBackendProvider _ibp;
        private ILocalStorageService _localStorageService;

        public UserContext(HttpClient client, IBackendProvider ibp, ILocalStorageService localStorageService)
        {
            this._client = client;
            this._ibp = ibp;
            this._localStorageService = localStorageService;
        }

        public bool IsLoggedIn { get; private set; } = false;
        public string Email { get; private set; } = String.Empty;

        private string _jwt = String.Empty;

        public async Task Initialize()
        {
            var jwt = await _localStorageService.GetItem<string>("jwt");
            if (jwt == null) 
                return;
            await Login(jwt);
        }

        public async Task Login(string jwtToken)
        {
            _client.DefaultRequestHeaders.Remove("Authorization");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

            try
            {

                var res = await _ibp.GetMyInfo();

                if (res.Response.IsSuccessStatusCode)
                {
                    IsLoggedIn = true;
                    Email = res.Data.Email;
                    _jwt = jwtToken;
                    await _localStorageService.SetItem("jwt", jwtToken);
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
                IsLoggedIn = false;
                Email = String.Empty;
                _jwt = String.Empty;
                _client.DefaultRequestHeaders.Remove("Authorization");
                await _localStorageService.RemoveItem("jwt");

                //invalidate refresh token on server side
                (await _ibp.Logout()).ThrowWhenUnsuccessful();
            }
            finally
            {
                NotifyStateChanged();
            }
        }
    }
}
