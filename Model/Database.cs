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
    }
}