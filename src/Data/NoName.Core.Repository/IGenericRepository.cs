using NoName.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NoName.Core.Repository
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        Task<IList<T>> FilterByAsync(Expression<Func<T, bool>> predicate = null);

        void Add(T entity);

        Task AddAndSaveChangesAsync(T entity);

        void Update(T entity);

        Task UpdateAndSaveChangesAsync(T entity);

        Task SaveChangesAsync();
    }
}