using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SocialNet.Backend.DataFilters;
using SocialNet.Backend.DataObjects;

namespace SocialNet.Backend.Catalogs
{
    public class UserCredentialCatalog : SuperCatalog
    {
        public UserCredentialCatalog(Application application = null)
            : base(application, null)
        {
        }

        internal static readonly Dictionary<string, string> MapperProperties = new Dictionary<string, string>();

        protected override Dictionary<string, string> GetMapperProperties()
        {
            return MapperProperties;
        }

        public new static void InitIndex()
        {
            // Hts.T (descending)

            // WRF._id
        }

        public new static void Init()
        {
            BsonClassMap.RegisterClassMap<UserCredential>(initializer =>
            {
                initializer.AutoMap();
                initializer.SetIgnoreExtraElements(true);
            });
        }

        public async Task<UserCredential> StoreAsync(UserCredential item, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.StoreAsync(item, cancellationToken);
        }

        public async Task IncrementAsync(string id, Dictionary<Expression<Func<UserCredential, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.IncrementAsync<UserCredential>(id, fields, cancellationToken);
        }

        public async Task DecrementAsync(string id, Dictionary<Expression<Func<UserCredential, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DecrementAsync<UserCredential>(id, fields, cancellationToken);
        }

        public async Task IncrementAsync(UserCredentialItemFilter filter, Dictionary<Expression<Func<UserCredential, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.IncrementAsync<UserCredential>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task DecrementAsync(UserCredentialItemFilter filter, Dictionary<Expression<Func<UserCredential, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DecrementAsync<UserCredential>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task IncrementAsync(UserCredentialListFilter filter, Dictionary<Expression<Func<UserCredential, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.IncrementAsync<UserCredential>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task DecrementAsync(UserCredentialListFilter filter, Dictionary<Expression<Func<UserCredential, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DecrementAsync<UserCredential>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task AddToSetAsync(UserCredentialListFilter filter, Expression<Func<UserCredential, IEnumerable<string>>> field, List<string> values, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.AddToSetAsync<UserCredential>(GetFilterDefinition(filter), field, values, cancellationToken);
        }

        public async Task UpdateAsync(UserCredentialListFilter filter, Dictionary<string, object> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.UpdateAsync<UserCredential>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task UpdateAsync(string id, Dictionary<string, object> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.UpdateAsync<UserCredential>(id, fields, cancellationToken);
        }

        public async Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.ExistsAsync<UserCredential>(id, cancellationToken);
        }

        public async Task<UserCredential> GetAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetAsync<UserCredential>(id, cancellationToken);
        }

        public async Task<UserCredential> FetchAsync(string id, IEnumerable<Expression<Func<UserCredential, object>>> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.FetchAsync<UserCredential>(id, fields, cancellationToken);
        }

        public async Task<List<UserCredential>> FetchAsync(List<string> ids, IEnumerable<Expression<Func<UserCredential, object>>> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.FetchAsync<UserCredential>(ids, fields, cancellationToken);
        }

        public async Task<List<UserCredential>> GetAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetAsync<UserCredential>(ids, cancellationToken);
        }

		public async Task<UserCredential> GetAsync(UserCredentialItemFilter filter, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await base.GetAsync<UserCredential>(GetFilterDefinition(filter), cancellationToken);
		}

		public async Task<List<UserCredential>> GetAsync(UserCredentialListFilter filter, string sortOrder = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetAsync<UserCredential>(GetFilterDefinition(filter), sortOrder, cancellationToken);
        }

        public async Task<PagedResult<UserCredential>> GetPageAsync(UserCredentialListFilter filter, int pageNumber, int pageSize, string sortOrder = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetPageAsync<UserCredential>(GetFilterDefinition(filter), pageNumber, pageSize, sortOrder, cancellationToken);
        }

        public async Task<long> GetCountAsync(UserCredentialListFilter filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetCountAsync<UserCredential>(GetFilterDefinition(filter), cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DeleteAsync<UserCredential>(id, cancellationToken);
        }

        public async Task DeleteAsync(UserCredentialListFilter filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DeleteAsync<UserCredential>(GetFilterDefinition(filter), cancellationToken);
        }

        public async Task DeleteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DeleteAsync<UserCredential>(cancellationToken);
        }

        FilterDefinition<UserCredential> GetFilterDefinition(UserCredentialItemFilter filter)
        {
            var builder = Builders<UserCredential>.Filter;

            var queries = new List<FilterDefinition<UserCredential>>();

            var baseFilter = base.GetFilterDefinition<UserCredential>(filter);
            queries.Add(baseFilter);

            if (!string.IsNullOrWhiteSpace(filter.UserId))
            {
                queries.Add(builder.Eq(it => it.UserId, filter.UserId));
            }

			if (filter.Type != null)
			{
				queries.Add(builder.Eq(it => it.Type, (UserCredentialType)filter.Type));
			}

			if (filter.Status != null)
			{
				queries.Add(builder.Eq(it => it.Status, (UserCredentialStatus)filter.Status));
			}

			return builder.And(queries);
        }

        FilterDefinition<UserCredential> GetFilterDefinition(UserCredentialListFilter filter)
        {
            var builder = Builders<UserCredential>.Filter;

            var queries = new List<FilterDefinition<UserCredential>>();

            var baseFilter = base.GetFilterDefinition<UserCredential>(filter);
            queries.Add(baseFilter);
            			
            return builder.And(queries);
        }
    }
}
