using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Model;

public class Database
{
    private string? DatabaseConnectionString()
    {
        try
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = "127.0.0.1",
                UserID = "SA",
                Password = "KaasKnabbel123!",
                InitialCatalog = "TestDB",
                ConnectTimeout = 3
            };

            return builder.ConnectionString;
        }

        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
            return null;
        }
    }


    public List<char> GetWord(int amountOfWords)
    {
        var wordList = new List<char>();
        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            var sql = "SELECT TOP (@amountOfWords) Words FROM Words ORDER BY NEWID()";
            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@amountOfWords", amountOfWords);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    wordList.AddRange(reader[i].ToString());

                    //Adds space between words 
                    wordList.Add(' ');
                }
            }

            connection.Close();
        }

        wordList.RemoveAt(wordList.Count - 1);
        return wordList;
    }

    //This method is used to get stories from the database
    public string GetStory()
    {
        string StoryString = "";
        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            var sql = "SELECT TOP 1 Verhaaltje FROM Verhaal ORDER BY NEWID()";
            var command = new SqlCommand(sql, connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    StoryString = reader[i].ToString();
                }
            }

            connection.Close();
        }

        return StoryString;
    }

    public string GetStatisticsDB()
    {
        var statistics = new StringBuilder();
        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            var sql = "SELECT PupilID FROM PupilStatistics;";
            var command = new SqlCommand(sql, connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    statistics.Append(reader[i].ToString());
                    statistics.Append(" ");
                }

                statistics.Append(Environment.NewLine);
            }

            connection.Close();
        }

        return statistics.ToString();
    }


    // get password from the database
    public string? GetPasswordWithId(bool? isTeacher, string? loginKey)
    {
        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            if (isTeacher == null || loginKey == null)
            {
                return null;
            }

            connection.Open();
            SqlCommand command;
            if (isTeacher == true)
            {
                command = new SqlCommand("SELECT Password, TeacherId FROM Teacher WHERE Email = (@loginKey)", connection);
            }
            else
            {
                command = new SqlCommand("SELECT Password, PupilId FROM Pupil WHERE Username = (@loginKey)", connection);
            }

            command.Parameters.AddWithValue("@LoginKey", loginKey);
            var reader = command.ExecuteReader();
            //Debug.WriteLine(reader[0].ToString());

            var passwordWithId = "";
            while (reader.Read())
            {
                passwordWithId = reader[0].ToString();
                passwordWithId += "," + reader[1];
                Debug.WriteLine(passwordWithId);
            }

            connection.Close();

            return passwordWithId;
        }
    }

    // hash incoming string and return the hashed value
    public string HashPassword(string password)
    {
        var data = Encoding.ASCII.GetBytes(password);
        data = new SHA256Managed().ComputeHash(data);
        var hash = Encoding.ASCII.GetString(data);
        return hash;
    }

    public async Task<bool> IsServerConnected()
    {
        await using var connection = new SqlConnection(DatabaseConnectionString());

        try
        {
            await connection.OpenAsync();
            return true;
        }
        catch (SqlException)
        {
            return false;
        }
    }
}