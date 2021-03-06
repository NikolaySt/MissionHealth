using System.Collections.Generic;

namespace SocialNet.Backend.DataObjects
{
    public class UserRoles : IdDataObject
    {

		public string UserId { get; set; }
		
        public string[] Roles { get; set; }

    }
}
