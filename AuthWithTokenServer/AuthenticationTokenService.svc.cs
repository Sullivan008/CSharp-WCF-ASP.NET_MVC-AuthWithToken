using AuthWithTokenServer.Classes.BasicAuthFunctions;
using AuthWithTokenServer.Classes.Token;
using AuthWithTokenServer.Classes.Validator;
using AuthWithTokenServer.Interfaces.Validator;
using AuthWithTokenServer.Models;
using AuthWithTokenServer.Models.DBModel;
using AuthWithTokenServer.Models.Errors;
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
        /// <summary>
        ///     A paraméterként megkapott hitelesítési adatoknak
        ///     megfelelően Authenticáltat egy User-t.
        ///     A Kapott adatokat JSON objektumként, POST-ban kapja
        ///     meg a Klienstől
        /// </summary>
        /// <param name="credentials">Hitelesítési adatokat tartalmazó objektum</param>
        /// <returns>
        ///     Ha a hitelesítési adatok megfelelőek        -> Egy statikus Token-t térít vissza
        ///     Ha a hitelesítési adatok nem megfelelőek    -> InvalidCredentialException()
        /// </returns>
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        public string Authenticate(Credentials credentials)
        {
            /// Basic authentication végrehajtása
            if (credentials == null && WebOperationContext.Current != null)
            {
                credentials = new BasicAuth(WebOperationContext.Current.IncomingRequest.Headers["Authorization"]).Credentials;
            }

            /// Valid Token generálás a Valid User-hez
            using (var authDBContext = new AuthenticationEntities())
            {
                ICredentialsValidator credentialsValidator = new DBCredentialsValidator(authDBContext);

                if (credentialsValidator.IsValid(credentials))
                {
                    return new DBTokenBuilder(authDBContext).Build(credentials);
                }

                throw new WebFaultException<RequestErrorData>(
                    new RequestErrorData((int)HttpStatusCode.Forbidden, "Authentikációs hiba", "Nem megfelelőek az authentikációs adatok"),
                    HttpStatusCode.Unauthorized);
            }
        }
    }
}
