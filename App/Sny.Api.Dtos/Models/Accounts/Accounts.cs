using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sny.Api.Dtos.Models.Accounts
{
    public record LoginRequestDto(string Email, string Password);

    public record RegisterRequestDto(string Email, string Password, string PasswordAgain);

    public record LoginResponseDto(string Jwt, string RefreshToken, DateTime ExpiryAtUtc);

    public record MyInfoResponseDto(string Email);

    public record RegisterResponseDto(RegisterStatusDto RegisterStatus);

    public record LogoutRequestDto(string RefreshToken);

    public enum RegisterStatusDto
    {
        Success,
        AlreadyExists,
        WeakPassword,
        PasswordsNotSame,
        BadEmailFormat,
        Error
    }
}
