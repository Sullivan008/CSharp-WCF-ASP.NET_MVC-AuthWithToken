using AuthWithTokenServer.Dtos.Core.Validator;

namespace AuthWithTokenServer.Core.Validator.UserCredentialsValidator
{
    public interface IUserCredentialsValidator
    {
        UserDto CredentialsIsValid(UserCredentialsDto userCredentials);
    }
}