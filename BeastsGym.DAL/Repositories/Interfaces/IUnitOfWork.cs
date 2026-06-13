using BeastsGym.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeastsGym.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        public IgenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();

        public ISessionRepository SessionRepository { get; }
        public Task<int> CompleteAsync();
    }
}
