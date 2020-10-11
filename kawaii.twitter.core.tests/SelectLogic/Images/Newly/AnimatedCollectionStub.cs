using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using kawaii.twitter.db;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace kawaii.twitter.core.tests.SelectLogic.Images.Newly
{
	/// <summary>
	/// Стаб только для теста аргументов (не может работать в нормальном режиме)
	/// </summary>
	class AnimatedCollectionStub : IMongoCollection<AnimatedImage>
	{
		public CollectionNamespace CollectionNamespace => throw new NotImplementedException();

		public IMongoDatabase Database => throw new NotImplementedException();

		public IBsonSerializer<AnimatedImage> DocumentSerializer => throw new NotImplementedException();

		public IMongoIndexManager<AnimatedImage> Indexes => throw new NotImplementedException();

		public MongoCollectionSettings Settings => throw new NotImplementedException();

		public IAsyncCursor<TResult> Aggregate<TResult>(PipelineDefinition<AnimatedImage, TResult> pipeline, AggregateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IAsyncCursor<TResult> Aggregate<TResult>(IClientSessionHandle session, PipelineDefinition<AnimatedImage, TResult> pipeline, AggregateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IAsyncCursor<TResult>> AggregateAsync<TResult>(PipelineDefinition<AnimatedImage, TResult> pipeline, AggregateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IAsyncCursor<TResult>> AggregateAsync<TResult>(IClientSessionHandle session, PipelineDefinition<AnimatedImage, TResult> pipeline, AggregateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public void AggregateToCollection<TResult>(PipelineDefinition<AnimatedImage, TResult> pipeline, AggregateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public void AggregateToCollection<TResult>(IClientSessionHandle session, PipelineDefinition<AnimatedImage, TResult> pipeline, AggregateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task AggregateToCollectionAsync<TResult>(PipelineDefinition<AnimatedImage, TResult> pipeline, AggregateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task AggregateToCollectionAsync<TResult>(IClientSessionHandle session, PipelineDefinition<AnimatedImage, TResult> pipeline, AggregateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public BulkWriteResult<AnimatedImage> BulkWrite(IEnumerable<WriteModel<AnimatedImage>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public BulkWriteResult<AnimatedImage> BulkWrite(IClientSessionHandle session, IEnumerable<WriteModel<AnimatedImage>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<BulkWriteResult<AnimatedImage>> BulkWriteAsync(IEnumerable<WriteModel<AnimatedImage>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<BulkWriteResult<AnimatedImage>> BulkWriteAsync(IClientSessionHandle session, IEnumerable<WriteModel<AnimatedImage>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public long Count(FilterDefinition<AnimatedImage> filter, CountOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public long Count(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, CountOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<long> CountAsync(FilterDefinition<AnimatedImage> filter, CountOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<long> CountAsync(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, CountOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public long CountDocuments(FilterDefinition<AnimatedImage> filter, CountOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public long CountDocuments(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, CountOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<long> CountDocumentsAsync(FilterDefinition<AnimatedImage> filter, CountOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<long> CountDocumentsAsync(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, CountOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public DeleteResult DeleteMany(FilterDefinition<AnimatedImage> filter, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public DeleteResult DeleteMany(FilterDefinition<AnimatedImage> filter, DeleteOptions options, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public DeleteResult DeleteMany(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<DeleteResult> DeleteManyAsync(FilterDefinition<AnimatedImage> filter, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<DeleteResult> DeleteManyAsync(FilterDefinition<AnimatedImage> filter, DeleteOptions options, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<DeleteResult> DeleteManyAsync(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public DeleteResult DeleteOne(FilterDefinition<AnimatedImage> filter, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public DeleteResult DeleteOne(FilterDefinition<AnimatedImage> filter, DeleteOptions options, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public DeleteResult DeleteOne(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<DeleteResult> DeleteOneAsync(FilterDefinition<AnimatedImage> filter, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<DeleteResult> DeleteOneAsync(FilterDefinition<AnimatedImage> filter, DeleteOptions options, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<DeleteResult> DeleteOneAsync(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IAsyncCursor<TField> Distinct<TField>(FieldDefinition<AnimatedImage, TField> field, FilterDefinition<AnimatedImage> filter, DistinctOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IAsyncCursor<TField> Distinct<TField>(IClientSessionHandle session, FieldDefinition<AnimatedImage, TField> field, FilterDefinition<AnimatedImage> filter, DistinctOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IAsyncCursor<TField>> DistinctAsync<TField>(FieldDefinition<AnimatedImage, TField> field, FilterDefinition<AnimatedImage> filter, DistinctOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IAsyncCursor<TField>> DistinctAsync<TField>(IClientSessionHandle session, FieldDefinition<AnimatedImage, TField> field, FilterDefinition<AnimatedImage> filter, DistinctOptions options = null, CancellationToken cancellationToken = default)
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

		public Task<IAsyncCursor<TProjection>> FindAsync<TProjection>(FilterDefinition<AnimatedImage> filter, FindOptions<AnimatedImage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IAsyncCursor<TProjection>> FindAsync<TProjection>(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, FindOptions<AnimatedImage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public TProjection FindOneAndDelete<TProjection>(FilterDefinition<AnimatedImage> filter, FindOneAndDeleteOptions<AnimatedImage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public TProjection FindOneAndDelete<TProjection>(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, FindOneAndDeleteOptions<AnimatedImage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<TProjection> FindOneAndDeleteAsync<TProjection>(FilterDefinition<AnimatedImage> filter, FindOneAndDeleteOptions<AnimatedImage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<TProjection> FindOneAndDeleteAsync<TProjection>(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, FindOneAndDeleteOptions<AnimatedImage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public TProjection FindOneAndReplace<TProjection>(FilterDefinition<AnimatedImage> filter, AnimatedImage replacement, FindOneAndReplaceOptions<AnimatedImage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public TProjection FindOneAndReplace<TProjection>(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, AnimatedImage replacement, FindOneAndReplaceOptions<AnimatedImage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<TProjection> FindOneAndReplaceAsync<TProjection>(FilterDefinition<AnimatedImage> filter, AnimatedImage replacement, FindOneAndReplaceOptions<AnimatedImage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<TProjection> FindOneAndReplaceAsync<TProjection>(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, AnimatedImage replacement, FindOneAndReplaceOptions<AnimatedImage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public TProjection FindOneAndUpdate<TProjection>(FilterDefinition<AnimatedImage> filter, UpdateDefinition<AnimatedImage> update, FindOneAndUpdateOptions<AnimatedImage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public TProjection FindOneAndUpdate<TProjection>(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, UpdateDefinition<AnimatedImage> update, FindOneAndUpdateOptions<AnimatedImage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<TProjection> FindOneAndUpdateAsync<TProjection>(FilterDefinition<AnimatedImage> filter, UpdateDefinition<AnimatedImage> update, FindOneAndUpdateOptions<AnimatedImage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<TProjection> FindOneAndUpdateAsync<TProjection>(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, UpdateDefinition<AnimatedImage> update, FindOneAndUpdateOptions<AnimatedImage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IAsyncCursor<TProjection> FindSync<TProjection>(FilterDefinition<AnimatedImage> filter, FindOptions<AnimatedImage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IAsyncCursor<TProjection> FindSync<TProjection>(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, FindOptions<AnimatedImage, TProjection> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public void InsertMany(IEnumerable<AnimatedImage> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public void InsertMany(IClientSessionHandle session, IEnumerable<AnimatedImage> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task InsertManyAsync(IEnumerable<AnimatedImage> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task InsertManyAsync(IClientSessionHandle session, IEnumerable<AnimatedImage> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public void InsertOne(AnimatedImage document, InsertOneOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public void InsertOne(IClientSessionHandle session, AnimatedImage document, InsertOneOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task InsertOneAsync(AnimatedImage document, CancellationToken _cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task InsertOneAsync(AnimatedImage document, InsertOneOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task InsertOneAsync(IClientSessionHandle session, AnimatedImage document, InsertOneOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IAsyncCursor<TResult> MapReduce<TResult>(BsonJavaScript map, BsonJavaScript reduce, MapReduceOptions<AnimatedImage, TResult> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IAsyncCursor<TResult> MapReduce<TResult>(IClientSessionHandle session, BsonJavaScript map, BsonJavaScript reduce, MapReduceOptions<AnimatedImage, TResult> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IAsyncCursor<TResult>> MapReduceAsync<TResult>(BsonJavaScript map, BsonJavaScript reduce, MapReduceOptions<AnimatedImage, TResult> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IAsyncCursor<TResult>> MapReduceAsync<TResult>(IClientSessionHandle session, BsonJavaScript map, BsonJavaScript reduce, MapReduceOptions<AnimatedImage, TResult> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IFilteredMongoCollection<TDerivedDocument> OfType<TDerivedDocument>() where TDerivedDocument : AnimatedImage
		{
			throw new NotImplementedException();
		}

		public ReplaceOneResult ReplaceOne(FilterDefinition<AnimatedImage> filter, AnimatedImage replacement, ReplaceOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public ReplaceOneResult ReplaceOne(FilterDefinition<AnimatedImage> filter, AnimatedImage replacement, UpdateOptions options, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public ReplaceOneResult ReplaceOne(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, AnimatedImage replacement, ReplaceOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public ReplaceOneResult ReplaceOne(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, AnimatedImage replacement, UpdateOptions options, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<ReplaceOneResult> ReplaceOneAsync(FilterDefinition<AnimatedImage> filter, AnimatedImage replacement, ReplaceOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<ReplaceOneResult> ReplaceOneAsync(FilterDefinition<AnimatedImage> filter, AnimatedImage replacement, UpdateOptions options, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<ReplaceOneResult> ReplaceOneAsync(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, AnimatedImage replacement, ReplaceOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<ReplaceOneResult> ReplaceOneAsync(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, AnimatedImage replacement, UpdateOptions options, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public UpdateResult UpdateMany(FilterDefinition<AnimatedImage> filter, UpdateDefinition<AnimatedImage> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public UpdateResult UpdateMany(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, UpdateDefinition<AnimatedImage> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<UpdateResult> UpdateManyAsync(FilterDefinition<AnimatedImage> filter, UpdateDefinition<AnimatedImage> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<UpdateResult> UpdateManyAsync(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, UpdateDefinition<AnimatedImage> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public UpdateResult UpdateOne(FilterDefinition<AnimatedImage> filter, UpdateDefinition<AnimatedImage> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public UpdateResult UpdateOne(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, UpdateDefinition<AnimatedImage> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<UpdateResult> UpdateOneAsync(FilterDefinition<AnimatedImage> filter, UpdateDefinition<AnimatedImage> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<UpdateResult> UpdateOneAsync(IClientSessionHandle session, FilterDefinition<AnimatedImage> filter, UpdateDefinition<AnimatedImage> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IChangeStreamCursor<TResult> Watch<TResult>(PipelineDefinition<ChangeStreamDocument<AnimatedImage>, TResult> pipeline, ChangeStreamOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IChangeStreamCursor<TResult> Watch<TResult>(IClientSessionHandle session, PipelineDefinition<ChangeStreamDocument<AnimatedImage>, TResult> pipeline, ChangeStreamOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IChangeStreamCursor<TResult>> WatchAsync<TResult>(PipelineDefinition<ChangeStreamDocument<AnimatedImage>, TResult> pipeline, ChangeStreamOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IChangeStreamCursor<TResult>> WatchAsync<TResult>(IClientSessionHandle session, PipelineDefinition<ChangeStreamDocument<AnimatedImage>, TResult> pipeline, ChangeStreamOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IMongoCollection<AnimatedImage> WithReadConcern(ReadConcern readConcern)
		{
			throw new NotImplementedException();
		}

		public IMongoCollection<AnimatedImage> WithReadPreference(ReadPreference readPreference)
		{
			throw new NotImplementedException();
		}

		public IMongoCollection<AnimatedImage> WithWriteConcern(WriteConcern writeConcern)
		{
			throw new NotImplementedException();
		}
	}
}
