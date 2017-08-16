// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zu.FileUtils;

namespace Zu.Firefox
{
    public static class FirefoxProfilesWorker
    {
        public static string PreferencesFileName = "prefs.js";
        public static string UserPreferencesFileName = "user.js";

        public static string FirefoxBinaryFileName; // = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";
        public static string FirefoxDeveloperBinaryFileName;
        public static string FirefoxProfilesDir = @"c:\firefox\profiles\";

        static FirefoxProfilesWorker()
        {
            if (File.Exists(@"C:\Program Files (x86)\Mozilla Firefox\firefox.exe"))
                FirefoxBinaryFileName = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";
            else if (File.Exists(@"C:\Program Files\Mozilla Firefox\firefox.exe"))
                FirefoxBinaryFileName = @"C:\Program Files\Mozilla Firefox\firefox.exe";
            if (File.Exists(@"C:\Program Files (x86)\Firefox Developer Edition\firefox.exe"))
            {
                FirefoxDeveloperBinaryFileName = @"C:\Program Files (x86)\Firefox Developer Edition\firefox.exe";
            }
            else if (File.Exists(@"C:\Program Files\Firefox Developer Edition\firefox.exe"))
            {
                FirefoxDeveloperBinaryFileName = @"C:\Program Files\Firefox Developer Edition\firefox.exe";
            }
        }

        public static int OpenFirefoxProfileTimoutMs { get; set; } = 10000;

        public static Dictionary<string, string> GetProfiles()
        {
            var profiles = new Dictionary<string, string>();
            var userDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appDataDirectory = Path.Combine(userDir, "Mozilla", "Firefox");
            var profilesIniFile = Path.Combine(appDataDirectory, "profiles.ini");
            if (File.Exists(profilesIniFile))
            {
                var reader = new IniFileReader(profilesIniFile);
                var sectionNames = reader.SectionNames;
                foreach (var sectionName in sectionNames)
                    if (sectionName.StartsWith("profile", StringComparison.OrdinalIgnoreCase))
                    {
                        var name = reader.GetValue(sectionName, "name");
                        var isRelative = reader.GetValue(sectionName, "isrelative") == "1";
                        var profilePath = reader.GetValue(sectionName, "path");
                        var fullPath = string.Empty;
                        if (isRelative)
                            fullPath = Path.Combine(appDataDirectory, profilePath);
                        else
                            fullPath = profilePath;

                        profiles.Add(name, fullPath);
                    }
            }
            return profiles;
        }

        public static string GetProfileDir(string profileName)
        {
            return GetProfiles().FirstOrDefault(v => v.Key == profileName).Value;
        }

        public static Process OpenFirefoxProfile(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return null;
            var process = new Process();
            process.StartInfo.FileName =
                FirefoxBinaryFileName; // @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";
            process.StartInfo.Arguments = $@"-marionette -no-remote -P ""{key}""";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();

            Thread.Sleep(1000);

            // wait for closing previos Firefox
            if (process.MainWindowTitle != "" && process.MainWindowTitle != "Mozilla Firefox")
            {
                var reader = process.StandardOutput;
                var v = reader.ReadToEnd();
            }
            return process;
        }
        public static Process OpenFirefoxDeveloperProfile(string key)
        {
            var process = new Process();
            process.StartInfo.FileName = FirefoxDeveloperBinaryFileName;
            process.StartInfo.Arguments = $@"-marionette -no-remote -P ""{key}""";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            Thread.Sleep(1000);

            // wait for closing previos Firefox
            if (process.MainWindowTitle != "" && process.MainWindowTitle != "Firefox Developer Edition")
            {
                var reader = process.StandardOutput;
                var v = reader.ReadToEnd();
            }
            return process;
        }

