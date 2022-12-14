using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Model;


public class Database
{
    
    public Dictionary<char, int> AlphabetWithPoints { get; private set; }
    private const int _maxPoints = 45;
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
        AlphabetWithPoints = new Dictionary<char, int>();
        FillDictAlphabet();

    }

    private void FillDictAlphabet()
    {
        //Fill the dictionary with the lowercase alphabet and the points per letter
        AlphabetWithPoints.Add('a', 2);
        AlphabetWithPoints.Add('b', 5);
        AlphabetWithPoints.Add('c', 5);
        AlphabetWithPoints.Add('d', 1);
        AlphabetWithPoints.Add('e', 4);
        AlphabetWithPoints.Add('f', 1);
        AlphabetWithPoints.Add('g', 3);
        AlphabetWithPoints.Add('h', 3);
        AlphabetWithPoints.Add('i', 4);
        AlphabetWithPoints.Add('j', 1);
        AlphabetWithPoints.Add('k', 1);
        AlphabetWithPoints.Add('l', 2);
        AlphabetWithPoints.Add('m', 5);
        AlphabetWithPoints.Add('n', 4);
        AlphabetWithPoints.Add('o', 4);
        AlphabetWithPoints.Add('p', 4);
        AlphabetWithPoints.Add('q', 4);
        AlphabetWithPoints.Add('r', 3);
        AlphabetWithPoints.Add('s', 2);
        AlphabetWithPoints.Add('t', 3);
        AlphabetWithPoints.Add('u', 3);
        AlphabetWithPoints.Add('v', 4);
        AlphabetWithPoints.Add('w', 4);
        AlphabetWithPoints.Add('x', 4);
        AlphabetWithPoints.Add('y', 3);
        AlphabetWithPoints.Add('z', 5);
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
            TotalPoints += AlphabetWithPoints[letter];
        }

        switch (TotalPoints)
        {
            case <= (_maxPoints / 5):
                return 1;
                break;
            case <= (_maxPoints / 5) * 2:
                return 2;
                break;
            case <= (_maxPoints / 5) * 3:
                return 3;
                break;
            case <= (_maxPoints / 5) * 4:
                return 4;
                break; 
            default:
                return 5;
            break;
        }
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