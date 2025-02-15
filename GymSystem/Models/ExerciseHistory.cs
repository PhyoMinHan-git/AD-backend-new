using System.Text.Json.Serialization;

namespace GymSystem.Models
{
    public class ExerciseHistory
    {
        public int id {  get; set; }
        public int customerId {  get; set; }
        public string coachName {  get; set; }
        public int duration {  get; set; }
        public string date {  get; set; }
        public int calories {  get; set; }
        public int coachId {  get; set; }
       
        [JsonPropertyName("bPM")]  
        public int BPM {  get; set; }
        public string type {  get; set; }
        public string exerciseType {  get; set; }
        public bool isRated {  get; set; }
    }
    
}
