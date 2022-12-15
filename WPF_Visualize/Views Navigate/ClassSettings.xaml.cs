using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Visualize.ViewLogic;

namespace WPF_Visualize.Views_Navigate
{
    /// <summary>
    /// Interaction logic for ClassSettings.xaml
    /// </summary>
    /// 

    public partial class ClassSettings : UserControl
    {
        public Database Database = new();
        private int classId;
        private int choice;
        int user_id = LoginController.GetUserId();
        TeacherController teacherController = new();

        public ClassSettings()
        {
            InitializeComponent();
            choice = 1;
        }
        public ClassSettings(int classId)
        {
            InitializeComponent();
            this.classId = classId;
            teacherController.FillListWithStudents(this.classId);
            AddCurrentsStudentsToStackPanel(this.classId);
            SetLabelsAndTextBoxes(this.classId);
            choice = 2;
        }

        private void AddCurrentsStudentsToStackPanel(int classId)
        {
            {
                List<int> pupils = Database.GetStudents(classId);
                foreach (int pupilId in pupils)
                {
                    string[] pupil = Database.GetStudentName(pupilId);
                    var label = new Label
                    {
                        Content = pupil[0] + " " + pupil[1],
                        Foreground = Brushes.White,
                        FontSize = 25
                    };
                    
                    StudentListPanel.Children.Add(label);
                }
            }
        }

        private void AddCurrentsStudentsToStackPanel(List<string[]> Pupils)
        {
            {
                foreach (string[] pupil in Pupils)
                {
                    var label = new Label
                    {
                        Content = pupil[0] + " " + pupil[1],
                        Foreground = Brushes.White,
                        FontSize = 25
                    };

                    StudentListPanel.Children.Add(label);
                }
            }
        }

        private void SetLabelsAndTextBoxes(int classId)
        {
            tbClassName.Text = Database.GetClassName(classId);
        }

        private void OnBack(object sender, RoutedEventArgs e)
        {
            if (choice == 1)
            {
                UserControlController.MainWindowChange(this, new ClassSelect());
            }
            else
            {
                UserControlController.MainWindowChange(this, new TeacherMain(classId));
            }
        }

        private void OnConfirm(object sender, RoutedEventArgs e)
        {
            if (choice == 1)
            {
                int newClassId = teacherController.MakeNewClass(user_id, tbClassName.Text);
                teacherController.AddStudents(newClassId);
                UserControlController.MainWindowChange(this, new TeacherMain(newClassId)); 
            }
            else
            {
                teacherController.UpdateClassName(classId, tbClassName.Text);
                teacherController.AddStudents(classId);
                UserControlController.MainWindowChange(this, new TeacherMain(classId));
            }
        }

        private void OnStudentAdd(object sender, RoutedEventArgs e)
        {
            teacherController.AddPupilToClass(tbFirstName.Text, tbLastName.Text);
            tbFirstName.Text = "";
            tbLastName.Text = "";
            StudentListPanel.Children.Clear();
            AddCurrentsStudentsToStackPanel(teacherController.ClassStudents);
        }
    }
}
