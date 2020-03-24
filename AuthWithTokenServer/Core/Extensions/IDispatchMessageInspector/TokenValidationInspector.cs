using AuthWithTokenServer.Core.Decoder.BasicAuthentication;
using AuthWithTokenServer.Core.Validator.TokenValidator;
using AuthWithTokenServer.Core.Validator.UserCredentialsValidator;
using AuthWithTokenServer.Infrastructure;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using AuthWithTokenServer.Dtos.Core.Error;
using AuthWithTokenServer.Dtos.Core.Validator;

namespace AuthWithTokenServer.Core.Extensions.IDispatchMessageInspector
{
    public class TokenValidationInspector : System.ServiceModel.Dispatcher.IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            if (WebOperationContext.Current == null)
            {
                throw new WebFaultException<RequestErrorDto>(new RequestErrorDto
                {
                    StatusCode = (int)HttpStatusCode.Forbidden,
                    Reason = "Incorrect Request",
                    Details = "Please, check your request path."
                }, HttpStatusCode.BadRequest);
            }

            string token = WebOperationContext.Current.IncomingRequest.Headers["Token"];

            if (!string.IsNullOrWhiteSpace(token))
            {
                TokenValidation(token);
            }
            else
            {
                BasicAuthenticationValidation();
            }

            return null;


        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        { }

        #region PRIVATE Helper Methods

        private static void TokenValidation(string secureToken)
        {
            using (AuthenticationExampleDbContext context = new AuthenticationExampleDbContext())
            {
                UserDto userDto = new TokenValidator(context).TokenIsValid(secureToken);

                if (userDto == null)
                {
                    throw new WebFaultException<RequestErrorDto>(
                        new RequestErrorDto
                        {
                            StatusCode = (int)HttpStatusCode.Forbidden,
                            Reason = "Permission Denied! (Expired/ Invalid Token)",
                            Details = "Please sign in to access this service!"
                        }, HttpStatusCode.Forbidden);
                }

                WebOperationContext.Current?.IncomingRequest.Headers.Add("UserName", userDto.UserName);
                WebOperationContext.Current?.IncomingRequest.Headers.Add("UserID", userDto.Id.ToString());
            }
        }

        private static void BasicAuthenticationValidation()
        {
            string authorizationHeaderAttribute = WebOperationContext.Current?.IncomingRequest.Headers["Authorization"];

            if (!string.IsNullOrWhiteSpace(authorizationHeaderAttribute))
            {
                using (AuthenticationExampleDbContext context = new AuthenticationExampleDbContext())
                {
                    BasicAuthenticationDecoder basicAuthenticationDecoder =
                        new BasicAuthenticationDecoder(authorizationHeaderAttribute);

                    UserDto userDto = new UserCredentialsValidator(context).CredentialsIsValid(basicAuthenticationDecoder.GetUserCredentials());

                    if (userDto == null)
                    {
                        throw new WebFaultException<RequestErrorDto>(
                            new RequestErrorDto
                            {
                                StatusCode = (int)HttpStatusCode.Unauthorized,
                                Reason = "Permission Denied! ",
                                Details = "The User Credentials are incorrect or the cookie has been changed. (Change suspected!)"
                            }, HttpStatusCode.Unauthorized);
                    }

                    WebOperationContext.Current.IncomingRequest.Headers.Add("UserID", userDto.Id.ToString());
                    WebOperationContext.Current.IncomingRequest.Headers.Add("UserName", userDto.UserName);
                }
            }
            else
            {
                throw new WebFaultException<RequestErrorDto>(
                    new RequestErrorDto
                    {
                        StatusCode = (int)HttpStatusCode.Forbidden,
                        Reason = "Permission Denied!",
                        Details = "Please sign in to access this service!"
                    }, HttpStatusCode.Forbidden);
            }
        }

        #endregion
    }
}