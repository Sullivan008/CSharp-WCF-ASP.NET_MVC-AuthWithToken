using AuthWithTokenServer.Classes.BasicAuthFunctions;
using AuthWithTokenServer.Classes.Token;
using AuthWithTokenServer.Classes.Validator;
using AuthWithTokenServer.Interfaces.Token;
using AuthWithTokenServer.Interfaces.Validator;
using AuthWithTokenServer.Models.DBModel;
using AuthWithTokenServer.Models.Errors;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;

namespace AuthWithTokenServer.Classes.IDispatchMessageInspectorFunctions
{
    /// <summary>
    ///     Az üzenetet ellenőrző osztály
    /// </summary>
    public class TokenValidationInspector : IDispatchMessageInspector
    {
        /// <summary>
        ///     A következőképp fut le: A bejövő kérelem után, de mielőtt a választ küldené a tervezet
        ///     műveletnek. Vizsgálja, a kérelem válaszküldés előtt, hogy a Header-ben található Token
        ///     létezik-e, és ha igen, akkor valid-e. Ha nem létezik, akkor a BasicAuth-nak megfelelő
        ///     Header-rel vizsgálja, hogy Valid-e a User
        /// </summary>
        /// <param name="request">A kérés üzenete</param>
        /// <param name="channel">A bejövő csatorna</param>
        /// <param name="instanceContext">Az aktuális szolgáltatási példány</param>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            BadRequestInvestigation();

            /// A Token kiolvasása a Header-ből
            var token = WebOperationContext.Current.IncomingRequest.Headers["Token"];

            /// Vizsgálat, hogy a Header tartalmazott-e Token-t. Ha igen, akkor
            /// Token-nel történik meg a validálás, ha nem akkor vizsgálat, hogy a
            /// Header tartalmazza e a Basic Auth. Token-jét
            if (!string.IsNullOrWhiteSpace(token))
            {
                ValidateToken(token);
            }
            else
            {
                ValidateBasicAuthentication();
            }

            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {

        }

        #region Helpers
        /// <summary>
        ///     Vizsgálat, hogy a Paraméterben kapott Token 
        ///     valid-e. Ha nem, akkor a hozzáférést nem engedélyezzük
        /// </summary>
        /// <param name="token">A vizsgálandó Token</param>
        private void ValidateToken(string token)
        {
            using (var authDBContext = new AuthenticationEntities())
            {
                ITokenValidator validator = new DBTokenValidator(authDBContext);

                if (!validator.IsValid(token))
                {
                    throw new WebFaultException<RequestErrorData>(
                        new RequestErrorData((int)HttpStatusCode.Forbidden, "Hozzáférés megtagadva", "Kérem jelentkezzen be a szolgáltatás eléréséhez! (Lejárt/ Érvénytelen Token)"),
                        HttpStatusCode.Forbidden);
                }
                else
                {
                    WebOperationContext.Current.IncomingRequest.Headers.Add("UserName", validator.Token.User.Name);
                    WebOperationContext.Current.IncomingRequest.Headers.Add("UserID", validator.Token.User.Id.ToString());
                }
            }
        }

        /// <summary>
        ///     Megvizsgáljuk, hogy a Basic Auth. által elkészített Authorization Kulcs tartalmaz-e
        ///     valamilyen értéket. Ha igen és az Valid, akkor engedélyezzük a lekérdezést.
        ///     Különben pedig elutasítjuk
        /// </summary>
        private void ValidateBasicAuthentication()
        {
            var authorization = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];

            /// Vizsgálat, hogy az Authorization Kulcs tartalmaz-e értéket
            if (!string.IsNullOrWhiteSpace(authorization))
            {
                using (var authDBContext = new AuthenticationEntities())
                {
                    var basicAuth = new BasicAuth(authorization);

                    ICredentialsValidator validator = new DBCredentialsValidator(authDBContext);

                    /// Vizsgálat, hogy a Header-ből kiolvasott hitelesítési adatok Validak-e még
                    if (!validator.IsValid(basicAuth.Credentials))
                    {
                        throw new WebFaultException<RequestErrorData>(
                            new RequestErrorData((int)HttpStatusCode.Unauthorized, "Hozzáférés megtagadva", "Kérem jelentkezzen be a szolgáltatás eléréséhez!" +
                            "Nem megfelelőek a hitelesítési adatok, vagy a Cookie módosult (Módosítás gyanúja lépett fel!)"),
                            HttpStatusCode.Unauthorized);
                    }
                    else
                    {
                        WebOperationContext.Current.IncomingRequest.Headers.Add("UserName", validator.User.Name);
                        WebOperationContext.Current.IncomingRequest.Headers.Add("UserID", validator.User.Id.ToString());
                    }
                }
            }
            else
            {
                throw new WebFaultException<RequestErrorData>(
                    new RequestErrorData((int)HttpStatusCode.Forbidden, "Hozzáférés megtagadva", "Kérem jelentkezzen be a szolgáltatás eléréséhez!"),
                    HttpStatusCode.Forbidden);
            }
        }

        /// <summary>
        ///     Megvizsgálja, hogy a kérelem üres-e. Ha igen, akkor
        ///     WebFaultException dobása
        /// </summary>
        private void BadRequestInvestigation()
        {
            if (WebOperationContext.Current == null)
            {
                throw new WebFaultException<RequestErrorData>(
                        new RequestErrorData((int)HttpStatusCode.Forbidden, "Hibás kérelem", "Kérem ellenőrizze az elérési útvonalat"),
                        HttpStatusCode.BadRequest);
            }
        }
        #endregion
    }
}