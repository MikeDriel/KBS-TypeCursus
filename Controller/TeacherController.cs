using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;


namespace Controller
{
    public class TeacherController
    {
        public Database Database;
        public List<string[]> ClassStudents;
        public List<string[]> ClassStudentsNewlyAdded;
        public EventHandler<TeacherEventArgs> TeacherEvent;
        private int _classId;

        public TeacherController()
        {
            ClassStudentsNewlyAdded = new();
            ClassStudents = new();
            Database = new();
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

        public void addNewClass(int user_id, string className)
        {
            List<int> listclasses = Database.GetClasses(user_id);
            if (!Database.CheckIfClassExists(user_id, className))
            {
                int classId = Database.AddNewClass(user_id, className);
                AddStudents(classId);
                TeacherEvent?.Invoke(this, new TeacherEventArgs(true, classId));
            }
            else
            {
                TeacherEvent?.Invoke(this, new TeacherEventArgs(false, -1));
            }
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

            MakePdfWithAddedStudentPasswords(studentsInformation, Database.GetClassName(classId), classId);
        }

        public void MakePdfWithAddedStudentPasswords(Dictionary<int, string> dictionary, string classname, int classId)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            int height = 40;
            int width = 0;
            PdfDocument UserNameAndPasswordList = new();
            PdfPage page = UserNameAndPasswordList.AddPage();
            XGraphics graphics = XGraphics.FromPdfPage(page);
            XFont font = new("Segoe UI Variable", 15, XFontStyle.Bold);
            classname.Replace(' ', '_');
            string filename = $"{classname}_{classId}.pdf";
            foreach (var KeyValue in dictionary)
            {
                string UserName = Database.getPupilUserName(KeyValue.Key);
                string[] studentNameArray = Database.GetStudentName(KeyValue.Key);
                string naam = studentNameArray[0] + " " + studentNameArray[1];
                string Password = KeyValue.Value;
                graphics.DrawString($"Naam: {naam}", font, XBrushes.Black, new XRect(0, height, page.Width, 100),
                    XStringFormats.TopCenter);
                height += 20;
                graphics.DrawString($"gebruikersnaam: {UserName}    Wachtwoord: {Password}", font, XBrushes.Black,
                    new XRect(0, height, page.Width, 100), XStringFormats.TopCenter);
                height += 70;
                if (page.Height < height)
                {
                    page = UserNameAndPasswordList.AddPage();
                    graphics = XGraphics.FromPdfPage(page);
                    height = 40;
                }
            }

            UserNameAndPasswordList.Save(GetDownloadFolderPath() + "/" + filename);
        }

        string GetDownloadFolderPath()
        {
            return Registry
                .GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders",
                    "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();
        }
    }

    //EVENT FOR LIVE STATISTICS UPDATE
    public class TeacherEventArgs : EventArgs
    {
        public bool InformationIsCorrect { get; set; }
        public int ClassId { get; set; }

        public TeacherEventArgs(bool informationIsCorrect, int classId)
        {
            InformationIsCorrect = informationIsCorrect;
            ClassId = classId;
        }
    }
}