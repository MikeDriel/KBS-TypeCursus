﻿using System.Collections;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

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
	public List<List<string>> GenerateLeaderboard(List<int> userids, int classid)
	{
		List<List<string>> leaderboard = new();

		using (SqlConnection connection =
		   new SqlConnection(DatabaseConnectionString()))
		{
			foreach (var userid in userids)
			{
				string sql = $"SELECT Pupil.PupilID, Pupil.Firstname, Pupil.Lastname, SUM(PupilStats.Score) FROM PupilStatistics PupilStats JOIN Pupil Pupil ON Pupil.PupilID = PupilStats.PupilID WHERE Pupil.ClassID = {classid} AND Pupil.PupilID = {userid} GROUP BY Pupil.PupilID, Pupil.Firstname, Pupil.Lastname ORDER BY SUM(PupilStats.Score) DESC;";
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