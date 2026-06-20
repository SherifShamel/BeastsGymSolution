using BeastsGym.BLL.ViewModels.AnalyticsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeastsGym.BLL.Interfaces
{
    public interface IAnalyticsServices
    {
        public Task<AnalyticsViewModel> GetAnalyticsDataAsync(CancellationToken ct);
    }
}
