using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Requisition.Data
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected Context RepositoryContext { get; set; }

        public Repository(Context repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }
        public bool Any(Expression<Func<T, bool>> expression)
        {
            return this.RepositoryContext.Set<T>().Any(expression);
        }
        public IEnumerable<T> FindAll()
        {
            return this.RepositoryContext.Set<T>();
        }
        public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            var query = this.RepositoryContext.Set<T>().Where(expression);
            if (include != null)
            {
                query = include(query);
            }
            return query;
        }
        public T FindSingle(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            var query = this.RepositoryContext.Set<T>().Where(expression);
            if (include != null)
            {
                query = include(query);
            }
            return query.SingleOrDefault() ?? null;
        }

        public int CountByCondition(Expression<Func<T, bool>> expression)
        {
            return this.RepositoryContext.Set<T>().Count(expression);
        }

        public void Create(T entity)
        {
            this.RepositoryContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this.RepositoryContext.Set<T>().Update(entity);
        }

        public void Update(IEnumerable<T> entity)
        {
            this.RepositoryContext.Set<T>().UpdateRange(entity);
        }

        public void Delete(T entity)
        {
            this.RepositoryContext.Set<T>().Remove(entity);
        }
        public void DeleteByCondition(Expression<Func<T, bool>> expression)
        {
            this.RepositoryContext.Set<T>().RemoveRange(RepositoryContext.Set<T>().Where(expression));
        }
        public void Delete(IEnumerable<T> entity)
        {
            this.RepositoryContext.Set<T>().RemoveRange(entity);
        }

        public void Create(IEnumerable<T> entity)
        {
            this.RepositoryContext.Set<T>().AddRange(entity);
        }

        public void Save()
        {
            this.RepositoryContext.SaveChanges();
        }


    }
}
