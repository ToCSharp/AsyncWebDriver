// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Zu.Firefox
{
    public class FirefoxPreferences
    {
        static  Dictionary<string, string> defaultPrefs;
        public static Dictionary<string, string> DefaultPrefs
        {
            get
            {
                if(defaultPrefs == null)
                {
                    //https://github.com/mozilla/geckodriver/releases -> Source code -> prefs.rs
                    // ' ("' -> ' {"'
                    // Pref::new( -> '"'
                    // '"},' -> '"},'
                    defaultPrefs = new Dictionary<string, string>
                    {
        // Disable automatic downloading of new releases
        {"app.update.auto", "false"},

        // Disable automatically upgrading Firefox
        {"app.update.enabled", "false"},

        // Increase the APZ content response timeout in tests to 1
        // minute.  This is to accommodate the fact that test environments
        // tends to be slower than production environments (with the
        // b2g emulator being the slowest of them all), resulting in the
        // production timeout value sometimes being exceeded and causing
        // false-positive test failures.
        //
        // (bug 1176798, bug 1177018, bug 1210465)
        {"apz.content_response_timeout", "60000"},

        // Enable the dump function, which sends messages to the system
        // console
        {"browser.dom.window.dump.enabled", "true"},

        // Indicate that the download panel has been shown once so
        // that whichever download test runs first does not show the popup
        // inconsistently
        {"browser.download.panel.shown", "true"},

        // Implicitly accept license
        {"browser.EULA.override", "true"},

        // use about:blank as new tab page
        {"browser.newtabpage.enabled", "false"},

        // Assume the about:newtab pages intro panels have been shown
        // to not depend on which test runs first and happens to open
        // about:newtab
        {"browser.newtabpage.introShown", "true"},

        // Never start the browser in offline mode
        {"browser.offline", "false"},

        // Background thumbnails in particular cause grief, and disabling
        // thumbnails in general cannot hurt
        {"browser.pagethumbnails.capturing_disabled", "true"},

        // Avoid performing Reader Mode intros during tests
        {"browser.reader.detectedFirstArticle", "true"},

        // Disable safebrowsing components
        {"browser.safebrowsing.blockedURIs.enabled", "false"},
        {"browser.safebrowsing.downloads.enabled", "false"},
        {"browser.safebrowsing.passwords.enabled", "false"},
        {"browser.safebrowsing.malware.enabled", "false"},
        {"browser.safebrowsing.phishing.enabled", "false"},

        // Disable updates to search engines
        {"browser.search.update", "false"},

        // Do not restore the last open set of tabs if the browser crashed
        {"browser.sessionstore.resume_from_crash", "false"},

        // Skip check for default browser on startup
        {"browser.shell.checkDefaultBrowser", "false"},

        // Do not warn when quitting with multiple tabs
        {"browser.showQuitWarning", "false"},

        // Disable Android snippets
        {"browser.snippets.enabled", "false"},
        {"browser.snippets.syncPromo.enabled", "false"},
        {"browser.snippets.firstrunHomepage.enabled", "false"},

        // Do not redirect user when a milestone upgrade of Firefox
        // is detected
        {"browser.startup.homepage_override.mstone", "\"ignore\""},

        // Start with a blank page (about:blank)
        {"browser.startup.page", "0"},

        // Disable tab animation
        {"browser.tabs.animate", "false"},

        // Do not warn when quitting a window with multiple tabs
        {"browser.tabs.closeWindowWithLastTab", "false"},

        // Do not allow background tabs to be zombified, otherwise for
        // tests that open additional tabs, the test harness tab itself
        // might get unloaded
        {"browser.tabs.disableBackgroundZombification", "false"},

        // Do not warn on exit when multiple tabs are open
        {"browser.tabs.warnOnClose", "false"},

        // Do not warn when closing all other open tabs
        {"browser.tabs.warnOnCloseOtherTabs", "false"},

        // Do not warn when multiple tabs will be opened
        {"browser.tabs.warnOnOpen", "false"},

        // Disable first run splash page on Windows 10
        {"browser.usedOnWindows10.introURL", "\"\""},

        // Disable the UI tour
        {"browser.uitour.enabled", "false"},

        // Do not warn on quitting Firefox
        {"browser.warnOnQuit", "false"},

        // Do not show datareporting policy notifications which can
        // interfere with tests
        {"datareporting.healthreport.about.reportUrl", "\"http://%(server)s/dummy/abouthealthreport/\""},
        {"datareporting.healthreport.documentServerURI", "\"http://%(server)s/dummy/healthreport/\""},
        {"datareporting.healthreport.logging.consoleEnabled", "false"},
        {"datareporting.healthreport.service.enabled", "false"},
        {"datareporting.healthreport.service.firstRun", "false"},
        {"datareporting.healthreport.uploadEnabled", "false"},
        {"datareporting.policy.dataSubmissionEnabled", "false"},
        {"datareporting.policy.dataSubmissionPolicyAccepted", "false"},
        {"datareporting.policy.dataSubmissionPolicyBypassNotification", "true"},

        // Disable popup-blocker
        {"dom.disable_open_during_load", "false"},

        // Enabling the support for File object creation in the content process
        {"dom.file.createInChild", "true"},

        // Disable the ProcessHangMonitor
        {"dom.ipc.reportProcessHangs", "false"},

        // Disable slow script dialogues
        {"dom.max_chrome_script_run_time", "0"},
        {"dom.max_script_run_time", "0"},

        // Only load extensions from the application and user profile
        // AddonManager.SCOPE_PROFILE + AddonManager.SCOPE_APPLICATION
        {"extensions.autoDisableScopes", "0"},
        {"extensions.enabledScopes", "5"},

        // don't block add-ons for e10s
        {"extensions.e10sBlocksEnabling", "false"},

        // Disable metadata caching for installed add-ons by default
        {"extensions.getAddons.cache.enabled", "false"},

        // Disable intalling any distribution extensions or add-ons
        {"extensions.installDistroAddons", "false"},

        // Make sure Shield doesn't hit the network.
        {"extensions.shield-recipe-client.api_url", "\"\""},

        {"extensions.showMismatchUI", "false"},

        // Turn off extension updates so they do not bother tests
        {"extensions.update.enabled", "false"},
        {"extensions.update.notifyUser", "false"},

        // Make sure opening about:addons will not hit the network
        {"extensions.webservice.discoverURL", "\"http://%(server)s/dummy/discoveryURL\""},

        // Allow the application to have focus even it runs in the
        // background
        {"focusmanager.testmode", "true"},

        // Disable useragent updates
        {"general.useragent.updates.enabled", "false"},

        // Always use network provider for geolocation tests so we bypass
        // the macOS dialog raised by the corelocation provider
        {"geo.provider.testing", "true"},

        // Do not scan wi-fi
        {"geo.wifi.scan", "false"},

        // No hang monitor
        {"hangmonitor.timeout", "0"},

        // Show chrome errors and warnings in the error console
        {"javascript.options.showInConsole", "true"},

        // Make sure the disk cache does not get auto disabled
        {"network.http.bypass-cachelock-threshold", "200000"},

        // Do not prompt with long usernames or passwords in URLs
        {"network.http.phishy-userpass-length", "255"},

        // Do not prompt for temporary redirects
        {"network.http.prompt-temp-redirect", "false"},

        // Disable speculative connections so they are not reported as
        // leaking when they are hanging around
        {"network.http.speculative-parallel-limit", "0"},

        // Do not automatically switch between offline and online
        {"network.manage-offline-status", "false"},

        // Make sure SNTP requests do not hit the network
        {"network.sntp.pools", "\"%(server)s\""},

        // Disable Flash.  The plugin container it is run in is
        // causing problems when quitting Firefox from geckodriver,
        // c.f. https://github.com/mozilla/geckodriver/issues/225.
        {"plugin.state.flash", "0"},

        // Local documents have access to all other local docments,
        // including directory listings.
        {"security.fileuri.strict_origin_policy", "false"},

        // Tests don't wait for the notification button security delay
        {"security.notification_enable_delay", "0"},

        // Ensure blocklist updates don't hit the network
        {"services.settings.server", "\"http://%(server)s/dummy/blocklist/\""},

        // Do not automatically fill sign-in forms with known usernames
        // and passwords
        {"signon.autofillForms", "false"},

        // Disable password capture, so that tests that include forms
        // are not influenced by the presence of the persistent doorhanger
        // notification
        {"signon.rememberSignons", "false"},

        // Disable first run pages
        {"startup.homepage_welcome_url", "\"about:blank\""},
        {"startup.homepage_welcome_url.additional", "\"\""},

        // Prevent starting into safe mode after application crashes
        {"toolkit.startup.max_resumed_crashes", "-1"},

        // We want to collect telemetry, but we don't want to send in the results
        {"toolkit.telemetry.server", "\"https://%(server)s/dummy/telemetry/\""},
                    };
                }
                return defaultPrefs;
            }
            set
            {
                defaultPrefs = value;
            }
        }
    }
}