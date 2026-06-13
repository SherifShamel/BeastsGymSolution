using BeastsGym.BLL.Interfaces;
using BeastsGym.BLL.ViewModels.MemberViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BeastsGym.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberServices iMemberServices;

        public MemberController(IMemberServices iMemberServices)
        {
            this.iMemberServices = iMemberServices;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var tempResult = TempData["Result"];

            var Members = await iMemberServices.GetAllMembersAsync(ct);
            return View(Members);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), model);

            var Result = await iMemberServices.CreateMemberAsync(model, ct);

            if (Result)
                TempData["Success"] = "Member created successfully.";
            else 
                TempData["Fail"] = "Failed to create member.";

            
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            var member = await iMemberServices.GetMemberDetailsAsync(id, ct);
            if(member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        [HttpGet]
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var healthRecord = await iMemberServices.GetMemberHealthRecordAsync(id, ct);
            if (healthRecord is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(healthRecord);
        }
    }
}

