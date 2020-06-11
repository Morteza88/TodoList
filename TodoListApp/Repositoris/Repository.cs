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
        private DbSet<T> entities;
        string errorMessage = string.Empty;
        public Repository(TodoListDBContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await entities.ToListAsync();
        }
        public async Task<T> GetById(Guid id)
        {
            return await entities.SingleOrDefaultAsync(s => s.Id == id);
        }
        public async Task<int> Insert(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            await entities.AddAsync(entity);
            return await context.SaveChangesAsync();
        }
        public async Task<int> Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            entities.Update(entity);
            //context.Entry(entity).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }
        public async Task<int> Delete(Guid id)
        {
            if (id == null) throw new ArgumentNullException("entity");
            T entity = entities.SingleOrDefault(s => s.Id == id);
            entities.Remove(entity);
            return await context.SaveChangesAsync();
        }
    }
}
