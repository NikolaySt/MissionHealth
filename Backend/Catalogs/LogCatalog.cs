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
    public class LogCatalog : SuperCatalog
    {
        public LogCatalog(Application application = null)
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
            BsonClassMap.RegisterClassMap<Log>(initializer =>
            {
                initializer.AutoMap();
                initializer.SetIgnoreExtraElements(true);
            });
        }

        public async Task<Log> StoreAsync(Log item, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.StoreAsync(item, cancellationToken);
        }

        public async Task IncrementAsync(string id, Dictionary<Expression<Func<Log, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.IncrementAsync<Log>(id, fields, cancellationToken);
        }

        public async Task DecrementAsync(string id, Dictionary<Expression<Func<Log, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DecrementAsync<Log>(id, fields, cancellationToken);
        }

        public async Task IncrementAsync(LogItemFilter filter, Dictionary<Expression<Func<Log, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.IncrementAsync<Log>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task DecrementAsync(LogItemFilter filter, Dictionary<Expression<Func<Log, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DecrementAsync<Log>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task IncrementAsync(LogListFilter filter, Dictionary<Expression<Func<Log, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.IncrementAsync<Log>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task DecrementAsync(LogListFilter filter, Dictionary<Expression<Func<Log, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DecrementAsync<Log>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task AddToSetAsync(LogListFilter filter, Expression<Func<Log, IEnumerable<string>>> field, List<string> values, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.AddToSetAsync<Log>(GetFilterDefinition(filter), field, values, cancellationToken);
        }

        public async Task UpdateAsync(LogListFilter filter, Dictionary<string, object> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.UpdateAsync<Log>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task UpdateAsync(string id, Dictionary<string, object> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.UpdateAsync<Log>(id, fields, cancellationToken);
        }

        public async Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.ExistsAsync<Log>(id, cancellationToken);
        }

        public async Task<Log> GetAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetAsync<Log>(id, cancellationToken);
        }

        public async Task<Log> FetchAsync(string id, IEnumerable<Expression<Func<Log, object>>> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.FetchAsync<Log>(id, fields, cancellationToken);
        }

        public async Task<List<Log>> FetchAsync(List<string> ids, IEnumerable<Expression<Func<Log, object>>> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.FetchAsync<Log>(ids, fields, cancellationToken);
        }

        public async Task<List<Log>> GetAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetAsync<Log>(ids, cancellationToken);
        }

        public async Task<List<Log>> GetAsync(LogListFilter filter, string sortOrder = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetAsync<Log>(GetFilterDefinition(filter), sortOrder, cancellationToken);
        }

        public async Task<PagedResult<Log>> GetPageAsync(LogListFilter filter, int pageNumber, int pageSize, string sortOrder = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetPageAsync<Log>(GetFilterDefinition(filter), pageNumber, pageSize, sortOrder, cancellationToken);
        }

        public async Task<long> GetCountAsync(LogListFilter filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetCountAsync<Log>(GetFilterDefinition(filter), cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DeleteAsync<Log>(id, cancellationToken);
        }

        public async Task DeleteAsync(LogListFilter filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DeleteAsync<Log>(GetFilterDefinition(filter), cancellationToken);
        }

        public async Task DeleteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DeleteAsync<Log>(cancellationToken);
        }

        FilterDefinition<Log> GetFilterDefinition(LogItemFilter filter)
        {
            var builder = Builders<Log>.Filter;

            var queries = new List<FilterDefinition<Log>>();

            var baseFilter = base.GetFilterDefinition<Log>(filter);

            queries.Add(baseFilter);

            return builder.And(queries);
        }

        FilterDefinition<Log> GetFilterDefinition(LogListFilter filter)
        {
            var builder = Builders<Log>.Filter;

            var queries = new List<FilterDefinition<Log>>();

            var baseFilter = base.GetFilterDefinition<Log>(filter);

            queries.Add(baseFilter);          

            return builder.And(queries);
        }
    }
}
