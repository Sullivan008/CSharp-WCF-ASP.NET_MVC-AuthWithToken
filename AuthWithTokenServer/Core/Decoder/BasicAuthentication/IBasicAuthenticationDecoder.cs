using AuthWithTokenServer.Dtos.Core.Validator;

namespace AuthWithTokenServer.Core.Decoder.BasicAuthentication
{
    public interface IBasicAuthenticationDecoder
    {
        UserCredentialsDto GetUserCredentials();
    }
}