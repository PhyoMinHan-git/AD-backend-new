using System.Reflection;
using GymSystem.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace GymSystem.Repository
{
    public class ExerciseHistoryRepository
    {
        string connectionString;

        public ExerciseHistoryRepository(IConfiguration config)
        {
            connectionString = config.GetConnectionString("DefaultConnection");
        }
        public List<ExerciseHistory> FindHistoryByCustomerId(int customerId)
        {
            List<ExerciseHistory> histories = new List<ExerciseHistory>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT * FROM adproject.exercisehistories WHERE customerId=@id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", customerId);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ExerciseHistory history = new ExerciseHistory()
                    {
                        id = (int)reader["id"],
                        customerId = reader["customerId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["customerId"]),
                        //coachId = (int)reader["coachId"],
                        duration = reader["duration"] == DBNull.Value ? 0 : Convert.ToInt32(reader["duration"]),
                        BPM = reader["BPM"] == DBNull.Value ? 0 : Convert.ToInt32(reader["BPM"]),
                        date = reader["date"] == DBNull.Value ? string.Empty : reader["date"].ToString(),
                        calories = reader["calories"] == DBNull.Value ? 0 : Convert.ToInt32(reader["calories"]),
                        coachName = reader["coachName"] == DBNull.Value ? string.Empty : reader["coachName"].ToString(),
                        type = reader["type"] == DBNull.Value ? string.Empty : reader["type"].ToString(),
                        coachId = reader["coachId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["coachId"]),
                        exerciseType = reader["exerciseType"] == DBNull.Value ? string.Empty : reader["exerciseType"].ToString(),
                        isRated = reader["isRated"] == DBNull.Value ? false : Convert.ToBoolean(reader["isRated"]),
                    };
                    histories.Add(history);
                }
                conn.Close();
                return histories;
            }
        }
        public List<ExerciseHistory> FindHistoryByCoachId(int coachId)
        {
            List<ExerciseHistory> histories = new List<ExerciseHistory>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT * FROM adproject.exercisehistories WHERE coachId=@id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", coachId);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ExerciseHistory history = new ExerciseHistory()
                    {
                        id = (int)reader["id"],
                        customerId = reader["customerId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["customerId"]),
                        //coachId = (int)reader["coachId"],
                        duration = reader["duration"] == DBNull.Value ? 0 : Convert.ToInt32(reader["duration"]),
                        BPM = reader["BPM"] == DBNull.Value ? 0 : Convert.ToInt32(reader["BPM"]),
                        date = reader["date"] == DBNull.Value ? string.Empty : reader["date"].ToString(),
                        calories = reader["calories"] == DBNull.Value ? 0 : Convert.ToInt32(reader["calories"]),
                        coachName = reader["coachName"] == DBNull.Value ? string.Empty : reader["coachName"].ToString(),
                        type = reader["type"] == DBNull.Value ? string.Empty : reader["type"].ToString(),
                        coachId = reader["coachId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["coachId"]),
                        exerciseType = reader["exerciseType"] == DBNull.Value ? string.Empty : reader["exerciseType"].ToString(),
                        isRated = reader["isRated"] == DBNull.Value ? false : Convert.ToBoolean(reader["isRated"]),
                    };
                    histories.Add(history);
                }
                conn.Close();
                return histories;
            }
        }
        public void UpdateRatedStatus(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE adproject.exercisehistories SET isRated = TRUE WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void CreateHistory(int customerId,string coachName,int duration,int calories,string date,int BPM,string type,int coachId,string exerciseType)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO adproject.exercisehistories(customerId, coachName, duration, calories, date,BPM,type,coachId,exerciseType)
                VALUES(@customerId, @coachName, @duration, @calories,@date,@BPM,@type,@coachId,@exerciseType)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@customerId",customerId);
                cmd.Parameters.AddWithValue("@coachName", coachName);
                cmd.Parameters.AddWithValue("@duration", duration);
                cmd.Parameters.AddWithValue("@calories", calories);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@BPM", BPM);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@coachId", coachId);
                cmd.Parameters.AddWithValue("@exerciseType", exerciseType);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public int GetNumOfExerciseType(int customerId,string exerciseType)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT COUNT(*) AS record_count FROM adproject.exercisehistories
                WHERE customerId = @id AND exerciseType = @type";
                MySqlCommand cmd = new MySqlCommand( sql, conn);
                cmd.Parameters.AddWithValue("@id", customerId);
                cmd.Parameters.AddWithValue("@type", exerciseType);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int num = Convert.ToInt32(reader["record_count"]);
                    conn.Close();
                    return num;
                }
                return 0;
            }
        }
        public int GetNumOfExercise()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT COUNT(*) AS TotalCount FROM adproject.exercisehistories";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int num = Convert.ToInt32(reader["TotalCount"]);
                    conn.Close();
                    return num;
                }
                return 0;
            }
        }
        public int GetNumOfExerciseinOneWeek()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT COUNT(*) AS TotalCount FROM adproject.exercisehistories
                WHERE STR_TO_DATE(date, '%Y%m%d') >= DATE_SUB(CURDATE(), INTERVAL 7 DAY)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int num = Convert.ToInt32(reader["TotalCount"]);
                    conn.Close();
                    return num;
                }
                return 0;
            }
        }
        public string GetMostPopularType()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT exerciseType, COUNT(*) AS totalCount FROM adproject.exercisehistories
                GROUP BY exerciseType
                ORDER BY totalCount DESC
                LIMIT 1;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string type = (string)reader["exerciseType"];
                    conn.Close();
                    return type;
                }
                return null;
            }
        }
    }
}
