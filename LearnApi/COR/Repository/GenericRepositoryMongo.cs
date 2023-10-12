using LearnApi.COR.IRespository;
using LearnApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using LearnApi.Helper;
using LearnApi.Entity;
using MongoDB.Driver;
using MongoDB.Bson;

namespace LearnApi.COR.Repository
{
    public class GenericRepositoryMongo<T> : IGenericRepositoryMongo<T> where T : MongoEntity
    {
        private readonly IMongoCollection<T> _collection;
        private readonly ILogger<GenericRepositoryMongo<T>> _logger;

        public GenericRepositoryMongo(MongoDBConnection mongoDBConnection, ILogger<GenericRepositoryMongo<T>> logger)
        {
            _logger = logger;
            _collection = mongoDBConnection.context.GetCollection<T>(GetCollectionName(typeof(T)));
        }

        private protected string GetCollectionName(Type documentType)
        {
            return documentType.Name;
        }
        public  IQueryable<T> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public  IEnumerable<T> FilterBy(
            Expression<Func<T, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual T FindOne(Expression<Func<T, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public virtual Task<T> FindOneAsync(Expression<Func<T, bool>> filterExpression)
        {
            return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual T FindById(string id)
        {
            var filter = Builders<T>.Filter.Eq(entity => entity.Id, id);
            return _collection.Find(filter).SingleOrDefault();
        }

        public virtual Task<T> FindByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<T>.Filter.Eq(entity => entity.Id, id);
                return _collection.Find(filter).SingleOrDefaultAsync();
            });
        }


        public virtual void InsertOne(T entity)
        {
            _collection.InsertOne(entity);
        }

        public virtual Task InsertOneAsync(T entity)
        {
            return Task.Run(() => _collection.InsertOneAsync(entity));
        }

        public void InsertMany(ICollection<T> entities)
        {
            _collection.InsertMany(entities);
        }


        public virtual async Task InsertManyAsync(ICollection<T> entities)
        {
            await _collection.InsertManyAsync(entities);
        }

        public void ReplaceOne(T document)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace(filter, document);
        }

        public virtual async Task ReplaceOneAsync(T document)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public void DeleteOne(Expression<Func<T, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        public Task DeleteOneAsync(Expression<Func<T, bool>> filterExpression)
        {
            return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
        }

        public void DeleteById(string id)
        {
            var filter = Builders<T>.Filter.Eq(entity => entity.Id, id);
            _collection.FindOneAndDelete(filter);
        }

        public Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<T>.Filter.Eq(entity => entity.Id, id);
                _collection.FindOneAndDeleteAsync(filter);
            });
        }

        public void DeleteMany(Expression<Func<T, bool>> filterExpression)
        {
            _collection.DeleteMany(filterExpression);
        }

        public Task DeleteManyAsync(Expression<Func<T, bool>> filterExpression)
        {
            return Task.Run(() => _collection.DeleteManyAsync(filterExpression));
        }

    }
}