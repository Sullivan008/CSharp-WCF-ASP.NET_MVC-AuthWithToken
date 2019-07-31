using System.Runtime.Serialization;

namespace AuthWithTokenServer.Models.Errors
{
    [DataContract]
    public class RequestErrorData
    {
        [DataMember]
        public int StatusCode { get; private set; }

        [DataMember]
        public string Reason { get; private set; }

        [DataMember]
        public string InformationAbouotReason { get; private set; }

        /// <summary>
        ///     Konstruktor
        /// </summary>
        /// <param name="statusCode">HTTP Error Error Kódja</param>
        /// <param name="reason">A hiba oka</param>
        /// <param name="informationAboutReason">További információ a hiba okáról</param>
        public RequestErrorData(int statusCode, string reason, string informationAboutReason)
        {
            Reason = reason;
            InformationAbouotReason = informationAboutReason;
            StatusCode = statusCode;
        }
    }
}