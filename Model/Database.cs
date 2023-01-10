using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Model;

/// <summary>
/// enumerator with all the possible difficulty's
/// </summary>
public enum Difficulty
{
    Level1 = 1,
    Level2 = 2,
    Level3 = 3,
    Level4 = 4,
    Level5 = 5
}

/// <summary>
/// enumerator with all the possible game types
/// </summary>
public enum TypeExercise
{
    Letter,
    Word,
    Story
}

public class Database
{
    public Database()
    {
        AlphabetWithPoints = new Dictionary<char, int>();
        FillDictAlphabet();
    }

    // dictionary with all the letters from the alphabet and their assigned points
    public Dictionary<char, int> AlphabetWithPoints { get; set; }
    public int SizeExercise { get; private set; }

    private string? DatabaseConnectionString()
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
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

    /// <summary>
    /// Method to get the username based on the userId
    /// </summary>
    /// <param name="userid"></param>
    /// <returns>Username</returns>
    public string GetName(int userid)
    {
        List<string> nameList = new List<string>();
        string name = "";

        using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            string sql = " SELECT Firstname FROM Pupil WHERE PupilID = (@userid)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@userid", userid);
            SqlDataReader? reader = command.ExecuteReader();


            while (reader.Read())
            {
                name = reader[0].ToString();
            }

