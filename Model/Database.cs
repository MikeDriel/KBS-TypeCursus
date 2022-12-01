using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class Database
	{

		static void Main(string[] args)
		{

			try
			{
				SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
				builder.DataSource = "127.0.0.1";
				builder.UserID = "SA";
				builder.Password = "KaasKnabbel123!";
				builder.InitialCatalog = "TestDB";

				using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
				{
					Console.WriteLine("\nQuery data example:");
					Console.WriteLine("=========================================\n");

					//Hiet zet je de query die je wilt uitvoeren
					String sql = "SELECT * FROM Inventory";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						connection.Open();
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								for (int i = 0; i < reader.FieldCount; i++)
								{
									{
										Console.Write(reader.GetValue(i) + " ");

									}
								}
							}
						}
					}
				}
			}
			catch (SqlException e)
			{
				Console.WriteLine(e.ToString());
			}
			Console.ReadLine();
		}
	}
}