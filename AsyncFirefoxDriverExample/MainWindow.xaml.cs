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

namespace AsyncFirefoxDriverExample
{
    public partial class MainWindow : Window
    {
        private WebDriver asyncDriver;
        private readonly ObservableCollection<string> evs = new ObservableCollection<string>();
        private AsyncFirefoxDriver ffDriver;

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
            if (ffDriver == null)
            {
                var profileName = tbProfileName.Text;
                ffDriver = new AsyncFirefoxDriver(profileName);
                asyncDriver = new WebDriver(ffDriver);
            }
            return "ok";
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Prepare() != "ok") return;

            var url = tbUrl.Text;

            if (requestListener == null)
            {
                requestListener = new ZuRequestListener(ffDriver);
                // or 
                //requestListener = new ZuRequestListener(asyncDriver.browserClient);
                requestListener.DoSendBinary = false;
                //requestListener.SaveAllFilesToFolder = "D:\\Temp\\files\\";
                lbRequests.ItemsSource = loadedFiles;
                requestListener.FileLoaded += RequestListener_FileLoaded;
                await requestListener.StartListeningFileLoaded();
            }

            await asyncDriver.SetContextContent();
            var res2 = await asyncDriver.GoToUrl(url); //"https://www.google.com/");
            tbRes.Text = res2 + Environment.NewLine + Environment.NewLine + tbRes.Text;
        }

        private void RequestListener_FileLoaded(object sender, ZuRequestInfo e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate { loadedFiles.Insert(0, e); });
        }

        private void bEvalEvents_Click_16(object sender, RoutedEventArgs e)
        {
            ffDriver.AddEventListener(tbEvalEvents5.Text, ffDriverEventListener);
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
            if (ffDriver != null)
            {
                var res = await ffDriver.AddSendEventFuncIfNo();
                res = await ffDriver.SendEvent(tbEvalEvents8.Text, tbEvalEvents9.Text);
            }
        }

        private async void bEvalEvents_Click_18(object sender, RoutedEventArgs e)
        {
            if (ffDriver != null)
            {
                await ffDriver.AddSendEventFuncIfNo();
                await ffDriver.EvalInChrome($"top.zuSendEvent({tbEvalEvents3.Text})");
            }
        }

        private async void bEvalEvents_Click_17(object sender, RoutedEventArgs e)
        {
            var code = tbEvalCode.Text;
            if (ffDriver != null)
            {
                var res = await ffDriver.Eval(code);
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ffDriver != null)
            {
                var res = await ffDriver?.GetContext();
                tbRes.Text = res + Environment.NewLine + Environment.NewLine + tbRes.Text;
            }
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (ffDriver != null) await ffDriver.SetContextChrome();
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (ffDriver != null) await ffDriver.SetContextContent();
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
            await ffDriver.CheckConnected();
        }

        private async void Button_Click_8(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Prepare() != "ok") return;
                await asyncDriver.SetContextContent();
                var res2 = await asyncDriver.GoToUrl("https://www.google.com/");
                //var query = await asyncDriver.FindElement(By.Name("q"));
                var query = await asyncDriver.WaitForElementWithName("q");
                query = await asyncDriver.WaitForElementWithId("lst-ib");
                query = await asyncDriver.WaitForWebElement(async () => await asyncDriver.FindElement(By.Name("q")));
                foreach (var v in tbSendKeys.Text.ToList())
                {
                    await Task.Delay(500 + new Random().Next(1000));
                    await query.SendKeys(v.ToString());
                }
                await Task.Delay(500);
                await query.SendKeys(Keys.Enter);
                await Task.Delay(2000);
                query = await asyncDriver.WaitForElementWithId("lst-ib");
                //query = await asyncDriver.WaitForElementWithName("q");
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
            var syncDriver = new SyncWebDriver(asyncDriver);
            syncDriver.SetContextContent();
            syncDriver.GoToUrl("https://www.google.com/");
            var s = "";
            var q1 = syncDriver.FindElement(By.Name("q"));
            // start adding your commands it sync
            // Visual Studio does not allow add await while debugging
            q1.SendKeys("T");
            q1.SendKeys("o");
            q1.SendKeys("C");
            q1.SendKeys("S");
            q1.SendKeys("h");
            q1.SendKeys("a");
            q1.SendKeys("r");
            q1.SendKeys("p");
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
            if (asyncDriver != null) asyncDriver.CloseSync();
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
    }
}