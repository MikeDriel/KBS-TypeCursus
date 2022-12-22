﻿using Controller;
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
        private bool IsNewClass;
        int user_id = LoginController.GetUserId();
        TeacherController teacherController = new();


        //2 constructors, depending on where teacher came from, when class is new the first constructor is used, when class already existed 2 constructor is used.
        public ClassSettings()
        {
            InitializeComponent();
            IsNewClass = true;
            teacherController.TeacherEvent += TeacherController_TeacherEvent;
            AddStudentButton.IsEnabled = false;
            ConfirmButton.IsEnabled = false;
        }

        public ClassSettings(int classId)
        {
            InitializeComponent();
            tbClassName.IsReadOnly = true;
            this.classId = classId;
            teacherController.FillListWithStudents(this.classId);
            AddCurrentsStudentsToStackPanel(this.classId);
            SetLabelsAndTextBoxes(this.classId);
            IsNewClass = false;
            AddStudentButton.IsEnabled = false;
        }

        /// <summary>
        /// Adds the students that are already in the class to the stackpanel, used when class already existed
        /// </summary>
        /// <param name="classId"></param>
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

        /// <summary>
        /// Add the students that are newly added to the stackpanel, used when it's a new class
        /// </summary>
        /// <param name="Pupils"></param>
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

        /// <summary>
        /// Sets the content of the classname label to the current classname.
        /// </summary>
        /// <param name="classId"></param>
        private void SetLabelsAndTextBoxes(int classId)
        {
            tbClassName.Text = Database.GetClassName(classId);
        }

        /// <summary>
        /// Back button, 2 possibilty's depending on if it's  a new class or already existed class. Choice 1 = new class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBack(object sender, RoutedEventArgs e)
        {
            if (IsNewClass)
            {
                UserControlController.MainWindowChange(this, new ClassSelect());
            }
            else
            {
                UserControlController.MainWindowChange(this, new TeacherMain(classId));
            }
        }

        /// <summary>
        /// Confirm button, 2 possibilty's depending on if it's  a new class or already existed class. Choice 1 = new class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// // To do: fix deze troep
        private void OnConfirm(object sender, RoutedEventArgs e)
        {
            if (IsNewClass)
            {
                teacherController.addNewClass(user_id, tbClassName.Text);
            }
            else
            {
                teacherController.AddStudents(classId);
            }
        }

        /// <summary>
        /// When new student is added, it adds it to the student list to the right of the screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStudentAdd(object sender, RoutedEventArgs e)
        {
            teacherController.AddPupilToClass(tbFirstName.Text, tbLastName.Text);
            tbFirstName.Text = "";
            tbLastName.Text = "";
            StudentListPanel.Children.Clear();
            AddCurrentsStudentsToStackPanel(teacherController.ClassStudents);
        }

        private void SetStudentAddButton()
        {
            AddStudentButton.IsEnabled = (tbFirstName.Text != "") && (tbLastName.Text != "");
        }

        private void SetConfirmButton()
        {
            ConfirmButton.IsEnabled = tbClassName.Text != "";
        }

        private void tbFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetStudentAddButton();
        }

        private void tbLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetStudentAddButton();
        }

        private void tbClassName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetConfirmButton();
        }


        private void TeacherController_TeacherEvent(object sender, TeacherEventArgs e)
        {
            if (e.InformationIsCorrect)
            {
                MessageBox.Show("De klas is toegevoegd en er staat een .pdf bestand in de download map op uw computer");
                UserControlController.MainWindowChange(this, new TeacherMain(e.ClassId));
            }
            else if (!e.InformationIsCorrect)
            {
                ErrorText.Visibility = Visibility.Visible;
            }
        }
    }
}
