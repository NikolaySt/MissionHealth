using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MongoDB.Bson.Serialization;
using MongoDB.Driver;

using SocialNet.Backend.DataFilters;
using SocialNet.Backend.DataObjects;

namespace SocialNet.Backend.Catalogs
{
    public class MessageCatalog : SuperCatalog
    {
        public MessageCatalog(Application application = null)
            : base(application, null)
        {
        }

        internal static readonly Dictionary<string, string> MapperProperties = new Dictionary<string, string>();

        protected override Dictionary<string, string> GetMapperProperties()
        {
            return MapperProperties;
        }

        public new static void Init()
        {
            BsonClassMap.RegisterClassMap<Message>(initializer =>
            {
                initializer.AutoMap();
                initializer.SetIgnoreExtraElements(true);
            });
        }

        public async Task<Message> StoreAsync(Message item, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.StoreAsync(item, cancellationToken);
        }

        public async Task UpdateAsync(MessageListFilter filter, Dictionary<string, object> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.UpdateAsync<Message>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task UpdateAsync(string id, Dictionary<string, object> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.UpdateAsync<Message>(id, fields, cancellationToken);
        }

		public async Task<Message> GetAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetAsync<Message>(id, cancellationToken);
        }

        public async Task<List<Message>> GetAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetAsync<Message>(ids, cancellationToken);
        }

        public async Task<List<Message>> GetAsync(MessageListFilter filter, string sortOrder = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetAsync<Message>(GetFilterDefinition(filter), sortOrder, cancellationToken);
        }

        public async Task<PagedResult<Message>> GetPageAsync(MessageListFilter filter, int pageNumber, int pageSize, string sortOrder = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetPageAsync<Message>(GetFilterDefinition(filter), pageNumber, pageSize, sortOrder, cancellationToken);
        }

        public async Task<long> GetCountAsync(MessageListFilter filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetCountAsync<Message>(GetFilterDefinition(filter), cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DeleteAsync<Message>(id, cancellationToken);
        }

        public async Task DeleteAsync(MessageListFilter filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DeleteAsync<Message>(GetFilterDefinition(filter), cancellationToken);
        }

        public async Task DeleteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DeleteAsync<Message>(cancellationToken);
        }

        FilterDefinition<Message> GetFilterDefinition(MessageItemFilter filter)
        {
            var builder = Builders<Message>.Filter;

            var queries = new List<FilterDefinition<Message>>();

            var baseFilter = base.GetFilterDefinition<Message>(filter);

            queries.Add(baseFilter);

            return builder.And(queries);
        }

        FilterDefinition<Message> GetFilterDefinition(MessageListFilter filter)
        {
            var builder = Builders<Message>.Filter;

            var queries = new List<FilterDefinition<Message>>();

            var baseFilter = base.GetFilterDefinition<Message>(filter);
            queries.Add(baseFilter);

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                queries.Add(
                    builder.Eq(it => it.Name, filter.Name)
                    );
            }

			if (!string.IsNullOrWhiteSpace(filter.EMail))
			{
				queries.Add(
					builder.Eq(it => it.EMail, filter.EMail)
					);

			}

			if (filter.Seen != null)
			{
				queries.Add(
					builder.Eq(it => it.Seen, (bool)filter.Seen)
					);
			}

			return builder.And(queries);
        }
    }
}
