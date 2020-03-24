using System.Runtime.Serialization;

namespace AuthWithTokenServer.Dtos.Core.Error
{
    [DataContract]
    public class RequestErrorDto
    {
        [DataMember]
        public int StatusCode { get; set; }

        [DataMember]
        public string Reason { get; set; }

        [DataMember]
        public string Details { get; set; }
    }
}