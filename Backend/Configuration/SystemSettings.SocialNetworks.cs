namespace SocialNet.Backend.Configuration
{
    public partial class SystemSettings
    {
        private static string _facebookKey;

        private static string _facebookSecret;

        private static string _twitterSecret;

        private static string _twitterKey;



        public static string TwitterSecret
        {
            get { return _twitterSecret; }
        }

        public static string TwitterKey
        {
            get { return _twitterKey; }
        }

        public static string FacebookSecret
        {
            get { return _facebookSecret; }
        }

        public static string FacebookKey
        {
            get { return _facebookKey; }
        }
    }
}
