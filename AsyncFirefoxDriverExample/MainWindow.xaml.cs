using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Newtonsoft.Json.Linq;
using Zu.AsyncWebDriver;
using Zu.AsyncWebDriver.Remote;
using Zu.Browser;
using Zu.Firefox;
using Zu.WebBrowser.BasicTypes;
using System.IO;

namespace AsyncFirefoxDriverExample
{
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<string> evs = new ObservableCollection<string>();

        private readonly ObservableCollection<ZuRequestInfo> loadedFiles = new ObservableCollection<ZuRequestInfo>();
        private ZuRequestListener requestListener;
        private AsyncFirefoxDriver asyncFirefoxDriver;
        private WebDriver webDriver;
        private SyncWebDriver syncWebDriver;
        private WebDriver devToolsWebDriver;

        public MainWindow()
        {
            InitializeComponent();
        }

        private string Prepare()
        {
            if (asyncFirefoxDriver == null)
            {
                var profileName = tbProfileName.Text;
                asyncFirefoxDriver = new AsyncFirefoxDriver(profileName);
                webDriver = new WebDriver(asyncFirefoxDriver);
            }
            return "ok";
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //if (Prepare() != "ok") return;
            if (webDriver == null)
            {
                asyncFirefoxDriver = new AsyncFirefoxDriver(new FirefoxDriverConfig().SetIsMultiprocessFalse());
                webDriver = new WebDriver(asyncFirefoxDriver);
            }

            var url = tbUrl.Text;

            if (requestListener == null)
            {
                requestListener = new ZuRequestListener(asyncFirefoxDriver);
                // or 
                //requestListener = new ZuRequestListener(asyncDriver.browserClient);
                requestListener.DoSendBinary = false;
                //requestListener.SaveAllFilesToFolder = "D:\\Temp\\files\\";
                lbRequests.ItemsSource = loadedFiles;
                requestListener.FileLoaded += RequestListener_FileLoaded;
                await requestListener.StartListeningFileLoaded();
            }

            var res2 = await webDriver.GoToUrl(url); //"https://www.google.com/");
            tbRes.Text = res2 + Environment.NewLine + Environment.NewLine + tbRes.Text;
        }

