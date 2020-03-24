using AuthWithTokenServer.Core.TokenBuilder;
using AuthWithTokenServer.Core.Validator.UserCredentialsValidator;
using AuthWithTokenServer.Dtos.Core.Error;
using AuthWithTokenServer.Dtos.Core.TokenBuilder;
using AuthWithTokenServer.Dtos.Core.Validator;
using AuthWithTokenServer.Dtos.Service.AuthenticationTokenService.Authentication;
using AuthWithTokenServer.Infrastructure;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace AuthWithTokenServer
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class AuthenticationTokenService
    {
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        public AuthenticateResponseDto Authenticate(UserCredentialsViewModel credentialsViewModel)
        {
            using (AuthenticationExampleDbContext context = new AuthenticationExampleDbContext())
            {
                UserDto userDto = new UserCredentialsValidator(context).CredentialsIsValid(new UserCredentialsDto
                {
                    UserName = credentialsViewModel.UserName,
                    Password = credentialsViewModel.Password
                });

                if (userDto != null)
                {
                    return new AuthenticateResponseDto
                    {
                        SecureToken = new TokenBuilder(context).TokenBuild(new LoggedUserDto
                        {
                            UserId = userDto.Id,
                            UserName = userDto.UserName,
                            Password = credentialsViewModel.Password
                        })
                    };
                }
            }

            throw new WebFaultException<RequestErrorDto>(
                new RequestErrorDto
                {
                    StatusCode = (int)HttpStatusCode.Forbidden,
                    Reason = "Authentication Error!",
                    Details = "Username/ password pair is incorrect!"
                }, HttpStatusCode.Unauthorized);
        }
    }
}