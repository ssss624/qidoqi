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
using StarLight_Core.Authentication;
using StarLight_Core.Enum;
using StarLight_Core.Launch;
using StarLight_Core.Models.Authentication;
using StarLight_Core.Models.Launch;

namespace oop.Page
{
    /// <summary>
    /// homePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {

            BaseAccount account = null!;

            if (Const.main.SettingsPage.Login == 0)
            {
                var auth = new MicrosoftAuthentication("c06d4d68-7751-4a8a-a2ff-d1b46688f428");
                var code = await auth.RetrieveDeviceCodeInfo();
                Clipboard.Clear();
                Clipboard.SetText(code.UserCode);
                MessageBox.Show(code.UserCode);
                Process.Start(new ProcessStartInfo(code.VerificationUri)
                {
                    UseShellExecute = true,
                    Verb = "open"
                });
                var token = await auth.GetTokenResponse(code);
                var mAccount = await auth.MicrosoftAuthAsync(token, x =>
                {
                    Progress.Text = x;
                });
                account = new BaseAccount()
                {
                    AccessToken = mAccount.AccessToken,
                    ClientToken = mAccount.ClientToken,
                    Name = mAccount.Name,
                    Type = AuthType.Microsoft,
                    Uuid = mAccount.Uuid
                };
            }
            else if (Const.main.SettingsPage.Login == 1)
            {
                account = new OfflineAuthentication(Const.main.SettingsPage.PlayerName.Text.ToString()).OfflineAuth();
            }
            else
            {
                var auth = new YggdrasilAuthenticator(Const.main.SettingsPage.PlayerName.Text
                    , Const.main.SettingsPage.Password.Text, Const.main.SettingsPage.Server.Text);
                account = (await auth.YggdrasilAuthAsync()).FirstOrDefault()!;
            }

            //操作一下

            if (account == null)
            {
                MessageBox.Show("Account Error");
                return;
            }

            LaunchConfig args = new() // 配置启动参数
            {
                Account = new Account() { BaseAccount = account },
                GameCoreConfig = new()
                {
                    Root = ".minecraft", // 游戏根目录(可以是绝对的也可以是相对的,自动判断)
                    Version = Const.main.SettingsPage.GaemeVer, // 启动的版本
                    IsVersionIsolation = true,
                },
                JavaConfig = new()
                {
                    JavaPath = Const.main.SettingsPage.javaLibrary,
                    MaxMemory = 4000,
                    MinMemory = 1000
                }
            };

            if (Const.main.SettingsPage.Login == 2)
            {
                args.GameCoreConfig.UnifiedPassServerId = Const.main.SettingsPage.UserServerid;
            }

            var launch = new MinecraftLauncher(args); // 实例化启动器
            var la = await launch.LaunchAsync(ReportProgress); // 启动

            // 日志输出
            la.ErrorReceived += (output) => Console.WriteLine($"{output}");
            la.OutputReceived += (output) => Console.WriteLine($"{output}");

            if (la.Status == Status.Succeeded)
            {
                MessageBox.Show("启动成功");
                // 启动成功执行操作
            }
            else
            {
                MessageBox.Show("启动失败" + la.Exception);
            }


        }
        private void ReportProgress(ProgressReport report)
        {
            Progress.Text = report.Description + " " + report.Percentage + "%";

        }
    }
}
