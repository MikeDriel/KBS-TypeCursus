using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPF_Visualize.ViewLogin;

namespace WPF_Visualize.ViewLogic
{
    // Because MainWindow and the usercontrols are not really accesible from other files we 
    // are using this class UserControlController as a sort of "bridge" between the classes
    // Via this class we can change the content of the mainwindow to any usercontroller
    public static class UserControlController
    {
        public static event EventHandler<OnWindowChangeEventArgs> OnWindowChange;
        public static UserControl Content;

        static UserControlController()
        {
            Content = new LoginChoose();
            // Here we make it so that when the event gets new content it sets the content of this file
            // to that kind of content
            OnWindowChange = new EventHandler<OnWindowChangeEventArgs>((sender, args) => { Content = args.Content; });
        }

        // this is the method we use to invoke the event to start the process of changing the content
        public static void MainWindowChange(UserControl Sender, UserControl content)
        {
            OnWindowChange?.Invoke(Sender, new OnWindowChangeEventArgs(content));
        }
    }


    // This is the event that we use to acces the mainwindow to change the usercontrol
    public class OnWindowChangeEventArgs : EventArgs
    {
        public UserControl Content { get; set; }
        public OnWindowChangeEventArgs(UserControl content)
        {
            Content = content;
        }
    }
}
