namespace AuthWithTokenClient.Models.Error
{
    public class ResponseErrorDataViewModel
    {
        public int StatusCode { get; set; }

        public string Reason { get; set; }

        public string Details { get; set; }
    }
}