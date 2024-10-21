using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Context.Repositories.Generic
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbContext context;

        public GenericRepository(DbContext context)
        {
            this.context = context;
        }
        public async Task<TEntity> Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public TEntity AddSync(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            context.SaveChanges();
            return entity;
        }

        public async Task<int> AddRangeAsync(ICollection<TEntity> t)
        {
            context.Set<TEntity>().AddRange(t);
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpdateRangeAsync(ICollection<TEntity> fields)
        {
            context.Set<TEntity>().UpdateRange(fields);
            return await context.SaveChangesAsync();
        }

        public async Task<TEntity> Delete(int id)
        {
            var entity = await context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();

            return entity;
        }

        public async Task<int> DeleteAsync(TEntity t)
        {
            context.Set<TEntity>().Remove(t);
            return await context.SaveChangesAsync();
        }

        public async Task<TEntity?> GetAsync(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }
        public TEntity? Get(int id)
        {
            return context.Set<TEntity>().Find(id);
        }
        public async Task<bool> Exist(Expression<Func<TEntity, bool>> expression)
        {
            return await context.Set<TEntity>().AnyAsync(expression);
        }

        public IQueryable<TEntity> GetAllQuerable()
        {
            return context.Set<TEntity>().AsNoTracking().AsQueryable();
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
        }

        public TEntity UpdateSync(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
            return entity;
        }

        public IQueryable<TEntity> DeleteAll()
        {
            var entities = context.Set<TEntity>().AsNoTracking().AsQueryable();
            context.Set<TEntity>().RemoveRange(entities);
            context.SaveChanges();

            return entities;
        }
    }
}