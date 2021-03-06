using System;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;

using SocialNet.Backend.Security;
using SocialNet.Backend.Managers;

namespace SocialNet.WebSite
{
    public class SecurityManager
    {
        private static readonly DateTime EpochStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public string GetAppRealmId(string token)
        {
            var principal = GetClaims(token);
            if (principal == null) throw new UnauthorizedAccessException();

            var claim = principal.FirstOrDefault(it => it.Type == Claims.AppRealmId);
            if (claim == null) return null;

            return claim.Value;
        }

        public string GetAppId(string token)
        {
            var principal = GetClaims(token);
            if (principal == null) throw new UnauthorizedAccessException();

            var claim = principal.FirstOrDefault(it => it.Type == Claims.AppId);
            if (claim == null) return null;

            return claim.Value;            
        }

        public List<Claim> GetClaims(string token)
        {
            var parser = TokenParserFactory.GetInstance();
            return parser.GetClaims(token).ToList();            
        }

        public void SetContext(string token)
        {
            var parser = TokenParserFactory.GetInstance();
            var claims = parser.GetClaims(token).ToList();
            SetContext(claims);
        }

        public void SetContext(List<Claim> claims)
        {
            if (!string.IsNullOrWhiteSpace(SecurityContext.UserId) && claims.FirstOrDefault(it => it.Type == Claims.UserId) == null)
            {
                claims.Add(new Claim(Claims.UserId, SecurityContext.UserId));
            }

            if (claims.FirstOrDefault(it => it.Type == ClaimTypes.NameIdentifier) == null)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, "website"));
            }

            Expired(claims);

            SecurityContext.SetSecurityContext(claims);
        }

        public void Expired(IEnumerable<Claim> claims)
        {
            var expiresOn = claims.FirstOrDefault(it => it.Type == Claims.ExpirationTime);
            if (expiresOn == null) throw new System.Security.SecurityException("Missing expiration claim");
            if (string.IsNullOrWhiteSpace(expiresOn.Value)) throw new System.Security.SecurityException("Invalid expiration claim");

            var seconds = uint.Parse(expiresOn.Value);

            var interval = DateTime.UtcNow - EpochStart;

            if (seconds < Convert.ToUInt64(interval.TotalSeconds)) throw new System.Security.SecurityException("Token has expired");
        }

        public long GetExpirationIntervalSeconds(TimeSpan seconds)
        {
            var interval = DateTime.UtcNow - EpochStart + seconds;
            return (long)interval.TotalSeconds;
        }
    }
}

