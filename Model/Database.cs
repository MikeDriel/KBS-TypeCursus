using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Model;

public enum Difficulty
{
    niveau1,
    niveau2,
    niveau3,
    niveau4,
    niveau5

}

public class Database
{
    private Dictionary<char, int> _alphabetWithPoints;
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

    public Database()
    {
        _alphabetWithPoints = new Dictionary<char, int>();
        FillDictAlphabet();
        
        Debug.WriteLine(Difficulty.niveau4.ToString());
    }

    private void FillDictAlphabet()
    {
        //Fill the dictionary with the lowercase alphabet and the points per letter
        _alphabetWithPoints.Add('a', 1);
        _alphabetWithPoints.Add('b', 4);
        _alphabetWithPoints.Add('c', 4);
        _alphabetWithPoints.Add('d', 1);
        _alphabetWithPoints.Add('e', 3);
        _alphabetWithPoints.Add('f', 1);
        _alphabetWithPoints.Add('g', 2);
        _alphabetWithPoints.Add('h', 2);
        _alphabetWithPoints.Add('i', 3);
        _alphabetWithPoints.Add('j', 1);
        _alphabetWithPoints.Add('k', 1);
        _alphabetWithPoints.Add('l', 1);
        _alphabetWithPoints.Add('m', 4);
        _alphabetWithPoints.Add('n', 3);
        _alphabetWithPoints.Add('o', 3);
        _alphabetWithPoints.Add('p', 3);
        _alphabetWithPoints.Add('q', 3);
        _alphabetWithPoints.Add('r', 2);
        _alphabetWithPoints.Add('s', 1);
        _alphabetWithPoints.Add('t', 2);
        _alphabetWithPoints.Add('u', 2);
        _alphabetWithPoints.Add('v', 3);
        _alphabetWithPoints.Add('w', 3);
        _alphabetWithPoints.Add('x', 4);
        _alphabetWithPoints.Add('y', 2);
        _alphabetWithPoints.Add('z', 4);
    }


    public List<char> GetWord(int difficulty, int amountOfWords)
    {
        var wordList = new List<char>();
        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            var sql = "SELECT TOP (@amountOfWords) Words FROM Words WHERE Difficulty = (@difficulty) ORDER BY NEWID()";
            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@amountOfWords", amountOfWords);
            command.Parameters.AddWithValue("@difficulty", difficulty);
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
                command = new SqlCommand("SELECT Password, TeacherId FROM Teacher WHERE Email = (@loginKey)",
                    connection);
            }
            else
            {
                command = new SqlCommand("SELECT Password, PupilId FROM Pupil WHERE Username = (@loginKey)",
                    connection);
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

    private int getWordDifficulty(string word)
    {
        int TotalPoints = 0;
        foreach (char letter in word)
        {
            TotalPoints += _alphabetWithPoints[letter];
        }
        return TotalPoints;
    }

    public void AddDifficultyToDatabase()
    {
        var wordList = new List<string>();
        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            var sql = "SELECT Words FROM Words ORDER BY NEWID()";
            var command = new SqlCommand(sql, connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    wordList.Add(reader[i].ToString());
                }
            }

            connection.Close();
        }

        foreach (string word in wordList)
        {
            using (var connection = new SqlConnection(DatabaseConnectionString()))
            {
                connection.Open();
                var sql = "UPDATE Words SET Difficulty = @difficulty WHERE Words = @word";
                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@difficulty", getWordDifficulty(word));
                command.Parameters.AddWithValue("@word", word);
                command.ExecuteReader();
                connection.Close();
            }
        }
    }
}