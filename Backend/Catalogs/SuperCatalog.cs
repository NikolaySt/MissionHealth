using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

using SocialNet.Backend.DataFilters;
using SocialNet.Backend.DataObjects;
using SocialNet.Backend.Database;

namespace SocialNet.Backend.Catalogs
{
    public abstract class SuperCatalog
    {
        private readonly Application _application;
        private readonly MongoProvider _provider;

        public static void Init()
        {
            BsonClassMap.RegisterClassMap<DataObject>(initializer =>
            {
                initializer.AutoMap();
                initializer.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<IdDataObject>(initializer =>
            {
                initializer.AutoMap();
                initializer.MapIdProperty(it => it.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
                initializer.SetIgnoreExtraElements(true);
            });
        }

        protected SuperCatalog(Application application, MongoProvider provider = null)
        {
            _provider = provider ?? MongoProvider.Instance;

            if (application == null) return;

            _application = new Application
            {
                Id = "",//(string)application.Id.Clone(),
            };
        }

        protected abstract Dictionary<string, string> GetMapperProperties();

        protected Application Application
        {
            get { return _application; }
        }

        protected string GenerateId()
        {
            return ObjectId.GenerateNewId().ToString();
        }

        protected Exception GetInvalidApplicationIdException()
        {
            return new ApplicationException("Catalaog application Id does not match item application Id");
        }

        protected Exception GetNotReachableException()
        {
            return new ApplicationException("Database not reachable");
        }

        protected Exception GetOperationFailedException()
        {
            return new ApplicationException("Database operation failed");
        }

        protected Exception GetUnathorizedException()
        {
            return new UnauthorizedAccessException("Context application cannot write to this application scope");
        }

        protected Exception GetUncopatibleFilterException()
        {
            return new ApplicationException("Incompatible filter type");
        }


        protected IMongoCollection<T> GetAsyncCollection<T>()
            where T : IdDataObject, new()
        {
            var collection = _provider.GetAsyncCollection<T>();
            if (collection == null) throw GetNotReachableException();

            return collection;
        }


        protected virtual SortDefinition<T> GetSortDefinition<T>(string sortOrder)
            where T : IdDataObject, new()
        {
            if (string.IsNullOrWhiteSpace(sortOrder)) return null;

            var builder = Builders<T>.Sort;

            var sortOrders = sortOrder.Trim().Split(',');

            if (!sortOrders.Any()) return null;

            var definitions = new List<SortDefinition<T>>();

            foreach (var field in sortOrders)
            {
                var type = field.Trim().Split(' ');
                if (!type.Any()) continue;

                var bsonElementName = type[0];

                if (type.Length == 2 && type[1].ToUpper() == "DESC")
                {
                    definitions.Add(builder.Descending(new StringFieldDefinition<T>(bsonElementName)));
                    continue;
                }

                definitions.Add(builder.Ascending(new StringFieldDefinition<T>(bsonElementName)));
            }

            return builder.Combine(definitions);
        }

        protected virtual SortDefinition<T> GetSortDefinition<T>(Dictionary<Expression<Func<T, object>>, string> sortOrders)
            where T : IdDataObject, new()
        {
            if (!sortOrders.Any()) return null;

            var builder = Builders<T>.Sort;

            var definitions = new List<SortDefinition<T>>();

            foreach (var field in sortOrders)
            {
                if (field.Value.ToUpper().Trim() == "DESC")
                {
                    definitions.Add(builder.Descending(field.Key));
                    continue;
                }
                definitions.Add(builder.Ascending(field.Key));
            }
            return builder.Combine(definitions);
        }

        protected FilterDefinition<T> GetFilterDefinition<T>(IEnumerable<string> ids)
             where T : IdDataObject, new()
        {
            var builder = Builders<T>.Filter;

            return builder.In(it => it.Id, ids);
        }

        protected FilterDefinition<T> GetFilterDefinition<T>(string id)
             where T : IdDataObject, new()
        {
            var builder = Builders<T>.Filter;

            return builder.Eq(it => it.Id, id);
        }

        protected virtual FilterDefinition<T> GetFilterDefinition<T>(SuperListFilter superFilter) where T : IdDataObject, new()
        {
            var queries = new List<FilterDefinition<T>>();

            var builder = Builders<T>.Filter;

            if (superFilter.CreatedAfter != null)
            {
                queries.Add(builder.Gte(it => it.Created, BsonValue.Create(superFilter.CreatedAfter)));
            }

            if (superFilter.UpdatedAfter != null)
            {
                queries.Add(builder.Gte(it => it.Updated, BsonValue.Create(superFilter.UpdatedAfter)));
            }

            if (queries.Count == 0) return new BsonDocumentFilterDefinition<T>(new BsonDocument());

            return builder.And(queries);
        }

        protected virtual FilterDefinition<T> GetFilterDefinition<T>(SuperFilter superFilter) where T : IdDataObject, new()
        {
            return new BsonDocumentFilterDefinition<T>(new BsonDocument()); ;
        }

        protected async Task<T> StoreAsync<T>(T item, CancellationToken cancellationToken = default(CancellationToken))
            where T : IdDataObject, new()
        {
            if (item == null) throw new ArgumentNullException("item");

            if (item.IsNew)
            {
                if (String.IsNullOrWhiteSpace(item.AppId))
                {
                    if (Application != null)
                    {
                        item.AppId = Application.Id;
                    }
                }
                item.Created = DateTime.UtcNow;
            }


            item.Updated = DateTime.UtcNow;

            var collection = GetAsyncCollection<T>();

            if (!item.IsNew)
            {
                await collection.ReplaceOneAsync(Builders<T>.Filter.Eq(it => it.Id, item.Id), item, null, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await collection.InsertOneAsync(item, cancellationToken).ConfigureAwait(false);
            }

            return item;
        }

        protected async Task UpdateAsync<T>(FilterDefinition<T> filterDefinition, Dictionary<string, object> fields, CancellationToken cancellationToken = default(CancellationToken)) where T : IdDataObject, new()
        {
            if (filterDefinition == null) throw new ArgumentException("filterDefinition");

            var updates = new List<UpdateDefinition<T>>();
            foreach (var field in fields)
            {
                var bsonMemberName = field.Key;

                var update = ParseToUpdateDefinition<T>(bsonMemberName, field.Value);

                updates.Add(update);
            }

            var builder = Builders<T>.Update;
            var updatrFilter = builder.Combine(updates);

            var collection = GetAsyncCollection<T>();

            await collection.UpdateManyAsync(filterDefinition, updatrFilter, null, cancellationToken).ConfigureAwait(false);
        }

        protected async Task UpdateAsync<T>(string id, Dictionary<string, object> fields, CancellationToken cancellationToken = default(CancellationToken))
            where T : IdDataObject, new()
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("id");

            var filterDefinition = GetFilterDefinition<T>(id);
            //var filterDefinition = Builders<T>.Filter.And(GetFilterDefinition<T>(filter), GetScopeFilterDefinition<T>());

            var updates = new List<UpdateDefinition<T>>();

            foreach (var field in fields)
            {
                var bsonMemberName = field.Key;

                var update = ParseToUpdateDefinition<T>(bsonMemberName, field.Value);

                updates.Add(update);
            }

            var builder = Builders<T>.Update;
            var updatrFilter = builder.Combine(updates);

            var collection = GetAsyncCollection<T>();

            await collection.UpdateManyAsync(filterDefinition, updatrFilter, new UpdateOptions(), cancellationToken).ConfigureAwait(false);
        }

        private UpdateDefinition<T> ParseToUpdateDefinition<T>(string field, object value) where T : class
        {            
            var collection = value as ICollection;
            if (collection != null)
            {
                var array = new BsonArray();

                foreach (var item in collection)
                {
                    if (item is IdDataObject)
                    {
                        array.Add(item.ToBsonDocument(item.GetType()));
                    }
                    else
                    {
                        try
                        {
                            array.Add(BsonValue.Create(item));
                        }
                        catch
                        {
                            array.Add(item.ToBsonDocument(item.GetType()));
                        }
                    }
                }
                return Builders<T>.Update.Set(field, array);;
            }

            if (value is IdDataObject)
            {
                return Builders<T>.Update.Set(field, value.ToBsonDocument(value.GetType()));
            }

            try
            {
                return Builders<T>.Update.Set(field, BsonValue.Create(value));
            }
            catch
            {
                return Builders<T>.Update.Set(field, value.ToBsonDocument(value.GetType()));
            }
        }

        protected async Task UpdatesertAsync<T>(FilterDefinition<T> filterDefinition, Dictionary<string, object> fields, CancellationToken cancellationToken = default(CancellationToken))
                  where T : IdDataObject, new()
        {
            if (filterDefinition == null) throw new ArgumentException("filterDefinition");

            var updates = new List<UpdateDefinition<T>>();

            foreach (var field in fields)
            {
                var bsonMemberName = field.Key;
                var update = Builders<T>.Update.Set(bsonMemberName, BsonValue.Create(field.Value));
                updates.Add(update);
            }

            var builder = Builders<T>.Update;
            var updatrFilter = builder.Combine(updates);

            var collection = GetAsyncCollection<T>();

            var options = new UpdateOptions { IsUpsert = true };

            await collection.UpdateOneAsync(filterDefinition, updatrFilter, options, cancellationToken);
        }

        protected async Task IncrementAsync<T>(string id, Dictionary<Expression<Func<T, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
            where T : IdDataObject, new()
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("id");

            var filterDefinition = GetFilterDefinition<T>(id);

            var updates = fields.Select(field => Builders<T>.Update.Inc(field.Key, field.Value)).ToList();

            var builder = Builders<T>.Update;
            var updatrFilter = builder.Combine(updates);

            var collection = GetAsyncCollection<T>();

            await collection.UpdateManyAsync(filterDefinition, updatrFilter, null, cancellationToken).ConfigureAwait(false);
        }

        protected async Task IncrementAsync<T>(FilterDefinition<T> filterDefinition, Dictionary<Expression<Func<T, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
          where T : IdDataObject, new()
        {
            if (filterDefinition == null) throw new ArgumentException("filterDefinition");

            var updates = fields.Select(field => Builders<T>.Update.Inc(field.Key, field.Value)).ToList();

            var builder = Builders<T>.Update;

            var updatrFilter = builder.Combine(updates);

            var collection = GetAsyncCollection<T>();

            await collection.UpdateManyAsync(filterDefinition, updatrFilter, null, cancellationToken).ConfigureAwait(false);
        }

        protected async Task IncsertOneAsync<T>(FilterDefinition<T> filterDefinition, Dictionary<Expression<Func<T, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
          where T : IdDataObject, new()
        {
            if (filterDefinition == null) throw new ArgumentException("filterDefinition");

            var updates = fields.Select(field => Builders<T>.Update.Inc(field.Key, field.Value)).ToList();

            var builder = Builders<T>.Update;

            var updatrFilter = builder.Combine(updates);

            var collection = GetAsyncCollection<T>();

            var options = new UpdateOptions { IsUpsert = true };

            await collection.UpdateOneAsync(filterDefinition, updatrFilter, options, cancellationToken).ConfigureAwait(false);
        }

        protected async Task DecrementAsync<T>(string id, Dictionary<Expression<Func<T, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
            where T : IdDataObject, new()
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("id");

            var filterDefinition = GetFilterDefinition<T>(id);

            var updates = fields.Select(field => Builders<T>.Update.Inc(field.Key, -field.Value)).ToList();

            var builder = Builders<T>.Update;
            var updatrFilter = builder.Combine(updates);

            var collection = GetAsyncCollection<T>();

            await collection.UpdateManyAsync(filterDefinition, updatrFilter, null, cancellationToken).ConfigureAwait(false);
        }

        protected async Task DecrementAsync<T>(FilterDefinition<T> filterDefinition, Dictionary<Expression<Func<T, long>>, long> fields, CancellationToken cancellationToken = default(CancellationToken))
            where T : IdDataObject, new()
        {
            if (filterDefinition == null) throw new ArgumentException("filterDefinition");

            var updates = fields.Select(field => Builders<T>.Update.Inc(field.Key, -field.Value)).ToList();

            var builder = Builders<T>.Update;

            var updatrFilter = builder.Combine(updates);

            var collection = GetAsyncCollection<T>();

            await collection.UpdateManyAsync(filterDefinition, updatrFilter, null, cancellationToken).ConfigureAwait(false);
        }

        protected async Task AddToSetAsync<T>(FilterDefinition<T> filterDefinition, Expression<Func<T, IEnumerable<string>>> field, List<string> values, CancellationToken cancellationToken = default(CancellationToken))
            where T : IdDataObject, new()
        {
            if (filterDefinition == null) throw new ArgumentException("filter");

            var update = Builders<T>.Update.AddToSetEach(field, values);

            var collection = GetAsyncCollection<T>();

            await collection.UpdateManyAsync(filterDefinition, update, null, cancellationToken).ConfigureAwait(false);
        }

        protected async Task AddToSetAsync<T>(string id, Expression<Func<T, IEnumerable<string>>> field, List<string> values, CancellationToken cancellationToken = default(CancellationToken))
            where T : IdDataObject, new()
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("id");

            var filterDefinition = GetFilterDefinition<T>(id);

            var update = Builders<T>.Update.AddToSetEach(field, values);

            var collection = GetAsyncCollection<T>();

            await collection.UpdateManyAsync(filterDefinition, update, null, cancellationToken).ConfigureAwait(false);
        }

        protected async Task<bool> ExistsAsync<T>(string id, CancellationToken cancellationToken = default(CancellationToken))
            where T : IdDataObject, new()
        {

            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("id");

            var filterDefinition = GetFilterDefinition<T>(id);

            var collection = GetAsyncCollection<T>();

            var count = await collection.CountAsync(filterDefinition, null, cancellationToken).ConfigureAwait(false);

            return count > 0;
        }

        protected async Task<bool> ExistsAsync<T>(FilterDefinition<T> filterDefinition, CancellationToken cancellationToken = default(CancellationToken))
            where T : IdDataObject, new()
        {

            if (filterDefinition == null) throw new ArgumentException("filter");

            var collection = GetAsyncCollection<T>();

            var count = await collection.CountAsync(filterDefinition, null, cancellationToken).ConfigureAwait(false);

            return count > 0;
        }

        protected async Task<T> GetAsync<T>(string id, CancellationToken cancellationToken = default(CancellationToken))
            where T : IdDataObject, new()
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("id");

            var filterDefinition = GetFilterDefinition<T>(id);

            var collection = GetAsyncCollection<T>();

            using (var cursor = await collection.FindAsync(filterDefinition, new FindOptions<T, T>(), cancellationToken).ConfigureAwait(false))
            {
                await cursor.MoveNextAsync(cancellationToken).ConfigureAwait(false);
                return cursor.Current.FirstOrDefault();
            }
        }

        protected async Task<List<T>> GetAsync<T>(IEnumerable<string> ids, CancellationToken cancellationToken = default(CancellationToken))
            where T : IdDataObject, new()
        {
            var enumerable = ids as string[] ?? ids.ToArray();
            if (ids == null || !enumerable.Any()) throw new ArgumentException("ids");

            var filterDefinition = GetFilterDefinition<T>(enumerable);

            var collection = GetAsyncCollection<T>();

            var findOptions = new FindOptions<T, T> { BatchSize = enumerable.Count() };

            var batch = new List<T>();
            using (var cursor = await collection.FindAsync(filterDefinition, findOptions, cancellationToken).ConfigureAwait(false))
            {
                while (await cursor.MoveNextAsync(cancellationToken).ConfigureAwait(false))
                {
                    batch.AddRange(cursor.Current.ToList());
                }
            }

            return batch;
        }

        protected async Task<List<T>> GetAsync<T>(CancellationToken cancellationToken = default(CancellationToken))
            where T : IdDataObject, new()
        {         
            var filterDefinition = new BsonDocumentFilterDefinition<T>(new BsonDocument());

            var collection = GetAsyncCollection<T>();

            var findOptions = new FindOptions<T, T>();

            var batch = new List<T>();
            using (var cursor = await collection.FindAsync(filterDefinition, findOptions, cancellationToken).ConfigureAwait(false))
            {
                while (await cursor.MoveNextAsync(cancellationToken).ConfigureAwait(false))
                {
                    batch.AddRange(cursor.Current.ToList());
                }
            }

            return batch;
        }

        protected async Task<T> GetAsync<T>(FilterDefinition<T> filterDefinition, CancellationToken cancellationToken = default(CancellationToken))
            where T : IdDataObject, new()
        {
            if (filterDefinition == null) throw new ArgumentException("filterDefinition");

            var collection = GetAsyncCollection<T>();

            var findOptions = new FindOptions<T, T>();

            using (var cursor = await collection.FindAsync(filterDefinition, findOptions, cancellationToken).ConfigureAwait(false))
            {
                await cursor.MoveNextAsync(cancellationToken).ConfigureAwait(false);

                return cursor.Current.FirstOrDefault();
            }
        }

        protected async Task<List<T>> GetAsync<T>(FilterDefinition<T> filterDefinition, Dictionary<Expression<Func<T, object>>, string> sortOrder, CancellationToken cancellationToken = default(CancellationToken))
          where T : IdDataObject, new()
        {
            if (filterDefinition == null) throw new ArgumentNullException("filterDefinition");

            var collection = GetAsyncCollection<T>();

            var sortDefinition = GetSortDefinition(sortOrder);
            var findOptions = new FindOptions<T, T>();
            if (sortDefinition != null) findOptions.Sort = sortDefinition;

            using (var cursor = await collection.FindAsync(filterDefinition, findOptions, cancellationToken).ConfigureAwait(false))
            {
                await cursor.MoveNextAsync(cancellationToken).ConfigureAwait(false);

                return cursor.Current.ToList();
            }
        }

        protected async Task<PagedResult<T>> GetPageAsync<T>(FilterDefinition<T> filter, int pageNumber, int pageSize, Dictionary<Expression<Func<T, object>>, string> sortOrder, CancellationToken cancellationToken = default(CancellationToken))
            where T : IdDataObject, new()
        {

            if (filter == null) throw new ArgumentNullException("filter");

            var collection = GetAsyncCollection<T>();

            var sortDefinition = GetSortDefinition(sortOrder);
            var findOptions = new FindOptions<T, T> { Skip = pageNumber * pageSize, Limit = pageSize };

            if (sortDefinition != null) findOptions.Sort = sortDefinition;

            List<T> items;
            using (var cursor = await collection.FindAsync(filter, findOptions, cancellationToken).ConfigureAwait(false))
            {
                await cursor.MoveNextAsync(cancellationToken).ConfigureAwait(false);

                items = cursor.Current.ToList();
            }

            var result = new PagedResult<T>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = items
            };

            return result;
        }

        protected async Task<List<T>> GetAsync<T>(FilterDefinition<T> filterDefinition, string sortOrder, CancellationToken cancellationToken = default(CancellationToken))
          where T : IdDataObject, new()
        {
            if (filterDefinition == null) throw new ArgumentNullException("filterDefinition");

            var collection = GetAsyncCollection<T>();

            var sortDefinition = GetSortDefinition<T>(sortOrder);
            var findOptions = new FindOptions<T, T>();
            if (sortDefinition != null) findOptions.Sort = sortDefinition;

            using (var cursor = await collection.FindAsync(filterDefinition, findOptions, cancellationToken).ConfigureAwait(false))
            {
                await cursor.MoveNextAsync(cancellationToken).ConfigureAwait(false);

                return cursor.Current.ToList();
            }
        }

        protected async Task<PagedResult<T>> GetPageAsync<T>(FilterDefinition<T> filter, int pageNumber, int pageSize, string sortOrder = null, CancellationToken cancellationToken = default(CancellationToken))
            where T : IdDataObject, new()
        {

            if (filter == null) throw new ArgumentNullException("filter");

            var collection = GetAsyncCollection<T>();

            var sortDefinition = GetSortDefinition<T>(sortOrder);
            var findOptions = new FindOptions<T, T> { Skip = pageNumber * pageSize, Limit = pageSize };

            if (sortDefinition != null) findOptions.Sort = sortDefinition;

            List<T> items;
            using (var cursor = await collection.FindAsync(filter, findOptions, cancellationToken).ConfigureAwait(false))
            {
                await cursor.MoveNextAsync(cancellationToken).ConfigureAwait(false);

                items = cursor.Current.ToList();
            }

            var result = new PagedResult<T>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = items.ToList()
            };

            return result;
        }

        protected async Task<long> GetCountAsync<T>(FilterDefinition<T> filterDefinition, CancellationToken cancellationToken = default(CancellationToken))
            where T : IdDataObject, new()
        {
            if (filterDefinition == null) throw new ArgumentException("filterDefinition");

            var collection = GetAsyncCollection<T>();

            return await collection.CountAsync(filterDefinition, null, cancellationToken).ConfigureAwait(false);
        }

        protected async Task<T> FetchAsync<T>(string id, IEnumerable<Expression<Func<T, object>>> fields, CancellationToken cancellationToken = default(CancellationToken))
          where T : IdDataObject, new()
        {

            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("id");

            var filterDefinition = GetFilterDefinition<T>(id);

            var findOptions = new FindOptions<T, T>();

            var builder = Builders<T>.Projection;
            foreach (var field in fields)
            {
                findOptions.Projection = builder.Include(field);
            }

            var collection = GetAsyncCollection<T>();

            using (var cursor = await collection.FindAsync(filterDefinition, findOptions, cancellationToken).ConfigureAwait(false))
            {
                await cursor.MoveNextAsync(cancellationToken).ConfigureAwait(false);

                return cursor.Current.FirstOrDefault();
            }
        }

        protected async Task<List<T>> FetchAsync<T>(List<string> ids, IEnumerable<Expression<Func<T, object>>> fields, CancellationToken cancellationToken = default(CancellationToken))
          where T : IdDataObject, new()
        {

            if (ids == null || ids.Count == 0) throw new ArgumentException("ids");

            var filterDefinition = GetFilterDefinition<T>(ids);

            var findOptions = new FindOptions<T, T>();

            var builder = Builders<T>.Projection;
            foreach (var field in fields)
            {
                findOptions.Projection = builder.Include(field);
            }

            var collection = GetAsyncCollection<T>();

            using (var cursor = await collection.FindAsync(filterDefinition, findOptions, cancellationToken).ConfigureAwait(false))
            {
                await cursor.MoveNextAsync(cancellationToken).ConfigureAwait(false);

                return cursor.Current.ToList();
            }
        }

        protected async Task<List<T>> FetchAsync<T>(FilterDefinition<T> filterDefinition, IEnumerable<string> fields, CancellationToken cancellationToken = default(CancellationToken))
             where T : IdDataObject, new()
        {
            if (filterDefinition == null) throw new ArgumentException("filter");

            var findOptions = new FindOptions<T, T>();

            var builder = Builders<T>.Projection;
            foreach (var field in fields)
            {
                findOptions.Projection = builder.Include(field);
            }

            var collection = GetAsyncCollection<T>();

            using (var cursor = await collection.FindAsync(filterDefinition, findOptions, cancellationToken).ConfigureAwait(false))
            {
                await cursor.MoveNextAsync(cancellationToken).ConfigureAwait(false);

                return cursor.Current.ToList();
            }
        }

        protected async Task DeleteAsync<T>(string id, CancellationToken cancellationToken = default(CancellationToken))
            where T : IdDataObject, new()
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("id");

            var filterDefinition = GetFilterDefinition<T>(id);

            var collection = GetAsyncCollection<T>();

            await collection.DeleteOneAsync(filterDefinition, cancellationToken).ConfigureAwait(false);
        }

        protected async Task DeleteAsync<T>(FilterDefinition<T> filterDefinition, CancellationToken cancellationToken = default(CancellationToken))
            where T : IdDataObject, new()
        {
            if (filterDefinition == null) throw new ArgumentException("filterDefinition");

            var collection = GetAsyncCollection<T>();

            await collection.DeleteManyAsync(filterDefinition, cancellationToken).ConfigureAwait(false);
        }

        protected async Task DeleteAsync<T>(CancellationToken cancellationToken = default(CancellationToken))
            where T : IdDataObject, new()
        {
            if (Application == null) throw new InvalidOperationException();

            var filterDefinition = new BsonDocument();

            var collection = GetAsyncCollection<T>();

            await collection.DeleteManyAsync(filterDefinition, cancellationToken).ConfigureAwait(false);
        }

    }
}
