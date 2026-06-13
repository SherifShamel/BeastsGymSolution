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
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly BeastsGymDbContext dbContext;

        public SessionRepository(BeastsGymDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct)
        {
            var Sessions = dbContext.Sessions.AsNoTracking().Include(s => s.Trainer)
                .Include(s => s.Category);

            return await Sessions.ToListAsync(ct);
        }

        public async Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct)
        {
            return dbContext.Bookings.AsNoTracking().Count(b => b.SessionId == sessionId);
        }

        public async Task<Session> GetSingleSessionWithTrainerAndCategoryAsync(int sessionId, CancellationToken ct)
        {
            var Session = dbContext.Sessions.Include(s => s.Trainer)
                .Include(s => s.Category).FirstOrDefaultAsync(s => s.Id == sessionId);

            return await Session;
        }
    }
}
