﻿using System.Collections;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Model;

public enum Difficulty
{
    Niveau1 = 1,
    Niveau2 = 2,
    Niveau3 = 3,
    Niveau4 = 4,
    Niveau5 = 5
}

public enum TypeExercise
{
    Letter,
    Word,
    Story
}

public class Database
{
    public Dictionary<char, int> AlphabetWithPoints { get; private set; }
    public int SizeExercise { get; private set; }

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

	public string GetStatisticsNameDB(string userid)
	{
		List<string> nameList = new List<string>();
		string name = "";

		using (var connection = new SqlConnection(DatabaseConnectionString()))
		{
			connection.Open();
			string sql = $" SELECT Firstname FROM Pupil WHERE PupilID = {userid}";

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


    public List<char> GetWord(Difficulty difficulty, int amountOfWords)
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


    //Method that updates data in the Pupilstatistics table
    public void UpdatePupilStatistics(int pupilId, TypeExercise type, int amountCorrect, int amountFalse,
        double keyPerSec, int score)
    {
        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            var sql = "UPDATE PupilStatistics SET AmountFalse = AmountFalse + @amountFalse, AmountCorr = AmountCorr + @amountCorrect, KeyPerSec = ((KeyPerSec * AssignmentsMade) + @keyPerSec)/(AssignmentsMade+1), AssignmentsMade = AssignmentsMade+1, Score= Score + @score WHERE PupilID = @pupilId AND Type = @type; UPDATE PupilStatistics SET Score = 0 WHERE Score < 0";
            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@pupilId", pupilId);
            command.Parameters.AddWithValue("@type", type.ToString());
            command.Parameters.AddWithValue("@amountFalse", amountFalse);
            command.Parameters.AddWithValue("@amountCorrect", amountCorrect);
            command.Parameters.AddWithValue("@keyPerSec", keyPerSec);
            if (score > 0)
            {
                command.Parameters.AddWithValue("@score", score);
            } else
            {
                command.Parameters.AddWithValue("@score", score);
            }
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

    // make a txtfile with an given name and fill it with keys and values of a dictionary
    public void MakeTxtFile(string fileName, Dictionary<string, int> dict)
    {
        using (StreamWriter sw = new StreamWriter(fileName))
        {
            foreach (var item in dict)
            {
                sw.WriteLine(item.Key + " " + item.Value);
            }
        }
    }


			while (reader.Read())
			{
				name = reader[0].ToString();
			}

			connection.Close();
		}
		return name;
	}

	/// <summary>
	/// Gets the ClassID of the class of the current user.
	/// Used to get all the pupils in the class of the current user.
	/// </summary>
	/// <param name="userid"></param>
	/// <returns></returns>
	public int GetClassId(string userid)
	{
		int classid = new();
		List<string> userids = new();
		using (var connection = new SqlConnection(DatabaseConnectionString()))
		{
			connection.Open();
			string sql;
			sql = $"SELECT ClassID from Pupil WHERE PupilID = {userid}";

			var command = new SqlCommand(sql, connection);
			var reader = command.ExecuteReader();

			while (reader.Read())
			{
				for (var i = 0; i < reader.FieldCount; i++)
				{
					userids.Add(reader[i].ToString());
				}
				classid = (int)reader[0];
			}
			connection.Close();
		}
		return classid;
	}

	/// <summary>
	/// Gets all PupilID's from the Class with the given ClassID
	/// Used to generate leaderboard.
	/// </summary>
	/// <param name="classid"></param>
	/// <returns></returns>
	public List<string> GetClass(int classid)
	{
		List<string> userids = new();
		using (var connection = new SqlConnection(DatabaseConnectionString()))
		{
			connection.Open();
			string sql;
			sql = $"SELECT PupilID FROM Pupil WHERE ClassID = {classid}";

			var command = new SqlCommand(sql, connection);
			var reader = command.ExecuteReader();

			while (reader.Read())
			{
				for (var i = 0; i < reader.FieldCount; i++)
				{
					userids.Add(reader[i].ToString());
				}
			}
			connection.Close();
		}
		return userids;
	}

	/// <summary>
	/// Generates the leaderboard.
	/// Gets the PupilID, Firstname, Lastname and the SUM of the score of every user.
	/// Adds every user in a list of leaderboard.
	/// </summary>
	/// <param name="userids"></param>
	/// <param name="classid"></param>
	/// <returns></returns>
	public List<List<string>> GenerateLeaderboard(List<int> userids, int classid)
	{
		List<List<string>> leaderboard = new();
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

		using (SqlConnection connection =
		   new SqlConnection(DatabaseConnectionString()))
		{
			foreach (var userid in userids)
			{
				string sql = $"SELECT Pupil.PupilID, Pupil.Firstname, Pupil.Lastname, SUM(PupilStats.Score) " +
					$"FROM PupilStatistics PupilStats JOIN Pupil Pupil ON Pupil.PupilID = PupilStats.PupilID " +
					$"WHERE Pupil.ClassID = {classid} AND Pupil.PupilID = {userid} " +
					$"GROUP BY Pupil.PupilID, Pupil.Firstname, Pupil.Lastname " +
					$"ORDER BY SUM(PupilStats.Score) DESC;";
				SqlCommand command = new SqlCommand(sql, connection);

				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					leaderboard.Add(new List<string> { reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString() });
				}
				connection.Close();
			}
		}
		return leaderboard;
	}