        private void RequestListener_FileLoaded(object sender, ZuRequestInfo e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate { loadedFiles.Insert(0, e); });
        }

        private void bEvalEvents_Click_16(object sender, RoutedEventArgs e)
        {
            asyncFirefoxDriver.AddEventListener(tbEvalEvents5.Text, ffDriverEventListener);
        }

        private void ffDriverEventListener(JToken obj)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
           {
               evs.Insert(0, obj?["value"]?.ToString());
               lbEvalEvents1.ItemsSource = evs;
           });
        }

        private async void bEvalEvents_Click_19(object sender, RoutedEventArgs e)
        {
            if (asyncFirefoxDriver != null)
            {
                var res = await asyncFirefoxDriver.AddSendEventFuncIfNo();
                res = await asyncFirefoxDriver.SendEvent(tbEvalEvents8.Text, tbEvalEvents9.Text);
            }
        }

        private async void bEvalEvents_Click_18(object sender, RoutedEventArgs e)
        {
            if (asyncFirefoxDriver != null)
            {
                await asyncFirefoxDriver.AddSendEventFuncIfNo();
                await asyncFirefoxDriver.EvalInChrome($"top.zuSendEvent({tbEvalEvents3.Text})");
            }
        }

        private async void bEvalEvents_Click_17(object sender, RoutedEventArgs e)
        {
            var code = tbEvalCode.Text;
            if (asyncFirefoxDriver != null)
            {
                var res = await asyncFirefoxDriver.Eval(code);
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (asyncFirefoxDriver != null)
            {
                var res = await asyncFirefoxDriver?.GetContext();
                tbRes.Text = res + Environment.NewLine + Environment.NewLine + tbRes.Text;
            }
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (asyncFirefoxDriver != null) await asyncFirefoxDriver.SetContextChrome();
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (asyncFirefoxDriver != null) await asyncFirefoxDriver.SetContextContent();
        }

        private void lbRequests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var req = lbRequests.SelectedItem as ZuRequestInfo;
            tbRes.Text = req?.Body;
            tbUrl2.Text = req?.Url;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            lbProfiles.ItemsSource = FirefoxProfilesWorker.GetProfiles().Select(v => Tuple.Create(v.Key, v.Value));
        }

        private void lbProfiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var profileName = (lbProfiles.SelectedItem as Tuple<string, string>)?.Item1;
            tbMarionPort.Text = FirefoxProfilesWorker
                .GetMarionettePort(profileName)
                .ToString();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            var profileName = (lbProfiles.SelectedItem as Tuple<string, string>)?.Item1;
            if (int.TryParse(tbMarionPort.Text, out int port))
                FirefoxProfilesWorker.SetMarionettePort(profileName, port);
        }

        private void lbProfiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var profileName = (lbProfiles.SelectedItem as Tuple<string, string>)?.Item1;
            FirefoxProfilesWorker.OpenFirefoxProfile(profileName);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            var profileName = (lbProfiles.SelectedItem as Tuple<string, string>)?.Item1;
            if (!string.IsNullOrWhiteSpace(profileName))
                tbProfilesData.Text = string.Join(Environment.NewLine,
                    FirefoxProfilesWorker.ReadUserPreferences(profileName));
        }

        private async void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (Prepare() != "ok") return;
            await asyncFirefoxDriver.CheckConnected();
        }

        private async void Button_Click_8(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Prepare() != "ok") return;
                await webDriver.Options().Timeouts.SetImplicitWait(TimeSpan.FromSeconds(3));
                // name = "q", 0 - time to wait element, not use ImplicitWait
                var prevQuery = await webDriver.FindElementByNameOrDefault("q", 0);
                var res2 = await webDriver.GoToUrl("https://www.google.com/");
                // if element with name "q" from prev page wait for new element with name "q"
                var query = await webDriver.FindElementByName("q", prevQuery?.Id);

                //await query.SendKeys("ToCSharp");
                foreach (var v in tbSendKeys.Text.ToList())
                {
                    await Task.Delay(500 + new Random().Next(1000));
                    await query.SendKeys(v.ToString());
                }
                await Task.Delay(500);
                await query.SendKeys(Keys.Enter);
                await Task.Delay(2000);
                query = await webDriver.FindElementById("lst-ib");
                //query = await asyncDriver.FindElementByName("q");
                await query.SendKeys("t");
                await query.SendKeys(Keys.ArrowDown);
                await Task.Delay(1000);
                await query.SendKeys(Keys.ArrowDown);
                await Task.Delay(2000);
                await query.SendKeys(Keys.ArrowDown);
                await Task.Delay(1000);
                await query.SendKeys(Keys.ArrowUp);
                await Task.Delay(500);
                await query.SendKeys(Keys.Enter);
            }
            catch (Exception ex)
            {
                tbRes.Text = ex.ToString();
            }
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            if (Prepare() != "ok") return;
            // set breakpoint here
            var syncDriver = new SyncWebDriver(webDriver);
            syncDriver.GoToUrl("https://www.google.com/");
            var s = "";
            var q1 = syncDriver.FindElement(By.Name("q"));
            // start adding your commands it sync
            // Visual Studio does not allow add await while debugging
            q1.SendKeys("T");
            q1.SendKeys("o");
            q1.SendKeys("C");
            q1.SendKeys("Sharp");
            q1.SendKeys(Keys.Enter);
            syncDriver.SwitchTo().ActiveElement().SendKeys(Keys.Escape);
            s = "";
            syncDriver.SwitchTo().ActiveElement().SendKeys(Keys.Escape);
            syncDriver.SwitchTo().ActiveElement().SendKeys(Keys.PageDown);
            s = "";
            syncDriver.SwitchTo().ActiveElement().SendKeys(Keys.PageDown);

            var next = syncDriver.FindElementById("pnnext");
            next?.Click();
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            var profileName = (lbProfiles.SelectedItem as Tuple<string, string>)?.Item1;
            FirefoxProfilesWorker.OpenFirefoxDeveloperProfile(profileName);
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            var profileName = (lbProfiles.SelectedItem as Tuple<string, string>)?.Item1;
            FirefoxProfilesWorker.OpenFirefoxProfileOffline(profileName);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (webDriver != null) webDriver.CloseSync();
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            asyncFirefoxDriver = new AsyncFirefoxDriver(new FirefoxDriverConfig().SetIsMultiprocessFalse().SetDoSetDebuggerRemoteEnabled());
            webDriver = new WebDriver(asyncFirefoxDriver);
            syncWebDriver = new SyncWebDriver(webDriver);
            syncWebDriver.GoToUrl("https://www.google.com/");
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            if (webDriver != null) webDriver.CloseSync();
        }

        private async void Button_Click_14(object sender, RoutedEventArgs e)
        {
            if (asyncFirefoxDriver == null) return;
            await asyncFirefoxDriver.OpenBrowserDevTools();
            tbOpenRes.Text = "BrowserDevTools opened";
        }

        private async void Button_Click_15(object sender, RoutedEventArgs e)
        {
            try
            {
                asyncFirefoxDriver = new AsyncFirefoxDriver();
                await asyncFirefoxDriver.Connect();
                webDriver = new WebDriver(asyncFirefoxDriver);
                var browserDevToolsDriver = await asyncFirefoxDriver.OpenBrowserDevTools();
                // browserDevToolsDriver is AsyncFirefoxDriver
                devToolsWebDriver = new WebDriver(browserDevToolsDriver);
                //await asyncFirefoxDriver.SetContextChrome();
                await browserDevToolsDriver.Options.Timeouts.SetImplicitWait(TimeSpan.FromSeconds(2));
                //await devToolsWebDriver.SwitchTo().Frame("toolbox-iframe");
                var inspectorTab = await devToolsWebDriver.FindElementByXPath("//*[@id=\"toolbox-tab-inspector\"]"); //.FindElementById("toolbox-tab-inspector");
                await inspectorTab.Click();
                tbOpenRes.Text = "BrowserDevTools opened and clicked on toolbox-tab-inspector";
            }
            catch (Exception ex)
            {
                tbOpenRes.Text = ex.ToString();
            }
        }

        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            var syncDevTools = new SyncWebDriver(devToolsWebDriver);
            var s = "";
            try
            {
                //TODO not works now
                syncDevTools.SetContextContent();
                syncDevTools.SwitchTo().Frame("toolbox-iframe");
                var el = syncDevTools.FindElementByXPath("//*[@id=\"toolbox-tab-inspector\"]");
            }
            catch { }
            try
            {
                var el = syncDevTools.FindElementById("toolbox-tab-inspector");
            }
            catch { }
        }

        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            asyncFirefoxDriver?.BrowserDevTools?.CloseSync();
        }

        private async void Button_Click_18(object sender, RoutedEventArgs e)
        {
            if (asyncFirefoxDriver == null) return;
            await asyncFirefoxDriver.OpenBrowserDevTools(9876, false);
            tbOpenRes.Text = "BrowserDevTools opened in XUL";
        }

        private async void Button_Click_19(object sender, RoutedEventArgs e)
        {
            var src = tbJavaScriptEvalSource.Text;
            try
            {
                var res = await asyncFirefoxDriver.JavaScriptExecutor.ExecuteScript(src);
                tbJavaScriptEvalRes.Text = res?.ToString();
            }
            catch (Exception ex)
            {
                tbJavaScriptEvalRes.Text = ex.ToString();
            }
        }

        private async void Button_Click_20(object sender, RoutedEventArgs e)
        {
            if (asyncFirefoxDriver == null) return;
            await asyncFirefoxDriver.OpenBrowserDevTools2();
            tbOpenRes.Text = "BrowserDevTools opened";
        }

        private async void Button_Click_21(object sender, RoutedEventArgs e)
        {
            var profileName = tbOpenProfileName.Text;
            asyncFirefoxDriver = new AsyncFirefoxDriver(new FirefoxDriverConfig().SetProfileName(profileName).SetIsMultiprocessFalse().SetDoSetDebuggerRemoteEnabled());
            webDriver = new WebDriver(asyncFirefoxDriver);
            await webDriver.GoToUrl("https://www.google.com/");
        }

        private async void Button_Click_22(object sender, RoutedEventArgs e)
        {
            if(asyncFirefoxDriver?.BrowserDevTools != null)
            {
                devToolsWebDriver = new WebDriver(asyncFirefoxDriver.BrowserDevTools);
                try
                {
                    await asyncFirefoxDriver.BrowserDevTools.SetContextContent();
                    await devToolsWebDriver.SwitchTo().Frame("toolbox-iframe");
                    // Marionette do not send answer when SwitchTo().Frame("toolbox-iframe"); listener.js 1715
                    var inspectorTab = await devToolsWebDriver.FindElementById("toolbox-tab-inspector");
                    await inspectorTab.Click();
                }
                catch(Exception ex)
                {
                    tbOpenRes.Text = ex.ToString();
                }
            }
        }

        private async void Button_Click_23(object sender, RoutedEventArgs e)
        {
            if (asyncFirefoxDriver?.BrowserDevTools != null)
            {
                await asyncFirefoxDriver.BrowserDevTools.OpenBrowserDevTools(9654);
            }
        }

        private async void Button_Click_24(object sender, RoutedEventArgs e)
        {
            if (asyncFirefoxDriver?.BrowserDevTools != null)
            {
                devToolsWebDriver = new WebDriver(asyncFirefoxDriver.BrowserDevTools);
                try
                {
                    //await asyncFirefoxDriver.BrowserDevTools.SetContextContent();
                    var inspectorTab = await devToolsWebDriver.FindElementById("toolbox-tab-inspector");
                    await inspectorTab.Click();
                }
                catch (Exception ex)
                {
                    tbOpenRes.Text = ex.ToString();
                }
            }
        }

        private async void Button_Click_25(object sender, RoutedEventArgs e)
        {
            var profileName = tbOpenProfileName.Text;
            asyncFirefoxDriver = new AsyncFirefoxDriver(
                new FirefoxDriverConfig()
                .SetProfileName(profileName)
                .SetIsMultiprocessFalse()
                .SetDoSetDebuggerRemoteEnabled());
            webDriver = new WebDriver(asyncFirefoxDriver);
            await webDriver.GoToUrl("https://www.google.com/");
            await asyncFirefoxDriver.OpenBrowserDevTools();
            // asyncFirefoxDriver.BrowserDevTools is AsyncFirefoxDriver
            devToolsWebDriver = new WebDriver(asyncFirefoxDriver.BrowserDevTools);
            //// TODO: not works SwitchTo().Frame("toolbox-iframe");  listener.js 1715
            //await devToolsWebDriver.SwitchTo().Frame("toolbox-iframe");
            //var inspectorTab = await devToolsWebDriver.FindElementById("toolbox-tab-inspector");
            //await inspectorTab.Click();
        }

        private async void Button_Click_26(object sender, RoutedEventArgs e)
        {
            await webDriver.GoToUrl("https://www.google.com/");
        }

        private async void Button_Click_27(object sender, RoutedEventArgs e)
        {
            var query = await webDriver.FindElementById("lst-ib");
            await query.SendKeys(tbCommandsText.Text);
        }

        private async void Button_Click_28(object sender, RoutedEventArgs e)
        {
            var profileName= tbOpenProfileName.Text;
            var dir = Path.Combine(tbOpenProfileDir.Text, profileName);
            await FirefoxProfilesWorker.CreateFirefoxProfile(dir, profileName);
            asyncFirefoxDriver = new AsyncFirefoxDriver(new FirefoxDriverConfig()
                .SetProfileName(profileName)
                .SetIsMultiprocessFalse()
                .SetDoSetDebuggerRemoteEnabled());
            webDriver = new WebDriver(asyncFirefoxDriver);
            await webDriver.GoToUrl("https://www.google.com/");

            await asyncFirefoxDriver.OpenBrowserDevTools();

        }
    }
}