using GymSystem.Models;
using MySql.Data.MySqlClient;

namespace GymSystem.Repository
{
    public class CoachRepository
    {
        string connectionString;

        public CoachRepository(IConfiguration config)
        {
            connectionString = config.GetConnectionString("DefaultConnection");
        }
        public Coach FindCoachByName(string username)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT * FROM adproject.coaches WHERE username=@username";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@username", username);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Coach coach = new Coach()
                    {
                        id = (int)reader["id"],
                        username = (string)reader["username"],
                        pwd = (string)reader["password"],
                        email = reader["email"] == DBNull.Value ? string.Empty : reader["email"].ToString(),
                        phoneNumber = reader["phoneNumber"] == DBNull.Value ? string.Empty : reader["phoneNumber"].ToString(),
                        rating = reader["rating"] == DBNull.Value ? 0 : Convert.ToInt32(reader["rating"]),
                        level = reader["level"] == DBNull.Value ? 0 : Convert.ToInt32(reader["level"]),
                        numofRating = reader["numberofrating"] == DBNull.Value ? 0 : Convert.ToInt32(reader["numberofrating"]),
                        description = reader["description"] == DBNull.Value ? "" : Convert.ToString(reader["description"]),
                        isRookie = reader["isRookie"] == DBNull.Value ? false : Convert.ToBoolean(reader["isRookie"]),
                        dates = reader["dates"] == DBNull.Value ? new List<string>() : new List<string>(reader["dates"].ToString().Split(',')),
                        //availability= ((string)reader["availability"]).Split(',').ToList()
                        specialization = reader["specialization"] == DBNull.Value ? string.Empty : reader["specialization"].ToString(),
                        certifications = reader["certifications"] == DBNull.Value ? 0 : Convert.ToInt32(reader["certifications"]),
                        experienceYear = reader["experienceYear"] == DBNull.Value ? 0 : Convert.ToInt32(reader["experienceYear"]),
                        numofExercise = reader["numofExercise"] == DBNull.Value ? 0 : Convert.ToInt32(reader["numofExercise"])
                    };
                    conn.Close();
                    return coach;
                }
                conn.Close();
                return null;
            }
        }
        public Coach FindCoachById(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT * FROM adproject.coaches WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Coach coach = new Coach()
                    {
                        id = (int)reader["id"],
                        username = (string)reader["username"],
                        pwd = (string)reader["password"],
                        email = reader["email"] == DBNull.Value ? string.Empty : reader["email"].ToString(),
                        phoneNumber = reader["phoneNumber"] == DBNull.Value ? string.Empty : reader["phoneNumber"].ToString(),
                        rating= reader["rating"] == DBNull.Value ? 0 : Convert.ToDouble(reader["rating"]),
                        level = reader["level"] == DBNull.Value ? 0 : Convert.ToInt32(reader["level"]),
                        numofRating = reader["numberofrating"] == DBNull.Value ? 0 : Convert.ToInt32(reader["numberofrating"]),
                        description = reader["description"] == DBNull.Value ? string.Empty : reader["description"].ToString(),
                        isRookie = reader["isRookie"] == DBNull.Value ? false : Convert.ToBoolean(reader["isRookie"]),
                        dates = reader["dates"] == DBNull.Value ? new List<string>() : new List<string>(reader["dates"].ToString().Split(',')),
                        //availability = ((string)reader["availability"]).Split(',').ToList()
                        specialization = reader["specialization"] == DBNull.Value ? string.Empty : reader["specialization"].ToString(),
                        certifications = reader["certifications"] == DBNull.Value ? 0 : Convert.ToInt32(reader["certifications"]),
                        experienceYear = reader["experienceYear"] == DBNull.Value ? 0 : Convert.ToInt32(reader["experienceYear"]),
                        numofExercise = reader["numofExercise"] == DBNull.Value ? 0 : Convert.ToInt32(reader["numofExercise"]),
                    };
                    conn.Close();
                    return coach;
                }
                conn.Close();
                return null;
            }
        }
        public List<Coach> findAllCoaches()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT * FROM adproject.coaches";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<Coach> coaches = new List<Coach>();
                while(reader.Read())
                {
                    Coach coach = new Coach()
                    {
                        id = (int)reader["id"],
                        username = (string)reader["username"],
                        pwd = (string)reader["password"],
                        email = reader["email"] == DBNull.Value ? string.Empty : reader["email"].ToString(),
                        phoneNumber = reader["phoneNumber"] == DBNull.Value ? string.Empty : reader["phoneNumber"].ToString(),
                        level = reader["level"] == DBNull.Value ? 0 : Convert.ToInt32(reader["level"]),
                        rating = reader["rating"] == DBNull.Value ? 0 : Convert.ToDouble(reader["rating"]),
                        numofRating = reader["numberofrating"] == DBNull.Value ? 0 : Convert.ToInt32(reader["numberofrating"]),
                        description = reader["description"] == DBNull.Value ? string.Empty : reader["description"].ToString(),
                        dates = reader["dates"] == DBNull.Value ? new List<string>() : new List<string>(reader["dates"].ToString().Split(',')),
                        //availability = ((string)reader["availability"]).Split(',').ToList()
                        specialization = reader["specialization"] == DBNull.Value ? string.Empty : reader["specialization"].ToString(),
                        certifications = reader["certifications"] == DBNull.Value ? 0 : Convert.ToInt32(reader["certifications"]),
                        experienceYear = reader["experienceYear"] == DBNull.Value ? 0 : Convert.ToInt32(reader["experienceYear"]),
                        numofExercise = reader["numofExercise"] == DBNull.Value ? 0 : Convert.ToInt32(reader["numofExercise"]),
                    };
                    coaches.Add(coach);
                }
                conn.Close();
                return coaches;
            }
        }
        public void UpdateRating(int coachId,double rating,int number)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE adproject.coaches SET rating = @rating, numberofrating=@number WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", coachId);
                cmd.Parameters.AddWithValue("@rating",rating);
                cmd.Parameters.AddWithValue("@number", number);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void ModifyPwd(int id, string pwd)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE adproject.coaches SET password = @pwd WHERE id = @id";
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
                string sql = @"UPDATE adproject.coaches SET email = @email WHERE id = @id";
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
                string sql = @"UPDATE adproject.coaches SET phonenumber = @number WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@number", number);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void ModifyDescription(int id, string description)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE adproject.coaches SET description = @description WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void UpdateNumOfExercise(int id, int number)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE adproject.coaches SET numofExercise = @number WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@number", number);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public int GetRegisteredDays(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT DATEDIFF(CURDATE(), STR_TO_DATE(registerDate, '%Y-%m-%d')) AS registeredDays
                FROM adproject.coaches WHERE id = @id";
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
        public void UpdateCoachLevel(int id, int level)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE adproject.coaches SET level = @level WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@level", level);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void CreateCoach(string username, string pwd, string email, string phoneNumber, bool isRookie, string description,int experienceYear,string specialization,int certifications)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO adproject.coaches(username, password, email, phoneNumber, isRookie, description, experienceYear, specialization,certifications,registerDate)
                VALUES(@username, @pwd, @email, @phoneNumber,@isRookie,@description,@year,@specialization,@certifications,CURDATE())";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@pwd", pwd);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                cmd.Parameters.AddWithValue("@isRookie", isRookie);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@year", experienceYear);
                cmd.Parameters.AddWithValue("@specialization", specialization);
                cmd.Parameters.AddWithValue("@certifications", certifications);
                cmd.ExecuteNonQuery();
                conn.Close();
            }

        }
        public List<Coach> GetLowRatedCoaches()
        {
            List<Coach> coaches = new List<Coach>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT *  FROM adproject.coaches
                ORDER BY rating ASC LIMIT 5";
                MySqlCommand cmd = new MySqlCommand( sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Coach coach = new Coach()
                    {
                        id = (int)reader["id"],
                        username = (string)reader["username"],
                        pwd = (string)reader["password"],
                        email = reader["email"] == DBNull.Value ? string.Empty : reader["email"].ToString() ,
                        phoneNumber = reader["phoneNumber"] == DBNull.Value ? string.Empty : reader["phoneNumber"].ToString() ,
                        level = reader["level"] == DBNull.Value ? 0 : Convert.ToInt32(reader["level"]),
                        rating = reader["rating"] == DBNull.Value ? 0 : Convert.ToDouble(reader["rating"]),
                        numofRating = reader["numberofrating"] == DBNull.Value ? 0 : Convert.ToInt32(reader["numberofrating"]) ,
                        description = reader["description"] == DBNull.Value ? string.Empty : reader["description"].ToString(),
                        dates = reader["dates"] == DBNull.Value ? new List<string>() : new List<string>(reader["dates"].ToString().Split(',')),
                        //availability = ((string)reader["availability"]).Split(',').ToList()
                        specialization = (string)reader["specialization"],
                        certifications = reader["certifications"] == DBNull.Value ? 0 : Convert.ToInt32(reader["certifications"]),
                        experienceYear = reader["experienceYear"] == DBNull.Value ? 0 : Convert.ToInt32(reader["experienceYear"]),
                        numofExercise =  reader["numofExercise"] == DBNull.Value ? 0 : Convert.ToInt32(reader["numofExercise"])
                    };
                    coaches.Add(coach);
                }
                return coaches;
            }
            return null;
        }
        public void DeleteCoach(string username)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"DELETE FROM adproject.coaches WHERE username = @name";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", username);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }


        public void ModifyDate(int id, string dates)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE adproject.coaches SET dates = @dates WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@dates", dates);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }



    }
}
