using GymSystem.Models;
using GymSystem.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class DateController : Controller
    {
        private readonly AvailableTimeRepository timeRepo;
        private readonly CoachRepository coachRepo;
        public DateController(AvailableTimeRepository timeRepo, CoachRepository coachRepo)
        {
            this.timeRepo = timeRepo;
            this.coachRepo = coachRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Add(AddDateRequest request)
        {
            Coach coach = coachRepo.FindCoachByName(request.username);
    
            if (coach == null)
            {
                TempData["ErrorMessage"] = "Coach name not found";
                return RedirectToAction("Index", "Date");
            }
    
            foreach (string date in request.dates)
            {
                coach.dates.Add(date);
                coachRepo.ModifyDate(coach.id, string.Join(",", coach.dates));
                List<string> time = new List<string>();
                for (int i = 0; i < 24; i++)
                {
                    if (i >= request.startTime && i < request.endTime) time.Add("YES");
                    else time.Add("No");
                }
                timeRepo.CreateAvailableTime(coach.id, date, string.Join(",", time));
            }
    
            return RedirectToAction("ShowDashboard", "AdminHome");
        }

    }
    public class AddDateRequest
    {
        public string username {  get; set; }
        public int startTime {  get; set; }
        public int endTime { get; set; }
        public List<string> dates { get; set; }= new List<string>();
    }
}
