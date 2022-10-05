namespace Sny.Api.Options
{
    public class JwtOptions
    {
        public string Secret { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public int TokenValidityInMinutes { get; set; } = 10;
        public int RefreshTokenValidityInDays { get; set; } = 7;
    }
}
