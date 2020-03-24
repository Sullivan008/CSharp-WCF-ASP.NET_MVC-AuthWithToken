using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using AuthWithTokenServer.Dtos.Service.TestService.BasicAuthTest;
using AuthWithTokenServer.Dtos.Service.TestService.TokenTest;

namespace AuthWithTokenServer
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TestService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        public TokenTestDto TestGetWithTokenHeader()
        {
            return new TokenTestDto
            {
                Message = "The specified Token works!",
                UserId = int.Parse(WebOperationContext.Current?.IncomingRequest.Headers["UserID"] ?? throw new ArgumentNullException(nameof(TokenTestDto.UserId))),
                UserName = WebOperationContext.Current?.IncomingRequest.Headers["UserName"]
            };
        }

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        public BasicAuthTestDto TestPostWithBasicAuthHeader()
        {
            return new BasicAuthTestDto
            {
                Message = "The user credentials you entered are correct!",
                UserId = int.Parse(WebOperationContext.Current?.IncomingRequest.Headers["UserID"] ?? throw new ArgumentNullException(nameof(TokenTestDto.UserId))),
                UserName = WebOperationContext.Current?.IncomingRequest.Headers["UserName"]
            };
        }
    }
}
