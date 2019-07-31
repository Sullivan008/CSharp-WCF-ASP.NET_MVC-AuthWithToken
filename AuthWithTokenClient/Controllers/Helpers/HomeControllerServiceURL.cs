namespace AuthWithTokenClient.Controllers.Helpers
{
    public class HomeControllerServiceURL
    {
        /// <summary>
        ///     Visszaadja az Authentikációs szolgáltatás elérési útvonalát
        /// </summary>
        public static string AuthenticationServiceURL
        {
            get { return "https://localhost:44373/AuthenticationTokenService.svc"; }
        }

        /// <summary>
        ///     Visszaadj a TokenTestService szolgáltatás elérési útvonalát
        /// </summary>
        public static string TokenTestServiceURL
        {
            get { return "https://localhost:44373/TokenTestService.svc"; }
        }

        /// <summary>
        ///     Visszaadja a bejelentkezéshez szükséges Szolgáltatás metódusát
        /// </summary>
        public static string AuthenticationMethodName
        {
            get { return "Authenticate"; }
        }

        /// <summary>
        ///     Visszaadja a Token Test-hez szükséges Szolgáltatás metódusát
        /// </summary>
        public static string TestGetWithTokenHeaderMethodName
        {
            get { return "TestGetWithTokenHeader"; }
        }

        /// <summary>
        ///     Visszaadja a BasicAuth Test-hez szükséges Szolgáltatás metódusát
        /// </summary>
        public static string TestPostWithBasicAuthHeaderMethodName
        {
            get { return "TestPostWithBasicAuthHeader"; }
        }
    }
}