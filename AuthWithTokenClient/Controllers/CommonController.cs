using AuthWithTokenClient.Dtos;
using AuthWithTokenClient.Models.Error;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace AuthWithTokenClient.Controllers
{
    public class CommonController : Controller
    {
        public ServiceResponseDto<TResultModel> SendRequest<TResultModel, TRequestModel>(ServiceRequestDto<TRequestModel> requestDto) where TResultModel : class
        {
            try
            {
                string serviceUrl = $"{requestDto.ServiceUrl}/{requestDto.ServiceMethodName}";

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = requestDto.RequestType;
                httpWebRequest.ServerCertificateValidationCallback = CertificationValidation;

                foreach (KeyValuePair<string, string> item in requestDto.HeaderParameters)
                {
                    httpWebRequest.Headers.Add(item.Key, item.Value);
                }

                if (requestDto.RequestType == WebRequestMethods.Http.Post && requestDto.RequestModel != null)
                {
                    JsonConvert.SerializeObject(requestDto.RequestModel, Formatting.None);

                    using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(JsonConvert.SerializeObject(requestDto.RequestModel, Formatting.None));
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                }

                using (HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream() ??
                                                                        throw new InvalidOperationException("Response stream was null")))
                    {
                        return new ServiceResponseDto<TResultModel>
                        {
                            ResultViewModel =
                                JsonConvert.DeserializeObject<TResultModel>(streamReader.ReadToEnd())
                        };
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    if (ex.Response is HttpWebResponse)
                    {
                        return new ServiceResponseDto<TResultModel>
                        {
                            ErrorModel =
                                JsonConvert.DeserializeObject<ResponseErrorDataViewModel>(
                                    new StreamReader(ex.Response.GetResponseStream() ??
                                                     throw new InvalidOperationException("Response stream was null"))
                                        .ReadToEnd())
                        };
                    }
                }
                else if (ex.Status == WebExceptionStatus.ConnectFailure)
                {
                    if (ex.InnerException is Win32Exception innerException)
                    {
                        return new ServiceResponseDto<TResultModel>
                        {
                            ErrorModel = new ResponseErrorDataViewModel
                            {
                                StatusCode = innerException.ErrorCode,
                                Reason = "Connection Error",
                                Details = innerException.Message
                            }
                        };
                    }
                }

                return null;
            }
            catch (InvalidOperationException ex)
            {
                return new ServiceResponseDto<TResultModel>
                {
                    ErrorModel = new ResponseErrorDataViewModel
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                        Reason = "Internal Server Error",
                        Details = ex.Message
                    }
                };
            }
        }

        public ServiceResponseDto<TResultModel> SendRequest<TResultModel>(ServiceRequestDto requestDto) where TResultModel : class
        {
            try
            {
                string serviceUrl = $"{requestDto.ServiceUrl}/{requestDto.ServiceMethodName}";

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = requestDto.RequestType;
                httpWebRequest.ServerCertificateValidationCallback = CertificationValidation;

                foreach (KeyValuePair<string, string> item in requestDto.HeaderParameters)
                {
                    httpWebRequest.Headers.Add(item.Key, item.Value);
                }

                if (requestDto.RequestType == WebRequestMethods.Http.Post)
                {
                    using (new StreamWriter(httpWebRequest.GetRequestStream()))
                    { }
                }

                using (HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream() ??
                                                                        throw new InvalidOperationException(
                                                                            "Response stream was null")))
                    {
                        return new ServiceResponseDto<TResultModel>
                        {
                            ResultViewModel =
                                JsonConvert.DeserializeObject<TResultModel>(streamReader.ReadToEnd())
                        };
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    if (ex.Response is HttpWebResponse)
                    {
                        return new ServiceResponseDto<TResultModel>
                        {
                            ErrorModel =
                                JsonConvert.DeserializeObject<ResponseErrorDataViewModel>(
                                    new StreamReader(ex.Response.GetResponseStream() ??
                                                     throw new InvalidOperationException("Response stream was null"))
                                        .ReadToEnd())
                        };
                    }
                }
                else if (ex.Status == WebExceptionStatus.ConnectFailure)
                {
                    if (ex.InnerException is Win32Exception innerException)
                    {
                        return new ServiceResponseDto<TResultModel>
                        {
                            ErrorModel = new ResponseErrorDataViewModel
                            {
                                StatusCode = innerException.ErrorCode,
                                Reason = "Connection Error",
                                Details = innerException.Message
                            }
                        };
                    }
                }

                return null;
            }
            catch (InvalidOperationException ex)
            {
                return new ServiceResponseDto<TResultModel>
                {
                    ErrorModel = new ResponseErrorDataViewModel
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                        Reason = "Internal Server Error",
                        Details = ex.Message
                    }
                };
            }
        }

        #region PRIVATE Helper Methods

        private static bool CertificationValidation(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification,
            System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        #endregion
    }
}