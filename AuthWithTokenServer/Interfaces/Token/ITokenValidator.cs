namespace AuthWithTokenServer.Interfaces.Token
{
    /// <summary>
    ///     Interfész. Tartalmazza a Token validáláshoz
    ///     szükséges metódusokat.
    /// </summary>
    interface ITokenValidator
    {
        Models.DBModel.Token Token { get; set; }

        bool IsValid(string Token);
    }
}