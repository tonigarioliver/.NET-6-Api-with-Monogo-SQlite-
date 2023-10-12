using LearnApi.Entity;
using System.Linq.Expressions;

namespace LearnApi.COR.IRespository;
public interface IGenericRepositoryMongo<T> where T : MongoEntity
{
    IQueryable<T> AsQueryable();

    IEnumerable<T> FilterBy(
        Expression<Func<T, bool>> filterExpression);

    IEnumerable<TProjected> FilterBy<TProjected>(
        Expression<Func<T, bool>> filterExpression,
        Expression<Func<T, TProjected>> projectionExpression);

    T FindOne(Expression<Func<T, bool>> filterExpression);

    Task<T> FindOneAsync(Expression<Func<T, bool>> filterExpression);

    T FindById(string id);

    Task<T> FindByIdAsync(string id);

    void InsertOne(T entity);

    Task InsertOneAsync(T entity);

    void InsertMany(ICollection<T> entities);

    Task InsertManyAsync(ICollection<T> entities);

    void ReplaceOne(T entity);

    Task ReplaceOneAsync(T entity);

    void DeleteOne(Expression<Func<T, bool>> filterExpression);

    Task DeleteOneAsync(Expression<Func<T, bool>> filterExpression);

    void DeleteById(string id);

    Task DeleteByIdAsync(string id);

    void DeleteMany(Expression<Func<T, bool>> filterExpression);

    Task DeleteManyAsync(Expression<Func<T, bool>> filterExpression);

}