using GymSystem.Models;
using MySql.Data.MySqlClient;

namespace GymSystem.Repository
{
    public class ExerciseDataRepository
    {
        string connectionString;

        public ExerciseDataRepository(IConfiguration config)
        {
            connectionString = config.GetConnectionString("DefaultConnection");
        }
        public ExerciseData FindExerciseDataByCustomerId(int customerId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT * FROM adproject.exercisedata WHERE customerId=@id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", customerId);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ExerciseData exerciseData = new ExerciseData()
                    {
                        id = (int)reader["id"],
                        customerId = reader["customerId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["customerId"]),
                        BPM = reader["BPM"] == DBNull.Value ? 0 : Convert.ToInt32(reader["BPM"]),
                        BMI = reader["bmi"] == DBNull.Value ? 0 : Convert.ToInt32(reader["bmi"]),
                        height = reader["height"] == DBNull.Value ? 0 : Convert.ToInt32(reader["height"]),
                        weight = reader["weight"] == DBNull.Value ? 0 : Convert.ToInt32(reader["weight"]),
                        avgCalories = reader["avgCalories"] == DBNull.Value ? 0 : Convert.ToDouble(reader["avgCalories"]),
                        avgMinutes = reader["avgMinutes"] == DBNull.Value ? 0 : Convert.ToDouble(reader["avgMinutes"]),
                        exerciseTime = reader["exerciseTime"] == DBNull.Value ? 0 : Convert.ToInt32(reader["exerciseTime"]),
                    };
                    conn.Close();
                    return exerciseData;
                }
                conn.Close();
                return null;
            }
        }
        public void UpdateExerciseData(int id, int BPM, double avgCalories,double avgMinutes,int exerciseTime,double height,double weight)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE adproject.exerciseData SET BPM = @BPM, avgCalories=@calories, avgMinutes=@minutes, exerciseTime=@number, height=@height, weight=@weight WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@BPM", BPM);
                cmd.Parameters.AddWithValue("@calories", avgCalories);
                cmd.Parameters.AddWithValue("@minutes", avgMinutes);
                cmd.Parameters.AddWithValue("@number", exerciseTime);
                cmd.Parameters.AddWithValue("@height", height);
                cmd.Parameters.AddWithValue("@weight", weight);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void CreateExerciseData(int customerId,double height,double weight, int BPM, int calories,int duration)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO adproject.exerciseData(customerId, height, weight, BPM, avgCalories,avgMinutes,exerciseTime)
                VALUES(@id, @height, @weight, @BPM,@calories,@duration,1)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@BPM", BPM);
                cmd.Parameters.AddWithValue("@calories", calories);
                cmd.Parameters.AddWithValue("@duration", duration);
                cmd.Parameters.AddWithValue("@height", height);
                cmd.Parameters.AddWithValue("@weight", weight);
                cmd.Parameters.AddWithValue("@id", customerId);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