        public static Process OpenFirefoxProfileOffline(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return null;
            var process = new Process();
            process.StartInfo.FileName = FirefoxBinaryFileName;
            process.StartInfo.Arguments = $@"-marionette -no-remote -P ""{key}"" -offline";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            Thread.Sleep(1000);

            if (process.MainWindowTitle != "" && process.MainWindowTitle != "Mozilla Firefox")
            {
                var reader = process.StandardOutput;
                var v = reader.ReadToEnd();
            }
            return process;
        }
        public static Process OpenFirefoxDeveloperProfileOffline(string key)
        {
            var process = new Process();
            process.StartInfo.FileName = FirefoxDeveloperBinaryFileName;
            process.StartInfo.Arguments = $@"-marionette -no-remote -P ""{key}"" -offline";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            Thread.Sleep(1000);

            // wait for closing previos Firefox
            if (process.MainWindowTitle != "" && process.MainWindowTitle != "Firefox Developer Edition")
            {
                var reader = process.StandardOutput;
                var v = reader.ReadToEnd();
            }
            return process;
        }
        //public static string LastError;

        public static async Task<string> CreateFirefoxProfile(string profileDir, string profileName)
        {
            if (Directory.Exists(profileDir)) return "profileDir exist";
            await Task.Factory.StartNew(() =>
            {
                var process = new Process();
                process.StartInfo.FileName = FirefoxBinaryFileName;
                process.StartInfo.Arguments = $@"-CreateProfile ""{profileName} {profileDir}""";
                process.StartInfo.UseShellExecute = false;
                process.Start();
                Thread.Sleep(5000);
                process.Close();
            });
            return "ok";
        }


        public static List<KeyValuePairVM> ReadAllPreferences(string dir)
        {
            return ReadUserPreferencesFromDir(dir).Select(v => new KeyValuePairVM(v))
                .Concat(ReadExistingPreferences(dir).Select(v2 => new KeyValuePairVM(v2))).ToList();
        }

        public static Dictionary<string, string> ReadExistingPreferences(string dir)
        {
            var prefs = new Dictionary<string, string>();

            try
            {
                if (!string.IsNullOrEmpty(dir))
                {
                    var userPrefs = Path.Combine(dir, PreferencesFileName);
                    if (File.Exists(userPrefs))
                    {
                        var fileLines = File.ReadAllLines(userPrefs);
                        foreach (var line in fileLines)
                            if (line.StartsWith("user_pref(\"", StringComparison.OrdinalIgnoreCase))
                            {
                                var parsedLine = line.Substring("user_pref(\"".Length).Trim();
                                parsedLine = parsedLine.Substring(0, parsedLine.Length - ");".Length);
                                var i = parsedLine.IndexOf(',');
                                if (i < 1) continue;
                                var part1 = parsedLine.Substring(0, i - 1).Trim().Trim('"').Trim();
                                var part2 = parsedLine.Substring(i + 1).Trim();
                                prefs.Add(part1, part2);
                            }
                    }
                }
            }
            catch (IOException e)
            {
            }

            return prefs;
        }

        public static void SetMarionettePort(string profileName, int port)
        {
            var profileDir = GetProfileDir(profileName);
            if (!string.IsNullOrWhiteSpace(profileDir))
            {
                //AddWriteUserPreference(profileDir, "marionette.defaultPrefs.port", port.ToString());
                AddWriteUserPreference(profileDir, "marionette.port", port.ToString());
                //AddWriteUserPreference(profileDir, "marionette.defaultPrefs.enabled", "true");
            }
        }

        public static Dictionary<string, string> ReadUserPreferences(string profileName)
        {
            return ReadUserPreferencesFromDir(GetProfileDir(profileName));
        }

        public static Dictionary<string, string> ReadUserPreferencesFromDir(string dir)
        {
            var prefs = new Dictionary<string, string>();

            try
            {
                if (!string.IsNullOrEmpty(dir))
                {
                    var userPrefs = Path.Combine(dir, UserPreferencesFileName);
                    if (File.Exists(userPrefs))
                    {
                        var fileLines = File.ReadAllLines(userPrefs);
                        foreach (var item in fileLines)
                        {
                            var line = item.TrimStart();
                            if (line.StartsWith("user_pref(\"", StringComparison.OrdinalIgnoreCase))
                            {
                                var parsedLine = line.Substring("user_pref(\"".Length).Trim();
                                parsedLine = parsedLine.Substring(0, parsedLine.Length - ");".Length);
                                var i = parsedLine.IndexOf(',');
                                if (i < 1) continue;
                                var part1 = parsedLine.Substring(0, i - 1).Trim().Trim('"').Trim();
                                var part2 = parsedLine.Substring(i + 1).Trim();
                                prefs.Add(part1, part2);
                            }
                        }
                    }
                }
            }
            catch (IOException e)
            {
            }

            return prefs;
        }

