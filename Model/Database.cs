using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Model;

public class Database
{
    //still need to grab userID for logged in user stats
    //int UserID = LoginController.UserID;

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


    //Method that updates data in the Pupilstatistics table
    public void UpdatePupilStatistics(int pupilId, string type, int amountCorrect, int amountFalse,
        double keyPerSec, int score)
    {
        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            var sql = "UPDATE PupilStatistics SET AmountFalse = AmountFalse + @amountFalse, AmountCorr = AmountCorr + @amountCorrect, KeyPerSec = ((KeyPerSec * AssignmentsMade) + @keyPerSec)/(AssignmentsMade+1), AssignmentsMade = AssignmentsMade+1, Score=@score WHERE PupilID = @pupilId AND Type = @type";
            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@pupilId", pupilId);
            command.Parameters.AddWithValue("@type", type);
            command.Parameters.AddWithValue("@amountFalse", amountFalse);
            command.Parameters.AddWithValue("@amountCorrect", amountCorrect);
            command.Parameters.AddWithValue("@keyPerSec", keyPerSec);
            command.Parameters.AddWithValue("@score", score);
            command.ExecuteNonQuery();
            connection.Close();
        }
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


    public string GetStatisticsNameDB(string userid)
    {
        List<string> nameList = new List<string>();
        string name = "";

        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            string sql = $" SELECT Firstname FROM Pupil WHERE PupilID = {userid}"; 
           
            
            var command = new SqlCommand(sql, connection);
            var reader = command.ExecuteReader();


            while (reader.Read())
            {
                name = reader[0].ToString();
            }

            connection.Close();
        }
        return name;
    }



    public List<string> GetStatisticsDB(int type, string userid)
    {
        List<string> statistics = new List<string>();
        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            string typeString;
            if(type == 0)
            {
                typeString = "Letter";
            } else if (type == 1)
            {
                typeString = "Word";
            } else if (type == 2)
            {
                typeString = "Story";
            } else
            {
                return null;
            }
            string sql = $"SELECT PupilID, Type, AmountCorr, AmountFalse, AssignmentsMade, ROUND(KeyPerSec, 1), Score FROM PupilStatistics WHERE PupilID = {userid} AND type = '{typeString}'";
            var command = new SqlCommand(sql, connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    statistics.Add(reader[i].ToString());
                }
            }

            connection.Close();
        }

        return statistics;
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