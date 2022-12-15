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



        public int MakeNewClass(int user_id, string className)
        {
            {
                int classId = Database.AddNewClass(user_id, className);
                return classId;
            }
        }

        public void UpdateClassName(int classId, string className)
        {
            Database.UpdateClassName(classId, className);
        }

        public void FillListWithStudents(int classId)
        {
            List<int> pupils = Database.GetStudents(classId);
            foreach (int pupilId in pupils)
            {
                string[] pupil = Database.GetStudentName(pupilId);
                ClassStudents.Add(pupil);
            }
        }

        public void AddPupilToClass(string pupilFirstName, string pupilLastName)
        {
            string[] student = new string[2] { pupilFirstName, pupilLastName };
            ClassStudents.Add(student);
            ClassStudentsNewlyAdded.Add(student);
        }

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
