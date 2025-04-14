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

namespace Uranium.Controls
{
    /// <summary>
    /// Interaction logic for FolderItemControl.xaml
    /// </summary>
    public partial class FolderItemControl : UserControl
    {
        public bool HasSubs = false;
        public int FolderStackHeight;
        public List<UIElement> allItems = new List<UIElement>();
        public List<UIElement> SubItems = new List<UIElement>();
        public List<UIElement> ParentItems = new List<UIElement>();
        public List<UIElement> SubItemsChecked = new List<UIElement>();
        public int SubNum { get; set; }
        public bool IsParDown;
        public bool IsDown;
        public bool IsSub = false;
        public StackPanel ParentPanelMian;
        public StackPanel PasrentPanelSub;
        public FolderItemControl ParentFolderSub;
        public string BaseDir { get; set; }
        public bool IsCalculated = false;
        public double BaseHeight = 0;
        public double ThisHeight = 0;
        public FolderItemControl()
        {
            InitializeComponent();
        }
        private void BaseCalculation()
        {
            BaseHeight = FOlderStack.Height;
            IsCalculated = true;

        }

        private void CheckSubFolderDownStatus(FolderItemControl FicCheckStatus)
        {
            foreach (UIElement Checks in FicCheckStatus.SubItems)
            {
                if (Checks is FolderItemControl lim && (Checks as FolderItemControl).IsDown == true)
                {
                    SubItemsChecked.Add(Checks);
                    CheckSubFolderDownStatus(lim);
                }
            }
        }

        private void BringDownParents(FolderItemControl ItemparentsToBringDown)
        {
            if (ItemparentsToBringDown.ParentFolderSub != null && ItemparentsToBringDown.ParentFolderSub is FolderItemControl ficparbru)
            {
                (ficparbru as FolderItemControl).FOlderStack.Height = (ficparbru as FolderItemControl).FOlderStack.Height + 26;
                BringDownParents(ficparbru);
            }
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (FOlderStack.Height == 0 && FOlderStack.Children.Count > 0)
            {
                if (IsCalculated)
                {

                    ParentPanelMian.Height = ParentPanelMian.Height + BaseHeight;
                    FOlderStack.Height = BaseHeight;
                    foreach (UIElement fim in ParentItems)
                    {
                        if (fim is FolderItemControl licm)
                        {
                            licm.FOlderStack.Height = licm.FOlderStack.Height + BaseHeight;
                        }
                    }
                }
                else
                {
                    CheckSubFolderDownStatus(this);
                    IsDown = true;
                    foreach (object si in FOlderStack.Children)
                    {
                        ParentPanelMian.Height = ParentPanelMian.Height + 39;
                        FOlderStack.Height = FOlderStack.Height + 39;
                        if (IsSub == true)
                        {
                            BringDownParents(this);
                        }
                        else
                        {
                        }
                    }
                }
            }
            else
            {
                BaseCalculation();
                FOlderStack.Height = 0;
                foreach (UIElement fim in ParentItems)
                {
                    if (fim is FolderItemControl licm)
                    {
                        licm.FOlderStack.Height = licm.FOlderStack.Height - BaseHeight;
                    }
                }
                ParentPanelMian.Height = ParentPanelMian.Height - BaseHeight;
            }
        }
        private void MainBorder_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
