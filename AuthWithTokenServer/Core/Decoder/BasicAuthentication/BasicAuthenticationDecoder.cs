using System;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using AuthWithTokenServer.Dtos.Core.Error;
using AuthWithTokenServer.Dtos.Core.Validator;

namespace AuthWithTokenServer.Core.Decoder.BasicAuthentication
{
    public class BasicAuthenticationDecoder : IBasicAuthenticationDecoder
    {
        private readonly string _authorizationHeaderAttribute;
        private readonly Encoding _authorizationHeaderEncoding;

        private const string AuthenticationTypePrefix = "Basic ";

        public BasicAuthenticationDecoder(string authorizationHeaderAttribute) : this(authorizationHeaderAttribute, Encoding.UTF8)
        { }

        public BasicAuthenticationDecoder(string authorizationHeaderAttribute, Encoding authorizationHeaderEncoding)
        {
            _authorizationHeaderAttribute =
                authorizationHeaderAttribute ?? throw new ArgumentNullException(nameof(authorizationHeaderAttribute));
            _authorizationHeaderEncoding = authorizationHeaderEncoding ?? throw new ArgumentNullException(nameof(authorizationHeaderEncoding));
        }

        public UserCredentialsDto GetUserCredentials()
        {
            string decodedAuthorizationHeaderAttribute = _authorizationHeaderAttribute.StartsWith(AuthenticationTypePrefix, StringComparison.OrdinalIgnoreCase) ?
                _authorizationHeaderEncoding.GetString(Convert.FromBase64String(_authorizationHeaderAttribute.Substring(AuthenticationTypePrefix.Length))) :
                _authorizationHeaderEncoding.GetString(Convert.FromBase64String(_authorizationHeaderAttribute));

            string[] userCredentialsArray = decodedAuthorizationHeaderAttribute.Split(':');

            if (userCredentialsArray.Length < 2)
            {
                throw new WebFaultException<RequestErrorDto>(
                    new RequestErrorDto
                    {
                        StatusCode = (int)HttpStatusCode.Forbidden,
                        Reason = "Authentication Error",
                        Details = "Authentication data is incorrect! (Username/ Password)"
                    }, HttpStatusCode.Unauthorized);
            }

            return new UserCredentialsDto
            {
                UserName = userCredentialsArray[0],
                Password = userCredentialsArray[1]
            };
        }
    }
}