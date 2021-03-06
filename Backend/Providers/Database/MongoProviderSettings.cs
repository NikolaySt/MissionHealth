namespace SocialNet.Backend.Database

{
    public class MongoProviderSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionNamespace { get; set; }
        public int ConnectionTimeout { get; set; }
        public int ConnectionPoolSize { get; set; }
        public int WaitQueueSize { get; set; }
    }
}
