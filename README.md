# AsyncWebDriver
AsyncWebDriver is .Net WebDriver rewritten to async/await pattern from [selenium](https://github.com/SeleniumHQ/selenium) project.
It is base for browser drivers.
AsyncWebDriver has synchronous wrapper SyncWebDriver.

# AsyncFirefoxDriver
It is Firefox driver. It connects directly to Marionette and is async from this connection. No need in geckodriver.
AsyncFirefoxDriver implements IWebBrowserClient and can be used as AsyncWebDriver. 

But the main MISSION of this project is to provide Firefox specific capabilities.
Reqest listener and profiles worker already here.
Properties, debugger, all what addons and extensions can do, we can do with AsyncFirefoxDriver (coming soon).

# Other browsers
AsyncFirefoxDriver is the only one so far. 
For now we cannot provide something new compared to selenium. 
If it will be interestring we can add drivers with async/await pattern.

# Usage
Install from NuGet (coming soon)

# Examples
Look at AsyncFirefoxDriverExample

# ZuRequestListener
ZuRequestListener is AsyncFirefoxDriver extension. 
It is exemple how to extend WebDriver functionality.
