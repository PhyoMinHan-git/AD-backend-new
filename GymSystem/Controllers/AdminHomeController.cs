using GymSystem.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class AdminHomeController : Controller
    {
        private readonly ExerciseHistoryRepository historyRepo;
        public AdminHomeController(ExerciseHistoryRepository historyRepo)
        {
            this.historyRepo = historyRepo;
        }
        [HttpGet]
        public IActionResult ShowDashboard()
        {
            DashboardData data = new DashboardData();
            data.totExerciseNum = historyRepo.GetNumOfExercise();
            data.weekExerciseNum=historyRepo.GetNumOfExerciseinOneWeek();
            data.MostPopularType=historyRepo.GetMostPopularType();
            return View(data);
        }
    }
    public class DashboardData
    {
        public int weekExerciseNum { get; set; }
        public int totExerciseNum {  get; set; }
        public string MostPopularType {  get; set; }
    }
}
