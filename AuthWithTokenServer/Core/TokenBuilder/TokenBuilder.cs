using AuthWithTokenServer.Infrastructure;
using System;
using System.Security.Cryptography;
using AuthWithTokenServer.Dtos.Core.TokenBuilder;

namespace AuthWithTokenServer.Core.TokenBuilder
{
    public class TokenBuilder : ITokenBuilder
    {
        private readonly AuthenticationExampleDbContext _context;

        public TokenBuilder(AuthenticationExampleDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string TokenBuild(LoggedUserDto loggedUserDto)
        {
            string secureToken = BuildSecureToken();

            _context.Tokens.Add(new Token
            {
                UserId = loggedUserDto.UserId,
                SecureToken = secureToken,
                CreateDate = DateTime.Now
            });

            _context.SaveChanges();

            return secureToken;
        }

        #region PRIVATE Helper Methods

        private static string BuildSecureToken()
        {
            var buffer = new byte[100];

            using (RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rngCryptoServiceProvider.GetNonZeroBytes(buffer);
            }

            return Convert.ToBase64String(buffer);
        }

        #endregion
    }
}