namespace AuthWithTokenClient.Dtos
{
    public class ServiceRequestDto<TRequestModel> : ServiceRequestBaseDto
    {
        public TRequestModel RequestModel { get; set; }
    }

    public class ServiceRequestDto : ServiceRequestBaseDto
    { }
}