using BeastsGym.DAL.Contexts;
using BeastsGym.DAL.Entities;
using BeastsGym.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeastsGym.DAL.Repositories.classes
{
    public class UnitOfWork : IUnitOfWork
    {
        public BeastsGymDbContext DbContext { get; }

        public ISessionRepository SessionRepository { get; }

        public Dictionary<string, object> repository = [];

        public UnitOfWork( BeastsGymDbContext _dbContext)
        {
            DbContext = _dbContext;
            SessionRepository = new SessionRepository(_dbContext);
        }


        
        public IgenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var TypeName = typeof(TEntity).Name;

            if(repository.TryGetValue(TypeName, out object oldRepo))
            {
                return (IgenericRepository<TEntity>)oldRepo;
            }

            var newRepo = new GenericRepository<TEntity>(DbContext);
            
            repository[TypeName] = newRepo;

            return newRepo;

        }


        public async Task<int> CompleteAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

    }
}
