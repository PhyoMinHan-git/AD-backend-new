using GymSystem.Models;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Mysqlx.Notice.Warning.Types;

namespace GymSystem.Repository
{
    public class CustomerRepository
    {
        string connectionString;

        public CustomerRepository(IConfiguration config)
        {
            connectionString = config.GetConnectionString("DefaultConnection");
        }
        public Customer FindCustomerByName(string username)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT * FROM adproject.customers WHERE username=@username";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@username", username);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Customer customer = new Customer()
                    {
                        id = (int)reader["id"],
                        username = (string)reader["username"],
                        pwd = (string)reader["password"],
                        email = reader["email"] == DBNull.Value ? string.Empty : reader["email"].ToString(),
                        phoneNumber = reader["phoneNumber"] == DBNull.Value ? string.Empty : reader["phoneNumber"].ToString(),
                        level = reader["level"] == DBNull.Value ? 0 : Convert.ToInt32(reader["level"]),
                        stickyLevel = reader["stickyLevel"] == DBNull.Value ? 0 : Convert.ToInt32(reader["stickyLevel"]),
                        age = reader["age"] == DBNull.Value ? 0 : Convert.ToInt32(reader["age"]),
                        gender = reader["gender"] == DBNull.Value ? string.Empty : reader["gender"].ToString(),
                        numOfExercise = reader["numOfExercise"] == DBNull.Value ? 0 : Convert.ToInt32(reader["numOfExercise"]),
                        trainerEngagement = reader["trainerEngagement"] == DBNull.Value ? false : Convert.ToBoolean(reader["trainerEngagement"])
                    };
                    conn.Close();
                    return customer;
                }
                conn.Close();
                return null;
            }
        }
        public Customer FindCustomerById(int id) {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT * FROM adproject.customers WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Customer customer = new Customer()
                    {
                        id = (int)reader["id"],
                        username = (string)reader["username"],
                        pwd = (string)reader["password"],
                        email = reader["email"] == DBNull.Value ? string.Empty : reader["email"].ToString(),
                        phoneNumber = reader["phoneNumber"] == DBNull.Value ? string.Empty : reader["phoneNumber"].ToString(),
                        rating = reader["rating"] == DBNull.Value ? 0 : Convert.ToInt32(reader["rating"]),
                        /*BPM = (double)reader["bpm"],
                        BMI = (double)reader["bmi"],
                        height = (double)reader["height"],
                        weight = (double)reader["weight"],*/
                        level = reader["level"] == DBNull.Value ? 0 : Convert.ToInt32(reader["level"]),
                        stickyLevel = reader["stickyLevel"] == DBNull.Value ? 0 : Convert.ToInt32(reader["stickyLevel"]),
                        isRookie = reader["isRookie"] == DBNull.Value ? false : Convert.ToBoolean(reader["isRookie"]),
                        age = reader["age"] == DBNull.Value ? 0 : Convert.ToInt32(reader["age"]),
                        gender = reader["gender"] == DBNull.Value ? string.Empty : reader["gender"].ToString(),
                        numOfExercise = reader["numOfExercise"] == DBNull.Value ? 0 : Convert.ToInt32(reader["numOfExercise"]),
                        trainerEngagement = reader["trainerEngagement"] == DBNull.Value ? false : Convert.ToBoolean(reader["trainerEngagement"])
                    };
                    conn.Close();
                    return customer;
                }
                conn.Close();
                return null;
            }
        }
        public void CreateCustomer(string username,string pwd,string email,string phoneNumber,bool isRookie,int age,string gender)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO adproject.customers(username, password, email, phoneNumber, isRookie,age,gender,numOfExercise,registerDate,trainerEngagement)
                VALUES(@username, @pwd, @email, @phoneNumber,@isRookie,@age,@gender,0,CURDATE(),false)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@pwd", pwd);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                cmd.Parameters.AddWithValue("@isRookie", isRookie);
                cmd.Parameters.AddWithValue("@age", age);
                cmd.Parameters.AddWithValue("@gender", gender);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
               
        }
        public void ModifyPwd(int id,string pwd)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE adproject.customers SET password = @pwd WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@pwd", pwd);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void ModifyEmail(int id, string email)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE adproject.customers SET email = @email WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void ModifyPhoneNumber(int id, string number)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE adproject.customers SET phonenumber = @number WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@number", number);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void UpdateCustomerRating(int id, double rating,int numOfExercise)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE adproject.customers SET rating = @rating, numOfExercise=@number WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@rating", rating);
                cmd.Parameters.AddWithValue("@number", numOfExercise);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void UpdateCustomerLevel(int id, int level)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE adproject.customers SET level = @level WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@level", level);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void UpdateCustomerStickyLevel(int id, int level)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE adproject.customers SET stickyLevel = @level WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@level", level);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public string FindFavoriteType(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT type FROM adproject.exerciseHistories WHERE customerId = @id
                GROUP BY type ORDER BY COUNT(*) DESC LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string type = (string)reader["type"];
                    conn.Close();
                    return type;
                }
                return null;
            }
        }
        public int GetRegisteredDays(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT DATEDIFF(CURDATE(), STR_TO_DATE(registerDate, '%Y-%m-%d')) AS registeredDays
                FROM adproject.customers WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int days = Convert.ToInt32(reader["registeredDays"]);
                    conn.Close();
                    return days;
                }
                return 0;
            }
        }
        public List<Customer> GetCrisisCustomer()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                List<Customer> crisis = new List<Customer>();
                conn.Open();
                string sql = @"SELECT * FROM adproject.customers WHERE stickyLevel<=1";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Customer customer = new Customer()
                    {
                        id = (int)reader["id"],
                        username = (string)reader["username"],
                        pwd = (string)reader["password"],
                        email = reader["email"] == DBNull.Value ? string.Empty : reader["email"].ToString(),
                        phoneNumber = reader["phoneNumber"] == DBNull.Value ? string.Empty : reader["phoneNumber"].ToString(),
                        rating = reader["rating"] == DBNull.Value ? 0 : Convert.ToDouble(reader["rating"]),
                        level = reader["level"] == DBNull.Value ? 0 : Convert.ToInt32(reader["level"]),
                        stickyLevel = reader["stickyLevel"] == DBNull.Value ? 0 : Convert.ToInt32(reader["stickyLevel"]),
                        isRookie = reader["isRookie"] == DBNull.Value ? false : Convert.ToBoolean(reader["isRookie"]),
                        age = reader["age"] == DBNull.Value ? 0 : Convert.ToInt32(reader["age"]),
                        gender = reader["gender"] == DBNull.Value ? string.Empty : reader["gender"].ToString(),
                        numOfExercise = reader["numOfExercise"] == DBNull.Value ? 0 : Convert.ToInt32(reader["numOfExercise"]),
                        trainerEngagement = reader["trainerEngagement"] == DBNull.Value ? false : Convert.ToBoolean(reader["trainerEngagement"]),
                    };
                    crisis.Add(customer);
                }
                conn.Close();
                return crisis;
            }
        }
    }
}
