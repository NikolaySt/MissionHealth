
namespace SocialNet.Backend.DataObjects
{
    public class Application : IdDataObject
    {
        public Application()
        {
        }

        public string Name { get; set; }

        public string CustomerId { get; set; }
    }
}
