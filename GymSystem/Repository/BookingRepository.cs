using GymSystem.Models;
using MySql.Data.MySqlClient;

namespace GymSystem.Repository
{
    public class BookingRepository
    {
        string connectionString;

        public BookingRepository(IConfiguration config)
        {
            connectionString = config.GetConnectionString("DefaultConnection");
        }

        public List<Booking> FindBookingsByCustomerId(int customerId)
        {
            List<Booking> bookings = new List<Booking>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT b.*, c.username AS coachName FROM adproject.bookings b 
                               JOIN adproject.coaches c ON b.coachId = c.id 
                               WHERE b.customerId = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", customerId);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Booking booking = new Booking()
                    {
                        id = (int)reader["id"],
                        customerId = reader["customerId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["customerId"]),
                        coachId = reader["coachId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["coachId"]),
                        startTime = reader["startTime"] == DBNull.Value ? 0 : Convert.ToInt32(reader["startTime"]),
                        endTime = reader["endTime"] == DBNull.Value ? 0 : Convert.ToInt32(reader["endTime"]),
                        date = reader["date"] == DBNull.Value ? string.Empty : reader["date"].ToString(),
                        status = reader["status"] == DBNull.Value ? string.Empty : reader["status"].ToString(),
                        coachName = reader["coachName"] == DBNull.Value ? string.Empty : reader["coachName"].ToString(),
                    };
                    bookings.Add(booking);
                }
                conn.Close();
                if (!bookings.Any()) return null;
                return bookings;
            }
        }

        public Booking FindBookingById(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT * FROM adproject.bookings WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Booking booking = new Booking()
                    {
                        id = (int)reader["id"],
                        customerId = reader["customerId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["customerId"]),
                        coachId = reader["coachId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["coachId"]),
                        startTime = reader["startTime"] == DBNull.Value ? 0 : Convert.ToInt32(reader["startTime"]),
                        endTime = reader["endTime"] == DBNull.Value ? 0 : Convert.ToInt32(reader["endTime"]),
                        date = reader["date"] == DBNull.Value ? string.Empty : reader["date"].ToString(),
                        status = reader["status"] == DBNull.Value ? string.Empty : reader["status"].ToString(),
                    };
                    conn.Close();
                    return booking;
                }
                conn.Close();
                return null;
            }
        }
        public List<Booking> FindBookingsByCoachId(int coachId)
        {
            List<Booking> bookings = new List<Booking>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT b.*, c.username AS coachName FROM adproject.bookings b 
                               JOIN adproject.coaches c ON b.coachId = c.id 
                               WHERE b.coachId = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", coachId);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Booking booking = new Booking()
                    {
                        id = (int)reader["id"],
                        customerId = reader["customerId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["customerId"]),
                        coachId = reader["coachId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["coachId"]),
                        startTime = reader["startTime"] == DBNull.Value ? 0 : Convert.ToInt32(reader["startTime"]),
                        endTime = reader["endTime"] == DBNull.Value ? 0 : Convert.ToInt32(reader["endTime"]),
                        date = reader["date"] == DBNull.Value ? string.Empty : reader["date"].ToString(),
                        status = reader["status"] == DBNull.Value ? string.Empty : reader["status"].ToString(),
                        coachName = reader["coachName"] == DBNull.Value ? string.Empty : reader["coachName"].ToString(),
                    };
                    bookings.Add(booking);
                }
                conn.Close();
                if (!bookings.Any()) return null;
                return bookings;
            }
        }

        public void CreateBooking(int customerId, int coachId, string date, int startTime, int endTime, string coachName)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO adproject.Bookings (customerId, coachId, date, startTime, endTime, status, coachName) VALUES (@customerId, @coachId, @date, @startTime, @endTime, 'Successful', @coachName)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@customerId", customerId);
                cmd.Parameters.AddWithValue("@coachId", coachId);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@startTime", startTime);
                cmd.Parameters.AddWithValue("@endTime", endTime);
                cmd.Parameters.AddWithValue("@coachName", coachName);
                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteBooking(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql= @"DELETE FROM adproject.bookings WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
        public void FinishBooking(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE adproject.bookings SET status = 'finished' WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
