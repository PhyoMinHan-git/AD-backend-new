using GymSystem.Models;
using GymSystem.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class LowRatedCoachController : Controller
    {
        private readonly CoachRepository coachRepo;
        public LowRatedCoachController(CoachRepository coachRepo)
        {
            this.coachRepo=coachRepo;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Coach> coaches=coachRepo.GetLowRatedCoaches();
            return View(coaches);
        }
    }
}
