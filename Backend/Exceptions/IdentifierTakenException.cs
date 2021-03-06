using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNet.Backend.Exceptions
{
    public class IdentifierTakenException: Exception
    {
        public IdentifierTakenException(string message):base(message)
        {

        }
    }
}
