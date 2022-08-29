using System.Linq.Expressions;

namespace Microsoft.DataTransfer.MongoExtension;

public interface IRepository<TDocument>
{
    ValueTask Add(TDocument item);
    ValueTask AddRange(IEnumerable<TDocument> items);
    ValueTask AddRange(params TDocument[] items);
    ValueTask Update(Expression<Func<TDocument, bool>> filter, TDocument value);
    ValueTask Remove(Expression<Func<TDocument, bool>> filter);
    ValueTask RemoveRange(Expression<Func<TDocument, bool>> filter);
    IQueryable<TDocument> AsQueryable();
}
