using AuthWithTokenClient.Models.Error;

namespace AuthWithTokenClient.Dtos
{
    public class ServiceResponseDto<TResultModel> where TResultModel : class
    {
        public TResultModel ResultViewModel { get; set; }

        public ResponseErrorDataViewModel ErrorModel { get; set; }
    }
}