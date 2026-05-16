using BeastsGym.DAL.Contexts;
using BeastsGym.DAL.Entities;
using BeastsGym.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeastsGym.DAL.Repositories.classes
{
    public class PlanRepository : IPlanRepository
    {
        private BeastsGymDbContext dbContext;
        public PlanRepository()
        {
            dbContext = new BeastsGymDbContext();
        }
        public async void Add(Plan Plan)
        {
            dbContext.Plans.Add(Plan);
        }

        public async Task<int> CompleteAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            var plan = dbContext.Plans.FirstOrDefault(p => p.PlanId == id);

            if (plan != null)
                dbContext.Plans.Remove(plan);
        }

        public async Task<IEnumerable<Plan>> GetAllPlans()
        {
            return await dbContext.Plans.ToListAsync();
        }

        public async Task<Plan?> GetPlanById(int id)
        {
            return await dbContext.Plans.FirstOrDefaultAsync(p => p.PlanId == id);
        }

        public void Update(Plan Plan)
        {
            dbContext.Plans.Update(Plan);
        }
    }
}
