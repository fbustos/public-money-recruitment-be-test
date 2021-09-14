using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VacationRental.Domain;

namespace VacationRental.Infrastructure
{
    public abstract class RepositoryBase<T> where T : BaseEntity
    {
        private readonly IDictionary<int, T> _collection;

        protected RepositoryBase(IDictionary<int, T> collection)
        {
            _collection = collection;
        }

        public T Add(T entity)
        {
            entity.Id = _collection.Count + 1;
            _collection.Add(_collection.Count + 1, entity);
            return entity;
        }

        public T Update(T entity)
        {
            _collection[entity.Id] = entity;
            return entity;
        }

        public bool Delete(T entity)
        {
            return _collection.Remove(entity.Id);
        }

        public T Get(int id)
        {
            return _collection[id];
        }

        public List<T> List(Expression<Func<T, bool>> expression)
        {
            return _collection.Values.Where(expression.Compile()).ToList();
        }
    }
}