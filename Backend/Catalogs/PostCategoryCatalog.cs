using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SocialNet.Backend.DataFilters;
using SocialNet.Backend.DataObjects;

namespace SocialNet.Backend.Catalogs
{
    public class ArticleCategoryCatalog : SuperCatalog
    {
        public ArticleCategoryCatalog(Application application = null)
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
            BsonClassMap.RegisterClassMap<ArticleCategory>(initializer =>
            {
                initializer.AutoMap();
                initializer.SetIgnoreExtraElements(true);
            });
        }

        public async Task<ArticleCategory> StoreAsync(ArticleCategory item, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.StoreAsync(item, cancellationToken);
        }

        public async Task IncrementAsync(string id, Dictionary<Expression<Func<ArticleCategory, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.IncrementAsync<ArticleCategory>(id, fields, cancellationToken);
        }

        public async Task DecrementAsync(string id, Dictionary<Expression<Func<ArticleCategory, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DecrementAsync<ArticleCategory>(id, fields, cancellationToken);
        }

        public async Task IncrementAsync(PostCategoryItemFilter filter, Dictionary<Expression<Func<ArticleCategory, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.IncrementAsync<ArticleCategory>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task DecrementAsync(PostCategoryItemFilter filter, Dictionary<Expression<Func<ArticleCategory, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DecrementAsync<ArticleCategory>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task IncrementAsync(PostCategoryListFilter filter, Dictionary<Expression<Func<ArticleCategory, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.IncrementAsync<ArticleCategory>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task DecrementAsync(PostCategoryListFilter filter, Dictionary<Expression<Func<ArticleCategory, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DecrementAsync<ArticleCategory>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task AddToSetAsync(PostCategoryListFilter filter, Expression<Func<ArticleCategory, IEnumerable<string>>> field, List<string> values, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.AddToSetAsync<ArticleCategory>(GetFilterDefinition(filter), field, values, cancellationToken);
        }

        public async Task UpdateAsync(PostCategoryListFilter filter, Dictionary<string, object> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.UpdateAsync<ArticleCategory>(GetFilterDefinition(filter), fields, cancellationToken);
        }

        public async Task UpdateAsync(string id, Dictionary<string, object> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.UpdateAsync<ArticleCategory>(id, fields, cancellationToken);
        }

        public async Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.ExistsAsync<ArticleCategory>(id, cancellationToken);
        }


		public async Task<ArticleCategory> GetAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetAsync<ArticleCategory>(id, cancellationToken);
        }

        public async Task<ArticleCategory> FetchAsync(string id, IEnumerable<Expression<Func<ArticleCategory, object>>> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.FetchAsync<ArticleCategory>(id, fields, cancellationToken);
        }

        public async Task<List<ArticleCategory>> FetchAsync(List<string> ids, IEnumerable<Expression<Func<ArticleCategory, object>>> fields, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.FetchAsync<ArticleCategory>(ids, fields, cancellationToken);
        }

        public async Task<List<ArticleCategory>> GetAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetAsync<ArticleCategory>(ids, cancellationToken);
        }

        public async Task<List<ArticleCategory>> GetAsync(PostCategoryListFilter filter, string sortOrder = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetAsync<ArticleCategory>(GetFilterDefinition(filter), sortOrder, cancellationToken);
        }

        public async Task<PagedResult<ArticleCategory>> GetPageAsync(PostCategoryListFilter filter, int pageNumber, int pageSize, string sortOrder = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetPageAsync<ArticleCategory>(GetFilterDefinition(filter), pageNumber, pageSize, sortOrder, cancellationToken);
        }

        public async Task<long> GetCountAsync(PostCategoryListFilter filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetCountAsync<ArticleCategory>(GetFilterDefinition(filter), cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DeleteAsync<ArticleCategory>(id, cancellationToken);
        }

        public async Task DeleteAsync(PostCategoryListFilter filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DeleteAsync<ArticleCategory>(GetFilterDefinition(filter), cancellationToken);
        }

        public async Task DeleteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DeleteAsync<ArticleCategory>(cancellationToken);
        }

        FilterDefinition<ArticleCategory> GetFilterDefinition(PostCategoryItemFilter filter)
        {
            var builder = Builders<ArticleCategory>.Filter;

            var queries = new List<FilterDefinition<ArticleCategory>>();

            var baseFilter = base.GetFilterDefinition<ArticleCategory>(filter);
            queries.Add(baseFilter);

            return builder.And(queries);
        }

        FilterDefinition<ArticleCategory> GetFilterDefinition(PostCategoryListFilter filter)
        {
            var builder = Builders<ArticleCategory>.Filter;

            var queries = new List<FilterDefinition<ArticleCategory>>();

            var baseFilter = base.GetFilterDefinition<ArticleCategory>(filter);
            queries.Add(baseFilter);

            if (!string.IsNullOrWhiteSpace(filter.SubCategoryValue))
            {
                queries.Add(
                    builder.Eq(it => it.SubCategoryValue, filter.SubCategoryValue)
                    );
            }

            if (!string.IsNullOrWhiteSpace(filter.CategoryValue))
            {
                queries.Add(
                    builder.Eq(it => it.CategoryValue, filter.CategoryValue)
                    );
                
            }

            if (!string.IsNullOrWhiteSpace(filter.AreaValue))
            {
                queries.Add(
                    builder.Eq(it => it.AreaValue, filter.AreaValue)
                    );

            }

            return builder.And(queries);
        }
    }
}
