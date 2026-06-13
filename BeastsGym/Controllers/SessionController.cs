using BeastsGym.BLL.Interfaces;
using BeastsGym.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeastsGym.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionServices services;

        public SessionController(ISessionServices services)
        {
            this.services = services;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var sessions = await services.GetAllSessionsAsync(ct);
            return View(sessions);
        }

        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropDownAsync(ct);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownAsync(ct);
                return View(model);
            }
            var Result = await services.CreateSessionAsync(model, ct);
            if (Result.Success)
            {
                TempData["SuccessMessage"] = "Session created successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = Result.ErrorMessage;
            await PopulateDropDownAsync(ct);
            return RedirectToAction(nameof(Create));
        }



        private async Task PopulateDropDownAsync(CancellationToken ct)
        {
            ViewBag.Trainers = new SelectList(await services.GetTrainersForDropDownAsync(ct), "Id", "Name");
            ViewBag.Categories = new SelectList(await services.GetCategoriesForDropDownAsync(ct), "Id", "CategoryName");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var Session = await services.GetSessionByIdAsync(id, ct);
            if (Session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(Session);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var Session = await services.GetSessionToUpdateAsync(id, ct);
            if (Session is null)
            {
                TempData["ErrorMessage"] = "Session not found or can not be edited.";
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropDownAsync(ct);
            return View(Session);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownAsync(ct);
                return View(model);
            }

            var Result = await services.UpdateSessionAsync(id, model, ct);
            if (Result.Success)
            {
                TempData["SuccessMessage"] = "Session updated successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = Result.ErrorMessage;
            await PopulateDropDownAsync(ct);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var Session = await services.GetSessionByIdAsync(id, ct);

            if (Session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(Session);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var Result = await services.RemoveSessionAsync(id, ct);

            TempData[Result.Success ? "SuccessMessage" : "ErrorMessage"] = Result.Success ? "Session Deleted" : Result.ErrorMessage;

            return RedirectToAction(nameof(Index));
        }
    }
}
