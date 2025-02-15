namespace GymSystem.Models
{
    public class Booking
    {
        public int id { get; set; }
        public int customerId { get; set; }
        public int coachId { get; set; }
        public int startTime { get; set; }
        public int endTime { get; set; }
        public string status { get; set; }
        public string date { get; set; }

        public string coachName { get; set; } 
    }
}
