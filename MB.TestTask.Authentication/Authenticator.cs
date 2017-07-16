using MB.TestTask.Authentication.Extensions;
using MB.TestTask.DataAccess.Interfaces;
using MB.TestTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MB.TestTask.Authentication
{
    public class Authenticator : IAuthenticator
    {
        private readonly IEncryptionService _encryptionService;
        private readonly TimeSpan _tokenExpiration;
        private readonly IUserInfoReader _userInfoReader;
        private readonly IUserInfoWriter _userInfoWriter;
        private readonly IHashingService _hashingService;

        public Authenticator(
            IEncryptionService encryptionService,
            TimeSpan tokenExpariation,
            IUserInfoReader userInfoReader,
            IUserInfoWriter userInfoWriter,
            IHashingService hashingService)
        {
            if (tokenExpariation == null)
            {
                throw new ArgumentNullException(nameof(tokenExpariation));
            }
            if (encryptionService == null)
            {
                throw new ArgumentNullException(nameof(encryptionService));
            }
            if (userInfoReader == null)
            {
                throw new ArgumentNullException(nameof(userInfoReader));
            };
            if (userInfoWriter == null)
            {
                throw new ArgumentNullException(nameof(userInfoWriter));
            };
            if (hashingService == null)
            {
                throw new ArgumentNullException(nameof(hashingService));
            };

            _tokenExpiration = tokenExpariation;
            _encryptionService = encryptionService;
            _userInfoReader = userInfoReader;
            _userInfoWriter = userInfoWriter;
            _hashingService = hashingService;
        }

        public AuthenticationResult Authenticate(string login, string password)
        {
            var filter = new UserInfoFilter { Logins = new List<string> { login } };
            var user = _userInfoReader.List(filter).FirstOrDefault();
            var passHash = _hashingService.GetHash(login + password);

            if (user == null || user.PasswordHash != passHash)
            {
                return null;
            }

            user.LastSignInTime = DateTime.Now;
            user.SignInCount++;
            _userInfoWriter.Update(user);

            var authenticationToken = new AuthenticationToken
            {
                User = user,
                Expires = DateTime.Now.Add(_tokenExpiration)
            };
            var encryptedToken = _encryptionService.Encrypt(authenticationToken);

            return new AuthenticationResult
            {
                Token = encryptedToken,
                TokenExpirationMilliseconds = (long)_tokenExpiration.TotalMilliseconds
            };
        }
    }
}
