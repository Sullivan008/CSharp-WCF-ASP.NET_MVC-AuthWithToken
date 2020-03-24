using AuthWithTokenClient.Controllers.ServiceUrls;
using AuthWithTokenClient.Dtos;
using AuthWithTokenClient.Models.Home;
using AuthWithTokenClient.Models.Home.Authentication;
using AuthWithTokenClient.Models.Home.Test.TokenTest;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace AuthWithTokenClient.Controllers
{
    public class HomeController : CommonController
    {
        public ActionResult Index()
        {
            return View("Index", new HomeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Authenticate(CredentialsViewModel model)
        {
            ServiceResponseDto<AuthenticationResultViewModel> serviceResult =
                SendRequest<AuthenticationResultViewModel, CredentialsViewModel>(new ServiceRequestDto<CredentialsViewModel>
                {
                    ServiceUrl = HomeControllerServiceUrl.AuthenticationService,
                    ServiceMethodName = HomeControllerServiceUrl.AuthenticationMethod,
                    RequestType = WebRequestMethods.Http.Post,
                    RequestModel = model
                });

            if (serviceResult.ErrorModel != null)
            {
                Response.StatusCode = serviceResult.ErrorModel.StatusCode;

                return Json(serviceResult.ErrorModel, JsonRequestBehavior.AllowGet);
            }

            return Json(serviceResult.ResultViewModel, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetTokenTest(string secureToken)
        {
            ServiceResponseDto<TokenTestResultViewModel> serviceResult =
                SendRequest<TokenTestResultViewModel>(new ServiceRequestDto
                {
                    ServiceUrl = HomeControllerServiceUrl.TestService,
                    ServiceMethodName = HomeControllerServiceUrl.TestGetWithTokenHeaderMethod,
                    RequestType = WebRequestMethods.Http.Get,
                    HeaderParameters = new Dictionary<string, string>
                    {
                        { "Token", secureToken }
                    }
                });

            if (serviceResult.ErrorModel != null)
            {
                Response.StatusCode = serviceResult.ErrorModel.StatusCode;

                return Json(serviceResult.ErrorModel, JsonRequestBehavior.AllowGet);
            }

            return Json(serviceResult.ResultViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PostBasicAuthTest(string authorizationString)
        {
            ServiceResponseDto<TokenTestResultViewModel> serviceResult =
                SendRequest<TokenTestResultViewModel>(new ServiceRequestDto
                {
                    ServiceUrl = HomeControllerServiceUrl.TestService,
                    ServiceMethodName = HomeControllerServiceUrl.TestPostWithBasicAuthHeaderMethodName,
                    RequestType = WebRequestMethods.Http.Post,
                    HeaderParameters = new Dictionary<string, string>
                    {
                        { "Authorization", authorizationString }
                    }
                });

            if (serviceResult.ErrorModel != null)
            {
                Response.StatusCode = serviceResult.ErrorModel.StatusCode;

                return Json(serviceResult.ErrorModel, JsonRequestBehavior.AllowGet);
            }

            return Json(serviceResult.ResultViewModel, JsonRequestBehavior.AllowGet);
        }
    }
}