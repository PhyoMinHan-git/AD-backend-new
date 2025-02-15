using GymSystem.Models;
using MySql.Data.MySqlClient;

namespace GymSystem.Repository
{
    public class AdminRepository
    {
        string connectionString;

        public AdminRepository(IConfiguration config)
        {
            connectionString = config.GetConnectionString("DefaultConnection");
        }    
        public Admin FindAdminByName(string username)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT * FROM adproject.admin WHERE username=@username";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@username", username);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Admin admin = new Admin()
                    {
                        id = (int)reader["id"],
                        username = (string)reader["username"],
                        pwd = (string)reader["password"],
                        email = reader["email"] == DBNull.Value ? "" : Convert.ToString(reader["email"]),
                        phoneNumber = reader["phoneNumber"] == DBNull.Value ? "" : Convert.ToString(reader["email"]),
                    };
                    conn.Close();
                    return admin;
                }
                conn.Close();
                return null;
            }
        }
    }
}
