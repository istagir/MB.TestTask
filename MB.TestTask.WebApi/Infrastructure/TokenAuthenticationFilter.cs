using MB.TestTask.Models;
using MB.TestTask.Authentication;
using MB.TestTask.Authentication.Extensions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace MB.TestTask.WebAPI.Infrastructure
{
    public class TokenAuthenticationFilter : IAuthenticationFilter
    {
        private readonly IUnityContainer _container;
        private readonly IEncryptionService _encryptionService;
        private readonly string _headerName;
        private readonly string _anonymouseUser;

        public TokenAuthenticationFilter(
            IUnityContainer container,
            IEncryptionService encryptionService,
            string headerName,
            string anonymouseUser)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            if (encryptionService == null)
            {
                throw new ArgumentNullException(nameof(encryptionService));
            }
            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }
            if (string.IsNullOrEmpty(anonymouseUser))
            {
                throw new ArgumentNullException(nameof(anonymouseUser));
            }

            _container = container;
            _encryptionService = encryptionService;
            _headerName = headerName;
            _anonymouseUser = anonymouseUser;
        }

        public bool AllowMultiple
        {
            get
            {
                return false;
            }
        }

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var serverContext = _container.Resolve<IServerContext>();

            IEnumerable<string> headerValues;
            if (context.Request.Headers.TryGetValues(_headerName, out headerValues))
            {
                var authenticationTokenHeader = headerValues.FirstOrDefault();
                serverContext.Token = Convert.FromBase64String(authenticationTokenHeader);
                AuthenticationToken authenticationToken;
                try
                {
                    authenticationToken = _encryptionService.Decrypt<AuthenticationToken>(serverContext.Token);
                }
                catch (CryptographicException)
                {
                    throw new AuthenticationException("Authentication token is incorrect.");
                }
                if (authenticationToken.Expires <= DateTime.Now)
                {
                    throw new AuthenticationException("Authentication token has expired.");
                }

                serverContext.CurrentUser = authenticationToken.User;
                return Task.FromResult(0);
            }

            serverContext.Token = null;
            serverContext.CurrentUser =
                new UserInfo
                {
                    Login = _anonymouseUser,
                    Id = null
                };
            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}