            connection.Close();
        }

        return name;
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

    /// <summary>
    /// Get the words for the exercise based on difficulty and amount of words
    /// </summary>
    /// <param name="difficulty"></param>
    /// <param name="amountOfWords"></param>
    /// <returns>List with all chars for the exercise</returns>
    public List<char> GetWord(Difficulty difficulty, int amountOfWords)
    {
        List<char> wordList = new List<char>();
        using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            string sql =
                "SELECT TOP (@amountOfWords) Words FROM Words WHERE Difficulty = (@difficulty) ORDER BY NEWID()";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@amountOfWords", amountOfWords);
            command.Parameters.AddWithValue("@difficulty", difficulty);
            SqlDataReader? reader = command.ExecuteReader();

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
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

    /// <summary>
    /// check if the class that the teacher wants to create already exists
    /// </summary>
    /// <param name="teacherId"></param>
    /// <param name="classname"></param>
    /// <returns>true if the class already exists otherwise false</returns>
    public bool CheckIfClassExists(int teacherId, string classname)
    {
        List<int> classIds = GetClasses(teacherId);
        foreach (int classId in classIds)
        {
            if (GetClassName(classId).Equals(classname))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// get all classes linked to the teacherId
    /// </summary>
    /// <param name="teacherId"></param>
    /// <returns>List with all classId's linked to the given teacher</returns>
    public List<int> GetClasses(int teacherId)
    {
        List<int> classes = new List<int>();
        using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            string sql = "SELECT ClassID FROM Classes WHERE TeacherID = (@teacherId)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@teacherId", teacherId);
            SqlDataReader? reader = command.ExecuteReader();

            while (reader.Read())
            {
                classes.Add(reader.GetInt32(0));
            }

            connection.Close();
        }

        return classes;
    }

    /// <summary>
    /// Get the name of a class based ont their classId
    /// </summary>
    /// <param name="classId"></param>
    /// <returns>The class name</returns>
    public string GetClassName(int classId)
    {
        string className = "";
        using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            string sql = "SELECT ClassName FROM Classes WHERE ClassID = (@classId)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@classId", classId);
            SqlDataReader? reader = command.ExecuteReader();

            while (reader.Read())
            {
                className = reader.GetString(0);
            }

            connection.Close();
        }

        return className;
    }

    /// <summary>
    /// Get all students linked to the classId
    /// </summary>
    /// <param name="classId"></param>
    /// <returns>Returns all students from the given class</returns>
    public List<int> GetStudents(int classId)
    {
        List<int> studentIds = new List<int>();
        using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            string sql = "SELECT PupilID FROM Pupil WHERE ClassID = (@classId)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@classId", classId);
            SqlDataReader? reader = command.ExecuteReader();

            while (reader.Read())
            {
                studentIds.Add(reader.GetInt32(0));
            }

            connection.Close();
        }

        return studentIds;
    }

    /// <summary>
    /// Returns the name of the student based on the studentId
    /// </summary>
    /// <param name="studentID"></param>
    /// <returns>an array with in first position their firstname and in second position their lastname</returns>
    public string[] GetStudentName(int studentID)
    {
        string[] Pupil = new string[2];
        using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            string sql = "SELECT Firstname,LastName FROM Pupil WHERE PupilID = (@studentID)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@studentID", studentID);
            SqlDataReader? reader = command.ExecuteReader();

            while (reader.Read())
            {
                Pupil[0] = reader[0].ToString();
                Pupil[1] = reader[1].ToString();
            }

            connection.Close();
        }

        return Pupil;
    }

    /// <summary>
    /// Make new class and return the auto incremented class id using ExecuteScalar and output.insereted.classid
    /// </summary>
    /// <param name="TeacherId"></param>
    /// <param name="ClassName"></param>
    /// <returns>the class id of the newly added class</returns>
    public int AddNewClass(int TeacherId, string ClassName)
    {
        object newClassId;
        using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            string sqlInsert = "INSERT INTO Classes (TeacherID, ClassName)" + "output inserted.ClassID " +
                               "VALUES (@TeacherId, @ClassName)";
            SqlCommand commandInsert = new SqlCommand(sqlInsert, connection);
            commandInsert.Parameters.AddWithValue("@TeacherId", TeacherId);
            commandInsert.Parameters.AddWithValue("@ClassName", ClassName);
            newClassId = commandInsert.ExecuteScalar();
            connection.Close();
        }

        return (int)newClassId;
    }

    /// <summary>
    /// Update CLassName
    /// </summary>
    /// <param name="ClassId"></param>
    /// <param name="NewClassName"></param>
    public void UpdateClassName(int ClassId, string NewClassName)
    {
        using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            string sqlInsert = "UPDATE Classes SET ClassName = (@NewClassName) WHERE ClassID = (@ClassId)";
            SqlCommand commandInsert = new SqlCommand(sqlInsert, connection);
            commandInsert.Parameters.AddWithValue("@ClassId", ClassId);
            commandInsert.Parameters.AddWithValue("@NewClassName", NewClassName);
            commandInsert.ExecuteReader();
            connection.Close();
        }
    }

    /// <summary>
    /// Add an array filled with studentnames to the database with the right class
    /// </summary>
    /// <param name="student"></param>
    /// <param name="classId"></param>
    /// <returns>The student id and unhashed password</returns>
    public string[] AddStudent(string[] student, int classId)
    {
        string UnhashedPassword = GetRandomPassword(5);
        string HashedPassword = HashPassword(UnhashedPassword);
        object newPupilId;
        string Username = student[0] + '_' + student[1];
        Username.Replace(' ', '_');
        string UsernameSave = Username;
        int number = 1;
        while (CheckIfUserNameExists(UsernameSave))
        {
            UsernameSave = Username;
            UsernameSave += number;
            number++;
        }

        Username = UsernameSave;
        using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            string sqlInsert = "INSERT INTO Pupil (Firstname, Lastname, ClassID, Username, Password)" +
                               "output inserted.PupilID " +
                               "VALUES ((@FirstName), (@LastName), (@classId),(@UserName),(@HashedPassword))";
            SqlCommand commandInsert = new SqlCommand(sqlInsert, connection);
            commandInsert.Parameters.AddWithValue("@FirstName", student[0]);
            commandInsert.Parameters.AddWithValue("@LastName", student[1]);
            commandInsert.Parameters.AddWithValue("@classId", classId);
            commandInsert.Parameters.AddWithValue("@UserName", Username);
            commandInsert.Parameters.AddWithValue("@HashedPassword", HashedPassword);
            newPupilId = commandInsert.ExecuteScalar();
            connection.Close();
        }

        return new string[2] { newPupilId.ToString(), UnhashedPassword };
    }
