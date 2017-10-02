using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Zu.AsyncWebDriver.Remote;
using Zu.Chrome;
using Zu.Firefox;
using Zu.Opera;
using Zu.WebBrowser;
using Zu.WebBrowser.BasicTypes;

namespace AllDriversExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WebDriver webDriver;
        private List<WebDriver> driversToClose = new List<WebDriver>();

        private AsyncFirefoxDriver asyncFirefoxDriver;
        private AsyncChromeDriver asyncChromeDriver;
        private AsyncOperaDriver asyncOperaDriver;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                var userDir = tbOpenProfileDir.Text;
                if (chbOpenProfileHeadless.IsChecked == true)
                {
                    var (width, height) = GetWidthHeight();
                    asyncChromeDriver = new AsyncChromeDriver(new ChromeDriverConfig().SetHeadless().SetWindowSize(width, height).SetUserDir(userDir));
                }
                else asyncChromeDriver = new AsyncChromeDriver(new ChromeDriverConfig().SetUserDir(userDir));
                webDriver = new WebDriver(asyncChromeDriver);
                // await asyncChromeDriver.Connect(); // browser opens here
                await webDriver.GoToUrl("https://www.google.com/"); // browser opens here
                driversToClose.Add(webDriver);
                var mess = $"opened on port {asyncChromeDriver.Config.Port} in dir {asyncChromeDriver.Config.UserDir} \nWhen close, dir will NOT be DELETED";
                tbDevToolsRes2.Text = mess;
            }
            catch (Exception ex)
            {
                tbDevToolsRes2.Text = ex.ToString();
            }

        }

        private (int width, int height) GetWidthHeight()
        {
            var width = 1200;
            var height = 900;
            int.TryParse(tbOpenProfileHeadlessWidth.Text, out width);
            int.TryParse(tbOpenProfileHeadlessHeight.Text, out height);
            return (width, height);
        }

        private async void Button_Click_11(object sender, RoutedEventArgs e)
        {
            try
            {
                IAsyncWebBrowserClient browserClient = null;
                DriverConfig config = null;
                if (chbOpenProfileHeadless.IsChecked == true)
                {
                    var (width, height) = GetWidthHeight();
                    // one config for all or 
                    config = new DriverConfig().SetHeadless().SetWindowSize(width, height);

                }
                else
                {
                    config = new DriverConfig();
                }

                if (rbOpenFirefox.IsChecked == true)
                {
                    asyncFirefoxDriver = new AsyncFirefoxDriver(config);
                    browserClient = asyncFirefoxDriver;
                }
                else if (rbOpenChrome.IsChecked == true)
                {
                    asyncChromeDriver = new AsyncChromeDriver(config);
                    browserClient = asyncChromeDriver;
                }
                else if (rbOpenOpera.IsChecked == true)
                {
                    asyncOperaDriver = new AsyncOperaDriver(config);
                    browserClient = asyncOperaDriver;
                }

                webDriver = new WebDriver(browserClient);
                driversToClose.Add(webDriver);
                //await asyncFirefoxDriver.Connect(); // browser opens here
                await webDriver.GoToUrl("https://www.bing.com/"); // browser opens here
                var mess = $"opened on port {config.Port} in dir {config.UserDir} \nWhen close, dir will be DELETED";
                tbDevToolsRes2.Text = mess;
            }
            catch (Exception ex)
            {
                tbDevToolsRes2.Text = ex.ToString();
            }
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (webDriver != null) await webDriver.Close();
            tbDevToolsRes2.Text = "closed";
        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                var port = 7777;
                int.TryParse(tbOpenProfilePort.Text, out port);
                var userDir = tbOpenProfileDir.Text;
                if (chbOpenProfileHeadless.IsChecked == true)
                {
                    var (width, height) = GetWidthHeight();
                    asyncChromeDriver = new AsyncChromeDriver(new ChromeDriverConfig().SetHeadless().SetWindowSize(width, height).SetUserDir(userDir).SetPort(port));
                }
                else asyncChromeDriver = new AsyncChromeDriver(new ChromeDriverConfig().SetUserDir(userDir).SetPort(port));
                webDriver = new WebDriver(asyncChromeDriver);
                // await asyncChromeDriver.Connect(); // browser opens here
                await webDriver.GoToUrl("https://www.google.com/"); // browser opens here
                driversToClose.Add(webDriver);
                var mess = $"opened on port {asyncChromeDriver.Config.Port} in dir {asyncChromeDriver.Config.UserDir} \nWhen close, dir will NOT be DELETED";
                tbDevToolsRes2.Text = mess;
            }
            catch (Exception ex)
            {
                tbDevToolsRes2.Text = ex.ToString();
            }
        }

        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                IAsyncWebBrowserClient browserClient = null;
                DriverConfig config = null;
                if (chbOpenProfileHeadless.IsChecked == true)
                {
                    var (width, height) = GetWidthHeight();
                    config = new DriverConfig().SetHeadless().SetWindowSize(width, height).SetIsDefaultProfile();
                }
                else config = new DriverConfig().SetIsDefaultProfile();

                if (rbOpenFirefox.IsChecked == true)
                {
                    asyncFirefoxDriver = new AsyncFirefoxDriver(config);
                    browserClient = asyncFirefoxDriver;
                }
                else if (rbOpenChrome.IsChecked == true)
                {
                    asyncChromeDriver = new AsyncChromeDriver(config);
                    browserClient = asyncChromeDriver;
                }
                else if (rbOpenOpera.IsChecked == true)
                {
                    asyncOperaDriver = new AsyncOperaDriver(config);
                    browserClient = asyncOperaDriver;
                }

                webDriver = new WebDriver(browserClient);
                driversToClose.Add(webDriver);
                await webDriver.GoToUrl("https://www.google.com/"); // browser opens here
                var mess = $"opened on port {config.Port} in dir {config.UserDir} \nWhen close, dir will NOT be DELETED";
                tbDevToolsRes2.Text = mess;
            }
            catch (Exception ex)
            {
                tbDevToolsRes2.Text = ex.ToString();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (var item in driversToClose)
            {
                try
                {
                    item.CloseSync();
                }
                catch { }
            }
        }

        private async void Button_Click_6(object sender, RoutedEventArgs e)
        {
            var name = tbOpenProfileName.Text;
            try
            {
                if (chbOpenProfileHeadless.IsChecked == true)
                {
                    var (width, height) = GetWidthHeight();
                    asyncFirefoxDriver = new AsyncFirefoxDriver(new FirefoxDriverConfig().SetHeadless().SetWindowSize(width, height).SetProfileName(name));
                }
                else asyncFirefoxDriver = new AsyncFirefoxDriver(name);
                webDriver = new WebDriver(asyncFirefoxDriver);
                driversToClose.Add(webDriver);
                //await asyncFirefoxDriver.Connect(); // browser opens here
                await webDriver.GoToUrl("https://www.google.com/"); // browser opens here
                var mess = $"Profile {asyncFirefoxDriver.Config.UserDir} opened on port {asyncFirefoxDriver.Port} \nWhen close, dir will NOT be DELETED";
                tbDevToolsRes2.Text = mess;
            }
            catch (Exception ex)
            {
                tbDevToolsRes2.Text = ex.ToString();
            }
        }

        private async void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (webDriver != null)
            {
                var dir = tbScreenshotDir.Text;
                var screenshot = await webDriver.GetScreenshot();
                //screenshot.SaveAsFile(GetFilePathToSaveScreenshot(), Zu.WebBrowser.BasicTypes.ScreenshotImageFormat.Png);
                using (MemoryStream imageStream = new MemoryStream(screenshot.AsByteArray))
                {
                    System.Drawing.Image screenshotImage = System.Drawing.Image.FromStream(imageStream);
                    screenshotImage.Save(GetFilePathToSaveScreenshot(dir), System.Drawing.Imaging.ImageFormat.Png);
                }

            }
        }
        private static string GetFilePathToSaveScreenshot(string dir = null)
        {
            dir = dir ?? @"C:\temp";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            var i = 0;
            var path = "";
            do
            {
                i++;
                path = Path.Combine(dir, $"screenshot{i}.png");
            } while (File.Exists(path));
            return path;

        }
        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            lbFirefoxProfiles.ItemsSource = FirefoxProfilesWorker.GetProfiles().Select(v => Tuple.Create(v.Key, v.Value));
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            var profile = lbFirefoxProfiles.SelectedItem as Tuple<string, string>;
            if (profile != null)
            {
                FirefoxProfilesWorker.RemoveProfile(profile.Item1);
                lbFirefoxProfiles.ItemsSource = FirefoxProfilesWorker.GetProfiles().Select(v => Tuple.Create(v.Key, v.Value));
            }
        }

        private async void Button_Click_10(object sender, RoutedEventArgs e)
        {
            var name = tbOpenProfileName.Text;
            try
            {
                if (chbOpenProfileHeadless.IsChecked == true)
                {
                    var (width, height) = GetWidthHeight();
                    asyncFirefoxDriver = new AsyncFirefoxDriver(new FirefoxDriverConfig().SetHeadless().SetWindowSize(width, height).SetProfileName(name).SetOpenOffline());
                }
                else asyncFirefoxDriver = new AsyncFirefoxDriver(new FirefoxDriverConfig().SetProfileName(name).SetOpenOffline());
                webDriver = new WebDriver(asyncFirefoxDriver);
                driversToClose.Add(webDriver);
                // await asyncFirefoxDriver.Connect(); // browser opens here
                await webDriver.GoToUrl("https://www.google.com/"); // browser opens here
                var mess = $"Profile {asyncFirefoxDriver.Config.UserDir} opened on port {asyncFirefoxDriver.Port} \nWhen close, dir will NOT be DELETED";
                tbDevToolsRes2.Text = mess;
            }
            catch (Exception ex)
            {
                tbDevToolsRes2.Text = ex.ToString();
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            asyncFirefoxDriver = new AsyncFirefoxDriver(new FirefoxDriverConfig().SetHeadless().SetWindowSize(1200, 900));
            webDriver = new WebDriver(asyncFirefoxDriver);
            await webDriver.GoToUrl("https://www.google.com/");
            var screenshot = await webDriver.GetScreenshot();
            using (MemoryStream imageStream = new MemoryStream(screenshot.AsByteArray))
            {
                System.Drawing.Image screenshotImage = System.Drawing.Image.FromStream(imageStream);
                screenshotImage.Save(GetFilePathToSaveScreenshot(@"C:\temp"), System.Drawing.Imaging.ImageFormat.Png);
            }
        }
    }
}
