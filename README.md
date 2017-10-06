### AsyncWebDriver

AsyncWebDriver is .Net WebDriver rewritten to async/await pattern from [selenium](https://github.com/SeleniumHQ/selenium) project.
It uses browser drivers via [IAsyncWebBrowserClient](https://github.com/ToCSharp/IAsyncWebBrowserClient) interfaces.
AsyncWebDriver has synchronous wrapper [SyncWebDriver](https://github.com/ToCSharp/AsyncWebDriver/tree/master/AsyncWebDriver/SyncWrapper) and [SeleniumAdapter](https://github.com/ToCSharp/AsyncChromeDriverExamplesAndTests/tree/master/AsyncWebDriver.SeleniumAdapter).

[![Join the chat at https://gitter.im/AsyncWebDriver/Lobby](https://badges.gitter.im/AsyncWebDriver/Lobby.svg)](https://gitter.im/AsyncWebDriver/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## [AsyncFirefoxDriver](https://github.com/ToCSharp/AsyncWebDriver/tree/master/AsyncFirefoxDriver)
It is Firefox driver. It connects directly to Marionette and is async from this connection. No need in geckodriver.
AsyncFirefoxDriver implements [IAsyncWebBrowserClient](https://github.com/ToCSharp/IAsyncWebBrowserClient) interfaces and can be used as AsyncWebDriver. 

But the main MISSION of this project is to provide Firefox specific capabilities.
Reqest listener and profiles worker already here.

More extensions in [AsyncFirefoxDriverExtensions](https://github.com/ToCSharp/AsyncFirefoxDriverExtensions) project.

Debugger, all what addons and extensions can do, we can do with AsyncFirefoxDriver (coming soon).

## [AsyncChromeDriver](https://github.com/ToCSharp/AsyncChromeDriver)
Chrome WebDriver and Chrome DevTools in one library.  
It connects directly to Chrome DevTools and is async from this connection. No need in chromedriver.exe.

## [AsyncOperaDriver](https://github.com/ToCSharp/AsyncOperaDriver)
Opera WebDriver and Opera DevTools in one library.  
It connects directly to Opera DevTools and is async from this connection. No need in operadriver.exe.

### Other browsers
If it will be interestring we can add async drivers.

### Usage
#### Install via NuGet

If you want to include AsyncFirefoxDriver in your project, you can [install it directly from NuGet](https://www.nuget.org/packages/AsyncFirefoxDriver/)
```
PM> Install-Package AsyncFirefoxDriver
```
If you want to include AsyncChromeDriver, you can [install it directly from NuGet](https://www.nuget.org/packages/AsyncChromeDriver/)
```
PM> Install-Package AsyncChromeDriver
```
And Opera [NuGet](https://www.nuget.org/packages/AsyncOperaDriver/)
```
PM> Install-Package AsyncOperaDriver
```
### Write code example

#### Chrome
```csharp
     var asyncChromeDriver = new AsyncChromeDriver();
     var webDriver = new WebDriver(asyncChromeDriver);
     await webDriver.GoToUrl("https://www.google.com/");
     var query = await webDriver.WaitForElementWithName("q");
     //await query.SendKeys("ToCSharp");
     foreach (var v in "ToCSharp".ToList())
     {
        await Task.Delay(500 + new Random().Next(500));
        await query.SendKeys(v.ToString());
      }
      await Task.Delay(500);
      var prevQuery = await webDriver.FindElement(By.Name("q"));
      await query.SendKeys(Keys.Enter);
      query = await webDriver.WaitForElementWithName("q", prevQuery?.Id);
      var allCookies = await asyncChromeDriver.DevTools.Session.Network.GetAllCookies(new GetAllCookiesCommand());

```

#### Firefox
```csharp
     var webDriver = new WebDriver(new AsyncFirefoxDriver());
     await webDriver.GoToUrl("https://www.google.com/");
     var query = await webDriver.FindElement(By.Name("q"));
     await query.SendKeys("ToCSharp");
     await Task.Delay(500);
     await query.SendKeys(Keys.Enter);
```
##### Firefox Developer Tools
```csharp
     await FirefoxProfilesWorker.CreateFirefoxProfile(dir, profileName);
     asyncFirefoxDriver = new AsyncFirefoxDriver(new FirefoxDriverConfig()
         .SetProfileName(profileName)
         .SetIsMultiprocessFalse()
         .SetDoSetDebuggerRemoteEnabled());
     webDriver = new WebDriver(asyncFirefoxDriver);
     await webDriver.GoToUrl("https://www.google.com/");

     await asyncFirefoxDriver.OpenBrowserDevTools();
     // asyncFirefoxDriver.BrowserDevTools is AsyncFirefoxDriver
     devToolsWebDriver = new WebDriver(asyncFirefoxDriver.BrowserDevTools);

     // Then we can debug Firefox developer tools itself: 
     await asyncFirefoxDriver.BrowserDevTools.OpenBrowserDevTools(9654);
```
#### Open
```csharp
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
```
### Unit tests
**[AsyncWebDriver](https://github.com/ToCSharp/AsyncWebDriver)** is async and is different from Selenium. SyncWebDriver is synchronous wrapper over AsyncWebDriver.

[SeleniumAdapter](https://github.com/ToCSharp/AsyncChromeDriverExamplesAndTests/tree/master/AsyncWebDriver.SeleniumAdapter) is adapter of [Selenium interfaces](https://github.com/ToCSharp/AsyncChromeDriverExamplesAndTests/tree/master/AsyncWebDriver.SeleniumAdapter/Selenium) to [SyncWebDriver](https://github.com/ToCSharp/AsyncWebDriver/tree/master/AsyncWebDriver/SyncWrapper). So we can run Selenium tests on [AsyncWebDriver](https://github.com/ToCSharp/AsyncWebDriver). [Here is Unit Tests from Selenuim](https://github.com/ToCSharp/AsyncChromeDriverExamplesAndTests/tree/master/AsyncWebDriver.SeleniumAdapter.Common.Tests), which we can run to test functionality of all projects and its connections.


### Examples
Look at AsyncFirefoxDriverExample, AllDriversExample, [AsyncChromeDriverExample](https://github.com/ToCSharp/AsyncChromeDriver/tree/master/AsyncChromeDriverExample).

Run built Example in release tab.

### [AsyncFirefoxDriverExtensions](https://github.com/ToCSharp/AsyncFirefoxDriverExtensions)
* LiveIp to get ip
* LivePreferences to view and edit Firefox preferences of running profile
* AddonManager have methods GetAddonsList, InstallAddon, InstallTemporaryAddon, UninstallAddon.

