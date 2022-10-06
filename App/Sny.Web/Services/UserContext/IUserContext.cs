using Sny.Web.Model;

namespace Sny.Web.Services.UserContext
{
    public interface IUserContext
    {
        public bool IsLoggedIn { get; }
        public string Email { get; }
        public Task Login(BackendApiCredentials credentials);
        public Task Logout();

        /// <summary>
        /// Pokusí se načíst JWT token z User storage a provést přihlášení.
        /// Pokud se nepodaří uživatele přihlásit, je přesměrován na "/login".
        /// </summary>
        /// <returns></returns>
        public Task Initialize();

        public event Action? StateChanged;
    }
}
