using Microsoft.Web.WebView2.Wpf;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Uranium.Controls;

namespace Uranium;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public bool IsVisible = true;
    public WebView2 browser;
    private HttpClient HttpClient = new HttpClient();
    public MainWindow()
    {
        InitializeComponent();
        UserNameLabel.Content = "Espresso";
        LoginLabel.Content = "Last Login: " + DateTime.Now;
        ChangelogsPanel1.Children.Add(new Changelog("Added new feature"));
        NewPanel1.Children.Add(new News("Savage Skidded my weiner"));
        getscripts();
        initbrowser();
        EditorHolder.Children.Add(browser);
        FillTreeView(MainScriptsHolder, MainScriptsHolder, BaseSscriptDirectory);
    }

    #region Window Buttons
    private void CloseClick(object sender, MouseButtonEventArgs e)
    {
        this.Close();
    }
    private void MaxClick(object sender, MouseButtonEventArgs e)
    {
        if (WindowState == WindowState.Normal)
        {
            WindowState = WindowState.Maximized;
            return;
        }
        WindowState = WindowState.Normal;
    }
    private void MinClick(object sender, MouseButtonEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
    private void TopBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        this.DragMove();
    }
    #endregion

    #region Navigation Animations
    void AnimateButtonSwap(Border expandTarget, Border shrinkTarget, Border shrinkTarget2, Border shrinkTarget3, Border shrinkTarget4)
    {
        var expandWidth = new DoubleAnimation(80, TimeSpan.FromMilliseconds(300)) { EasingFunction = new QuadraticEase() };
        var expandHeight = new DoubleAnimation(29, TimeSpan.FromMilliseconds(300)) { EasingFunction = new QuadraticEase() };
        expandTarget.BeginAnimation(FrameworkElement.WidthProperty, expandWidth);
        expandTarget.BeginAnimation(FrameworkElement.HeightProperty, expandHeight);

        var shrinkWidth = new DoubleAnimation(33, TimeSpan.FromMilliseconds(300)) { EasingFunction = new QuadraticEase() };
        var shrinkHeight = new DoubleAnimation(29, TimeSpan.FromMilliseconds(300)) { EasingFunction = new QuadraticEase() };
        shrinkTarget.BeginAnimation(FrameworkElement.WidthProperty, shrinkWidth);
        shrinkTarget.BeginAnimation(FrameworkElement.HeightProperty, shrinkHeight);
        shrinkTarget2.BeginAnimation(FrameworkElement.WidthProperty, shrinkWidth);
        shrinkTarget2.BeginAnimation(FrameworkElement.HeightProperty, shrinkHeight);
        shrinkTarget3.BeginAnimation(FrameworkElement.WidthProperty, shrinkWidth);
        shrinkTarget3.BeginAnimation(FrameworkElement.HeightProperty, shrinkHeight);
        shrinkTarget4.BeginAnimation(FrameworkElement.WidthProperty, shrinkWidth);
        shrinkTarget4.BeginAnimation(FrameworkElement.HeightProperty, shrinkHeight);

        AnimateGradient(expandTarget.Background as LinearGradientBrush, "#FF06080D", "#FF1F2B55");
        AnimateGradient(expandTarget.BorderBrush as LinearGradientBrush, "#FF232A42", "#FF586BA8");

        AnimateGradient(shrinkTarget.Background as LinearGradientBrush, "#0A0D16", "#0A0D16");
        AnimateGradient(shrinkTarget.BorderBrush as LinearGradientBrush, "#0A0D16", "#0A0D16");
        AnimateGradient(shrinkTarget2.Background as LinearGradientBrush, "#0A0D16", "#0A0D16");
        AnimateGradient(shrinkTarget2.BorderBrush as LinearGradientBrush, "#0A0D16", "#0A0D16");
        AnimateGradient(shrinkTarget3.Background as LinearGradientBrush, "#0A0D16", "#0A0D16");
        AnimateGradient(shrinkTarget3.BorderBrush as LinearGradientBrush, "#0A0D16", "#0A0D16");
        AnimateGradient(shrinkTarget4.Background as LinearGradientBrush, "#0A0D16", "#0A0D16");
        AnimateGradient(shrinkTarget4.BorderBrush as LinearGradientBrush, "#0A0D16", "#0A0D16");

        var expandLabel = FindLabel(expandTarget);
        if (expandLabel != null)
        {
            expandLabel.Visibility = Visibility.Visible;
            var fadeIn = new DoubleAnimation(1, TimeSpan.FromMilliseconds(200));
            expandLabel.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }

        var shrinkLabel = FindLabel(shrinkTarget);
        if (shrinkLabel != null)
        {
            var fadeOut = new DoubleAnimation(0, TimeSpan.FromMilliseconds(200));
            fadeOut.Completed += (s, e) => shrinkLabel.Visibility = Visibility.Hidden;
            shrinkLabel.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }
        var shrinkLabel2 = FindLabel(shrinkTarget2);
        if (shrinkLabel2 != null)
        {
            var fadeOut = new DoubleAnimation(0, TimeSpan.FromMilliseconds(200));
            fadeOut.Completed += (s, e) => shrinkLabel2.Visibility = Visibility.Hidden;
            shrinkLabel2.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }
        var shrinkLabel3 = FindLabel(shrinkTarget3);
        if (shrinkLabel3 != null)
        {
            var fadeOut = new DoubleAnimation(0, TimeSpan.FromMilliseconds(200));
            fadeOut.Completed += (s, e) => shrinkLabel3.Visibility = Visibility.Hidden;
            shrinkLabel3.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }
        var shrinkLabel4 = FindLabel(shrinkTarget3);
        if (shrinkLabel4 != null)
        {
            var fadeOut = new DoubleAnimation(0, TimeSpan.FromMilliseconds(200));
            fadeOut.Completed += (s, e) => shrinkLabel4.Visibility = Visibility.Hidden;
            shrinkLabel4.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }
    }

    void AnimateGradient(LinearGradientBrush brush, string color1Hex, string color2Hex)
    {
        if (brush == null || brush.GradientStops.Count < 2) return;

        var color1 = (Color)ColorConverter.ConvertFromString(color1Hex);
        var color2 = (Color)ColorConverter.ConvertFromString(color2Hex);

        var anim1 = new ColorAnimation(color1, TimeSpan.FromMilliseconds(400));
        var anim2 = new ColorAnimation(color2, TimeSpan.FromMilliseconds(400));

        brush.GradientStops[0].BeginAnimation(GradientStop.ColorProperty, anim1);
        brush.GradientStops[1].BeginAnimation(GradientStop.ColorProperty, anim2);
    }

    Label FindLabel(Border border)
    {
        foreach (var child in ((Grid)border.Child).Children)
        {
            if (child is Label lbl)
                return lbl;
        }
        return null;
    }
    #endregion

    #region Navigation Buttons
    private void HomeBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        EditorHolder.Visibility = Visibility.Hidden;
        AnimateButtonSwap(HomeBtn, EditorBtn, ScriptsBtn, ChatBtn, SettingsBtn);
        SwitchScriptPages(Homepage, EditorPage, ScriptsPage, ChatPage, SettingsPage);
    }

    private void EditorBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        EditorHolder.Visibility = Visibility.Visible;
        AnimateButtonSwap(EditorBtn, HomeBtn, ScriptsBtn, ChatBtn, SettingsBtn);
        SwitchScriptPages(EditorPage, Homepage, ScriptsPage, ChatPage, SettingsPage);
    }

    private void ScriptsBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        EditorHolder.Visibility = Visibility.Hidden;
        AnimateButtonSwap(ScriptsBtn, HomeBtn, EditorBtn, ChatBtn, SettingsBtn);
        SwitchScriptPages(ScriptsPage, Homepage, EditorPage, ChatPage, SettingsPage);
    }

    private void ChatBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        EditorHolder.Visibility = Visibility.Hidden;
        AnimateButtonSwap(ChatBtn, HomeBtn, EditorBtn, ScriptsBtn, SettingsBtn);
        SwitchScriptPages(ChatPage, Homepage, EditorPage, ScriptsPage, SettingsPage);
    }

    private void SettingsBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        EditorHolder.Visibility = Visibility.Hidden;
        AnimateButtonSwap(SettingsBtn, HomeBtn, EditorBtn, ScriptsBtn, ChatBtn);
        SwitchScriptPages(SettingsPage, Homepage, EditorPage, ScriptsPage, ChatPage);
    }
    #endregion

    #region Scripts
    private async void getscripts()
    {
        try
        {
            PopularScripts.Children.Clear();
            HttpResponseMessage response = await HttpClient.GetAsync("https://scriptblox.com/api/script/search?q=Universal");
            HttpContent content = response.Content;
            object dynJson = JsonConvert.DeserializeObject(await content.ReadAsStringAsync());
            foreach (dynamic item in ((dynamic)dynJson).result.scripts)
            {
                dynamic jsonitem = JsonConvert.DeserializeObject<object>(((object)item).ToString());
                dynamic script = Convert.ToString(jsonitem.script);

                if (script != null)
                {
                    dynamic scriptpic = Convert.ToString(jsonitem.game.imageUrl);
                    if (!string.IsNullOrEmpty(scriptpic))
                    {
                        scriptpic = "https://scriptblox.com" + scriptpic;
                    }
                    dynamic dastitle = Convert.ToString(jsonitem.title);
                    dynamic descthingy = Convert.ToString(jsonitem.slug);
                    PopularScript pop = new PopularScript(scriptpic, dastitle, script);
                    pop.MouseWheel += new MouseWheelEventHandler(ScrollPopular);
                    PopularScripts.Children.Add(pop);
                }
            }
        }
        catch
        {
            MessageBox.Show("Something went wrong fetching scripts!");
        }
    }
    #endregion

    #region Navigation
    private void ScrollNews(object sender, MouseWheelEventArgs e)
    {
        this.NewsPanel.ScrollToHorizontalOffset(this.NewsPanel.VerticalOffset + (double)(e.Delta / 10));
    }
    private void ScrollPopular(object sender, MouseWheelEventArgs e)
    {
        this.PopularScripts1.ScrollToHorizontalOffset(this.PopularScripts1.HorizontalOffset + (double)(e.Delta / 10));
    }
    public async void SwitchScriptPages(UIElement openElement, UIElement Close1, UIElement Close2, UIElement Close3, UIElement Close4)
    {
        openElement.Visibility = Visibility.Visible;
        Storyboard scriptStoryboard = new Storyboard();

        DoubleAnimation openAnim = new DoubleAnimation()
        {
            To = 1,
            Duration = TimeSpan.FromSeconds(.2)
        };
        DoubleAnimation close1Anim = new DoubleAnimation()
        {
            To = 0,
            Duration = TimeSpan.FromSeconds(.2)
        };
        DoubleAnimation close2Anim = new DoubleAnimation()
        {
            To = 0,
            Duration = TimeSpan.FromSeconds(.3)
        };
        DoubleAnimation close3Anim = new DoubleAnimation()
        {
            To = 0,
            Duration = TimeSpan.FromSeconds(.3)
        };
        DoubleAnimation close4Anim = new DoubleAnimation()
        {
            To = 0,
            Duration = TimeSpan.FromSeconds(.3)
        };

        Storyboard.SetTarget(openAnim, openElement);
        Storyboard.SetTarget(close1Anim, Close1);
        Storyboard.SetTarget(close2Anim, Close2);
        Storyboard.SetTarget(close3Anim, Close3);
        Storyboard.SetTarget(close4Anim, Close4);
        Storyboard.SetTargetProperty(openAnim, new PropertyPath(OpacityProperty));
        Storyboard.SetTargetProperty(close1Anim, new PropertyPath(OpacityProperty));
        Storyboard.SetTargetProperty(close2Anim, new PropertyPath(OpacityProperty));
        Storyboard.SetTargetProperty(close3Anim, new PropertyPath(OpacityProperty));
        Storyboard.SetTargetProperty(close4Anim, new PropertyPath(OpacityProperty));

        scriptStoryboard.Children.Add(openAnim);
        scriptStoryboard.Children.Add(close1Anim);
        scriptStoryboard.Children.Add(close2Anim);
        scriptStoryboard.Children.Add(close3Anim);
        scriptStoryboard.Children.Add(close4Anim);

        scriptStoryboard.Begin();

        await Task.Delay(300);
        Close1.Visibility = Visibility.Hidden;
        Close2.Visibility = Visibility.Hidden;
        Close3.Visibility = Visibility.Hidden;
        Close4.Visibility = Visibility.Hidden;
    }

    #endregion

    #region Monaco
    public void initbrowser()
    {
        browser = new WebView2();
        browser.Source = new Uri(Directory.GetCurrentDirectory() + "\\Monaco\\Monaco.html");
        browser.NavigationCompleted += (sender, e) =>
        {

        };
        if (TabPanel.Children.Count == 0)
        {
            browser.Visibility = Visibility.Hidden;
        }
    }
    internal async Task<string> GetText()
    {
        string response = await browser.ExecuteScriptAsync("GetText()");
        return System.Text.Json.JsonSerializer.Deserialize<string>(response);
    }

    internal async Task SetText(string text)
    {
        try
        {
            string escapedText = text.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\'", "\"");
            string script = $"SetText('{escapedText}');";
            await browser.ExecuteScriptAsync(script);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Failed to set text: " + ex.Message);
        }
    }
    #endregion

    #region Tabs
    private void ScrollTabs(object sender, MouseWheelEventArgs e)
    {
        this.ChangelogsPanel.ScrollToHorizontalOffset(this.ChangelogsPanel.VerticalOffset + (double)(e.Delta / 10));
    }
    public async void AddTab(string namee = "", string text = "")
    {
        if (IsVisible == true)
        {
            browser.Visibility = Visibility.Visible;
        }
        MainWindow mainWindow = this;
        Controls.TabControl tab = new Controls.TabControl(TabPanel, this);
        this.TabPanel.Children.Add(tab);
        tab.TabLabel.Content = namee;
        tab.script = text;
        ScriptScroller.ScrollToRightEnd();
        tab.MouseWheel += new MouseWheelEventHandler(ScrollTabs);
        await Task.Delay(10);
        tab.Opacity = 0;
        tab.RenderTransform = new TranslateTransform(-50, 0);

        // Create animations
        DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
        DoubleAnimation slideIn = new DoubleAnimation(-50, 0, TimeSpan.FromSeconds(0.1));

        // Apply animations
        tab.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        (tab.RenderTransform as TranslateTransform).BeginAnimation(TranslateTransform.XProperty, slideIn);
        tab.Select();
    }

    private void AddTabBTN_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        AddTab($"Tab {TabPanel.Children.Count.ToString()}.lua", "--Welcome To Comet!");
    }
    #endregion

    #region Script List
    public string BaseSscriptDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\scripts";

    public string[] GrabbedDirs { get; set; }

    private void AddParents(FolderItemControl lim)
    {
        if (lim.IsSub)
        {
            lim.ParentItems.Add(lim.ParentFolderSub);
        }
    }

    private void AddItemsToAList(List<UIElement> listToAdd, FolderItemControl itemToAdd)
    {
        listToAdd.Add(itemToAdd);
        if (itemToAdd.IsSub)
        {
            AddItemsToAList(listToAdd, itemToAdd.ParentFolderSub);
        }
    }
    public void FillTreeView(StackPanel treeView, StackPanel TopStack, string directory, FolderItemControl ParentSubFolderItem = null)
    {
        treeView.Children.Clear();
        string[] directories = Directory.GetDirectories(directory);
        foreach (string path in directories)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FolderItemControl folderItem = new FolderItemControl();

            folderItem.NameText.Text = directoryInfo.Name;

            treeView.Children.Add(folderItem);
            folderItem.Width = (folderItem.Parent as StackPanel).Width;
            folderItem.ParentPanelMian = treeView;
            folderItem.ParentFolderSub = (folderItem.Parent as FolderItemControl);
            if (folderItem.Parent != TopStack)
            {
                ParentSubFolderItem.HasSubs = true;
                ParentSubFolderItem.SubItems.Add(folderItem);
                folderItem.ParentFolderSub = ParentSubFolderItem;
                folderItem.ParentPanelMian = TopStack;
                folderItem.PasrentPanelSub = treeView;
                folderItem.IsSub = true;
                AddItemsToAList(folderItem.ParentItems, folderItem.ParentFolderSub);
            }
            else
            {
                folderItem.ParentFolderSub = null;
                folderItem.ParentPanelMian = TopStack;
                folderItem.IsSub = false;
                folderItem.PasrentPanelSub = null;
            }
            folderItem.FOlderStack.Height = 0;
            FillTreeView(folderItem.FOlderStack, TopStack, path, folderItem);
        }

        foreach (string file in Directory.GetFiles(directory))
        {
            FileInfo fileInfo = new FileInfo(file);
            ScriptItemControl fileItem = new ScriptItemControl();
            fileItem.MainName = fileInfo.Name;
            fileItem.NameText.Text = fileInfo.Name;
            fileItem.FullPath = fileInfo.FullName;

            treeView.Children.Add(fileItem);

        }
    }
    #endregion
    private void PopularScripts1_ScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        
    }
}