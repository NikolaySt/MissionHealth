using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace SocialNet.Backend.Database
{
    public class MongoProvider
    {
        private MongoClient Client { get; set; }
        private IMongoDatabase AsyncDatabase { get; set; }
        private string CollectionNamespace { get; set; }

        private static readonly object InstanceLock = new object();
        private static Dictionary<string, MongoProvider> _listInstances = new Dictionary<string, MongoProvider>();
        private static MongoProvider _defaultInstance;

        public static MongoProvider Instance
        {
            get { return _defaultInstance; }
        }

        public static MongoProvider InstanceByName(string databaseName)
        {
            MongoProvider mongoProvider;
            return _listInstances.TryGetValue(databaseName.ToLower(), out mongoProvider) ? mongoProvider : null;
        }

        public static void InitDefaultInstance(MongoProviderSettings settings)
        {
            lock (InstanceLock)
            {                
                if (_defaultInstance != null) return;
                _defaultInstance = new MongoProvider();

                var url = new MongoUrl(settings.ConnectionString);
                var clientSettings = MongoClientSettings.FromUrl(url);
                clientSettings.WaitQueueSize = settings.WaitQueueSize;
                clientSettings.ConnectTimeout = TimeSpan.FromSeconds(settings.ConnectionTimeout);
                clientSettings.MaxConnectionPoolSize = settings.ConnectionPoolSize;
                var client = new MongoClient(clientSettings);

                var databaseSettings = new MongoDatabaseSettings();
                var asyncDatabase = client.GetDatabase(settings.DatabaseName, databaseSettings);
                var database = client.GetDatabase(settings.DatabaseName, databaseSettings);

                _defaultInstance.Client = client;
                _defaultInstance.AsyncDatabase = asyncDatabase;
                _defaultInstance.CollectionNamespace = settings.CollectionNamespace;

                if (_listInstances == null)
                {
                    _listInstances = new Dictionary<string, MongoProvider>();
                }
                else
                {
                    try
                    {
                        _listInstances[settings.DatabaseName.ToLower()] = _defaultInstance;
                        return;
                    }
                    catch
                    {
                        //nothing
                    }
                }
                _listInstances.Add(settings.DatabaseName.ToLower(), _defaultInstance);               
            }
        }

        public static void InitInstance(MongoProviderSettings settings)
        {
            lock (InstanceLock)
            {
                if (_listInstances.ContainsKey(settings.DatabaseName.ToLower())) return;
                
                var mongoProvider = new MongoProvider();                

                var url = new MongoUrl(settings.ConnectionString);
                var clientSettings = MongoClientSettings.FromUrl(url);
                clientSettings.ConnectTimeout = TimeSpan.FromSeconds(settings.ConnectionTimeout);
                clientSettings.MaxConnectionPoolSize = settings.ConnectionPoolSize;
                clientSettings.WaitQueueSize = settings.WaitQueueSize;
                var client = new MongoClient(clientSettings);


                var databaseSettings = new MongoDatabaseSettings();

                var database = client.GetDatabase(settings.DatabaseName, databaseSettings);
                var asyncDatabase = client.GetDatabase(settings.DatabaseName, databaseSettings);

                mongoProvider.Client = client;
                mongoProvider.AsyncDatabase = asyncDatabase;
                mongoProvider.CollectionNamespace = settings.CollectionNamespace;

                _listInstances.Add(settings.DatabaseName.ToLower(), mongoProvider);
            }
        }

        public IMongoCollection<TType> GetAsyncCollection<TType>()  where TType : class
        {
            var name = typeof(TType).Name;
            if (!string.IsNullOrWhiteSpace(CollectionNamespace)) name = String.Format("{0}.{1}", CollectionNamespace, name);

            var collection = AsyncDatabase.GetCollection<TType>(name);

            if (collection == null) throw new Exception(typeof(TType).Name + " collection not found.");

            return collection;
        }

    }
}
