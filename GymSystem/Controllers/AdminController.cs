using GymSystem.Models;
using GymSystem.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly AdminRepository adminRepo;
        private readonly CustomerRepository customerRepo;
        public AdminController(AdminRepository adminRepo,CustomerRepository customerRepo)
        {
            this.adminRepo = adminRepo;
            this.customerRepo = customerRepo;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ValidateLogin(AdminLoginRequest request)
        {
            Admin admin = adminRepo.FindAdminByName(request.username);
            if (admin.pwd == request.password) return RedirectToAction("ShowDashboard","AdminHome");
            ViewBag.Error = "Wrong username or password!";
            return View();
        }
        
    }
    
    public class AdminLoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
