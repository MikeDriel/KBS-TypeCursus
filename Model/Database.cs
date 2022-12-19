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

    public List<int> GetClasses(int teacherId)
    {
        List<int> classes = new List<int>();
        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            var sql = "SELECT ClassID FROM Classes WHERE TeacherID = (@teacherId)";
            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@teacherId", teacherId);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                classes.Add(reader.GetInt32(0));
            }

            connection.Close();
        }

        return classes;
    }

    public string GetClassName(int classId)
    {
        string className = "";
        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            var sql = "SELECT ClassName FROM Classes WHERE ClassID = (@classId)";
            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@classId", classId);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                className = reader.GetString(0);
            }

            connection.Close();
        }

        return className;
    }

    public List<int> GetStudents(int classId)
    {
        List<int> studentIds = new List<int>();
        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            var sql = "SELECT PupilID FROM Pupil WHERE ClassID = (@classId)";
            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@classId", classId);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                studentIds.Add(reader.GetInt32(0));
            }

            connection.Close();
        }

        return studentIds;
    }

    public string[] GetStudentName(int studentID)
    {
        string[] Pupil = new string[2];
        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            var sql = "SELECT Firstname,LastName FROM Pupil WHERE PupilID = (@studentID)";
            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@studentID", studentID);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Pupil[0] = reader[0].ToString();
                Pupil[1] = reader[1].ToString();
            }

            connection.Close();
        }

        return Pupil;
    }

    //Make new class and return the auto incremented class id using ExecuteScalar and output.insereted.classid
    public int AddNewClass(int TeacherId, string ClassName)
    {
        object newClassId;
        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            var sqlInsert = "INSERT INTO Classes (TeacherID, ClassName)" + "output inserted.ClassID " + "VALUES (@TeacherId, @ClassName)";
            var commandInsert = new SqlCommand(sqlInsert, connection);
            commandInsert.Parameters.AddWithValue("@TeacherId", TeacherId);
            commandInsert.Parameters.AddWithValue("@ClassName", ClassName);
            newClassId = commandInsert.ExecuteScalar();
            connection.Close();
        }

        return (int)newClassId;
    }

    //Update CLassName
    public void UpdateClassName(int ClassId, string NewClassName)
    {
        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            var sqlInsert = "UPDATE Classes SET ClassName = (@NewClassName) WHERE ClassID = (@ClassId)";
            var commandInsert = new SqlCommand(sqlInsert, connection);
            commandInsert.Parameters.AddWithValue("@ClassId", ClassId);
            commandInsert.Parameters.AddWithValue("@NewClassName", NewClassName);
            commandInsert.ExecuteReader();
            connection.Close();
        }
    }


    public string[] AddStudent(string[] student, int classId)
    {
        string UnhashedPassword = GetRandomPassword(5);
        string HashedPassword = HashPassword(UnhashedPassword);
        object newPupilId;
        string Username = student[0] +'_'+ student[1];
        Username.Replace(' ', '_');
        string UsernameSave = Username;
        int number = 1;
        while (checkIfUserNameExists(UsernameSave))
        {
            UsernameSave = Username;
            UsernameSave += number;
            number++;
        }
        Username = UsernameSave;
        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            var sqlInsert = "INSERT INTO Pupil (Firstname, Lastname, ClassID, Username, Password, UnHashedPasswords)"  + "output inserted.PupilID " + "VALUES ((@FirstName), (@LastName), (@classId),(@UserName),(@HashedPassword),(@UnhashedPassword))";
            var commandInsert = new SqlCommand(sqlInsert, connection);
            commandInsert.Parameters.AddWithValue("@FirstName", student[0]);
            commandInsert.Parameters.AddWithValue("@LastName", student[1]);
            commandInsert.Parameters.AddWithValue("@classId", classId);
            commandInsert.Parameters.AddWithValue("@UserName", Username);
            commandInsert.Parameters.AddWithValue("@HashedPassword", HashedPassword);
            commandInsert.Parameters.AddWithValue("@UnhashedPassword", UnhashedPassword);
            newPupilId = commandInsert.ExecuteScalar();
            connection.Close();
        }

        return new string[2] { newPupilId.ToString(), UnhashedPassword };
    }
    public string GetRandomPassword(int length)
    {
        const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        StringBuilder sb = new StringBuilder();
        Random rnd = new Random();

        for (int i = 0; i < length; i++)
        {
            int index = rnd.Next(chars.Length);
            sb.Append(chars[index]);
        }

        return sb.ToString();
    }

    public string getPupilUserName(int studentID)
    {
        string Pupil = "";
        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            var sql = "SELECT Username FROM Pupil WHERE PupilID = (@studentID)";
            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@studentID", studentID);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Pupil = reader[0].ToString();
            }

            connection.Close();
        }

        return Pupil;
    } 
    
    public bool checkIfUserNameExists(string username)
    {
        string? Pupil = null;
        using (var connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            var sql = "SELECT Username FROM Pupil WHERE Username = (@Username)";
            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Username", username);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Pupil = reader[0].ToString();
            }
            connection.Close();
        }

        if (Pupil == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}