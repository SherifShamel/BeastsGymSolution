using BeastsGym.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeastsGym.DAL.Repositories.Interfaces
{
    public interface IPlanRepository
    {
        Task<IEnumerable<Plan>> GetAllPlans();

        Task<Plan?> GetPlanById(int id);

        void Add(Plan Plan);
        void Update(Plan Plan);
        void Delete(int id);

        Task<int> CompleteAsync();
    }
}
