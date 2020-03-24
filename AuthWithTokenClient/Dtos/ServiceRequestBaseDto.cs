using System.Collections.Generic;

namespace AuthWithTokenClient.Dtos
{
    public class ServiceRequestBaseDto
    {
        public string ServiceUrl { get; set; }

        public string ServiceMethodName { get; set; }

        public string RequestType { get; set; }

        public Dictionary<string, string> HeaderParameters { get; set; }

        public ServiceRequestBaseDto()
        {
            HeaderParameters = new Dictionary<string, string>();
        }
    }
}