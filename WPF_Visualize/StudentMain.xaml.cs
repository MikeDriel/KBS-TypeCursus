﻿using System;
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
using System.Windows.Shapes;

namespace WPF_Visualize
{
    /// <summary>
    /// Interaction logic for StudentMain.xaml
    /// </summary>
    public partial class StudentMain : UserControl
    {
        public StudentMain()
        {
            InitializeComponent();
        }
        
        private void OnExerciseSelect(object sender, RoutedEventArgs e)
        {
            UserControlEvent.InvokeEvent(this, new ExerciseSelect());
        }
    }
}
