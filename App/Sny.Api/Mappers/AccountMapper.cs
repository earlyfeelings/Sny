using Sny.Api.Dtos.Models.Accounts;
using Sny.Core.Interfaces.Core;

namespace Sny.Api.Mappers
{
    public static class AccountMapper
    {
        public static RegisterModel ToRegisterModel(this RegisterRequestDto model)
        {
            return new RegisterModel(model.Email, model.Password, model.PasswordAgain);
        }

        public static RegisterResponseDto ToRegisterResponseModel(this RegisterResult model)
        {
            return new RegisterResponseDto(MapStatus(model.RegisterStatus));
        }

        private static RegisterStatusDto MapStatus(RegisterStatus registerStatus)
        {
            return registerStatus switch
            {
                RegisterStatus.BadEmailFormat => RegisterStatusDto.BadEmailFormat,
                RegisterStatus.WeakPassword => RegisterStatusDto.WeakPassword,
                RegisterStatus.AlreadyExists => RegisterStatusDto.AlreadyExists,
                RegisterStatus.Error => RegisterStatusDto.Error,
                RegisterStatus.Success => RegisterStatusDto.Success,
                RegisterStatus.PasswordsNotSame => RegisterStatusDto.PasswordsNotSame,
                _ => RegisterStatusDto.Error


            };
        }
    }
}
