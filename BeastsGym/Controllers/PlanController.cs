using BeastsGym.DAL.Contexts;
using BeastsGym.DAL.Entities;
using BeastsGym.DAL.Repositories.classes;
using BeastsGym.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeastsGym.Controllers
{
    public class PlanController : Controller
    {

        private readonly IgenericRepository<Plan> planRepository;

        public PlanController(IgenericRepository<Plan> _planRepository)
        {
            planRepository = _planRepository;
        }

        public async Task<IActionResult> Index(CancellationToken token)
        {
            var plans = await planRepository.GetAll(false, token);
            return View(plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken token)
        {
            var plan = await planRepository.GetById(id, token);
            if (plan == null)
            {
                RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
    }
}
