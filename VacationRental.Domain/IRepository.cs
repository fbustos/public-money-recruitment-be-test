using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VacationRental.Domain
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Add(T entity);

        T Update(T entity);

        bool Delete(T entity);

        T Get(int id);

        List<T> List(Expression<Func<T, bool>> expression);
    }
}