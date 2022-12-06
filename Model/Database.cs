using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Database
    {
        //private static string connectionString = "Data Source=DESKTOP-2QJQJ3G\\SQLEXPRESS;Initial Catalog=Test;Integrated Security=True";

        public string DatabaseConnectionString()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "127.0.0.1";
                builder.UserID = "SA";
                builder.Password = "KaasKnabbel123!";
                builder.InitialCatalog = "TestDB";

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
            List<char> wordList = new List<char>();
            using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
            {
                connection.Open();
                string sql = "SELECT TOP (@amountOfWords) Words FROM Words ORDER BY NEWID()";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@amountOfWords", amountOfWords);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        foreach (char character in reader[i].ToString())
                        {
                            wordList.Add(character);
                        }

                        //Adds space between words 
                        wordList.Add(' ');
                    }
                }
                connection.Close();
            }
			wordList.RemoveAt(wordList.Count - 1);
            return wordList;
        }

		
		// get password from the database
		public string? GetPasswordWithId(bool? isTeacher, string? loginKey)
		{
			using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
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
				SqlDataReader reader = command.ExecuteReader();
				//Debug.WriteLine(reader[0].ToString());
				
				string passwordWithId = "";
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
			byte[] data = Encoding.ASCII.GetBytes(password);
			data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
			string hash = Encoding.ASCII.GetString(data);
			return hash;
		}
		
		public void HashAllPasswords()
		{
			string[] passwordsToHash = { "NOK","Mike","Beer", "Bassie", "Luccie"};
			string[] Usernames = {"nok","Mike", "Raivo", "Bas", "Luc"};
			int i = 0;
			Database database = new Database();
			foreach (string password in passwordsToHash)
			{
				using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
				{
					connection.Open();
					SqlCommand command = new SqlCommand("UPDATE Pupil SET Password= (@password) WHERE Username=(@username)", connection);
					command.Parameters.AddWithValue("@password", database.HashPassword(password));
					command.Parameters.AddWithValue("@username", Usernames[i]);
					//command.Prepare();
					command.ExecuteReader();
					connection.Close();
				}

				i++;
			}
		}
	}
}