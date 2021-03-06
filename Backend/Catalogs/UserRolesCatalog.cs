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
    public class UserRolesCatalog : SuperCatalog
    {
        public UserRolesCatalog(Application application = null)
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
            BsonClassMap.RegisterClassMap<UserRoles>(initializer =>
            {
                initializer.AutoMap();
                initializer.SetIgnoreExtraElements(true);
            });
        }

        public async Task<UserRoles> StoreAsync(UserRoles item, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.StoreAsync(item, cancellationToken);
        }

        public async Task IncrementAsync(string id, Dictionary<Expression<Func<UserRoles, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.IncrementAsync<UserRoles>(id, fields, cancellationToken);
        }

        public async Task DecrementAsync(string id, Dictionary<Expression<Func<UserRoles, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DecrementAsync<UserRoles>(id, fields, cancellationToken);
        }

        public async Task IncrementAsync(UserRolesItemFilter filter, Dictionary<Expression<Func<UserRoles, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.IncrementAsync<UserRoles>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task DecrementAsync(UserRolesItemFilter filter, Dictionary<Expression<Func<UserRoles, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DecrementAsync<UserRoles>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task IncrementAsync(UserRolesListFilter filter, Dictionary<Expression<Func<UserRoles, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.IncrementAsync<UserRoles>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task DecrementAsync(UserRolesListFilter filter, Dictionary<Expression<Func<UserRoles, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DecrementAsync<UserRoles>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task AddToSetAsync(UserRolesListFilter filter, Expression<Func<UserRoles, IEnumerable<string>>> field, List<string> values, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.AddToSetAsync<UserRoles>(GetFilterDefinition(filter), field, values, cancellationToken);
        }

        public async Task UpdateAsync(UserRolesListFilter filter, Dictionary<string, object> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.UpdateAsync<UserRoles>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task UpdateAsync(string id, Dictionary<string, object> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.UpdateAsync<UserRoles>(id, fields, cancellationToken);
        }

        public async Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.ExistsAsync<UserRoles>(id, cancellationToken);
        }

        public async Task<UserRoles> GetAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetAsync<UserRoles>(id, cancellationToken);
        }

		public async Task<UserRoles> GetAsync(UserRolesItemFilter filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetAsync<UserRoles>(GetFilterDefinition(filter), cancellationToken);
        }

        public async Task<UserRoles> FetchAsync(string id, IEnumerable<Expression<Func<UserRoles, object>>> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.FetchAsync<UserRoles>(id, fields, cancellationToken);
        }

        public async Task<List<UserRoles>> FetchAsync(List<string> ids, IEnumerable<Expression<Func<UserRoles, object>>> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.FetchAsync<UserRoles>(ids, fields, cancellationToken);
        }

        public async Task<List<UserRoles>> GetAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetAsync<UserRoles>(ids, cancellationToken);
        }

        public async Task<List<UserRoles>> GetAsync(UserRolesListFilter filter, string sortOrder = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetAsync<UserRoles>(GetFilterDefinition(filter), sortOrder, cancellationToken);
        }

        public async Task<PagedResult<UserRoles>> GetPageAsync(UserRolesListFilter filter, int pageNumber, int pageSize, string sortOrder = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetPageAsync<UserRoles>(GetFilterDefinition(filter), pageNumber, pageSize, sortOrder, cancellationToken);
        }

        public async Task<long> GetCountAsync(UserRolesListFilter filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetCountAsync<UserRoles>(GetFilterDefinition(filter), cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DeleteAsync<UserRoles>(id, cancellationToken);
        }

        public async Task DeleteAsync(UserRolesListFilter filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DeleteAsync<UserRoles>(GetFilterDefinition(filter), cancellationToken);
        }

        public async Task DeleteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DeleteAsync<UserRoles>(cancellationToken);
        }

        FilterDefinition<UserRoles> GetFilterDefinition(UserRolesItemFilter filter)
        {
            var builder = Builders<UserRoles>.Filter;

            var queries = new List<FilterDefinition<UserRoles>>();

            var baseFilter = base.GetFilterDefinition<UserRoles>(filter);
            queries.Add(baseFilter);

           if (!string.IsNullOrWhiteSpace(filter.UserId))
           {
               queries.Add(builder.Eq(it => it.UserId, filter.UserId));
           }

            return builder.And(queries);
        }

        FilterDefinition<UserRoles> GetFilterDefinition(UserRolesListFilter filter)
        {
            var builder = Builders<UserRoles>.Filter;

            var queries = new List<FilterDefinition<UserRoles>>();

            var baseFilter = base.GetFilterDefinition<UserRoles>(filter);
            queries.Add(baseFilter);            

            return builder.And(queries);
        }
    }
}
