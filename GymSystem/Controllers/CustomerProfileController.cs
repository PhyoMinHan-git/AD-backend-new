using GymSystem.Repository;
using GymSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerProfileController : ControllerBase
    {
        private readonly CustomerRepository CusRepo;
        public CustomerProfileController(CustomerRepository CusRepo)
        {
            this.CusRepo = CusRepo;
        }
        [HttpPost("password")]
        public IActionResult ModifyPassword([FromBody] ModificationRequest request)
        {
            CusRepo.ModifyPwd(request.customerId,request.content);
            return Ok();
        }
        [HttpPost("email")]
        public IActionResult ModifyEmail([FromBody] ModificationRequest request)
        {
            CusRepo.ModifyEmail(request.customerId, request.content);
            return Ok();
        }
        [HttpPost("number")]
        public IActionResult ModifyPhoneNumber([FromBody] ModificationRequest request)
        {
            CusRepo.ModifyPhoneNumber(request.customerId, request.content);
            return Ok();
        }
    }
    public class ModificationRequest
    {
        public string content { get; set; }
        public int customerId { get; set; }
    }
}
