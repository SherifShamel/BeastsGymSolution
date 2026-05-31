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
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        private readonly BeastsGymDbContext _dbContext;

        public MemberRepository(BeastsGymDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        //New Features

    }
}
