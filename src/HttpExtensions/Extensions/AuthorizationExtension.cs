using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace HttpExtensions.Extensions
{
    public static class AuthorizationExtension
    {
        public static string GetAuthorization(this HttpRequest req, string authenticationType)
        {
            if (req is null)
                throw new ArgumentNullException(nameof(req));

            if (authenticationType is null)
                throw new ArgumentNullException(nameof(authenticationType));

            var identity = req.HttpContext.User
                .Identities
                .SingleOrDefault(id =>
                    id.AuthenticationType != null && id.AuthenticationType.Equals(authenticationType,
                        StringComparison.InvariantCultureIgnoreCase));

            return identity is null
                ? string.Empty 
                : identity.Claims
                    .Single(c => c.Type == ClaimTypes.Upn)
                    .Value;
        }
    }
}