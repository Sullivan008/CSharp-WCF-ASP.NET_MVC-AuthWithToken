using System.Runtime.Serialization;

namespace AuthWithTokenServer.Models
{
    [DataContract]
    public class Credentials
    {
        [DataMember]
        public string User { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}