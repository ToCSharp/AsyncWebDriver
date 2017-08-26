// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Copyright 2007-2011 WebDriver committers
// Copyright 2007-2011 Google Inc.
// Portions copyright 2011 Software Freedom Conservancy
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Zu.FileUtils
{
    /// <summary>
    /// Parses and reads an INI file.
    /// </summary>
    public class IniFileReader
    {
        #region Private members
        public Dictionary<string, Dictionary<string, string>> iniFileStore = new Dictionary<string, Dictionary<string, string>>();
        private string iniFilePath;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="IniFileReader"/> class.
        /// </summary>
        /// <param name="fileName">The full path to the .INI file to be read.</param>
        public IniFileReader(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName), "File name must not be null or empty");
            }
            iniFilePath = fileName;

            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("INI file not found", fileName);
            }

            Dictionary<string, string> section = new Dictionary<string, string>();
            string sectionName = string.Empty;

            string[] iniFileContent = File.ReadAllLines(fileName);
            foreach (string iniFileLine in iniFileContent)
            {
                if (!string.IsNullOrEmpty(iniFileLine.Trim()) && !iniFileLine.StartsWith(";", StringComparison.OrdinalIgnoreCase))
                {
                    if (iniFileLine.StartsWith("[", StringComparison.OrdinalIgnoreCase) && iniFileLine.EndsWith("]", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(sectionName))
                        {
                            this.iniFileStore.Add(sectionName, section);
                        }

                        sectionName = iniFileLine.Substring(1, iniFileLine.Length - 2); //.ToUpperInvariant();
                        section = new Dictionary<string, string>();
                    }
                    else
                    {
                        string[] entryParts = iniFileLine.Split(new char[] { '=' }, 2);
                        string name = entryParts[0];//.ToUpperInvariant();
                        string value = string.Empty;
                        if (entryParts.Length > 1)
                        {
                            value = entryParts[1];
                        }

                        section.Add(name, value);
                    }
                }
            }

            this.iniFileStore.Add(sectionName, section);
        }
        #endregion

        public void DeleteSecton(String sectionName)
        {
            if(iniFileStore.ContainsKey(sectionName))
            {
                iniFileStore.Remove(sectionName);
            }
        }

        public void SaveSettings(String newFilePath)
        {
            var sb = new StringBuilder();

            foreach (var section in iniFileStore)
            {
                sb.AppendLine("[" + section.Key + "]");

                foreach (var sectionPair in section.Value)
                {
                    sb.AppendLine(sectionPair.Key + "=" + sectionPair.Value);
                }

                sb.AppendLine();
            }
            File.WriteAllText(newFilePath, sb.ToString());
        }

        /// <summary>
        /// Save settings back to ini file.
        /// </summary>
        public void SaveSettings()
        {
            SaveSettings(iniFilePath);
        }


        #region Properties
        /// <summary>
        /// Gets a <see cref="ReadOnlyCollection{T}"/> containing the names of the sections in the .INI file.
        /// </summary>
        public ReadOnlyCollection<string> SectionNames
        {
            get
            {
                List<string> keyList = new List<string>(this.iniFileStore.Keys);
                return new ReadOnlyCollection<string>(keyList);
            }
        } 
        #endregion

        #region Methods
        /// <summary>
        /// Gets a value from the .INI file.
        /// </summary>
        /// <param name="sectionName">The section in which to find the key-value pair.</param>
        /// <param name="valueName">The key of the key-value pair.</param>
        /// <returns>The value associated with the given section and key.</returns>
        public string GetValue(string sectionName, string valueName)
        {
            var sn = sectionName.ToLowerInvariant();
            var vn = valueName.ToLowerInvariant();
            return iniFileStore.FirstOrDefault(v => v.Key.ToLowerInvariant() == sn).Value?.FirstOrDefault(v => v.Key.ToLowerInvariant() == vn).Value;

            //if (string.IsNullOrEmpty(sectionName))
            //{
            //    throw new ArgumentNullException(nameof(sectionName), "Section name cannot be null or empty");
            //}

            //string lowerCaseSectionName = sectionName.ToUpperInvariant();

            //if (string.IsNullOrEmpty(valueName))
            //{
            //    throw new ArgumentNullException(nameof(valueName), "Text name cannot be null or empty");
            //}

            //string lowerCaseValueName = valueName.ToUpperInvariant();

            //if (!this.iniFileStore.ContainsKey(lowerCaseSectionName))
            //{
            //    throw new ArgumentException("Section does not exist: " + sectionName, nameof(sectionName));
            //}

            //Dictionary<string, string> section = this.iniFileStore[lowerCaseSectionName];

            //if (!section.ContainsKey(lowerCaseValueName))
            //{
            //    throw new ArgumentException("Text does not exist: " + valueName, nameof(valueName));
            //}

            //return section[lowerCaseValueName];
        }
        #endregion
    }
}
