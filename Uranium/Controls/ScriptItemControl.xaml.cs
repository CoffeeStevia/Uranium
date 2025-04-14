using System;
using System.Collections.Generic;
using System.IO;
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

namespace Uranium.Controls
{
    /// <summary>
    /// Interaction logic for ScriptItemControl.xaml
    /// </summary>
    public partial class ScriptItemControl : UserControl
    {
        public string MainName { get; set; }
        public string FullPath { get; set; }
        public MainWindow MainInstance { get; set; }
        public ScriptItemControl()
        {
            InitializeComponent();
            foreach (Window mainwindow in Application.Current.Windows)
            {
                if (mainwindow.GetType() == typeof(MainWindow))
                {
                    this.MainInstance = (MainWindow)mainwindow;
                }
            }
        }
        private void OnClickMain(object sender, MouseButtonEventArgs e)
        {
            using (StreamReader NormalReader = File.OpenText(this.FullPath))
            {
                string s = File.ReadAllText(this.FullPath);
                {
                    foreach (Window mainwin in Application.Current.Windows)
                    {
                        if (mainwin.GetType() == typeof(MainWindow))
                        {
                            MainInstance.AddTab(this.MainName, s);
                        }
                    }
                }
            }
        }
        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
        }
    }
}
