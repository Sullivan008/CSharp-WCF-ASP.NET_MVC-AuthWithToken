using AuthWithTokenServer.Models;

namespace AuthWithTokenServer.Interfaces.Token
{
    /// <summary>
    ///     Interfész. Tartalmazza a Token Generálásához
    ///     szükséges metódust
    /// </summary>
    interface ITokenBuilder
    {
        string Build(Credentials credentials);
    }
}