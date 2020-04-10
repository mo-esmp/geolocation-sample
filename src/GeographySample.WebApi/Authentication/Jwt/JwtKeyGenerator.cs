using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace GeographySample.WebApi.Authentication.Jwt
{
    public class JwtKeyGenerator
    {
        public static SymmetricSecurityKey Generate(string secret)
        {
            if (string.IsNullOrEmpty(secret))
                throw new ArgumentNullException(nameof(secret));

            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        }
    }
}