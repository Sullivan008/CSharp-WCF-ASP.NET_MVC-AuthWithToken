using AuthWithTokenServer.Interfaces.Token;
using AuthWithTokenServer.Models.DBModel;
using System;
using System.Linq;

namespace AuthWithTokenServer.Classes.Token
{
    public class DBTokenValidator : ITokenValidator
    {
        /// Tároljuk, hogy a Token mennyi ideig érvényes
        private static double DefaultSecondsUntilTokenExpires = 1800;
        private readonly AuthenticationEntities authDBContext;

        public Models.DBModel.Token Token { get; set; }

        /// <summary>
        ///     Konstruktor. 
        /// </summary>
        /// <param name="authDBContext">Az Authenticaton DB Contextus, amely a Szolgáltatásban példányosult</param>
        public DBTokenValidator(AuthenticationEntities authDBContext)
        {
            this.authDBContext = authDBContext;
        }

        /// <summary>
        ///     Megvizsgálja, hogy a praméterként kapott Token Valid-e
        /// </summary>
        /// <param name="SecureToken">A paraméterként kapott Secure Token</param>
        /// <returns>TRUE - Ha valid; FALSE - Ha nem</returns>
        public bool IsValid(string SecureToken)
        {
            return TokenIsValid(GetToken(SecureToken));
        }

        #region Helpers
        /// <summary>
        ///     Megvizsgálja, hogy a paraméterként kapott Token
        ///     objektum valid-e még
        /// </summary>
        /// <param name="token">Token objektum, amely minden információt tartalmaz a DB-ből lekérdezett Secure Token-ről</param>
        /// <returns>TRUE - Ha valid; FALSE - Ha nem</returns>
        private bool TokenIsValid(Models.DBModel.Token token)
        {
            return token != null && !IsExpired(token);
        }

        /// <summary>
        ///     Megvizsgálja, hogy a Token a megadott időtartományon belül 
        ///     van-e még. Azaz, hogy nem-e járt még le.
        /// </summary>
        /// <param name="token">Token objektum, amely minden információt tartalmaz a DB-ből lekérdezett Secure Token-ről</param>
        /// <returns>TRUE - Ha lejárt; False - Ha nem járt le</returns>
        private bool IsExpired(Models.DBModel.Token token)
        {
            return (DateTime.Now - token.CreateDate).TotalSeconds > DefaultSecondsUntilTokenExpires;
        }

        /// <summary>
        ///     Lekérdezi azt a sort az adatbázisból, amelynek megfelel
        ///     a paraméterként megadott SecureToken.
        /// </summary>
        /// <param name="SecureToken">Az adatbázis Token táblájában keresendő Secure Token</param>
        /// <returns>
        ///     Token objektum, amely tartalmaz minden olyan adatot, amely a Token táblában
        ///     található
        /// </returns>
        private Models.DBModel.Token GetToken(string SecureToken)
        {
            return Token = authDBContext.Tokens.SingleOrDefault(x => x.SecureToken == SecureToken);
        }
        #endregion
    }
}