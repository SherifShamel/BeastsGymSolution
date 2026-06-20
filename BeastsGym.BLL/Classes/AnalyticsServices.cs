using BeastsGym.BLL.Interfaces;
using BeastsGym.BLL.ViewModels.AnalyticsViewModels;
using BeastsGym.DAL.Entities;
using BeastsGym.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeastsGym.BLL.Classes
{
    public class AnalyticsServices : IAnalyticsServices
    {
        private readonly IUnitOfWork unitOfWork;

        public AnalyticsServices(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<AnalyticsViewModel> GetAnalyticsDataAsync(CancellationToken ct)
        {
            var Sessions = await unitOfWork.GetRepository<Session>().GetAll(false, ct);

            var TotalMembers = await unitOfWork.GetRepository<Member>().CountAsync(ct: ct);
            var TotalTrainers = await unitOfWork.GetRepository<Trainer>().CountAsync(ct: ct);
            var ActiveMembers = await unitOfWork.GetRepository<Membership>().CountAsync(m => m.EndDate > DateTime.Now, ct);

            return new AnalyticsViewModel
            {
                TotalMembers = TotalMembers,
                ActiveMembers = ActiveMembers,
                TotalTrainers = TotalTrainers,
                UpcomingSessions = Sessions.Count(s => s.StartDate > DateTime.Now),
                OngoingSessions = Sessions.Count(s => s.StartDate <= DateTime.Now && s.EndDate > DateTime.Now),
                CompletedSessions = Sessions.Count(s => s.EndDate < DateTime.Now)
            };
        }
    }
}
