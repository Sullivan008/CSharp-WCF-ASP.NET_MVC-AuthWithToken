using AuthWithTokenServer.Dtos.Core.Validator;

namespace AuthWithTokenServer.Core.Validator.TokenValidator
{
    public interface ITokenValidator
    {
        UserDto TokenIsValid(string secureToken);
    }
}