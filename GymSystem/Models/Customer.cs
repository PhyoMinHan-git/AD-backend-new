namespace GymSystem.Models
{
    public class Customer: User
    {
        public double rating { get; set; }
        public int level { get; set; }
        public int stickyLevel { get; set; }
       /* public double height {  get; set; }
        public double BPM { get; set; }
        public double weight { get; set; }
        public double BMI { get; set; }*/
       public int age {  get; set; }
        public string gender {  get; set; }
       public bool isRookie {  get; set; }
       public int numOfExercise {  get; set; }
       public ExerciseData exerciseData { get; set; } = new ExerciseData();
        public List<Booking> bookings { get; set; } = new List<Booking>();
        public List<ExerciseHistory> exerciseHistory { get; set; }= new List<ExerciseHistory>();
        public string favoriteType {  get; set; }
        public bool trainerEngagement {  get; set; }
    }
}
