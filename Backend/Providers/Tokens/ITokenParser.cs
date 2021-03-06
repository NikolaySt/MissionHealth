using System.Collections.Generic;
using System.Security.Claims;

namespace SocialNet.Backend.Tokens
{
    public interface ITokenParser
    {
        IEnumerable<Claim> GetClaims(string token);
    }
}
