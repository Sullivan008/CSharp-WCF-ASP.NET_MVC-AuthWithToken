namespace AuthWithTokenServer.Dtos.Core.TokenBuilder
{
    public class LoggedUserDto
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}