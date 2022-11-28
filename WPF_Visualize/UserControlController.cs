using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPF_Visualize
{
    public static class UserControlController
    {
        public static event EventHandler<OnWindowChangeEventArgs> OnWindowChange;
        public static UserControl Content;

        static UserControlController()
        {
            Content = new StudentMain();
            OnWindowChange = new EventHandler<OnWindowChangeEventArgs>((sender, args) => { Content = args.Content; });
        }

        public static void InvokeEvent(UserControl Sender, UserControl content)
        {
            OnWindowChange?.Invoke(Sender, new OnWindowChangeEventArgs(content));
        }
    }
}
