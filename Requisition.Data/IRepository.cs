using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Requisition.Data
{
    public interface IRepository<T>
    {
        IEnumerable<T> FindAll();
        IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        T FindSingle(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        int CountByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Create(IEnumerable<T> entity);
        void Delete(IEnumerable<T> entity);
        void Update(IEnumerable<T> entity);
        void DeleteByCondition(Expression<Func<T, bool>> expression);
        void Save();
        bool Any(Expression<Func<T, bool>> expression);

    }
}
