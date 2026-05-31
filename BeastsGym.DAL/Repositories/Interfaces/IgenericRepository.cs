using BeastsGym.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BeastsGym.DAL.Repositories.Interfaces
{
    public interface IgenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        Task<IEnumerable<TEntity>> GetAll(bool isTracked, CancellationToken token = default);

        Task<TEntity?> GetById(int id, CancellationToken token = default);

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);

        Task<int> CompleteAsync();


        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool isTracked = false, CancellationToken ct = default);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, bool isTracked = false, CancellationToken ct = default);

    }
}
