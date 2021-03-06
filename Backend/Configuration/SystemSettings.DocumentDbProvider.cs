namespace SocialNet.Backend.Configuration
{
    public partial class SystemSettings
    {
        private static string _databaseConnectionString;
        private static string _databaseName;
        private static string _databaseCollectionNamespace;

        private static int _connetionTimeout;
        private static int _waitQueueSize;
        private static int _connectionPoolSize;

        public static string DatabaseConnectionString
        {
            get { return _databaseConnectionString; }
        }

        public static string DatabaseName
        {
            get { return _databaseName; }
        }

        public static string DatabaseCollectionNamespace
        {
            get { return _databaseCollectionNamespace; }
        }

        public static int ConnectionTimeout
        {
            get { return _connetionTimeout; }
        }

        public static int WaitQueueSize
        {
            get { return _waitQueueSize; }
        }

        public static int ConnectionPoolSize
        {
            get { return _connectionPoolSize; }
        }
    }
}
