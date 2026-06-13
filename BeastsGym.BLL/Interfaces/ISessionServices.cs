using BeastsGym.BLL.Common;
using BeastsGym.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeastsGym.BLL.Interfaces
{
    public interface ISessionServices
    {
        public Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct);

        public Task<SessionViewModel?> GetSessionByIdAsync(int sessionId, CancellationToken ct = default);
        public Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);

        public Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default);
        public Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct = default);

        public Task<UpdateSessionViewModel> GetSessionToUpdateAsync(int sessionId, CancellationToken ct);

        Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct);

        
        Task<Result> RemoveSessionAsync(int sessionId, CancellationToken ct);
    }
}
