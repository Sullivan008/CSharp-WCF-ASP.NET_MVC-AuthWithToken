using AuthWithTokenServer.Models;
using AuthWithTokenServer.Models.Errors;
using System;
using System.Net;
using System.ServiceModel.Web;
using System.Text;

namespace AuthWithTokenServer.Classes.BasicAuthFunctions
{
    /// <summary>
    ///     Az alapvető hitelesítési fejléc kezeléséhez
    ///     elkészített osztály. Kezeljük kódoláshoz, dekódoláshoz
    ///     az alapvető hitelesítési adatokat
    /// </summary>
    public class BasicAuth
    {
        private readonly string UserName;
        private readonly string Password;
        private const string AuthenticationType = "Basic ";
        private Credentials credentials;

        public string HeaderValue { get; }

        /// Visszaadja a Hitelesítési adatokat az osztályban
        /// található privaet objektumból. Ha az null, akkor inicializálja
        /// majd az inicializált objektumot térití vissza.
        public Credentials Credentials
        {
            get { return credentials ?? (credentials = new Credentials { User = UserName, Password = this.Password }); }
        }

        #region Constructors
        #region BasicAuth Decode Constructors
        /// <summary>
        ///     Ha csak a Header-t adjuk meg a konstruktornak
        ///     akkor a (string, Encoding) konstruktor fog meghívódni
        ///     alapértelmezett UTF8-as karakterkódolással
        /// </summary>
        /// <param name="encodedHeader">A kódolt Header</param>
        public BasicAuth(string encodedHeader) : this(encodedHeader, Encoding.UTF8)
        {

        }

        /// <summary>
        ///     Header dekódolását végrehajtó konstruktor. A Header-ben található
        ///     Authorize kulcs-ban található érték Base64-é kódolódik le, amikor
        ///     Basic Authorization-t használunk, úgyhogy Base64-ről kell visszafejteni
        ///     az adatokat
        /// </summary>
        /// <param name="encodedHeader">A kódolt header</param>
        /// <param name="encoding">A kódolás karakterkódolása</param>
        public BasicAuth(string encodedHeader, Encoding encoding)
        {
            HeaderValue = encodedHeader;

            try
            {
                /// Header dekódolása. Ha a Header a Prefix karakterlánccal kezdődik, akkor azt levágva
                /// történik a dekódolás. Ellenkező esetben nem kell semmit levágni, tehát az egész header
                /// tartalma dekódolható
                var decodedHeader = encodedHeader.StartsWith(AuthenticationType, StringComparison.OrdinalIgnoreCase)
                    ? encoding.GetString(Convert.FromBase64String(encodedHeader.Substring(AuthenticationType.Length)))
                    : encoding.GetString(Convert.FromBase64String(encodedHeader));

                /// A hitelesítési adatok kiolvasása. A karakterlánc darabolása a ":" karaktereknél
                string[] credentialArray = decodedHeader.Split(':');

                ///  A Hitelesítési adatok inicializálását végrehajtó metódus
                ///  A hitelesítési adatokat tartalmazó tömb 0 elemet tartalmazhat!!!
                for (int i = 0; i < credentialArray.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            UserName = credentialArray[i];
                            break;
                        case 1:
                            Password = credentialArray[i];
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
                throw new WebFaultException<RequestErrorData>(
                    new RequestErrorData((int)HttpStatusCode.Forbidden, "Authentikációs hiba", "Nem megfelelőek az authentikációs adatok"),
                    HttpStatusCode.Unauthorized);
            }
        }
        #endregion

        #region BasicAuth Encode Constructors
        /// <summary>
        ///     Ha csak a UserName-t és Password-ot adjuk meg a konstruktornak
        ///     akkor a (userName, password, Encoding) konstruktor fog meghívódni
        ///     alapértelmezett UTF8-as karakterkódolással
        /// </summary>
        /// <param name="userName">Felhasználónév</param>
        /// <param name="password">Jelszó</param>
        public BasicAuth(string userName, string password) : this(userName, password, Encoding.UTF8)
        {

        }

        /// <summary>
        ///     A dekódolt header elkészítésére elkészítésére szolgáló metódus.
        /// </summary>
        /// <param name="userName">Felhasználónév</param>
        /// <param name="password">Jelszó</param>
        /// <param name="encoding">A kódolás karakterkódolása</param>
        public BasicAuth(string userName, string password, Encoding encoding)
        {
            /// Property-k inicializálása
            UserName = userName;
            Password = password;

            /// Header
            HeaderValue = GetHeaderValue(encoding);
        }
        #endregion
        #endregion

        #region Helpers
        /// <summary>
        ///     Elkészít majd visszaad egy kódolt Header-t amelyben a hitelesítési
        ///     adatok taláhlatóak
        /// </summary>
        /// <param name="encoding">A kódolás karakterkódolása</param>
        /// <returns>Kódolt Header, amelyben a hitelesítési adatok találhatóak</returns>
        private string GetHeaderValue(Encoding encoding)
        {
            return AuthenticationType + Convert.ToBase64String(encoding.GetBytes(string.Format("{0}:{1}", UserName, Password)));
        }
        #endregion
    }
}