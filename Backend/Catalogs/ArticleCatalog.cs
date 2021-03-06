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
	public class ArticleCatalog : SuperCatalog
	{
		public ArticleCatalog(Application application = null)
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
			BsonClassMap.RegisterClassMap<Article>(initializer =>
			{
				initializer.AutoMap();
				initializer.SetIgnoreExtraElements(true);
			});
		}

		public async Task<Article> StoreAsync(Article item, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await base.StoreAsync(item, cancellationToken);
		}

		public async Task IncrementAsync(string id, Dictionary<Expression<Func<Article, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
		{
			await base.IncrementAsync<Article>(id, fields, cancellationToken);
		}

		public async Task DecrementAsync(string id, Dictionary<Expression<Func<Article, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
		{
			await base.DecrementAsync<Article>(id, fields, cancellationToken);
		}

		public async Task IncrementAsync(PostItemFilter filter, Dictionary<Expression<Func<Article, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
		{
			await base.IncrementAsync<Article>(GetFilterDefinition(filter), fields, cancellationToken);
		}

		public async Task DecrementAsync(PostItemFilter filter, Dictionary<Expression<Func<Article, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
		{
			await base.DecrementAsync<Article>(GetFilterDefinition(filter), fields, cancellationToken);
		}

		public async Task IncrementAsync(ArticleListFilter filter, Dictionary<Expression<Func<Article, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
		{
			await base.IncrementAsync<Article>(GetFilterDefinition(filter), fields, cancellationToken);
		}

		public async Task DecrementAsync(ArticleListFilter filter, Dictionary<Expression<Func<Article, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
		{
			await base.DecrementAsync<Article>(GetFilterDefinition(filter), fields, cancellationToken);
		}

		public async Task AddToSetAsync(ArticleListFilter filter, Expression<Func<Article, IEnumerable<string>>> field, List<string> values, CancellationToken cancellationToken = default(CancellationToken))
		{
			await base.AddToSetAsync<Article>(GetFilterDefinition(filter), field, values, cancellationToken);
		}

		public async Task UpdateAsync(ArticleListFilter filter, Dictionary<string, object> fields, CancellationToken cancellationToken = default(CancellationToken))
		{
			await base.UpdateAsync<Article>(GetFilterDefinition(filter), fields, cancellationToken);
		}

		public async Task UpdateAsync(string id, Dictionary<string, object> fields, CancellationToken cancellationToken = default(CancellationToken))
		{
			await base.UpdateAsync<Article>(id, fields, cancellationToken);
		}

		public async Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await base.ExistsAsync<Article>(id, cancellationToken);
		}

		public async Task<Article> GetAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
		{
			ObjectId objectId;
			if (ObjectId.TryParse(id, out objectId))
			{
				return await base.GetAsync<Article>(id, cancellationToken);
			}
			else
			{
				var filter = Builders<Article>.Filter.Eq(it => it.TitleId, id);
				return await base.GetAsync<Article>(filter, cancellationToken);
			}
		}

		public async Task<Article> FetchAsync(string id, IEnumerable<Expression<Func<Article, object>>> fields, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await base.FetchAsync<Article>(id, fields, cancellationToken);
		}

		public async Task<List<Article>> FetchAsync(List<string> ids, IEnumerable<Expression<Func<Article, object>>> fields, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await base.FetchAsync<Article>(ids, fields, cancellationToken);
		}

		public async Task<List<Article>> GetAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await base.GetAsync<Article>(ids, cancellationToken);
		}

		public async Task<List<Article>> GetAsync(ArticleListFilter filter, string sortOrder = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await base.GetAsync<Article>(GetFilterDefinition(filter), sortOrder, cancellationToken);
		}

		public async Task<PagedResult<Article>> GetPageAsync(ArticleListFilter filter, int pageNumber, int pageSize, string sortOrder = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await base.GetPageAsync<Article>(GetFilterDefinition(filter), pageNumber, pageSize, sortOrder, cancellationToken);
		}

		public async Task<long> GetCountAsync(ArticleListFilter filter, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await base.GetCountAsync<Article>(GetFilterDefinition(filter), cancellationToken);
		}

		public async Task DeleteAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
		{
			await base.DeleteAsync<Article>(id, cancellationToken);
		}

		public async Task DeleteAsync(ArticleListFilter filter, CancellationToken cancellationToken = default(CancellationToken))
		{
			await base.DeleteAsync<Article>(GetFilterDefinition(filter), cancellationToken);
		}

		public async Task DeleteAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			await base.DeleteAsync<Article>(cancellationToken);
		}

		FilterDefinition<Article> GetFilterDefinition(PostItemFilter filter)
		{
			var builder = Builders<Article>.Filter;

			var queries = new List<FilterDefinition<Article>>();

			var baseFilter = base.GetFilterDefinition<Article>(filter);
			queries.Add(baseFilter);

			//if (!string.IsNullOrWhiteSpace(filter.WinnerResponseId))
			//{
			//    queries.Add(builder.Eq(it => it.WinnerResponse.Id, filter.WinnerResponseId));
			//}

			return builder.And(queries);
		}

		FilterDefinition<Article> GetFilterDefinition(ArticleListFilter filter)
		{
			var builder = Builders<Article>.Filter;

			var queries = new List<FilterDefinition<Article>>();

			var baseFilter = base.GetFilterDefinition<Article>(filter);
			queries.Add(baseFilter);


			if (!string.IsNullOrWhiteSpace(filter.CategoryValue))
			{
				queries.Add(
					builder.Eq(it => it.Category.CategoryValue, filter.CategoryValue)
					);

			}

			if (!string.IsNullOrWhiteSpace(filter.SubCategoryValue))
			{
				queries.Add(
					builder.Eq(it => it.Category.SubCategoryValue, filter.SubCategoryValue)
					);

			}

			if (!string.IsNullOrWhiteSpace(filter.AreaValue))
			{
				queries.Add(
					builder.Eq(it => it.Category.AreaValue, filter.AreaValue)
					);
			}

			if (filter.Interesting != null)
			{
				queries.Add(builder.Eq(it => it.Interesting, (bool)filter.Interesting));
			}

			if (filter.Top != null)
			{
				queries.Add(builder.Eq(it => it.Top, (bool)filter.Top));
			}

			if (filter.Published != null)
			{
				queries.Add(builder.Eq(it => it.Published, (bool)filter.Published));
			}

			if (filter.ExcludeAreaValue != null && filter.ExcludeAreaValue.Any())
			{
				queries.Add(builder.Nin(it => it.Category.AreaValue, filter.ExcludeAreaValue.ToList()));
			}

			if (!string.IsNullOrWhiteSpace(filter.Criteria))
			{
				if (filter.CriteriaType != null && filter.CriteriaType == CriteriaType.Regex)
				{
					var words = filter.Criteria.ToLower().Split(' ').Select(it => it.Trim()).Where(it => it.Length > 1).ToList();
					if (words.Any())
					{
						queries.AddRange(words.Select(word => builder.Regex(pr => pr.Keywords, new BsonRegularExpression("/^" + word + ".*/"))));
						//queries.Add(Query.And(words.Select(it => Query<Post>.Matches(i => i.Keywords, new BsonRegularExpression("/^" + it + ".*/")))));
					}
				}
				else
				{
					var criteria = filter.Criteria.Split(' ')
						.Select(i => i.Trim())
						.Where(i => i.Length > 1)
						.ToList()
						.Aggregate("", (current, word) => current + ("\"" + word + "\""));
					queries.Add(builder.Text(criteria));
				}
			}

			return builder.And(queries);
		}
	}
}
