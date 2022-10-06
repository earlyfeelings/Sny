namespace Sny.Web.Model
{
    public record BackendApiCredentials(string Jwt, string RefreshToken, DateTime ExpireAtUtc);
}
