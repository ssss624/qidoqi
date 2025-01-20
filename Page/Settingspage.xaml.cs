using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using StarLight_Core.Models.Launch;
using StarLight_Core.Utilities;

namespace oop.Page
{
    /// <summary>
    /// SettingPuge.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsPage
    {
        public  string GaemeVer{ get; set; }

        public  string javaLibrary { get; set;}

        public  int Login;

        public  string UserName { get; set; }

        public  string UserPassword { get; set; }

        public  string UserServerid { get; set; }

        public SettingsPage()
        {
            InitializeComponent();

            GetGameVer();
            Getjavas();

            LoginMode.SelectedIndex = 0;
        }

        void GetGameVer()
        {
            GameVersion.DisplayMemberPath = "Id";
            GameVersion.SelectedValuePath = "Id";
            GameVersion.ItemsSource = GameCoreUtil.GetGameCores();
        }

        async Task Getjavas()
        {
            javaPath.DisplayMemberPath = "JavaVersion";
            javaPath.SelectedValuePath = "JavaPath";
            javaPath.ItemsSource = JavaUtil.GetJavas();
        }

        private void ReportProgress(ProgressReport report)
        {
          
        }

        private void LoginMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LoginMode.SelectedIndex == 0)
            {
                Login = LoginMode.SelectedIndex;
            }
            else if(LoginMode.SelectedIndex == 1)
            {
                Login = LoginMode.SelectedIndex;
                PlayerNameText.Visibility = Visibility.Visible;
                PlayerName.Visibility = Visibility.Visible;
                PasswordText.Visibility = Visibility.Collapsed;
                Password.Visibility = Visibility.Collapsed;
                ServeridText.Visibility = Visibility.Collapsed;
                ServeridText.Visibility = Visibility.Collapsed;
            }
            else
            {
                Login = LoginMode.SelectedIndex;
                            PasswordText.Visibility = Visibility.Visible;
                Password.Visibility = Visibility.Visible;
                ServeridText.Visibility = Visibility.Visible;
                ServeridText.Visibility = Visibility.Visible;
            }
        }

        private void GameVersion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GaemeVer = GameVersion.SelectedValue.ToString();
        }

        private void javaPath_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            javaLibrary = javaPath.SelectedValue.ToString();
        }

        private void PlayerName_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UserName = PlayerName.Text;
        }

        private void Password_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UserPassword = Password.Text;
        }

        private void Server_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UserServerid = ServeridText.Text;
        }
    }
}
