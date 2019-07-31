using AuthWithTokenServer.Models;
using AuthWithTokenServer.Models.DBModel;

namespace AuthWithTokenServer.Interfaces.Validator
{
    /// <summary>
    ///     Interfész. Tartalmazza a hitelesítési adatok
    ///     validáláshoz szükséges metódusokat.
    /// </summary>
    interface ICredentialsValidator
    {
        User User { get; set; }

        bool IsValid(Credentials credentials);
    }
}