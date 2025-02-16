using GymSystem.Models;
using MySql.Data.MySqlClient;

namespace GymSystem.Repository
{
    public class AvailableTimeRepository
    {
        string connectionString;

        public AvailableTimeRepository(IConfiguration config)
        {
            connectionString = config.GetConnectionString("DefaultConnection");
        }
        public string FindAvailableTimeByDate(string date,int coachId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT time FROM adproject.availabletime WHERE coachId=@id AND date=@date";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", coachId);
                cmd.Parameters.AddWithValue("@date", date);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string time=(string)reader["time"];
                    conn.Close();
                    return time;

                }
                conn.Close();
                return null;
            } 
        }
        public void UpdateTime(int coachId,string date,string availability)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE adproject.availabletime SET time = @time WHERE coachId = @id AND date = @date";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", coachId);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@time",availability);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void CreateAvailableTime(int coachId,string date,string availability)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO adproject.availabletime (coachId, date, time)
                VALUES(@id, @date, @time)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", coachId);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@time", availability);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
