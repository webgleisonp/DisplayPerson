using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DisplayPerson.DAL.Interfaces;

namespace DisplayPerson.DAL
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private ContextDAL context;

        public Repository(ContextDAL pEntityContext)
        {
            this.context = pEntityContext;
        }

        protected DbSet<T> DbSet { get { return this.context.Set<T>(); } }

        public void Add(T pEntity)
        {
            DbSet.Add(pEntity);
        }

        public void Update(T pEntity)
        {
            var entry = context.Entry(pEntity);
            DbSet.Attach(pEntity);
            entry.State = EntityState.Modified;
        }

        public void Delete(Expression<Func<T, bool>> pWhere)
        {
            var entities = List(pWhere);

            foreach (var entity in entities)
            {
                DbSet.Remove(entity);
            }
        }

        public IQueryable<T> List()
        {
            return DbSet.AsQueryable<T>();
        }

        public IQueryable<T> List(Expression<Func<T, bool>> pWhere)
        {
            return DbSet.Where(pWhere).AsQueryable<T>();
        }

        public T Get(Expression<Func<T, bool>> pWhere)
        {
            return DbSet.Where(pWhere).SingleOrDefault<T>();
        }

        public void SaveChanges() {
            this.context.SaveChanges();
        }
    }
}