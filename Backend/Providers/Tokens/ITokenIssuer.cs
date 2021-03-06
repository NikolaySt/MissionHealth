using System.Collections.Generic;
using System.Security.Claims;

namespace SocialNet.Backend.Tokens
{
    public interface ITokenIssuer
    {
        string GetToken(IEnumerable<Claim> claims);
    }
}
