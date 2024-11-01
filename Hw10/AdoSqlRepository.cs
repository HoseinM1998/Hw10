using Dapper;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

public class AdoSqlRepository:IUserRepository
{

    public void Add(User user)
    {
        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            using (SqlCommand command = new SqlCommand(UserQueries.Create, connection))
            {
                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Status", user.Status);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
    public List<User> GetAll()
    {
        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            using (SqlCommand command = new SqlCommand(UserQueries.GetAll, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var users = new List<User>();
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = (int)reader["Id"],
                            UserName = (string)reader["UserName"],
                            Password = (string)reader["Password"],
                            Status = (string)reader["Status"]
                        });
                    }
                    return users;
                }
            }
        }
    }
    public User Get(string username)
    {
        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            using (SqlCommand command = new SqlCommand(UserQueries.GetByUsername, connection))
            {
                command.Parameters.AddWithValue("@UserName", username);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = (int)reader["Id"],
                            UserName = (string)reader["UserName"],
                            Password = (string)reader["Password"],
                            Status = (string)reader["Status"]
                        };
                    }
                }
            }
        }
        return null;
    }

    public void Update(User user)
    {
        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            using (SqlCommand command = new SqlCommand(UserQueries.Update, connection))
            {
                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Status", user.Status);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
    public void Delete(string userName)
    {
        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            using (SqlCommand command = new SqlCommand(UserQueries.Delete, connection))
            {
                command.Parameters.AddWithValue("@UserName", userName);
                connection.Open();

                command.ExecuteNonQuery();
            }
        }
    }
}



