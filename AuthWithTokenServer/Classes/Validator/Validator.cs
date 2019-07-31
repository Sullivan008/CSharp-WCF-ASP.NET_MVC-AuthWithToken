using AuthWithTokenServer.Classes.EncryptDecrypt;
using AuthWithTokenServer.Interfaces.Validator;
using AuthWithTokenServer.Models;
using AuthWithTokenServer.Models.DBModel;
using System;
using System.Linq;

namespace AuthWithTokenServer.Classes.Validator
{
    public class DBCredentialsValidator : ICredentialsValidator
    {
        private readonly AuthenticationEntities authDBContext;

        public User User { get; set; }

        /// <summary>
        ///     Konstruktor. 
        /// </summary>
        /// <param name="authDBContext">Az Authenticaton DB Contextus, amely a Szolgáltatásban példányosult</param>
        public DBCredentialsValidator(AuthenticationEntities authDBContext)
        {
            this.authDBContext = authDBContext;
        }

        /// <summary>
        ///     Visszaadja, hogy a felhasználó által megadott hitelesítési adatok
        ///     valid-e
        /// </summary>
        /// <param name="credentials">A felhasználó által küldött hitelesítési adatokat tartalmazó objektum</param>
        /// <returns>TRUE - Ha megegyezik; FALSE - Ha nem egyezik meg</returns>
        public bool IsValid(Credentials credentials)
        {
            return UserIsValid(GetUser(credentials), credentials);
        }

        #region Helpers
        /// <summary>
        ///     Eldönti a felhasználó által megadott hitelesítési adatok alapján,
        ///     hogy megegyezik-e a DB-ben található hitelesítési adatokkal.
        ///     Ha van ilyen User a User táblában, akkor már csak összehasonlításra kerül
        ///     a felhasználó által, és a táblában található jelszó.
        /// </summary>
        /// <param name="user">User objektum, amely tartalmaz minden olyan adatot, amely a User táblában található</param>
        /// <param name="credentials">A felhasználó által küldött hitelesítési adatokat tartalmazó objektum</param>
        /// <returns>TRUE - Ha az adatok megegyeznek; FALSE - Ha pedig nem</returns>
        private bool UserIsValid(User user, Credentials credentials)
        {
            return (user != null &&
                Hash.Compare(credentials.Password, user.Salt, user.Password, Hash.DefaultHashType, Hash.DefaultEncoding));
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
            return User = authDBContext.Users.SingleOrDefault(x => x.Name.Equals(credentials.User, StringComparison.CurrentCultureIgnoreCase));
        }
        #endregion
    }
}