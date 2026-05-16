using BeastsGym.DAL.Contexts;
using BeastsGym.DAL.Repositories.classes;
using BeastsGym.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeastsGym.Controllers
{
    public class PlanController : Controller
    {

        private readonly IPlanRepository planRepository;

        public PlanController()
        {
            planRepository = new PlanRepository();
        }

        public async Task<IActionResult> Index()
        {
            var plans = await planRepository.GetAllPlans();
            return View(plans);
        }

        public async Task<IActionResult> Details(int id)
        {
            var plan = await planRepository.GetPlanById(id);
            if (plan == null)
            {
                RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
    }
}
