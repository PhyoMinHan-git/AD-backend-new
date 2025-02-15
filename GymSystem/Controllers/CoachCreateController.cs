using GymSystem.Repository;
using Microsoft.AspNetCore.Mvc;
using GymSystem.Models;

namespace GymSystem.Controllers
{
    public class CoachCreateController : Controller
    {
        private readonly CoachRepository coachRepo;
        public CoachCreateController(CoachRepository coachRepo)
        {
            this.coachRepo = coachRepo;
        }
        public IActionResult CoachCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SendCreateRequest(CreateCoachRequest request)
        {
            Coach coach = coachRepo.FindCoachByName(request.username);
    
            if (coach != null)
            {
                TempData["ErrorMessage"] = "Coach already exists!";
                return RedirectToAction("CoachCreate", "CoachCreate");
            }
            if(coachRepo.FindCoachByName(request.username)!=null)
            {
                ViewBag.Error = "Coach Exists";
                return View();
            }
            coachRepo.CreateCoach(request.username, request.pwd, request.email, request.phoneNumber, request.isRookie, request.description, request.experienceYear,request.specialization,request.certifications);
            return RedirectToAction("ShowDashboard", "AdminHome");
        }
        [HttpPost]
        public IActionResult DeleteCoach(string username)
        {
            Coach coach = coachRepo.FindCoachByName(username);
    
            if (coach == null)
            {
                TempData["ErrorMessage"] = "Coach name not found";
                return RedirectToAction("CoachDelete", "CoachCreate");
            }
            coachRepo.DeleteCoach(username);
            return RedirectToAction("ShowDashboard", "AdminHome");
        }
        public IActionResult CoachDelete()
        {
            return View();
        }
    }
    public class CreateCoachRequest
    {
        public string username {  get; set; }
        public string pwd { get; set; }
        public string email {  get; set; }
        public string phoneNumber {  get; set; }
        public bool isRookie {  get; set; }
        public string description {  get; set; }
        public int experienceYear {  get; set; }

        public int certifications { get; set; }
        public string specialization { get; set; }
    }
}
