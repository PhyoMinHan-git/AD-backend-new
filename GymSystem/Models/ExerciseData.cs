namespace GymSystem.Models
{
    public class ExerciseData
    {
        public int id { get; set; }
        public int customerId { get; set; }
        public int BPM { get; set; }
        public double height { get; set; }
        public double weight { get; set; }
        public double BMI { get; set; }
        public int exerciseTime {  get; set; }
        public double avgCalories {  get; set; }
        public double avgMinutes {  get; set; }

    }
}
