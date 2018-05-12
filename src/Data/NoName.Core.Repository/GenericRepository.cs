using Microsoft.EntityFrameworkCore;
using NoName.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NoName.Core.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected DbContext _context;

        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Entry(entity).State = EntityState.Added;
        }

        public async Task AddAndSaveChangesAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Added;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            var dbSet = _context.Set<T>().AsNoTracking();
            return await dbSet.AnyAsync(predicate);
        }

        public async Task<IList<T>> FilterByAsync(Expression<Func<T, bool>> predicate = null)
        {
            var dbSet = _context.Set<T>().AsNoTracking();
            return await dbSet.Where(predicate).ToListAsync();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null)
        {
            var dbSet = _context.Set<T>().AsNoTracking();
            return await dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task UpdateAndSaveChangesAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}