/// <summary>
/// Generate a random generated password
/// </summary>
/// <param name="length"></param>
/// <returns>A new password</returns>
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
/// <summary>
/// Get the student username from the studentId
/// </summary>
/// <param name="studentID"></param>
/// <returns>The student username</returns>
    public string GetPupilUserName(int studentID)
    {
        string Pupil = "";
        using SqlConnection connection = new SqlConnection(DatabaseConnectionString());
        connection.Open();
        string sql = "SELECT Username FROM Pupil WHERE PupilID = (@studentID)";
        SqlCommand command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@studentID", studentID);
        SqlDataReader? reader = command.ExecuteReader();

        while (reader.Read())
        {
            Pupil = reader[0].ToString();
        }

        connection.Close();

        return Pupil;
    }
    /// <summary>
    /// Checks if the autogenerated username already exists
    /// </summary>
    /// <param name="username"></param>
    /// <returns>true if the username already exists and false if it doesnt</returns>
    public bool CheckIfUserNameExists(string username)
    {
        string? pupil = null;
        using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            string sql = "SELECT Username FROM Pupil WHERE Username = (@Username)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Username", username);
            SqlDataReader? reader = command.ExecuteReader();

            while (reader.Read())
            {
                pupil = reader[0].ToString();
            }

            connection.Close();
        }

        if (pupil == null)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Method that updates data in the Pupilstatistics table
    /// </summary>
    /// <param name="pupilId"></param>
    /// <param name="type"></param>
    /// <param name="amountCorrect"></param>
    /// <param name="amountFalse"></param>
    /// <param name="keyPerSec"></param>
    /// <param name="score"></param>
    public void UpdatePupilStatistics(int pupilId, TypeExercise type, int amountCorrect, int amountFalse,
        double keyPerSec, int score)
    {
        using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            string sql =
                "UPDATE PupilStatistics SET AmountFalse = AmountFalse + @amountFalse, AmountCorr = AmountCorr + @amountCorrect, KeyPerSec = ((KeyPerSec * AssignmentsMade) + @keyPerSec)/(AssignmentsMade+1), AssignmentsMade = AssignmentsMade+1, Score= Score + @score WHERE PupilID = @pupilId AND Type = @type; UPDATE PupilStatistics SET Score = 0 WHERE Score < 0";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@pupilId", pupilId);
            command.Parameters.AddWithValue("@type", type.ToString());
            command.Parameters.AddWithValue("@amountFalse", amountFalse);
            command.Parameters.AddWithValue("@amountCorrect", amountCorrect);
            command.Parameters.AddWithValue("@keyPerSec", keyPerSec);
            if (score > 0)
            {
                command.Parameters.AddWithValue("@score", score);
            }
            else
            {
                command.Parameters.AddWithValue("@score", score);
            }

            command.ExecuteNonQuery();
            connection.Close();
        }
    }


    /// <summary>
    /// This method is used to get stories from the database
    /// </summary>
    /// <returns>a story</returns>
    public string GetStory()
    {
        string StoryString = "";
        using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            string sql = "SELECT TOP 1 Verhaaltje FROM Verhaal ORDER BY NEWID()";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader? reader = command.ExecuteReader();

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    StoryString = reader[i].ToString();
                }
            }

            connection.Close();
        }

        return StoryString;
    }

    /// <summary>
    ///     Gets the ClassID of the class of the current user.
    ///     Used to get all the pupils in the class of the current user.
    /// </summary>
    /// <param name="userid"></param>
    /// <returns></returns>
    public int GetClassId(string userid)
    {
        int classid = new int();
        List<string> userids = new List<string>();
        using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            string sql;
            sql = $"SELECT ClassID from Pupil WHERE PupilID = {userid}";

            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader? reader = command.ExecuteReader();

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
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
    ///     Gets all PupilID's from the Class with the given ClassID
    ///     Used to generate leaderboard.
    /// </summary>
    /// <param name="classid"></param>
    /// <returns>List with all PupilIds</returns>
    public List<int> GetClass(int classid)
    {
        List<int> userids = new List<int>();
        using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            string sql;
            sql = "SELECT PupilID FROM Pupil WHERE ClassID = (@classid)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@classid", classid);
            SqlDataReader? reader = command.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    userids.Add(int.Parse(reader[i].ToString()));
                }
            }

            connection.Close();
        }

        return userids;
    }

    /// <summary>
    ///     Generates the leaderboard.
    ///     Gets the PupilID, Firstname, Lastname and the SUM of the score of every user.
    ///     Adds every user in a list of leaderboard.
    /// </summary>
    /// <param name="userids"></param>
    /// <param name="classid"></param>
    /// <returns></returns>
    public List<List<string>> GenerateLeaderboard(List<int> userids, int classid)
    {
        List<List<string>> leaderboard = new List<List<string>>();
        using (SqlConnection connection =
               new SqlConnection(DatabaseConnectionString()))
        {
            foreach (int userid in userids)
            {
                string sql = "SELECT Pupil.PupilID, Pupil.Firstname, Pupil.Lastname, SUM(PupilStats.Score) " +
                             "FROM PupilStatistics PupilStats JOIN Pupil Pupil ON Pupil.PupilID = PupilStats.PupilID " +
                             $"WHERE Pupil.ClassID = {classid} AND Pupil.PupilID = {userid} " +
                             "GROUP BY Pupil.PupilID, Pupil.Firstname, Pupil.Lastname " +
                             "ORDER BY SUM(PupilStats.Score) DESC;";
                SqlCommand command = new SqlCommand(sql, connection);

                connection.Open();
                SqlDataReader? reader = command.ExecuteReader();
                while (reader.Read())
                {
                    leaderboard.Add(new List<string>
                        { reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString() });
                }

                connection.Close();
            }
        }

        return leaderboard;
    }

    /// <summary>
    /// get all the needed statistics from all types of exercises
    /// </summary>
    /// <param name="type"></param>
    /// <param name="userid"></param>
    /// <returns></returns>
    public List<string> GetStatistics(int type, string userid)
    {
        List<string> statistics = new List<string>();
        using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            string typeString;
            if (type == 0)
            {
                typeString = "Letter";
            }
            else if (type == 1)
            {
                typeString = "Word";
            }
            else if (type == 2)
            {
                typeString = "Story";
            }
            else
            {
                return null;
            }

            string sql =
                $"SELECT PupilID, Type, AmountCorr, AmountFalse, AssignmentsMade, ROUND(KeyPerSec, 1), Score FROM PupilStatistics WHERE PupilID = {userid} AND type = '{typeString}'";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader? reader = command.ExecuteReader();

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    statistics.Add(reader[i].ToString());
                }
            }

            connection.Close();
        }

        return statistics;
    }

    /// <summary>
    /// get password from the database
    /// </summary>
    /// <param name="isTeacher"></param>
    /// <param name="loginKey"></param>
    /// <returns>returns a string array with the password from the database and the user id</returns>
    public string[]? GetPasswordWithId(bool? isTeacher, string? loginKey)
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
                command = new SqlCommand("SELECT Password, TeacherId FROM Teacher WHERE Email = (@loginKey)",
                    connection);
            }
            else
            {
                command = new SqlCommand("SELECT Password, PupilId FROM Pupil WHERE Username = (@loginKey)",
                    connection);
            }

            command.Parameters.AddWithValue("@LoginKey", loginKey);
            SqlDataReader? reader = command.ExecuteReader();
            //Debug.WriteLine(reader[0].ToString());

            string[] passwordWithId = new string[2];
            while (reader.Read())
            {
                passwordWithId[0] = reader[0].ToString();
                passwordWithId[1] = reader[1].ToString();
                Debug.WriteLine(passwordWithId);
            }

            connection.Close();

            return passwordWithId;
        }
    }

    /// <summary>
    /// hash incoming string and return the hashed value
    /// </summary>
    /// <param name="password"></param>
    /// <returns>The inserted password hashed</returns>
    public string HashPassword(string password)
    {
        byte[] data = Encoding.ASCII.GetBytes(password);
        data = new SHA256Managed().ComputeHash(data);
        string hash = Encoding.ASCII.GetString(data);
        return hash;
    }
    /// <summary>
    /// check if a server connection has been astablished
    /// </summary>
    public async Task<bool> IsServerConnected()
    {
        await using SqlConnection connection = new SqlConnection(DatabaseConnectionString());

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

    /// <summary>
    /// calculate the diffulty for each word
    /// </summary>
    /// <param name="word"></param>
    /// <returns>The difficulty fit for the given word</returns>
    public Difficulty GetWordDifficulty(string word)
    {
        const int maxPoints = 45;
        int TotalPoints = 0;
        foreach (char letter in word)
        {
            TotalPoints += AlphabetWithPoints[letter];
        }

        switch (TotalPoints)
        {
            case <= maxPoints / 5:
                return Difficulty.Level1;
            case <= maxPoints / 5 * 2:
                return Difficulty.Level2;
            case <= maxPoints / 5 * 3:
                return Difficulty.Level3;
            case <= maxPoints / 5 * 4:
                return Difficulty.Level4;
            default:
                return Difficulty.Level5;
        }
    }

    //LEGACY CODE 
    //FOR SETTING THE WORD DIFFICULTY IN THE ENTIRE DATABASE
    //public void AddDifficultyToDatabase()
    //{
    //    List<string> wordList = new List<string>();
    //    using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
    //    {
    //        connection.Open();
    //        string sql = "SELECT Words FROM Words ORDER BY NEWID()";
    //        SqlCommand command = new SqlCommand(sql, connection);
    //        SqlDataReader? reader = command.ExecuteReader();

    //        while (reader.Read())
    //        {
    //            for (int i = 0; i < reader.FieldCount; i++)
    //            {
    //                wordList.Add(reader[i].ToString());
    //            }
    //        }

    //        connection.Close();
    //    }

    //    using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
    //    {
    //        foreach (string word in wordList)
    //        {
    //            connection.Open();
    //            string sql = "UPDATE Words SET Difficulty = @difficulty WHERE Words = @word";
    //            SqlCommand command = new SqlCommand(sql, connection);
    //            command.Parameters.AddWithValue("@difficulty", GetWordDifficulty(word));
    //            command.Parameters.AddWithValue("@word", word);
    //            command.ExecuteReader();
    //            command.Dispose();
    //            connection.Close();
    //        }
    //    }
    //}

    private int GetScore(int pupilId, TypeExercise type)
    {
        int score = 0;
        using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            string sql = "SELECT Score FROM PupilStatistics WHERE PupilId = @pupilId AND Type = @type";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@pupilId", pupilId);
            command.Parameters.AddWithValue("@type", type.ToString());
            SqlDataReader? reader = command.ExecuteReader();
            while (reader.Read())
            {
                score = Convert.ToInt32(reader[0]);
            }

            connection.Close();
        }

        return score;
    }

    public Difficulty GetLevel(int pupilID, TypeExercise typeExercise)
    {
        const int minSize = 20;
        const int maxscore = 100;
        int score = GetScore(pupilID, typeExercise);
        switch (score)
        {
            case <= maxscore / 5:
                SetSizeExercise(1, maxscore, score, minSize);
                return Difficulty.Level1;
            case <= maxscore / 5 * 2:
                SetSizeExercise(2, maxscore, score, minSize);
                return Difficulty.Level2;
            case <= maxscore / 5 * 3:
                SetSizeExercise(3, maxscore, score, minSize);
                return Difficulty.Level3;
            case <= maxscore / 5 * 4:
                SetSizeExercise(4, maxscore, score, minSize);
                return Difficulty.Level4;
            default:
                SetSizeExercise(5, maxscore, score, minSize);
                return Difficulty.Level5;
        }
    }

    private void SetSizeExercise(int sizeScore, int maxscore, int score, int minSize)
    {
        int maxsize = 30;

        if (score > maxscore)
        {
            score = maxscore;
        }

        SizeExercise = (maxscore / 5 * sizeScore - score) * minSize;
        if (SizeExercise <= 0)
        {
            SizeExercise = minSize;
        }

        if (SizeExercise > maxsize)
        {
            SizeExercise = maxsize;
        }
    }

    public void CheckIfPupilStatisticsExist(int pupilId)
    {
        bool exists = false;
        using SqlConnection connection = new SqlConnection(DatabaseConnectionString());
        foreach (object? VARIABLE in Enum.GetValues(typeof(TypeExercise)))
        {
            connection.Open();
            string sql = "SELECT * FROM PupilStatistics WHERE PupilId = @pupilId AND Type = @type";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@pupilId", pupilId);
            command.Parameters.AddWithValue("@type", VARIABLE.ToString());
            SqlDataReader? reader = command.ExecuteReader();
            while (reader.Read())
            {
                exists = true;
            }

            command.Dispose();
            connection.Close();

            if (!exists)
            {
                connection.Open();
                string sqlInsert =
                    "INSERT INTO PupilStatistics (PupilId, Type, Score,AmountCorr, AmountFalse, AssignmentsMade, KeyPerSec ) VALUES (@pupilId, @type, 1,0,0,0,0)";
                SqlCommand commandInsert = new SqlCommand(sqlInsert, connection);
                commandInsert.Parameters.AddWithValue("@pupilId", pupilId);
                commandInsert.Parameters.AddWithValue("@type", VARIABLE.ToString());
                commandInsert.ExecuteReader();
                connection.Close();
            }

            exists = false;
        }
    }

    public void DeleteStudent(int pupilId, int classId)
    {
        using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
        {
            connection.Open();
            string sqlDelete = "DELETE FROM Pupil WHERE PupilID = (@pupilId) AND ClassID = (@classId)";
            SqlCommand commandInsert = new SqlCommand(sqlDelete, connection);
            commandInsert.Parameters.AddWithValue("@pupilId", pupilId);
            commandInsert.Parameters.AddWithValue("@classId", classId);
            commandInsert.ExecuteReader();
            connection.Close();
        }
    }

    public List<Pupil> GenerateClassStatistics(List<int> userids, int classid)
    {
        List<Pupil> classtatistics = new List<Pupil>();
        using (SqlConnection connection = new SqlConnection(DatabaseConnectionString()))
        {
            foreach (int userid in userids)
            {
                string sql =
                    "SELECT Pupil.PupilID, Pupil.Firstname, Pupil.Lastname, SUM(PupilStats.Score), SUM(PupilStats.AssignmentsMade)" +
                    "FROM PupilStatistics PupilStats JOIN Pupil Pupil ON Pupil.PupilID = PupilStats.PupilID " +
                    $"WHERE Pupil.ClassID = {classid} AND Pupil.PupilID = {userid} " +
                    "GROUP BY Pupil.PupilID, Pupil.Firstname, Pupil.Lastname;";
                SqlCommand command = new SqlCommand(sql, connection);

                connection.Open();
                SqlDataReader? reader = command.ExecuteReader();
                while (reader.Read())
                {
                    classtatistics.Add(new Pupil(int.Parse(reader[0].ToString()), reader[1].ToString(),
                        reader[2].ToString(), int.Parse(reader[3].ToString()), int.Parse(reader[4].ToString())));
                }

                connection.Close();
            }
        }

        return classtatistics;
    }
}