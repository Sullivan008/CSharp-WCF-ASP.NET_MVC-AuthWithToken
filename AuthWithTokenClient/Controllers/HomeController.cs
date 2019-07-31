using AuthWithTokenClient.Controllers.Helpers;
using AuthWithTokenClient.Models.JSON;
using AuthWithTokenClient.Models.ViewModels;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace AuthWithTokenClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///     POST - Authenticate
        /// </summary>
        /// <param name="credentialsViewModel">A hitelesítési adatokat tartalmazó objektum</param>
        /// <returns>JSON Objektum - Amely tartalmazza a hitelesítéskor kapott TOKEN-t</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Authenticate(HomeViewModel homeViewModel)
        {
            /// A Szolgáltatás válasza
            string serviceResult = AuthenticationWebserviceRequest(homeViewModel.CredentialsViewModel);

            if (serviceResult.Contains("StatusCode"))
            {
                ResponseErrorData requestDataError = JsonConvert.DeserializeObject<ResponseErrorData>(serviceResult);

                return Json(new { requestDataError.StatusCode, requestDataError.Reason, requestDataError.InformationAbouotReason }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(serviceResult, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        ///     POST - GetTokenTest
        /// </summary>
        /// <param name="headersViewModel">ViewModel, amely tartalmazza a beállítandó Header-eket a Request Header-ben</param>
        /// <returns>JSON Objektum - Amely tartalmazza a szerver által visszaadott adatokat (Bejelentkezett USER - ID-val)</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetTokenTest(HomeViewModel homeViewModel)
        {
            string serviceResult = GetTokenTestWebserviceRequest(homeViewModel.HeadersViewModel);

            if (serviceResult.Contains("StatusCode"))
            {
                ResponseErrorData requestDataError = JsonConvert.DeserializeObject<ResponseErrorData>(serviceResult);

                return Json(new { requestDataError.StatusCode, requestDataError.Reason, requestDataError.InformationAbouotReason }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(serviceResult, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        ///     POST -  PostBasicAuthTest
        /// </summary>
        /// <param name="headersViewModel">ViewModel, amely tartalmazza a beállítandó Header-eket a Request Header-ben</param>
        /// <returns>JSON Objektum - Amely tartalmazza a szerver által visszaadott adatokat (Bejelentkezett USER - ID-val)</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PostBasicAuthTest(HomeViewModel homeViewModel)
        {
            string serviceResult = PostBasicAuthTestWebserviceRequest(homeViewModel.HeadersViewModel);

            if (serviceResult.Contains("StatusCode"))
            {
                ResponseErrorData requestDataError = JsonConvert.DeserializeObject<ResponseErrorData>(serviceResult);

                return Json(new { requestDataError.StatusCode, requestDataError.Reason, requestDataError.InformationAbouotReason }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(serviceResult, JsonRequestBehavior.AllowGet);
            }
        }

        #region Helpers
        /// <summary>
        ///     Metódus hívás, amely kérelmet küld az Authentikációs szolgáltatás Authenticate metódusa
        ///     felé
        /// </summary>
        /// <param name="credentialsViewModel">A hitelesítési adatokat tartalmazó bojektum</param>
        /// <returns>A szolgáltatás által küldött válasz</returns>
        private string AuthenticationWebserviceRequest(CredentialsViewModel credentialsViewModel)
        {
            try
            {
                /// A szolgáltatás URL-je
                string completServiceURL = string.Format("{0}/{1}", HomeControllerServiceURL.AuthenticationServiceURL, HomeControllerServiceURL.AuthenticationMethodName);

                /// HTTP Kérelem elkészítése/ bekonfigurálása
                HttpWebRequest httpWebRequest = CreateHttpWebRequest(completServiceURL);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                /// JSON Objektum elkészítése a Webszolgáltatás számára
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string jsonString = "{\"User\":\"" + credentialsViewModel.User + "\"," +
                        "\"Password\":\"" + credentialsViewModel.Password + "\"}";

                    streamWriter.Write(jsonString);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                /// HTTP Kérelem végrehajtása
                using (HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    /// A HttpResponse feldolgozása
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        /// A Válasz Token kiolvasása a Webszolgáltatás válaszából
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    if (ex.Response is HttpWebResponse response)
                    {
                        /// Visszatérése, ERROR String, amely egy JSON-string a benne található hiba adatokat tartalmazza
                        return new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    }
                }
                else if (ex.Status == WebExceptionStatus.ConnectFailure)
                {
                    if (ex.InnerException is Win32Exception innerException)
                    {
                        return "{ \"InformationAbouotReason\":\"" + innerException.Message + "\",\"Reason\":\"Kapcsolati hiba\",\"StatusCode\":" + innerException.ErrorCode + " }";
                    }
                }

                return null;
            }
        }

        /// <summary>
        ///     Metódus hívás, amely kérelmet küld a TestTokenService szolgáltatás TestGetWithTokenHeader
        ///     metódusa felé
        /// </summary>
        /// <param name="headersViewModel">ViewModel, amely tartalmazza a beállítandó Header-eket a Request Header-ben</param>
        /// <returns>A szolgáltatás válasza</returns>
        private string GetTokenTestWebserviceRequest(HeadersViewModel headersViewModel)
        {
            try
            {
                /// A szolgáltatás URL-je
                string completServiceURL = string.Format("{0}/{1}", HomeControllerServiceURL.TokenTestServiceURL, HomeControllerServiceURL.TestGetWithTokenHeaderMethodName);

                /// HTTP Kérelem elkészítése/ bekonfigurálása
                HttpWebRequest httpWebRequest = CreateHttpWebRequest(completServiceURL);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers.Add(nameof(HeadersViewModel.Token), headersViewModel.Token == null ? "" : (headersViewModel.Token.Replace("\"", "")).Replace("\\", ""));
                httpWebRequest.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                /// HTTP Kérelem végrehajtása
                using (HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    /// A HttpResponse feldolgozása
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        /// A Válasz Token kiolvasása a Webszolgáltatás válaszából
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    if (ex.Response is HttpWebResponse response)
                    {
                        /// Visszatérése, ERROR String, amely egy JSON-string a benne található hiba adatokat tartalmazza
                        return new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    }
                }
                else if (ex.Status == WebExceptionStatus.ConnectFailure)
                {
                    if (ex.InnerException is Win32Exception innerException)
                    {
                        return "{ \"InformationAbouotReason\":\"" + innerException.Message + "\",\"Reason\":\"Kapcsolati hiba\",\"StatusCode\":" + innerException.ErrorCode + " }";
                    }
                }

                return null;
            }
        }

        /// <summary>
        ///     Metdós hívás, amely kérelmet küld a TestTokenService szolgáltatás TestPostWithBasicAuthHeader
        ///     metódusa felé
        /// </summary>
        /// <param name="headersViewModel">ViewModel, amely tartalmazza a beállítandó Header-eket a Request Header-ben</param>
        /// <returns>A Szolgáltatás válasza</returns>
        private string PostBasicAuthTestWebserviceRequest(HeadersViewModel headersViewModel)
        {
            try
            {
                /// A szolgáltatás URL-je
                string completServiceURL = string.Format("{0}/{1}", HomeControllerServiceURL.TokenTestServiceURL, HomeControllerServiceURL.TestPostWithBasicAuthHeaderMethodName);

                /// HTTP Kérelem elkészítése/ bekonfigurálása
                HttpWebRequest httpWebRequest = CreateHttpWebRequest(completServiceURL);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add(nameof(HeadersViewModel.Authorization), headersViewModel.Authorization);
                httpWebRequest.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                /// HTTP Kérési Stream elkérése
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    /// HTTP Kérelem végrehajtása
                    using (HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                    {
                        /// A HttpResponse feldolgozása
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            /// A Válasz Token kiolvasása a Webszolgáltatás válaszából
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    if (ex.Response is HttpWebResponse response)
                    {
                        /// Visszatérése, ERROR String, amely egy JSON-string a benne található hiba adatokat tartalmazza
                        return new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    }
                }
                else if (ex.Status == WebExceptionStatus.ConnectFailure)
                {
                    if (ex.InnerException is Win32Exception innerException)
                    {
                        return "{ \"InformationAbouotReason\":\"" + innerException.Message + "\",\"Reason\":\"Kapcsolati hiba\",\"StatusCode\":" + innerException.ErrorCode + " }";
                    }
                }

                return null;
            }
        }

        /// <summary>
        ///     Elkészít egy HTTPWebRequest kapcsolatot a paraméterben megadott URL-en
        ///     keresztül
        /// </summary>
        /// <param name="completeServiceURL">A Szolgáltatás elérési útvonala</param>
        /// <returns>A Paraméterben megadott URL-el elkészített HTTPWebRequest</returns>
        private HttpWebRequest CreateHttpWebRequest(string completeServiceURL)
        {
            return (HttpWebRequest)WebRequest.Create(completeServiceURL);
        }

        /// <summary>
        ///     A WebSzerveren található SSL tanusítványok elfogadása
        /// </summary>
        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        #endregion
    }
}