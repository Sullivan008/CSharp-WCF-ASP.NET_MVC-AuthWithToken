namespace AuthWithTokenClient.Controllers.ServiceUrls
{
    public static class HomeControllerServiceUrl
    {
        #region AUTHENTICATION Service

        public static string AuthenticationService =>
            "https://localhost:44373/AuthenticationTokenService.svc";

        public static string AuthenticationMethod =>
            "Authenticate";

        #endregion

        #region TEST Service

        public static string TestService =>
            "https://localhost:44373/TestService.svc";

        public static string TestGetWithTokenHeaderMethod => 
            "TestGetWithTokenHeader";
        public static string TestPostWithBasicAuthHeaderMethodName => 
            "TestPostWithBasicAuthHeader";

        #endregion
    }
}