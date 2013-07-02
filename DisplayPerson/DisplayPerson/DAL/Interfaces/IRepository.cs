using System;
using System.Linq;
using System.Linq.Expressions;

namespace DisplayPerson.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T pEntity);
        void Update(T pEntity);
        void Delete(Expression<Func<T, bool>> pWhere);
        IQueryable<T> List();
        IQueryable<T> List(Expression<Func<T, bool>> pWhere);
        T Get(Expression<Func<T, bool>> pWhere);
        void SaveChanges();
    }
}
