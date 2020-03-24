using AuthWithTokenClient.Models.Home.Authentication;
using AuthWithTokenClient.Models.Home.Test;

namespace AuthWithTokenClient.Models.Home
{
    public class HomeViewModel
    {
        public CredentialsViewModel CredentialsViewModel { get; set; }

        public HeadersViewModel HeadersViewModel { get; set; }

        public HomeViewModel()
        {
            CredentialsViewModel = new CredentialsViewModel();
            HeadersViewModel = new HeadersViewModel();
        }
    }
}