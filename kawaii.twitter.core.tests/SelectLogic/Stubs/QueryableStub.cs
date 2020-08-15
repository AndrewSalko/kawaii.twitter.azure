using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using kawaii.twitter.db;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace kawaii.twitter.core.tests.SelectLogic.Stubs
{
	/// <summary>
	/// Этот стаб годится только для минимальных проверок на null, работать он не может
	/// </summary>
	/// <typeparam name="T"></typeparam>
	class QueryableStub<T>: IMongoQueryable<T>
	{
		public List<T> ResultData
		{
			get;
			set;
		}

		public Type ElementType => ResultData.AsQueryable().ElementType;

		public Expression Expression => ResultData.AsQueryable().Expression;

		public IQueryProvider Provider => ResultData.AsQueryable().Provider;

		public IEnumerator<T> GetEnumerator() => ResultData.AsQueryable().GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => ResultData.AsQueryable().GetEnumerator();

		public QueryableExecutionModel GetExecutionModel() => throw new NotImplementedException();

		public IAsyncCursor<T> ToCursor(CancellationToken cancellationToken = default) => throw new NotImplementedException();

		public Task<IAsyncCursor<T>> ToCursorAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();

	}
}
