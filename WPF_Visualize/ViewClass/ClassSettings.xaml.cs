using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPF_Visualize.ViewLogic;
using WPF_Visualize.Views_Navigate;
namespace WPF_Visualize.ViewClass;

/// <summary>
///     Interaction logic for ClassSettings.xaml
/// </summary>
public partial class ClassSettings : UserControl
{
    private readonly int _classId;
    private readonly bool _isNewClass;
    private readonly TeacherController _teacherController = new TeacherController();
    private readonly int _userId = LoginController.GetUserId();
    private readonly Database _database = new Database();


    //2 constructors, depending on where teacher came from, when class is new the first constructor is used, when class already existed 2 constructor is used.
    public ClassSettings()
    {
        InitializeComponent();
        _isNewClass = true;
        _teacherController.TeacherEvent += TeacherController_TeacherEvent;
        AddStudentButton.IsEnabled = false;
        ConfirmButton.IsEnabled = false;
    }

    public ClassSettings(int classId)
    {
        InitializeComponent();
        _teacherController.TeacherEvent += TeacherController_TeacherEvent;
        _classId = classId;
        _teacherController.FillListWithStudents(_classId);
        AddCurrentsStudentsToStackPanelAndComboBox(_classId);
        SetLabelsAndTextBoxes(_classId);
        _isNewClass = false;
        AddStudentButton.IsEnabled = false;
    }

    /// <summary>
    ///     Adds the students that are already in the class to the stackpanel, used when class already existed
    /// </summary>
    /// <param name="classId"></param>
    private void AddCurrentsStudentsToStackPanelAndComboBox(int classId)
    {


        List<int> pupils = _database.GetStudents(classId);


        foreach (int pupilId in pupils)
        {
            //Check if pupilID is also in teacherController.ClassStudentsDeleted
            if (_teacherController.ClassStudentsDeleted.Contains(pupilId))
            {
                continue;
            }

            string[] pupil = _database.GetStudentName(pupilId);
            var label = new Label
            {
                Content = pupil[0] + " " + pupil[1],
                Foreground = Brushes.White,
                FontSize = 25
            };

            var comboBoxItem = new ComboBoxItem
            {
                Content = $"{pupil[0]} {pupil[1]}",
                Name = $"_{pupilId}"
            };



            StudentListPanel.Children.Add(label);
            ComboBoxStudents.Items.Add(comboBoxItem);

        }
    }

    /// <summary>
    ///     Add the students that are newly added to the stackpanel, used when it's a new class / or when there are new
    ///     students
    /// </summary>
    /// <param name="pupils"></param>
    private void AddCurrentsStudentsToStackPanelAndComboBox(List<string[]> pupils)
    {
        {
            foreach (string[] pupil in pupils)
            {

                Label label = new Label
                {
                    Content = pupil[0] + " " + pupil[1],
                    Foreground = Brushes.White,
                    FontSize = 25
                };

                ComboBoxItem comboBoxItem = new ComboBoxItem
                {
                    Content = $"{pupil[0]} {pupil[1]}"
                };

                StudentListPanel.Children.Add(label);
                ComboBoxStudents.Items.Add(comboBoxItem);
            }


        }
    }

    /// <summary>
    ///     Sets the content of the classname label to the current classname.
    /// </summary>
    /// <param name="classId"></param>
    private void SetLabelsAndTextBoxes(int classId)
    {
        tbClassName.Text = _database.GetClassName(classId);
    }

    /// <summary>
    ///     Back button, 2 possibilty's depending on if it's  a new class or already existed class. Choice 1 = new class
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnBack(object sender, RoutedEventArgs e)
    {
        if (_isNewClass)
        {
            UserControlController.MainWindowChange(this, new ClassSelect());
        }
        else
        {
            UserControlController.MainWindowChange(this, new TeacherMain(_classId));
        }
    }

    /// <summary>
    /// Confirm button, 2 possibilty's depending on if it's  a new class or already existed class.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnConfirm(object sender, RoutedEventArgs e)
    {
        bool informationCorrect;
        if (_isNewClass)
        {
            informationCorrect = _teacherController.AddNewClassToDatabase(_userId, tbClassName.Text);
            _teacherController.SwitchScreen(_classId, informationCorrect, true);
        }
        else
        {
            bool classNameChanged;
            if (_database.GetClassName(_classId) == tbClassName.Text)
            {
                informationCorrect = true;
            }
            else
            {
                informationCorrect = !_database.CheckIfClassExists(_userId, tbClassName.Text);
            }

            if (informationCorrect)
            {
                _teacherController.UpdateClassName(_classId, tbClassName.Text);
                bool newStudentsAdded = _teacherController.AddStudentsToDatabase(_classId);
                _teacherController.DeleteStudentsFromDatabase(_classId);
                _teacherController.SwitchScreen(_classId, true, newStudentsAdded);
            }
            else
            {
                _teacherController.SwitchScreen(_classId, false, false);
            }


        }


    }


    /// <summary>
    ///     When new student is added, it adds it to the student list to the right of the screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnStudentAdd(object sender, RoutedEventArgs e)
    {
        _teacherController.AddPupilToClass(tbFirstName.Text, tbLastName.Text);
        tbFirstName.Text = "";
        tbLastName.Text = "";
        StudentListPanel.Children.Clear();
        ComboBoxStudents.Items.Clear();
        AddCurrentsStudentsToStackPanelAndComboBox(_teacherController.ClassStudents);
    }

    private void OnStudentRemove(object sender, RoutedEventArgs e)
    {
        if (ComboBoxStudents.SelectedItem != null)
        {
            string pupilid = ComboBoxStudents.SelectedValue.ToString().Replace("_", "");

            if (!string.IsNullOrEmpty(pupilid))
            {
                _teacherController.RemovePupilFromClass(Int32.Parse(pupilid));
            }
            else
            {
                string pupilName = ComboBoxStudents.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
                _teacherController.RemovePupilFromClass(pupilName);
            }

            StudentListPanel.Children.Clear();
            ComboBoxStudents.Items.Clear();
            //AddCurrentsStudentsToStackPanelAndComboBox(teacherController.ClassStudents);
            if (_isNewClass)
            {
                AddCurrentsStudentsToStackPanelAndComboBox(_teacherController.ClassStudents);
            }
            else
            {
                AddCurrentsStudentsToStackPanelAndComboBox(_classId);
            }

        }
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
            UserControlController.MainWindowChange(this, new TeacherMain(e.ClassId));
        }
        else if (!e.InformationIsCorrect)
        {
            ErrorText.Visibility = Visibility.Visible;
        }

        if (e.NewStudentsAdded)
        {
            MessageBox.Show("De klas is toegevoegd en er staat een .pdf bestand in de download map op uw computer");
        }
    }


}
