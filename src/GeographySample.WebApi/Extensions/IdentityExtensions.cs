using System;
using System.Security.Claims;
using System.Security.Principal;

namespace GeographySample.WebApi
{
    public static class IdentityExtensions
    {
        public static string GetUserId(this IIdentity identity)
        {
            if (identity == null)
                throw new ArgumentNullException(nameof(identity));

            var value = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.UserData)?.Value;

            return value;
        }
    }
}