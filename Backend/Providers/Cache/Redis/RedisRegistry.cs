using System.Collections.Concurrent;
using StackExchange.Redis;

namespace OrionsCloud.Backend.Cache.Redis
{
    public static class RedisRegistry
    {
        private static readonly object _defaultDatabaseLock = new object();
        private static IDatabase _defaultDatabase;

        private static readonly ConcurrentDictionary<string, IDatabase> _databases = new ConcurrentDictionary<string, IDatabase>();

        public static IDatabase DefaultDatabase
        {
            get { return _defaultDatabase; }
        }

        public static void InitDefaultDatabase(RedisSettings settings)
        {
            lock (_defaultDatabaseLock)
            {
                if (_defaultDatabase != null) return;
                
                _defaultDatabase = CreateDatabase(settings);
            }
        }

        public static IDatabase GetDatabase(string key, RedisSettings settings)
        {
            var database = _databases.GetOrAdd(key, dictionaryKey => null) ?? CreateDatabase(settings);

            _databases.AddOrUpdate(key, database, (dictionaryKey, oldValue) => database);

            return database;
        }

        private static IDatabase CreateDatabase(RedisSettings settings)
        {
            var connection = ConnectionMultiplexer.Connect(settings.ConnectionString);

            connection.ErrorMessage += (sender, args) =>
            {
                //Logger.Error("ErrorMessage with args: {0}", args.Message);
            };

            return connection.GetDatabase(settings.Database);
        }
    }
}
