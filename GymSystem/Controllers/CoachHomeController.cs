using GymSystem.Models;
using GymSystem.Repository;
using GymSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoachHomeController : ControllerBase
    {
        private readonly BookingRepository bookingRepo;
        private readonly CoachRepository coachRepo;
        private readonly CustomerRepository customerRepo;
        private readonly AvailableTimeRepository timeRepo;
        private readonly ILogger<LoginController> _logger;
        private readonly LoginService service;
        private readonly ExerciseHistoryRepository historyRepo;
        private readonly ExerciseDataRepository dataRepo;
        public CoachHomeController(BookingRepository bookingRepo, ILogger<LoginController> logger, CoachRepository coachRepo, AvailableTimeRepository timeRepo, LoginService service, ExerciseHistoryRepository historyRepo,CustomerRepository customerRepo, ExerciseDataRepository dataRepo)
        {
            this.bookingRepo = bookingRepo;
            this.coachRepo = coachRepo;
            _logger = logger;
            this.timeRepo = timeRepo;
            this.service = service;
            this.historyRepo = historyRepo;
            this.customerRepo = customerRepo;
            this.dataRepo = dataRepo;
        }
        
        [HttpGet("bookings/upcoming/{id}")]
        public IActionResult GetUpcomingBookings([FromRoute] int id)
        {
            _logger.LogInformation("Received request to get upcoming bookings for coach ID: {CoachId}", id);

            List<Booking> bookings = bookingRepo.FindBookingsByCoachId(id);

            if (bookings == null || bookings.Count == 0)
            {
                _logger.LogWarning("No bookings found for coach ID: {CoachId}", id);
                return Ok(new List<Booking>());
            }

            List<Booking> upcomingBookings = new List<Booking>();
            foreach (Booking booking in bookings)
            {
                if (booking.status == "Successful")
                {
                    _logger.LogInformation("Adding successful booking: {BookingId}", booking.id);
                    upcomingBookings.Add(booking);
                }
            }

            if (upcomingBookings.Count == 0)
            {
                _logger.LogWarning("No upcoming bookings found for coach ID: {CoachId}", id);
            }

            return Ok(upcomingBookings);
        }
        [HttpGet("bookings/finished/{id}")]
        public IActionResult GetFinishedBookings([FromRoute] int id)
        {
            List<Booking> bookings = bookingRepo.FindBookingsByCoachId(id);
            if (bookings == null) return BadRequest();
            List<Booking> finishedBookings = new List<Booking>();
            foreach (Booking booking in bookings) if (booking.status == "finished") finishedBookings.Add(booking);
            return Ok(finishedBookings);
        }
        [HttpPost("assessment")]
        public IActionResult SubmitAssessment([FromBody] TrainingAssessment assessment)
        {
            Booking booking = bookingRepo.FindBookingById(assessment.bookingId);
            bookingRepo.FinishBooking(booking.id);
            Coach coach=coachRepo.FindCoachById(assessment.coachId);
            historyRepo.CreateHistory(assessment.customerId, coach.username, assessment.duration, assessment.caloriesBurned, booking.date, assessment.BPM, assessment.exerciseType,assessment.coachId,assessment.exercise_type);
            Customer customer=service.getCustomerById(assessment.customerId);
            coachRepo.UpdateNumOfExercise(coach.id, coach.numofExercise);
            if (customer.exerciseData == null) dataRepo.CreateExerciseData(customer.id, assessment.height, assessment.weight, assessment.BPM, assessment.caloriesBurned, assessment.duration);
            else
            {
                customer.rating=(customer.rating*customer.numOfExercise+assessment.score)/(++customer.numOfExercise);
                customerRepo.UpdateCustomerRating(customer.id, customer.rating,customer.numOfExercise);
                ExerciseData exerciseData = customer.exerciseData;
                exerciseData.BPM = (exerciseData.BPM * exerciseData.exerciseTime + assessment.BPM) / (exerciseData.exerciseTime+1);
                exerciseData.avgCalories = (exerciseData.avgCalories * exerciseData.exerciseTime + assessment.caloriesBurned) / (exerciseData.exerciseTime+1);
                exerciseData.avgMinutes = (exerciseData.avgMinutes * exerciseData.exerciseTime + assessment.duration) / (exerciseData.exerciseTime+1);
                exerciseData.exerciseTime++;
                dataRepo.UpdateExerciseData(exerciseData.id, exerciseData.BPM,exerciseData.avgCalories,exerciseData.avgMinutes,exerciseData.exerciseTime,assessment.height,assessment.weight);
            }
            
            return Ok();
        }
        [HttpGet("profile/{id}")]
        public IActionResult GetProfile([FromRoute] int id)
        {
            Coach coach=service.getCoachById(id);
            if (coach == null) return BadRequest("Coach not found");
            return Ok(coach);
        }
        [HttpGet("data/{id}")]
        public IActionResult GetCoachData([FromRoute] int id)
        {
            CoachDataResponse response=new CoachDataResponse();
            Coach coach=service.getCoachById(id);
            response.id = coach.id;
            response.certifications = coach.certifications;
            response.specialization = coach.specialization;
            response.years_experience = coach.experienceYear;
            response.avg_client_rating = coach.rating;
            int tot_days = coachRepo.GetRegisteredDays(id);
            response.clients_per_week= coach.numofExercise/(tot_days/7+1);
            List<ExerciseHistory> histories = new List<ExerciseHistory>();
            histories=historyRepo.FindHistoryByCoachId(id);
            int minutes = 0;
            foreach (ExerciseHistory history in histories) minutes += history.duration;
            int hours=minutes/60;
            response.workout_hours_per_week=hours/(tot_days/7+1);
            return Ok(response);
        }
        [HttpPost("level")]
        public IActionResult StoreCoachLevel([FromBody] CoachLecelUpdateRequest request)
        {
            coachRepo.UpdateCoachLevel(request.coachId, request.level);
            return Ok();
        }
        [HttpGet("customerData/{id}")]
        public IActionResult GetCustomerData([FromRoute] int id)
        {
            ConsistencyDataResponse response = new ConsistencyDataResponse();
            Customer customer=customerRepo.FindCustomerById(id);
            response.age=customer.age;
            int tot_days = customerRepo.GetRegisteredDays(id);
            response.sessions_per_week = customer.numOfExercise / (tot_days / 7 + 1);
            ExerciseData exerciseData=customer.exerciseData;
            response.calories_burned_per_session = exerciseData.avgCalories;
            response.avg_duration_per_session = exerciseData.avgMinutes;
            response.membership_duration_months = tot_days / 30+1;
            response.exercise_type = customer.favoriteType;
            response.trainer_engagement = (customer.trainerEngagement == true) ? 1 : 0;
            return Ok(response);
        }
        [HttpPost("consistency-level")]
        public IActionResult StoreStickyLevel([FromBody] ConsistencyLevelRequest request)
        {
            _logger.LogInformation("Received request to store level: {StickyLevel}", request.consistencyLevel);
            customerRepo.UpdateCustomerStickyLevel(request.customerId, request.consistencyLevel);
            return Ok();    
        }
    }
    public class TrainingAssessment
    {
        public int bookingId { get; set; }
        public int coachId { get; set; }
        public int customerId { get; set; }
        public int score { get; set; }
        public int duration { get; set; }
        public int caloriesBurned { get; set; }
        public string exerciseType { get; set; }
        public int BPM {  get; set; }
        public double height {  get; set; }
        public double weight {  get; set; }
        public string exercise_type { get; set; }
    }
    public class CoachDataResponse
    {
        public int id { get; set; }
        public int years_experience { get; set; }
        public int certifications { get; set; }
        public int clients_per_week { get; set; }
        public double avg_client_rating { get; set; }
        public int workout_hours_per_week { get; set; }
        public string specialization { get; set; }
    }
    public class CoachLecelUpdateRequest
    {
        public int coachId { set; get; }
        public int level { get; set; }
    }
    public class ConsistencyDataResponse
    {
        public int age { get; set; }
        public int sessions_per_week {get; set; }
        public double avg_duration_per_session {  get; set; }
        public double calories_burned_per_session { get; set; }
        public int membership_duration_months {  get; set; }
        public string exercise_type {  get; set; }
        public int trainer_engagement {  get; set; }
    }
    public class ConsistencyLevelRequest
    {
        public int customerId { set; get; }
        public int consistencyLevel { get; set; }
        public int bookingId {  set; get; }
    }
}
