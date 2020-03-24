using AuthWithTokenServer.Core.EncryptDecrypt;
using AuthWithTokenServer.Infrastructure;
using System;
using System.Linq;
using AuthWithTokenServer.Dtos.Core.Validator;

namespace AuthWithTokenServer.Core.Validator.UserCredentialsValidator
{
    public class UserCredentialsValidator : IUserCredentialsValidator
    {
        private readonly AuthenticationExampleDbContext _context;

        public UserCredentialsValidator(AuthenticationExampleDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public UserDto CredentialsIsValid(UserCredentialsDto credentialsDto)
        {
            User user = _context.Users.SingleOrDefault(x => x.Name.Equals(credentialsDto.UserName, StringComparison.CurrentCultureIgnoreCase));

            if (user != null && PasswordCompare(user, credentialsDto))
            {
                return new UserDto
                {
                    Id = user.Id,
                    UserName = user.Name
                };
            }

            return null;
        }

        #region PRIVATE Helper Methods

        private static bool PasswordCompare(User user, UserCredentialsDto credentialsDto) =>
            Hash.Compare(credentialsDto.Password, user.Salt, user.Password,
                Hash.DefaultHashType, Hash.DefaultEncoding);

        #endregion
    }
}