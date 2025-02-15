namespace GymSystem.Models
{
    public class Coach:User
    {
        public double rating { get; set; }
        public int numofRating {  get; set; }
        public int numofExercise {  get; set; }
        public int level {  get; set; }
        public List<string> dates { get; set; } = new List<string>();
        //public int startTime {  get; set; }
        //public int endTime { get; set; }
        public List<string> availability { get; set; } = new List<string>();
        public string description {  get; set; }
        public bool isRookie {  get; set; }
        public int certifications {  get; set; }
        public string specialization {  get; set; }
        public int experienceYear {  get; set; }
    }
}
