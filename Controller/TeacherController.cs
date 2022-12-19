using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class TeacherController
    {
        public Database Database = new();
        public List<string[]> ClassStudents = new();
        public List<string[]> ClassStudentsNewlyAdded = new();


        /// <summary>
        /// Add new class to Database
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public int MakeNewClass(int user_id, string className)
        {
            
                int classId = Database.AddNewClass(user_id, className);
                return classId;
            
        }

        /// <summary>
        ///  Change the classname
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="className"></param>
        public void UpdateClassName(int classId, string className)
        {
            Database.UpdateClassName(classId, className);
        }

        /// <summary>
        /// Gets all students from the database, that are assigned to the class
        /// </summary>
        /// <param name="classId"></param>

        public void FillListWithStudents(int classId)
        {
            List<int> pupils = Database.GetStudents(classId);
            foreach (int pupilId in pupils)
            {
                string[] pupil = Database.GetStudentName(pupilId);
                ClassStudents.Add(pupil);
            }
        }

        /// <summary>
        ///  Adds newly added students to the classstudents list
        /// </summary>
        /// <param name="pupilFirstName"></param>
        /// <param name="pupilLastName"></param>
        public void AddPupilToClass(string pupilFirstName, string pupilLastName)
        {
            string[] student = new string[2] { pupilFirstName, pupilLastName };
            ClassStudents.Add(student);
            ClassStudentsNewlyAdded.Add(student);
        }

        /// <summary>
        /// Add newly added students to the database
        /// </summary>
        /// <param name="classId"></param>

        public void AddStudents(int classId)
        {
            Dictionary<int, string> studentsInformation = new Dictionary<int, string>();
            foreach (string[] student in ClassStudentsNewlyAdded)
            {
                string[] studentInfo = Database.AddStudent(student, classId);
                studentsInformation.Add(Int32.Parse(studentInfo[0]), studentInfo[1]);
            }
        }
    }
}
