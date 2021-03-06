
namespace SocialNet.Backend.DataObjects
{
    public class SocialNetwork : IdDataObject
    {
        public long CommentsCount{ get; set; }
        public long SharedCount { get; set; }
        public long LikesCount { get; set; }
        public long ViewsCount { get; set; }
    }
}
