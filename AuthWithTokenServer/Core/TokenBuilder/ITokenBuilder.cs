using AuthWithTokenServer.Dtos.Core.TokenBuilder;

namespace AuthWithTokenServer.Core.TokenBuilder
{
    public interface ITokenBuilder
    {
        string TokenBuild(LoggedUserDto tokenBuilderDto);
    }
}