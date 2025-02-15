using GymSystem.Models;
using GymSystem.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class CrisisCustomerController : Controller
    {
        private readonly CustomerRepository customerRepo;
        public CrisisCustomerController(CustomerRepository customerRepo)
        {
            this.customerRepo=customerRepo;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Customer> customers=new List<Customer>();
            customers = customerRepo.GetCrisisCustomer();
            return View(customers);
        }
    }
}
