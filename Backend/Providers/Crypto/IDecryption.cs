using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNet.Backend.Providers.Crypto
{
    public interface IDecryption
    {
        string Decrypt(string data);
    }

}
