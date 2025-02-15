using GymSystem.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoachProfileController : ControllerBase
    {
        private readonly CoachRepository CoaRepo;
        private readonly ILogger<LoginController> _logger;
        public CoachProfileController(CoachRepository CoaRepo, ILogger<LoginController> logger)
        {
            this.CoaRepo = CoaRepo;
            _logger = logger;
        }
        [HttpPost("password")]
        public IActionResult ModifyPassword([FromBody] CoachModificationRequest request)
        {
            _logger.LogInformation($"Received booking list request for customer ID: {request.coachId}");
            _logger.LogInformation($"Received booking list request for content: {request.content}");
            CoaRepo.ModifyPwd(request.coachId, request.content);
            return Ok();
        }
        [HttpPost("email")]
        public IActionResult ModifyEmail([FromBody] CoachModificationRequest request)
        {
            CoaRepo.ModifyEmail(request.coachId, request.content);
            return Ok();
        }
        [HttpPost("number")]
        public IActionResult ModifyPhoneNumber([FromBody] CoachModificationRequest request)
        {
            CoaRepo.ModifyPhoneNumber(request.coachId, request.content);
            return Ok();
        }
        [HttpPost("description")]
        public IActionResult ModifyDescription([FromBody] CoachModificationRequest request)
        {
            CoaRepo.ModifyDescription(request.coachId, request.content);
            return Ok();
        }
    }
    public class CoachModificationRequest
    {
        public string content { get; set; }
        public int coachId { get; set; }
    }
}
