using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPF_Visualize
{
    internal class OnWindowChange : EventArgs
    {
        public UserControl Content { get; set; }
        public OnWindowChange(UserControl content)
        {
            Content = content;
        }
    }
}