        public static void AddUserPreference(ref Dictionary<string, string> prefs, string key, string val)
        {
            if (prefs == null) prefs = new Dictionary<string, string>();
            if (prefs.ContainsKey(key))
                prefs[key] = val;
            else
                prefs.Add(key, val);
        }


        public static void AddWriteUserPreference(string dir, string key, string val)
        {
            var prefs = ReadUserPreferencesFromDir(dir);
            if (prefs.ContainsKey(key))
                prefs[key] = val;
            else
                prefs.Add(key, val);
            WriteUserPreferences(dir, prefs);
        }

        public static void AddWriteUserPreferences(string dir, Dictionary<string, string> newPrefs)
        {
            var prefs = ReadUserPreferencesFromDir(dir);
            foreach (var item in newPrefs)
                if (prefs.ContainsKey(item.Key))
                    prefs[item.Key] = item.Value;
                else
                    prefs.Add(item.Key, item.Value);
            WriteUserPreferences(dir, prefs);
        }

        private static void WriteUserPreferences(string dir, Dictionary<string, string> prefs)
        {
            var f = Path.Combine(dir, UserPreferencesFileName);
            File.WriteAllText(f,
                string.Join(Environment.NewLine, prefs.Select(v => $"user_pref(\"{v.Key}\", {v.Value});")));
        }


        public static void AddUserPreferenceForDownload(ref Dictionary<string, string> currentUserPrefs,
            string dir = "C:\\WebDriverDownloads")
        {
            dir = dir.Trim().Trim('"');
            var mimeTypesStr =
                @"text/html, application/octet-stream, application/zip, application/x-zip, application/x-compressed, application/x-zip-compressed, application/x-rar-compressed";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            AddUserPreference(ref currentUserPrefs, "browser.download.dir", $"\"{dir}\"");
            AddUserPreference(ref currentUserPrefs, "browser.download.folderList", "2");
            AddUserPreference(ref currentUserPrefs, "browser.helperApps.neverAsk.saveToDisk",
                $"\"{mimeTypesStr}\""); // "\"application/x-compressed;\"");
            AddUserPreference(ref currentUserPrefs, "browser.download.manager.showWhenStarting", "false");
            AddUserPreference(ref currentUserPrefs, "pdfjs.disabled", "true");
        }

        public static void CreateProfiles(string firefoxProfileStartName = "web", int startPort = 12200, int count = 20)
        {
            for (var i = 1; i <= count; i++)
            {
                var profileDir = $@"{FirefoxProfilesDir}{firefoxProfileStartName}{i}";
                if (Directory.Exists(profileDir)) continue;

                var process = new Process();
                process.StartInfo.FileName = FirefoxBinaryFileName;
                process.StartInfo.Arguments = $@"-CreateProfile ""{firefoxProfileStartName}{i} {profileDir}""";
                process.StartInfo.UseShellExecute = false;
                process.Start();
                Thread.Sleep(5000);
                process.Close();


                Thread.Sleep(2000);

                process = new Process();
                process.StartInfo.FileName = FirefoxBinaryFileName;
                process.StartInfo.Arguments = $@"-marionette -no-remote -P ""{firefoxProfileStartName}{i}""";
                process.StartInfo.UseShellExecute = false;
                process.Start();
                Thread.Sleep(10000);
                //builder.CloseMainWindow();
            }
        }

        public static int GetMarionettePort(string profileName)
        {
            return GetMarionettePortFromProfileDir(GetProfileDir(profileName));
        }

        public static int GetMarionettePortFromProfileDir(string profileDir)
        {
            if (string.IsNullOrWhiteSpace(profileDir)) return -1;
            //var port = ReadAllPreferences(profileDir).FirstOrDefault(v => v.Name == "marionette.defaultPrefs.port");
            var port = ReadAllPreferences(profileDir).FirstOrDefault(v => v.Name == "marionette.port");
            if (port == null) return 2828;
            int p;
            if (int.TryParse(port.Val, out p)) return p;
            return 0;
        }
    }
}