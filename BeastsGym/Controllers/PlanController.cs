using BeastsGym.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeastsGym.Controllers
{
    public class PlanController : Controller
    {
        private readonly BeastsDbContext dbContext = new BeastsDbContext(); 
        public async Task<IActionResult> Index()
        {
            var plans = await dbContext.Plans.ToListAsync();
            return View(plans);
        }

        public async Task<IActionResult> Details(int id)
        {
            var plan = await dbContext.Plans.FirstOrDefaultAsync(p=>p.PlanId == id);
            if (plan == null)
            {
                RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
    }
}
