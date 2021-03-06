using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Linq;
using System.Net;

namespace OrionsCloud.Backend.Cache.Redis
{
	public class RedisCache<T> : ICache<T>
	{
		private readonly RedisSettings _settings;
		private readonly Dictionary<string, ITransaction> _transactions;
		private readonly string _collectionName;
		private readonly bool _disableKeyMapping;

		public RedisCache(RedisSettings settings = null, bool disableKeyMapping = false)
		{
			_settings = settings;
			_collectionName = typeof(T).Name;
			_disableKeyMapping = disableKeyMapping;
            _transactions = new Dictionary<string, ITransaction>();
		}

		private IDatabase _database;
		private IDatabase GetDatabase()
		{
			if (_database != null) return _database;

			_database = _settings == null ?
				RedisRegistry.DefaultDatabase :
				RedisRegistry.GetDatabase(_settings.ConnectionString+"?database="+_settings.Database, _settings);

			return _database;
		}

		public async Task CreateAsync(
			string key,
			T item)
		{
			if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key");

			if (item == null) throw new ArgumentException("item");

			var json = JsonConvert.SerializeObject(item);

			var db = GetDatabase();
			var result = await db.StringSetAsync(MapCacheKey(key), json).ConfigureAwait(false);
			if (!result) throw new ApplicationException("Cache operation failed");
		}

		public async Task CreateAsync(
			string key,
			T item,
			string transactionId)
		{
			if (string.IsNullOrWhiteSpace(transactionId)) throw new ArgumentException("transactionId");
			if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key");
			if (item == null) throw new ArgumentException("item");

			var json = JsonConvert.SerializeObject(item);

			var transaction = GetTransaction(transactionId);
			if (transaction == null) throw new ArgumentException("transactionId");

			var result = await transaction.StringSetAsync(MapCacheKey(key), json).ConfigureAwait(false);
			if (!result) throw new ApplicationException("Cache operation failed");
		}

		public async Task UpdateAsync(
			string key,
			T item)
		{
			if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key");

			if (item == null) throw new ArgumentException("item");

			var json = JsonConvert.SerializeObject(item);

			var db = GetDatabase();
			var result = await db.StringSetAsync(MapCacheKey(key), json);
			if (!result) throw new ApplicationException("Cache operation failed");
		}

		public async Task UpdateAsync(
			string key,
			T item,
			string transactionId)
		{
			if (string.IsNullOrWhiteSpace(transactionId)) throw new ArgumentException("transactionId");
			if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key");
			if (item == null) throw new ArgumentException("item");

			var json = JsonConvert.SerializeObject(item);

			var transaction = GetTransaction(transactionId);
			if (transaction == null) throw new ArgumentException("transactionId");

			var result = await transaction.StringSetAsync(MapCacheKey(key), json).ConfigureAwait(false);
			if (!result) throw new ApplicationException("Cache operation failed");
		}

		public async Task<T> GetAsync(
			string key)
		{
			if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key");

			var db = GetDatabase();
			var json = await db.StringGetAsync(MapCacheKey(key)).ConfigureAwait(false);
			if (json.IsNullOrEmpty) return default(T);

			return JsonConvert.DeserializeObject<T>(json);
		}

        public async Task ClearAsync()
        {
            var list = GetKeys();
            var db = GetDatabase();
            foreach (var cachekey in list)
            {                
                var result = await db.KeyDeleteAsync(cachekey).ConfigureAwait(false);
            }
        }

		public async Task DeleteAsync(
			string key)
		{
			if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key");

			var db = GetDatabase();
			var result = await db.KeyDeleteAsync(MapCacheKey(key)).ConfigureAwait(false);
			if (!result) throw new ApplicationException("Cache operation failed");
		}

		public async Task DeleteAsync(
			string key,
			string transactionId)
		{
			if (string.IsNullOrWhiteSpace(transactionId)) throw new ArgumentException("transactionId");
			if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key");

			var transaction = GetTransaction(transactionId);

			if (transaction == null) throw new ArgumentException("transactionId");
			var result = await transaction.KeyDeleteAsync(MapCacheKey(key)).ConfigureAwait(false);
			if (!result) throw new ApplicationException("Cache operation failed");
		}

		public List<string> GetKeys(
			string searchPattern = "*")
		{
			var db = GetDatabase();
			var endPoints = db.Multiplexer.GetEndPoints();

			var list = new List<string>();

			foreach (var endpoint in endPoints)
			{
				var server = db.Multiplexer.GetServer(endpoint);
				var result = server.Keys(db.Database, searchPattern);
				var keys = result.Select(it => it.ToString()).ToList();
				list.AddRange(keys);
			}

			return list;
		}

		public async Task<string> GetRawValue(
			string key)
		{
			if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key");

			var db = GetDatabase();
			return await db.StringGetAsync(key).ConfigureAwait(false);
		}

		public string StartTransaction()
		{
			var newId = CreateNewUniqueId();
			var db = GetDatabase();
			var transaction = db.CreateTransaction();
			_transactions.Add(newId, transaction);

			return newId;
		}

		public async Task<bool> ExecuteTransactionAsync(
			string transactionId)
		{
			if (string.IsNullOrWhiteSpace(transactionId)) throw new ArgumentException("transactionId");

			var transaction = GetTransaction(transactionId);

			if (transaction == null) throw new ArgumentException("transactionId");

			var result = await transaction.ExecuteAsync();

			RemoveTransaction(transactionId);

			return result;
		}

		private static string CreateNewUniqueId()
		{
			return Guid.NewGuid().ToString();
		}

		private ITransaction GetTransaction(
			string id)
		{
			ITransaction transaction;
			_transactions.TryGetValue(id, out transaction);
			return transaction;
		}

		private void RemoveTransaction(
			string id)
		{
			_transactions.Remove(id);
		}

		private string MapCacheKey(
			string key)
		{
			if (!_disableKeyMapping)
				return String.Format("{0}_{1}", _collectionName, key);
			else
				return key;
		}
	}
}
