using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using kawaii.twitter.db;
using MongoDB.Bson.Serialization;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace kawaii.twitter.core.tests.SelectLogic.Page
{
	/// <summary>
	/// Стаб только для теста аргументов
	/// </summary>
	class PagesCollectionStub : IMongoCollection<SitePage>
	{
		public CollectionNamespace CollectionNamespace => throw new NotImplementedException();

		public IMongoDatabase Database => throw new NotImplementedException();

		public IBsonSerializer<SitePage> DocumentSerializer => throw new NotImplementedException();

		public IMongoIndexManager<SitePage> Indexes => throw new NotImplementedException();

		public MongoCollectionSettings Settings => throw new NotImplementedException();

		public IAsyncCursor<TResult> Aggregate<TResult>(PipelineDefinition<SitePage, TResult> pipeline, AggregateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IAsyncCursor<TResult> Aggregate<TResult>(IClientSessionHandle session, PipelineDefinition<SitePage, TResult> pipeline, AggregateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IAsyncCursor<TResult>> AggregateAsync<TResult>(PipelineDefinition<SitePage, TResult> pipeline, AggregateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IAsyncCursor<TResult>> AggregateAsync<TResult>(IClientSessionHandle session, PipelineDefinition<SitePage, TResult> pipeline, AggregateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public BulkWriteResult<SitePage> BulkWrite(IEnumerable<WriteModel<SitePage>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public BulkWriteResult<SitePage> BulkWrite(IClientSessionHandle session, IEnumerable<WriteModel<SitePage>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<BulkWriteResult<SitePage>> BulkWriteAsync(IEnumerable<WriteModel<SitePage>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<BulkWriteResult<SitePage>> BulkWriteAsync(IClientSessionHandle session, IEnumerable<WriteModel<SitePage>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public long Count(FilterDefinition<SitePage> filter, CountOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public long Count(IClientSessionHandle session, FilterDefinition<SitePage> filter, CountOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<long> CountAsync(FilterDefinition<SitePage> filter, CountOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<long> CountAsync(IClientSessionHandle session, FilterDefinition<SitePage> filter, CountOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public long CountDocuments(FilterDefinition<SitePage> filter, CountOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public long CountDocuments(IClientSessionHandle session, FilterDefinition<SitePage> filter, CountOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<long> CountDocumentsAsync(FilterDefinition<SitePage> filter, CountOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<long> CountDocumentsAsync(IClientSessionHandle session, FilterDefinition<SitePage> filter, CountOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public DeleteResult DeleteMany(FilterDefinition<SitePage> filter, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public DeleteResult DeleteMany(FilterDefinition<SitePage> filter, DeleteOptions options, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public DeleteResult DeleteMany(IClientSessionHandle session, FilterDefinition<SitePage> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<DeleteResult> DeleteManyAsync(FilterDefinition<SitePage> filter, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<DeleteResult> DeleteManyAsync(FilterDefinition<SitePage> filter, DeleteOptions options, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<DeleteResult> DeleteManyAsync(IClientSessionHandle session, FilterDefinition<SitePage> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public DeleteResult DeleteOne(FilterDefinition<SitePage> filter, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public DeleteResult DeleteOne(FilterDefinition<SitePage> filter, DeleteOptions options, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public DeleteResult DeleteOne(IClientSessionHandle session, FilterDefinition<SitePage> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<DeleteResult> DeleteOneAsync(FilterDefinition<SitePage> filter, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<DeleteResult> DeleteOneAsync(FilterDefinition<SitePage> filter, DeleteOptions options, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<DeleteResult> DeleteOneAsync(IClientSessionHandle session, FilterDefinition<SitePage> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IAsyncCursor<TField> Distinct<TField>(FieldDefinition<SitePage, TField> field, FilterDefinition<SitePage> filter, DistinctOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IAsyncCursor<TField> Distinct<TField>(IClientSessionHandle session, FieldDefinition<SitePage, TField> field, FilterDefinition<SitePage> filter, DistinctOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IAsyncCursor<TField>> DistinctAsync<TField>(FieldDefinition<SitePage, TField> field, FilterDefinition<SitePage> filter, DistinctOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IAsyncCursor<TField>> DistinctAsync<TField>(IClientSessionHandle session, FieldDefinition<SitePage, TField> field, FilterDefinition<SitePage> filter, DistinctOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public long EstimatedDocumentCount(EstimatedDocumentCountOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<long> EstimatedDocumentCountAsync(EstimatedDocumentCountOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IAsyncCursor<TProjection>> FindAsync<TProjection>(FilterDefinition<SitePage> filter, FindOptions<SitePage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IAsyncCursor<TProjection>> FindAsync<TProjection>(IClientSessionHandle session, FilterDefinition<SitePage> filter, FindOptions<SitePage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public TProjection FindOneAndDelete<TProjection>(FilterDefinition<SitePage> filter, FindOneAndDeleteOptions<SitePage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public TProjection FindOneAndDelete<TProjection>(IClientSessionHandle session, FilterDefinition<SitePage> filter, FindOneAndDeleteOptions<SitePage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<TProjection> FindOneAndDeleteAsync<TProjection>(FilterDefinition<SitePage> filter, FindOneAndDeleteOptions<SitePage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<TProjection> FindOneAndDeleteAsync<TProjection>(IClientSessionHandle session, FilterDefinition<SitePage> filter, FindOneAndDeleteOptions<SitePage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public TProjection FindOneAndReplace<TProjection>(FilterDefinition<SitePage> filter, SitePage replacement, FindOneAndReplaceOptions<SitePage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public TProjection FindOneAndReplace<TProjection>(IClientSessionHandle session, FilterDefinition<SitePage> filter, SitePage replacement, FindOneAndReplaceOptions<SitePage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<TProjection> FindOneAndReplaceAsync<TProjection>(FilterDefinition<SitePage> filter, SitePage replacement, FindOneAndReplaceOptions<SitePage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<TProjection> FindOneAndReplaceAsync<TProjection>(IClientSessionHandle session, FilterDefinition<SitePage> filter, SitePage replacement, FindOneAndReplaceOptions<SitePage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public TProjection FindOneAndUpdate<TProjection>(FilterDefinition<SitePage> filter, UpdateDefinition<SitePage> update, FindOneAndUpdateOptions<SitePage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public TProjection FindOneAndUpdate<TProjection>(IClientSessionHandle session, FilterDefinition<SitePage> filter, UpdateDefinition<SitePage> update, FindOneAndUpdateOptions<SitePage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<TProjection> FindOneAndUpdateAsync<TProjection>(FilterDefinition<SitePage> filter, UpdateDefinition<SitePage> update, FindOneAndUpdateOptions<SitePage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<TProjection> FindOneAndUpdateAsync<TProjection>(IClientSessionHandle session, FilterDefinition<SitePage> filter, UpdateDefinition<SitePage> update, FindOneAndUpdateOptions<SitePage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IAsyncCursor<TProjection> FindSync<TProjection>(FilterDefinition<SitePage> filter, FindOptions<SitePage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IAsyncCursor<TProjection> FindSync<TProjection>(IClientSessionHandle session, FilterDefinition<SitePage> filter, FindOptions<SitePage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public void InsertMany(IEnumerable<SitePage> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public void InsertMany(IClientSessionHandle session, IEnumerable<SitePage> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task InsertManyAsync(IEnumerable<SitePage> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task InsertManyAsync(IClientSessionHandle session, IEnumerable<SitePage> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public void InsertOne(SitePage document, InsertOneOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public void InsertOne(IClientSessionHandle session, SitePage document, InsertOneOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task InsertOneAsync(SitePage document, CancellationToken _cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task InsertOneAsync(SitePage document, InsertOneOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task InsertOneAsync(IClientSessionHandle session, SitePage document, InsertOneOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IAsyncCursor<TResult> MapReduce<TResult>(BsonJavaScript map, BsonJavaScript reduce, MapReduceOptions<SitePage, TResult> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IAsyncCursor<TResult> MapReduce<TResult>(IClientSessionHandle session, BsonJavaScript map, BsonJavaScript reduce, MapReduceOptions<SitePage, TResult> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IAsyncCursor<TResult>> MapReduceAsync<TResult>(BsonJavaScript map, BsonJavaScript reduce, MapReduceOptions<SitePage, TResult> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IAsyncCursor<TResult>> MapReduceAsync<TResult>(IClientSessionHandle session, BsonJavaScript map, BsonJavaScript reduce, MapReduceOptions<SitePage, TResult> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IFilteredMongoCollection<TDerivedDocument> OfType<TDerivedDocument>() where TDerivedDocument : SitePage
		{
			throw new NotImplementedException();
		}

		public ReplaceOneResult ReplaceOne(FilterDefinition<SitePage> filter, SitePage replacement, ReplaceOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public ReplaceOneResult ReplaceOne(FilterDefinition<SitePage> filter, SitePage replacement, UpdateOptions options, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public ReplaceOneResult ReplaceOne(IClientSessionHandle session, FilterDefinition<SitePage> filter, SitePage replacement, ReplaceOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public ReplaceOneResult ReplaceOne(IClientSessionHandle session, FilterDefinition<SitePage> filter, SitePage replacement, UpdateOptions options, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<ReplaceOneResult> ReplaceOneAsync(FilterDefinition<SitePage> filter, SitePage replacement, ReplaceOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<ReplaceOneResult> ReplaceOneAsync(FilterDefinition<SitePage> filter, SitePage replacement, UpdateOptions options, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<ReplaceOneResult> ReplaceOneAsync(IClientSessionHandle session, FilterDefinition<SitePage> filter, SitePage replacement, ReplaceOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<ReplaceOneResult> ReplaceOneAsync(IClientSessionHandle session, FilterDefinition<SitePage> filter, SitePage replacement, UpdateOptions options, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public UpdateResult UpdateMany(FilterDefinition<SitePage> filter, UpdateDefinition<SitePage> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public UpdateResult UpdateMany(IClientSessionHandle session, FilterDefinition<SitePage> filter, UpdateDefinition<SitePage> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<UpdateResult> UpdateManyAsync(FilterDefinition<SitePage> filter, UpdateDefinition<SitePage> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<UpdateResult> UpdateManyAsync(IClientSessionHandle session, FilterDefinition<SitePage> filter, UpdateDefinition<SitePage> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public UpdateResult UpdateOne(FilterDefinition<SitePage> filter, UpdateDefinition<SitePage> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public UpdateResult UpdateOne(IClientSessionHandle session, FilterDefinition<SitePage> filter, UpdateDefinition<SitePage> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<UpdateResult> UpdateOneAsync(FilterDefinition<SitePage> filter, UpdateDefinition<SitePage> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<UpdateResult> UpdateOneAsync(IClientSessionHandle session, FilterDefinition<SitePage> filter, UpdateDefinition<SitePage> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IChangeStreamCursor<TResult> Watch<TResult>(PipelineDefinition<ChangeStreamDocument<SitePage>, TResult> pipeline, ChangeStreamOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IChangeStreamCursor<TResult> Watch<TResult>(IClientSessionHandle session, PipelineDefinition<ChangeStreamDocument<SitePage>, TResult> pipeline, ChangeStreamOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IChangeStreamCursor<TResult>> WatchAsync<TResult>(PipelineDefinition<ChangeStreamDocument<SitePage>, TResult> pipeline, ChangeStreamOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IChangeStreamCursor<TResult>> WatchAsync<TResult>(IClientSessionHandle session, PipelineDefinition<ChangeStreamDocument<SitePage>, TResult> pipeline, ChangeStreamOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IMongoCollection<SitePage> WithReadConcern(ReadConcern readConcern)
		{
			throw new NotImplementedException();
		}

		public IMongoCollection<SitePage> WithReadPreference(ReadPreference readPreference)
		{
			throw new NotImplementedException();
		}

		public IMongoCollection<SitePage> WithWriteConcern(WriteConcern writeConcern)
		{
			throw new NotImplementedException();
		}
	}
}
