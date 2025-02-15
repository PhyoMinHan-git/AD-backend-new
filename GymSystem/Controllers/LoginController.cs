using GymSystem.Models;
using GymSystem.Repository;
using GymSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginService loginService;
        private readonly CustomerRepository repo;
        private readonly ILogger<LoginController> _logger;

        public LoginController(LoginService loginService, ILogger<LoginController> logger, CustomerRepository repo)
        {
            this.loginService = loginService;
            this.repo = repo;
            _logger = logger;
        }
        [HttpPost("customer")]
        public IActionResult CustomerLogin([FromBody] LoginRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("Empty Request!");
                return BadRequest("Request cannot be empty");
            }

            _logger.LogInformation($"Request recieved: Username - {request.username}, Password - {request.password}");

            if (string.IsNullOrEmpty(request.username) || string.IsNullOrEmpty(request.password))
            {
                _logger.LogWarning("Empty Username or password");
                return BadRequest("Empty Username or password");
            }

            Customer customer = loginService.getCustomerByUsername(request.username);
            if (customer == null)
            {
                _logger.LogWarning("User not found");
                return Unauthorized("Invalidate username or password");
            }

            bool validation = loginService.ValidateCustomer(request.username, request.password);
            if (!validation)
            {
                _logger.LogWarning("Login Failed");
                return Unauthorized("Invalidate username or password");
            }
            else
            {
                _logger.LogInformation("Login successful");
                return Ok(new { success = true, id = customer.id });
            }
        }
        [HttpPost("coach")]
        public IActionResult CoachLogin([FromBody] LoginRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("Empty Request!");
                return BadRequest("Request cannot be empty");
            }

            _logger.LogInformation($"Request recieved: Username - {request.username}, Password - {request.password}");

            if (string.IsNullOrEmpty(request.username) || string.IsNullOrEmpty(request.password))
            {
                _logger.LogWarning("Empty Username or password");
                return BadRequest("Empty Username or password");
            }
            Coach coach = loginService.getCoachByUsername(request.username);
            if (coach == null)
            {
                _logger.LogWarning("User not found");
                return Unauthorized("Invalidate username or password");
            }

            bool validation = loginService.ValidateCoach(request.username, request.password);
            if (!validation)
            {
                _logger.LogWarning("Login Failed");
                return Unauthorized("Invalidate username or password");
            }
            else
            {
                _logger.LogInformation("Login successful");
                return Ok(new { success = true, id = coach.id });
            }
        }
        [HttpPost("create")]
        public IActionResult CreateCustomer([FromBody] CreateRequest request)
        {
            if (repo.FindCustomerByName(request.username) != null) return BadRequest("Username exists");
            if (request == null)
            {
                _logger.LogWarning("Empty Request!");
                return BadRequest("Empty Request");
            }
            if (string.IsNullOrEmpty(request.username) || string.IsNullOrEmpty(request.pwd))
            {
                _logger.LogWarning("Empty username or password");
                return BadRequest("Empty username or password");
            }
            repo.CreateCustomer(request.username, request.pwd, request.email, request.phoneNumber, request.isRookie,request.age,request.gender);
            return Ok();
        }
    }
    public class LoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    public class CreateRequest
    {
        public string username { get; set; }
        public string pwd { get; set; }
        public string email {  get; set; }
        public string phoneNumber {  get; set; }
        public bool isRookie {  get; set; }
        public int age {  get; set; }
        public string gender {  get; set; }
    }

}
