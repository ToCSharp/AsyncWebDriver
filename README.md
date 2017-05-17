## AsyncWebDriver

[![Join the chat at https://gitter.im/AsyncWebDriver/Lobby](https://badges.gitter.im/AsyncWebDriver/Lobby.svg)](https://gitter.im/AsyncWebDriver/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
AsyncWebDriver is .Net WebDriver rewritten to async/await pattern from [selenium](https://github.com/SeleniumHQ/selenium) project.
It is base for browser drivers.
AsyncWebDriver has synchronous wrapper SyncWebDriver.

# AsyncFirefoxDriver
It is Firefox driver. It connects directly to Marionette and is async from this connection. No need in geckodriver.
AsyncFirefoxDriver implements IWebBrowserClient and can be used as AsyncWebDriver. 

But the main MISSION of this project is to provide Firefox specific capabilities.
Reqest listener and profiles worker already here.
Properties, debugger, all what addons and extensions can do, we can do with AsyncFirefoxDriver (coming soon).

## Other browsers
AsyncFirefoxDriver is the only one so far. 
For now we cannot provide something new compared to selenium. 
If it will be interestring we can add drivers with async/await pattern.

## Usage
### Install AsyncFirefoxDriver via NuGet

If you want to include AsyncFirefoxDriver in your project, you can [install it directly from NuGet](https://www.nuget.org/packages/AsyncFirefoxDriver/)
```
PM> Install-Package AsyncFirefoxDriver
```
### Write code example
```csharp
            var profileName = "default";
            if (FirefoxProfilesWorker.GetMarionettePort(profileName) == 0)
                FirefoxProfilesWorker.SetMarionettePort(profileName, 5432);
            FirefoxProfilesWorker.OpenFirefoxProfile(profileName);
            var firefoxDriver = new AsyncFirefoxDriver(profileName);
            await firefoxDriver.Connect();
            var webDriver = new WebDriver(firefoxDriver);
            await webDriver.SetContextContent();
            await webDriver.GoToUrl("https://www.google.com/");
            var query = await webDriver.FindElement(By.Name("q"));
            foreach (var v in "ToCSharp".ToList())
            {
                await Task.Delay(500 + new Random().Next(500));
                await query.SendKeys(v.ToString());
            }
            await Task.Delay(500);
            await query.SendKeys(Keys.Enter);
```
Add usings
```csharp
using Zu.AsyncWebDriver;
using Zu.AsyncWebDriver.Remote;
using Zu.Firefox;
```

## Examples
Look at AsyncFirefoxDriverExample

## ZuRequestListener
ZuRequestListener is AsyncFirefoxDriver extension. 
It is exemple how to extend WebDriver functionality.
