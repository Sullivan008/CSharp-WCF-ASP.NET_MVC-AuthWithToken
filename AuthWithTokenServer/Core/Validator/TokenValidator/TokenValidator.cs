using AuthWithTokenServer.Dtos.Core.Validator;
using AuthWithTokenServer.Infrastructure;
using System;
using System.Linq;

namespace AuthWithTokenServer.Core.Validator.TokenValidator
{
    public class TokenValidator : ITokenValidator
    {
        private const double DefaultTokenExpires = 1800;

        private readonly AuthenticationExampleDbContext _context;

        public TokenValidator(AuthenticationExampleDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public UserDto TokenIsValid(string secureToken)
        {
            var token = _context.Tokens.SingleOrDefault(x => x.SecureToken == secureToken);

            if (token != null && !TokenIsExpired(token))
            {
                return new UserDto
                {
                    Id = token.UserId,
                    UserName = token.User.Name
                };
            }

            return null;
        }

        #region PRIVATE Helper Methods

        private static bool TokenIsExpired(Token token) =>
            (DateTime.Now - token.CreateDate).TotalSeconds > DefaultTokenExpires;

        #endregion
    }
}