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
using static System.Net.Mime.MediaTypeNames;

namespace Uranium.Controls
{
    /// <summary>
    /// Interaction logic for PopularScript.xaml
    /// </summary>
    public partial class PopularScript : UserControl
    {
        string realscript;
        private string imageUrl;
        public MainWindow MainInstance { get; set; }
        public PopularScript(string imgurl, string scriptname, string script)
        {
            InitializeComponent();
            imageUrl = imgurl;
            this.ScriptNameLabel.Content = scriptname;
            realscript = script;
            foreach (Window mainwindow in System.Windows.Application.Current.Windows)
            {
                if (mainwindow.GetType() == typeof(MainWindow))
                {
                    this.MainInstance = (MainWindow)mainwindow;
                }
            }
        }
        internal static ImageSource ToImage(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                // Handle invalid URI case
                throw new ArgumentException("The provided URL is not valid.");
            }

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(url);
            image.EndInit();
            return (ImageSource)image;
        }
        private void AddImageToGrid()
        {
            if (imageUrl.EndsWith(".webp"))
            {
                this.BackImage.ImageSource = ToImage("https://tr.rbxcdn.com/3e86507fbb9beb6431c5747e5596b06d/768/432/Image/Png");
            }
            else
            {
                this.BackImage.ImageSource = ToImage(imageUrl);
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            AddImageToGrid();
        }

        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            
        }
    }
}
