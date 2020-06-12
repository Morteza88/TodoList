using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApp.Data;
using TodoListApp.Models;

namespace TodoListApp.Repositoris
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly TodoListDBContext context;
        protected DbSet<T> entities;
        public Repository(TodoListDBContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await entities.ToListAsync();
        }
        public async Task<T> GetByIdAsync(Guid id)
        {
            return await entities.SingleOrDefaultAsync(s => s.Id == id);
        }
        public async Task<int> InsertAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            await entities.AddAsync(entity);
            return await context.SaveChangesAsync();
        }
        public async Task<int> UpdateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            entities.Update(entity);
            //context.Entry(entity).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }
        public async Task<int> DeleteAsync(Guid id)
        {
            if (id == null) throw new ArgumentNullException("entity");
            T entity = entities.SingleOrDefault(s => s.Id == id);
            entities.Remove(entity);
            return await context.SaveChangesAsync();
        }
    }
}
