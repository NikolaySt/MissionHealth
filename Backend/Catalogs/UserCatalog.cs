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
    public class UserCatalog : SuperCatalog
    {
        public UserCatalog(Application application = null)
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
            BsonClassMap.RegisterClassMap<User>(initializer =>
            {
                initializer.AutoMap();
                initializer.SetIgnoreExtraElements(true);
            });
        }

		public async Task<User> StoreAsync(User item, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.StoreAsync(item, cancellationToken);
        }

        public async Task IncrementAsync(string id, Dictionary<Expression<Func<User, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.IncrementAsync<User>(id, fields, cancellationToken);
        }

        public async Task DecrementAsync(string id, Dictionary<Expression<Func<User, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DecrementAsync<User>(id, fields, cancellationToken);
        }

        public async Task IncrementAsync(UserItemFilter filter, Dictionary<Expression<Func<User, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.IncrementAsync<User>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task DecrementAsync(UserItemFilter filter, Dictionary<Expression<Func<User, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DecrementAsync<User>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task IncrementAsync(UserListFilter filter, Dictionary<Expression<Func<User, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.IncrementAsync<User>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task DecrementAsync(UserListFilter filter, Dictionary<Expression<Func<User, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DecrementAsync<User>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task AddToSetAsync(UserListFilter filter, Expression<Func<User, IEnumerable<string>>> field, List<string> values, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.AddToSetAsync<User>(GetFilterDefinition(filter), field, values, cancellationToken);
        }

        public async Task UpdateAsync(UserListFilter filter, Dictionary<string, object> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.UpdateAsync<User>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task UpdateAsync(string id, Dictionary<string, object> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.UpdateAsync<User>(id, fields, cancellationToken);
        }

        public async Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.ExistsAsync<User>(id, cancellationToken);
        }

        public async Task<User> GetAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetAsync<User>(id, cancellationToken);
        }

        public async Task<User> GetAsync(UserItemFilter filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetAsync<User>(GetFilterDefinition(filter), cancellationToken);
        }

        public async Task<User> FetchAsync(string id, IEnumerable<Expression<Func<User, object>>> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.FetchAsync<User>(id, fields, cancellationToken);
        }

        public async Task<List<User>> FetchAsync(List<string> ids, IEnumerable<Expression<Func<User, object>>> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.FetchAsync<User>(ids, fields, cancellationToken);
        }

        public async Task<List<User>> GetAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetAsync<User>(ids, cancellationToken);
        }

        public async Task<List<User>> GetAsync(UserListFilter filter, string sortOrder = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetAsync<User>(GetFilterDefinition(filter), sortOrder, cancellationToken);
        }

        public async Task<PagedResult<User>> GetPageAsync(UserListFilter filter, int pageNumber, int pageSize, string sortOrder = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetPageAsync<User>(GetFilterDefinition(filter), pageNumber, pageSize, sortOrder, cancellationToken);
        }

        public async Task<long> GetCountAsync(UserListFilter filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetCountAsync<User>(GetFilterDefinition(filter), cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DeleteAsync<User>(id, cancellationToken);
        }

        public async Task DeleteAsync(UserListFilter filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DeleteAsync<User>(GetFilterDefinition(filter), cancellationToken);
        }

        public async Task DeleteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DeleteAsync<User>(cancellationToken);
        }

        FilterDefinition<User> GetFilterDefinition(UserItemFilter filter)
        {
            var builder = Builders<User>.Filter;

            var queries = new List<FilterDefinition<User>>();

            var baseFilter = base.GetFilterDefinition<User>(filter);
            queries.Add(baseFilter);

            if (!string.IsNullOrWhiteSpace(filter.Email))
            {
                queries.Add(builder.Eq(it => it.Email, filter.Email));
            }

            return builder.And(queries);
        }

        FilterDefinition<User> GetFilterDefinition(UserListFilter filter)
        {
            var builder = Builders<User>.Filter;

            var queries = new List<FilterDefinition<User>>();

            var baseFilter = base.GetFilterDefinition<User>(filter);
            queries.Add(baseFilter);

            if (filter.BookDownloads != null)
            {
				queries.Add(builder.Eq(it => it.BookDownloads, (long)filter.BookDownloads));
            }

			if (filter.EmailStatus != null)
			{
				queries.Add(builder.Eq(it => it.EmailStatus, filter.EmailStatus));
			}

			if (filter.Email != null)
			{
				queries.Add(builder.Eq(it => it.Email, filter.Email));
			}

			return builder.And(queries);
        }
    }
}
