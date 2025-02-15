using System.Diagnostics;
using GymSystem.Models;
using GymSystem.Repository;
using GymSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mysqlx.Cursor;
using Org.BouncyCastle.Utilities.Collections;

namespace GymSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerHomeController : ControllerBase
    {
        private readonly BookingRepository bookingRepo;
        private readonly ExerciseHistoryRepository historyRepo;
        private readonly CoachRepository coachRepo;
        private readonly CustomerRepository customerRepo;
        private readonly AvailableTimeRepository timeRepo;
        private readonly ILogger<LoginController> _logger;
        private readonly LoginService service;

        public CustomerHomeController(BookingRepository bookingRepo, ILogger<LoginController> logger, CoachRepository coachRepo, AvailableTimeRepository timeRepo, LoginService service, ExerciseHistoryRepository historyRepo, CustomerRepository customerRepo)
        {
            this.bookingRepo = bookingRepo;
            this.coachRepo = coachRepo;
            _logger = logger;
            this.timeRepo = timeRepo;
            this.service = service;
            this.historyRepo = historyRepo;
            this.customerRepo = customerRepo;
        }


        [HttpGet("bookings/{id}")]
        public IActionResult getBookingList([FromRoute] int id)
        {
            _logger.LogInformation($"Received booking list request for customer ID: {id}");

            List<Booking> bookings = bookingRepo.FindBookingsByCustomerId(id);
            if (bookings == null)
            {
                _logger.LogWarning("No bookings found");
                return Ok(new List<Booking>());
            }
            _logger.LogInformation($"Booking list retrieved for customer ID: {id}");
            return Ok(bookings);
        }




        [HttpPost("coaches")]
        public IActionResult getAvailableCoach([FromBody] CoachListRequest request)
        {
            _logger.LogInformation($"Request recieved: Date - {request.date}, startTime - {request.startTime}");
            List<Coach> coaches = coachRepo.findAllCoaches();
            List<Coach> coachToReturn = new List<Coach>();

            foreach (Coach coach in coaches)
            {
                string availableTime = timeRepo.FindAvailableTimeByDate(request.date, coach.id);
                if (availableTime != null)
                {
                    coach.availability = availableTime.Split(',').ToList();
                    bool isAvailable = true;

                    foreach (string date in coach.dates)
                    {
                        if (date.Equals(request.date))
                        {
                            isAvailable = true;
                            break;
                        }
                    }

                    if (!isAvailable)
                    {
                        continue;
                    }

                    for (int j = request.startTime; j < request.endTime; j++)
                    {
                        if (coach.availability[j].Equals("No"))
                        {
                            isAvailable = false;
                            break;
                        }
                    }

                    if (isAvailable)
                    {
                        coachToReturn.Add(coach);
                    }
                }
            }

            foreach (Coach coach in coachToReturn)
            {
                _logger.LogInformation($"Coach found: id - {coach.id}");
            }

            if (coachToReturn.Count > 0)
            {
                return Ok(coachToReturn);
            }

            return Ok(null);
        }
        
        [HttpPost("book")]
        public IActionResult BookCoach([FromBody] BookCoachRequest request)
        {
            if (request == null)
            {
                _logger.LogError("Received a null request in BookCoach API");
                return BadRequest("Invalid request payload");
            }

            _logger.LogInformation($"Booking request received: CustomerId - {request.customerId}, CoachId - {request.coachId}, Date - {request.date}");

            Customer customer = service.getCustomerById(request.customerId);
            if (customer == null) return BadRequest("Invalid customer");

            Coach coach = service.getCoachById(request.coachId);
            if (coach == null) return BadRequest("Invalid coach");

            string availableTime = timeRepo.FindAvailableTimeByDate(request.date, coach.id);
            coach.availability = availableTime != null ? availableTime.Split(',').ToList() : new List<string>();

            bookingRepo.CreateBooking(customer.id, coach.id, request.date, request.startTime, request.endTime, request.coachName);

            for (int i = request.startTime; i < request.endTime; i++)
            {
                if (i < coach.availability.Count)
                    coach.availability[i] = "No";
            }

            timeRepo.UpdateTime(coach.id, request.date, string.Join(",", coach.availability));

            return Ok(bookingRepo.FindBookingsByCustomerId(request.customerId));
        }

        [HttpGet("profile/{id}")]
        public IActionResult getProfile([FromRoute] int id)
        {
            _logger.LogInformation($"Received profile request for customer ID: {id}");

            Customer customer = service.getCustomerById(id);
            if (customer == null)
            {
                _logger.LogWarning("Customer not found");
                return BadRequest("Customer not Found");
            }
            _logger.LogInformation($"Profile retrieved: {customer}");
            return Ok(customer);
        }
        [HttpGet("history/{id}")]
        public IActionResult getExerciseHistory([FromRoute] int id)
        {
            _logger.LogInformation($"Received request to get exercise history for customer ID: {id}");
            List<ExerciseHistory> histories = historyRepo.FindHistoryByCustomerId(id);
            if (histories == null) return BadRequest("Cannot find any exercise history");
            _logger.LogInformation($"Returning {histories.Count} history records for customer ID: {id}");
            foreach (var history in histories)
            {
                _logger.LogInformation($"History Record (Before JSON): ID: {history.id}, BPM: {history.BPM}, Date: {history.date}, Type: {history.type}");
            }
            string jsonOutput = System.Text.Json.JsonSerializer.Serialize(histories);
            _logger.LogInformation($"Final JSON Output: {jsonOutput}");
            return Ok(histories);
        }

        [HttpPost("rate")]
        public IActionResult RateCoach([FromBody] RateCoachRequest request)
        {
            Coach coach = service.getCoachById(request.coachId);
            coach.numofRating++;
            coach.rating = (coach.rating * (coach.numofRating - 1) + request.rating) / coach.numofRating;
            coachRepo.UpdateRating(request.coachId, coach.rating, coach.numofRating);
            historyRepo.UpdateRatedStatus(request.historyId);
            return Ok(coach);
        }
        [HttpGet("data/{id}")]
        public IActionResult GetMemberData([FromRoute] int id)
        {
            Customer customer = service.getCustomerById(id);
            if (customer == null) return BadRequest("Customer not found");

            ExerciseData exerciseData = customer.exerciseData ?? new ExerciseData()
            {
                height = 0,
                weight = 0,
                avgMinutes = 0,
                BPM = 0,
                avgCalories = 0,
                exerciseTime = 0
            };

            MemberDataResponse response = new MemberDataResponse
            {
                id = id,
                age = customer.age,
                height = exerciseData.height,
                weight = exerciseData.weight,
                avg_duration_per_session = exerciseData.avgMinutes,
                heartbeat = exerciseData.BPM,
                calories_burned_per_session = exerciseData.avgCalories,
                exercise_time = exerciseData.exerciseTime,
                exercise_type = customer.favoriteType ?? "Unknown"
            };

            int tot_days = customerRepo.GetRegisteredDays(id);
            response.sessions_per_week = customer.numOfExercise / (tot_days / 7 + 1);

            _logger.LogInformation($"Data Record (Before JSON): ID: {response.id}, age: {response.age}, times: {response.sessions_per_week}, Type: {response.exercise_type}, Days: {tot_days}");

            return Ok(response);
        }
        [HttpPost("level")]
        public IActionResult StoreCustomerLevel([FromBody] CustomerLevelRequest request)
        {
            customerRepo.UpdateCustomerLevel(request.customerId, request.level);
            return Ok();
        }



        [HttpGet("recommendation/{id}")]
        public IActionResult GetRecommendedExercise([FromRoute] int id)
        {
            RecommendationRespond respond = new RecommendationRespond
            {
                customerId = id
            };

            Customer customer = customerRepo.FindCustomerById(id);
            if (customer == null)
            {
                return NotFound("Customer not found");
            }

            respond.age = customer.age;

            List<string> exerciseTypes = new List<string>
            {
                "Bench Press", "Squats", "Deadlifts", "Pull-ups", "Bicep Curls",
                "Leg Press", "Treadmill", "Jump Rope", "Rowing Machine", "Planks"
            };

            foreach (string exercise in exerciseTypes)
            {
                int count = historyRepo.GetNumOfExerciseType(id, exercise);
                _logger.LogInformation($"Adding to Dictionary: {exercise} = {count}");
                respond.exercise[exercise] = count; // 确保正确添加
            }

            // 打印所有存入的键
            _logger.LogInformation("Final Dictionary Keys:");
            foreach (var key in respond.exercise.Keys)
            {
                _logger.LogInformation($"Key: '{key}'");
            }

            return Ok(respond);
        }
    }






    public class CoachListRequest
    {
        public string date { get; set; }
        public int startTime { get; set; }
        public int endTime { get; set; }
    }
    public class BookCoachRequest
    {
        public string date { get; set; }
        public int startTime { get; set; }
        public int endTime { get; set; }
        public int coachId { get; set; }
        public int customerId { get; set; }
        public string coachName { get; set; }
    }
    public class RateCoachRequest
    {
        public int coachId { get; set; }
        public int rating { get; set; }
        public int historyId {  get; set; }
    }
    public class MemberDataResponse
    {
        public int id { get; set; }
        public int age { get; set; }
        public int sessions_per_week { get; set; }
        public double avg_duration_per_session { get; set; }
        public double calories_burned_per_session { get; set; }
        public string exercise_type { get; set; }
        public int exercise_time { get; set; }
        public int heartbeat { get; set; }
        public double height { get; set; }
        public double weight { get; set; }
    }
    public class CustomerLevelRequest
    {
        public int customerId { get; set; }
        public int level { get; set; }
    }
    public class RecommendationRespond
    {
        public int customerId { get; set; }
        public int age { get; set; }
        public Dictionary<string, int> exercise { get; set; } = new Dictionary<string, int>(); // ✅ 这里加 = new Dictionary<string, int>()
    }

}