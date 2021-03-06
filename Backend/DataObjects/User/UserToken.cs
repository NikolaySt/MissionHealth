

namespace SocialNet.Backend.DataObjects
{
    public class UserToken : IdDataObject
    {
        public string UserId { get; set; }

        public string Code { get; set; }
    }
}
