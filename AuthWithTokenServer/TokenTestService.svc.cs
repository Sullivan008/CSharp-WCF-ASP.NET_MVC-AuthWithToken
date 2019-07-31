using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace AuthWithTokenServer
{
    /// <summary>
    ///     A szolgáltatás metódusa csak abban az esteben futnak le
    ///     ha a Header-ben található-e Token, illetve ha az a Token
    ///     Valid-e
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TokenTestService
    {
        /// <summary>
        ///     Visszaadja a használt Valid Token-hez tartozó User nevet
        ///     és a hozzá tartozó ID-ját
        /// </summary>
        /// <returns>A felhasználónév és a hozzá tartozó ID JSON formátumban</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        public string TestGetWithTokenHeader()
        {
            return string.Format("A megadott Token működik! A bejelentkezett felhasználó neve: {0}" +
                " A bejelentkezett felhasználó ID-ja: {1}",
                 WebOperationContext.Current.IncomingRequest.Headers["UserName"],
                 WebOperationContext.Current.IncomingRequest.Headers["UserID"]);
        }

        /// <summary>
        ///     Visszaadja a használt Valid BasicAuth-hoz tartozó User nevet
        ///     és a hozzá tartozó ID-ját
        /// </summary>
        /// <returns>A felhasználónév és a hozzá tartozó ID JSON formátumban</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        public string TestPostWithBasicAuthHeader()
        {
            return string.Format("A megadott bejelentkezési adatok megfelelőek! A bejelentkezett felhasználó neve: {0}" +
                " A bejelentkezett felhasználó ID-ja: {1}",
                 WebOperationContext.Current.IncomingRequest.Headers["UserName"],
                 WebOperationContext.Current.IncomingRequest.Headers["UserID"]);
        }
    }
}
