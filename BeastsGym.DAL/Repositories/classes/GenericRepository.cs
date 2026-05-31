using BeastsGym.DAL.Contexts;
using BeastsGym.DAL.Entities;
using BeastsGym.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BeastsGym.DAL.Repositories.classes
{
    public class GenericRepository<TEntity> : IgenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly BeastsGymDbContext _dbContext;

        public GenericRepository(BeastsGymDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public void Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);

        }

        public async Task<int> CompleteAsync()
        {
           return await _dbContext.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            var item = _dbContext.Set<TEntity>().FirstOrDefault(p => p.Id == id);

            if (item != null)
                _dbContext.Set<TEntity>().Remove(item);

        }

        public async Task<IEnumerable<TEntity>> GetAll(bool isTracked, CancellationToken token = default)
        {
            var entities = isTracked ? _dbContext.Set<TEntity>() : _dbContext.Set<TEntity>().AsNoTracking();
            return await entities.ToListAsync();
        }

        public async Task<TEntity?> GetById(int id, CancellationToken token = default)
        {
            var Item = _dbContext.Set<TEntity>().FirstOrDefaultAsync(p => p.Id == id);
            return await Item;
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool isTracked = false, CancellationToken ct = default)
        {
            var items = isTracked ? _dbContext.Set<TEntity>() : _dbContext.Set<TEntity>().AsNoTracking();

            return await items.FirstOrDefaultAsync(predicate, ct);
        }

        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, bool isTracked = false, CancellationToken ct = default)
        {
            return await _dbContext.Set<TEntity>().AnyAsync(predicate, ct);
        }
    }
}
