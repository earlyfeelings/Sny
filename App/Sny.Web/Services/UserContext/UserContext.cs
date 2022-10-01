using Sny.Api.Dtos.Models.Accounts;
using Sny.Web.Services.BackendProvider;
using System.Net.Http.Json;

namespace Sny.Web.Services.UserContext
{

    public interface IUserContext
    {
        public bool IsLoggedIn { get; }

        public string Email { get; }

        public Task Login(string JwtToken);

        public void Logout();

        public event Action? StateChanged;
    }

    public class UserContext : IUserContext
    {
        private readonly HttpClient _client;
        private readonly IBackendProvider _ibp;

        public UserContext(HttpClient client, IBackendProvider ibp)
        {
            this._client = client;
            this._ibp = ibp;
        }

        private static string defaultUser =  "unknown@user.cz";
        public bool IsLoggedIn { get; private set; } = false;
        public string Email { get; private set; } = defaultUser;

        private string _jwt = String.Empty;

        public async Task Login(string jwtToken)
        {

            _client.DefaultRequestHeaders.Remove("Authorization");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

            var res = await _client.GetFromJsonAsync<MyInfoResponseDto>(_ibp.GetUri("account/myinfo"));
            if (res != null)
            {
                IsLoggedIn = true;
                Email = res.Email;
                _jwt = jwtToken;
            }
            else
            {
                Logout();
            }
            NotifyStateChanged();
        }

        public event Action? StateChanged;

        private void NotifyStateChanged()
        {
            StateChanged?.Invoke();
        }

        public void Logout()
        {
            IsLoggedIn = false;
            Email = defaultUser;
            _jwt = String.Empty;
            _client.DefaultRequestHeaders.Remove("Authorization");
        }
    }
}
