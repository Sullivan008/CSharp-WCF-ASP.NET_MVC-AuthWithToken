using AuthWithTokenServer.Classes.Validator;
using AuthWithTokenServer.Interfaces.Token;
using AuthWithTokenServer.Models;
using AuthWithTokenServer.Models.DBModel;
using System;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;

namespace AuthWithTokenServer.Classes.Token
{
    public class DBTokenBuilder : ITokenBuilder
    {
        private readonly AuthenticationEntities authDBContext;

        /// <summary>
        ///     Konstruktor. 
        /// </summary>
        /// <param name="authDBContext">Az Authenticaton DB Contextus, amely a Szolgáltatásban példányosult</param>
        public DBTokenBuilder(AuthenticationEntities authDBContext)
        {
            this.authDBContext = authDBContext;
        }

        /// <summary>
        ///     Ha a hitelesítési adatok validatk, akkor generál egy
        ///     Token-t amely a felhasználóhoz fog tartozni, majd ezt a 
        ///     Token-t lementi a DB Token táblájába, mint létező Token
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public string Build(Credentials credentials)
        {
            if (!new DBCredentialsValidator(authDBContext).IsValid(credentials))
            {
                throw new AuthenticationException();
            }

            var secureToken = BuildSecureToken();

            SaveSecureTokenInDBForValidUser(GetUser(credentials), secureToken);

            return secureToken;
        }

        #region Helpers
        /// <summary>
        ///     Elkészít egy egyedi, Biztonsági Token-t.
        ///     Használja: RNGCryptoServiceProvider, Convert.ToBase64String
        /// </summary>
        /// <returns>Az egyedileg elkészített Biztonsági Token</returns>
        private string BuildSecureToken()
        {
            int tokenSize = 100;
            var buffer = new byte[tokenSize];

            /// Buffer feltöltése, amely Kryptográfiailag erős. Véletlenszerű nem nulla értékekkel
            /// fogja feltölteni
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rngCryptoServiceProvider.GetNonZeroBytes(buffer);
            }

            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        ///     A paraméterként megkapott Biztonsági Token-t elmenti az
        ///     adatbázisba Valid User-hez
        /// </summary>
        /// <param name="user">User objektum, amely tartalmaz minden olyan adatot, amely a User táblában található</param>
        /// <param name="secureToken">Egyedileg elkészített biztonsági Token</param>
        private void SaveSecureTokenInDBForValidUser(User user, string secureToken)
        {
            authDBContext.Tokens.Add(CreateToken(user, secureToken));
            authDBContext.SaveChanges();
        }

        /// <summary>
        ///     Elkészít egy Token objektumot, amely az DB Token táblájának
        ///     megfelelő formátumú
        /// </summary>
        /// <param name="user">User objektum, amely tartalmaz minden olyan adatot, amely a User táblában található</param>
        /// <param name="secureToken">Egyedileg elkészített biztonsági Token</param>
        /// <returns></returns>
        private Models.DBModel.Token CreateToken(User user, string secureToken)
        {
            return new Models.DBModel.Token
            {
                SecureToken = secureToken,
                User = user,
                CreateDate = DateTime.Now
            };
        }

        /// <summary>
        ///     Lekérdezi a User táblából azokat az adatokat
        ///     amely a felhasználó név által megadott felhasználó
        ///     névvel megegyezik. (Csak 1 rekordot ad vissza)
        /// </summary>
        /// <param name="credentials">A felhasználó ááltal küldött hitelesítési adatokat tartalmazó objektum</param>
        /// <returns>
        ///     User objektum, amely tartalmaz minden olyan adatot, amely a User táblában
        ///     található
        /// </returns>
        private User GetUser(Credentials credentials)
        {
            return authDBContext.Users.SingleOrDefault(x => x.Name.Equals(credentials.User, StringComparison.CurrentCultureIgnoreCase));
        }
        #endregion
    }
}