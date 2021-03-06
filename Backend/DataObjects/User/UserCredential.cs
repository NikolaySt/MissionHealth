
namespace SocialNet.Backend.DataObjects
{
    public class UserCredential : IdDataObject
    {
        public string UserId { get; set; }

        public UserCredentialType Type { get; set; }

        public UserCredentialStatus Status { get; set; }

        public string Value { get; set; }
    }
}
