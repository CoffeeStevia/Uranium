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
using Uranium.Classes;

namespace Uranium.Controls
{
    /// <summary>
    /// Interaction logic for TabControl.xaml
    /// </summary>
    public partial class TabControl : UserControl
    {
        public string script = "-- skibidi";
        public bool SelectedTab;
        public StackPanel Parent { get; set; }
        public MainWindow MainInstance { get; set; }
        public TabControl(StackPanel Parent, MainWindow Instance)
        {
            InitializeComponent();
            this.Parent = Parent;
            this.MainInstance = Instance;
        }
        public async void Select()
        {
            this.SelectedTab = true;
            if (Common.SelectedTab != null)
            {
                if (Common.SelectedTab != this)
                {
                    Common.SelectedTab.TabLabel.Visibility = Visibility.Visible;
                }
                Common.SelectedTab.script = await this.MainInstance.GetText();
                if (Common.SelectedTab == null)
                {
                    Common.SelectedTab = this;
                }
                else
                {
                    Common.SelectedTab.Deselect();
                }
            }
            await this.MainInstance.SetText(this.script);
            Common.SelectedTab = this;
            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0.5, 0),
                EndPoint = new Point(0.5, 1),
                GradientStops = new GradientStopCollection
            {
                new GradientStop((Color)ColorConverter.ConvertFromString("#FF06080D"), 0),
                new GradientStop((Color)ColorConverter.ConvertFromString("#FF1F2B55"), 1)
            }
            };
            MainBorder.Background = gradientBrush;
            MainBorder.BorderThickness = new Thickness(1);
        }
        private async void buttonclose_Click(object sender, RoutedEventArgs e)
        {
            if (Common.SelectedTab == this)
                this.script = await this.MainInstance.GetText();
            if (e != null && Common.SelectedTab == this)
            {
                int num = Parent.Children.IndexOf((UIElement)this);
                if (Parent.Children.Count > 2)
                {
                    if (num > 0)
                    {
                        ((TabControl)Parent.Children[num - 1]).Select();
                    }
                    else
                    {
                        if (Parent.Children.Count - num > 2)
                        {
                            ((TabControl)Parent.Children[num + 1]).Select();
                        }
                    }
                }
            }
            this.MainInstance.TabPanel.Children.Remove((UIElement)this);
            if (this.MainInstance.TabPanel.Children.Count == 0)
            {
                this.MainInstance.IsVisible = true;
            }
            else
            {
                return;
            }
        }

        public void Deselect()
        {
            this.SelectedTab = false;
            Common.SelectedTab = (TabControl)null;
            Common.PreviousTab = this;
            this.MainBorder.Background = new SolidColorBrush(Color.FromRgb(27, 33, 52));
            MainBorder.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void MainBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Select();
        }
    }
}