	public List<string> GetStatisticsDB(int type, string userid)
	{
		List<string> statistics = new List<string>();
		using (var connection = new SqlConnection(DatabaseConnectionString()))
		{
			connection.Open();
			string sql;
			if (type == 0)
			{
				sql = $" SELECT * FROM PupilStatistics WHERE PupilID = {userid} AND type = 'Letter' "; // letter statistics for type = 0
			}
			else if (type == 1)
			{
				sql = $" SELECT * FROM PupilStatistics WHERE PupilID = {userid} AND type = 'Word' ";// word statistics for type = 1
			}
			else if (type == 2)
			{
				sql = $" SELECT * FROM PupilStatistics WHERE PupilID = {userid} AND type = 'Story' ";// story statistics for type = 2
			}
			else
			{
				return null;
			}

			var command = new SqlCommand(sql, connection);
			var reader = command.ExecuteReader();

    // get password from the database
    public string[]? GetPasswordWithId(bool? isTeacher, string? loginKey)
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

		return statistics;
	}

            string[] passwordWithId = new string[2];
            while (reader.Read())
            {
                passwordWithId[0] = reader[0].ToString();
                passwordWithId[1] = reader[1].ToString();
                Debug.WriteLine(passwordWithId);
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

    private Difficulty getWordDifficulty(string word)
    {
        const int maxPoints = 45;
        int TotalPoints = 0;
        foreach (char letter in word)
        {
            TotalPoints += AlphabetWithPoints[letter];
        }

        switch (TotalPoints)
        {
            case <= (maxPoints / 5):
                return Difficulty.Niveau1;
                break;
            case <= (maxPoints / 5) * 2:
                return Difficulty.Niveau2;
                break;
            case <= (maxPoints / 5) * 3:
                return Difficulty.Niveau3;
                break;
            case <= (maxPoints / 5) * 4:
                return Difficulty.Niveau4;
                break;
            default:
                return Difficulty.Niveau5;
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

        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            foreach (string word in wordList)
            {
                connection.Open();
                var sql = "UPDATE Words SET Difficulty = @difficulty WHERE Words = @word";
                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@difficulty", getWordDifficulty(word));
                command.Parameters.AddWithValue("@word", word);
                command.ExecuteReader();
                command.Dispose();
                connection.Close();
            }
        }
    }

    private int getScore(int pupilId, TypeExercise type)
    {
        int score = 0;
        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            var sql = "SELECT Score FROM PupilStatistics WHERE PupilId = @pupilId AND Type = @type";
            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@pupilId", pupilId);
            command.Parameters.AddWithValue("@type", type.ToString());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                score = Convert.ToInt32(reader[0]);
            }

            connection.Close();
        }

        return score;
    }

    public Difficulty getNiveau(int pupilID, TypeExercise typeExercise)
    {
        const int minSize = 5;
        const int maxscore = 100;
        int score = getScore(pupilID, typeExercise);
        switch (score)
        {
            case <= (maxscore / 5):
                setSizeExercise(1,maxscore,score, minSize);
                return Difficulty.Niveau1;
                break;
            case <= (maxscore / 5) * 2:
                setSizeExercise(2,maxscore,score, minSize);
                return Difficulty.Niveau2;
                break;
            case <= (maxscore / 5) * 3:
                setSizeExercise(3,maxscore,score, minSize);
                return Difficulty.Niveau3;
                break;
            case <= (maxscore / 5) * 4:
                setSizeExercise(4,maxscore,score, minSize);
                return Difficulty.Niveau4;
                break;
            default:
                setSizeExercise(5,maxscore,score, minSize);
                return Difficulty.Niveau5;
                break;
        }
    }

    private void setSizeExercise(int sizeScore , int maxscore, int score, int minSize)
    {
        if (score > maxscore)
        {
            score = maxscore;
        }
        SizeExercise = ((((maxscore / 5)*sizeScore)- score) * minSize);
        if (SizeExercise <= 0)
        {
            SizeExercise = minSize;
        }
    }

    public void CheckIfPupilStatisticsExist(int pupilId)
    {
        bool exists = false;
        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            foreach (var VARIABLE in TypeExercise.GetValues(typeof(TypeExercise)))
            {
                connection.Open();
                var sql = "SELECT * FROM PupilStatistics WHERE PupilId = @pupilId AND Type = @type";
                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@pupilId", pupilId);
                command.Parameters.AddWithValue("@type", VARIABLE.ToString());
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    exists = true;
                }

                command.Dispose();
                connection.Close();

                if (!exists)
                {
                    connection.Open();
                    var sqlInsert =
                        "INSERT INTO PupilStatistics (PupilId, Type, Score,AmountCorr, AmountFalse, AssignmentsMade, KeyPerSec ) VALUES (@pupilId, @type, 1,0,0,0,0)";
                    var commandInsert = new SqlCommand(sqlInsert, connection);
                    commandInsert.Parameters.AddWithValue("@pupilId", pupilId);
                    commandInsert.Parameters.AddWithValue("@type", VARIABLE.ToString());
                    commandInsert.ExecuteReader();
                    connection.Close();
                }

                exists = false;
            }
        }
    }
}