using BeastsGym.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeastsGym.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IgenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct);
        Task<Session> GetSingleSessionWithTrainerAndCategoryAsync(int sessionId, CancellationToken ct);

        Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct);
    }
}
