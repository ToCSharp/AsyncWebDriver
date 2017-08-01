// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Zu.AsyncWebDriver
{
    /// <summary>
    ///     Representations of keys able to be pressed that are not text keys for sending to the browser.
    /// </summary>
    public static class Keys
    {
        /// <summary>
        ///     Represents the NUL keystroke.
        /// </summary>
        public static readonly string Null = Convert.ToString(Convert.ToChar(0xE000, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the Cancel keystroke.
        /// </summary>
        public static readonly string Cancel = Convert.ToString(Convert.ToChar(0xE001, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the Help keystroke.
        /// </summary>
        public static readonly string Help = Convert.ToString(Convert.ToChar(0xE002, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the Backspace key.
        /// </summary>
        public static readonly string Backspace = Convert.ToString(Convert.ToChar(0xE003, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the Tab key.
        /// </summary>
        public static readonly string Tab = Convert.ToString(Convert.ToChar(0xE004, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the Clear keystroke.
        /// </summary>
        public static readonly string Clear = Convert.ToString(Convert.ToChar(0xE005, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the Return key.
        /// </summary>
        public static readonly string Return = Convert.ToString(Convert.ToChar(0xE006, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the Enter key.
        /// </summary>
        public static readonly string Enter = Convert.ToString(Convert.ToChar(0xE007, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the Shift key.
        /// </summary>
        public static readonly string Shift = Convert.ToString(Convert.ToChar(0xE008, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the Shift key.
        /// </summary>
        public static readonly string LeftShift = Convert.ToString(Convert.ToChar(0xE008, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture); // alias

        /// <summary>
        ///     Represents the Control key.
        /// </summary>
        public static readonly string Control = Convert.ToString(Convert.ToChar(0xE009, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the Control key.
        /// </summary>
        public static readonly string LeftControl =
            Convert.ToString(Convert.ToChar(0xE009, CultureInfo.InvariantCulture),
                CultureInfo.InvariantCulture); // alias

        /// <summary>
        ///     Represents the Alt key.
        /// </summary>
        public static readonly string Alt = Convert.ToString(Convert.ToChar(0xE00A, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the Alt key.
        /// </summary>
        public static readonly string LeftAlt = Convert.ToString(Convert.ToChar(0xE00A, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture); // alias

        /// <summary>
        ///     Represents the Pause key.
        /// </summary>
        public static readonly string Pause = Convert.ToString(Convert.ToChar(0xE00B, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the Escape key.
        /// </summary>
        public static readonly string Escape = Convert.ToString(Convert.ToChar(0xE00C, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the Spacebar key.
        /// </summary>
        public static readonly string Space = Convert.ToString(Convert.ToChar(0xE00D, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the Page Up key.
        /// </summary>
        public static readonly string PageUp = Convert.ToString(Convert.ToChar(0xE00E, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the Page Down key.
        /// </summary>
        public static readonly string PageDown = Convert.ToString(Convert.ToChar(0xE00F, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the End key.
        /// </summary>
        public static readonly string End = Convert.ToString(Convert.ToChar(0xE010, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the Home key.
        /// </summary>
        public static readonly string Home = Convert.ToString(Convert.ToChar(0xE011, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the left arrow key.
        /// </summary>
        public static readonly string Left = Convert.ToString(Convert.ToChar(0xE012, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the left arrow key.
        /// </summary>
        public static readonly string ArrowLeft = Convert.ToString(Convert.ToChar(0xE012, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture); // alias

        /// <summary>
        ///     Represents the up arrow key.
        /// </summary>
        public static readonly string Up = Convert.ToString(Convert.ToChar(0xE013, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the up arrow key.
        /// </summary>
        public static readonly string ArrowUp = Convert.ToString(Convert.ToChar(0xE013, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture); // alias

        /// <summary>
        ///     Represents the right arrow key.
        /// </summary>
        public static readonly string Right = Convert.ToString(Convert.ToChar(0xE014, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the right arrow key.
        /// </summary>
        public static readonly string ArrowRight =
            Convert.ToString(Convert.ToChar(0xE014, CultureInfo.InvariantCulture),
                CultureInfo.InvariantCulture); // alias

        /// <summary>
        ///     Represents the Left arrow key.
        /// </summary>
        public static readonly string Down = Convert.ToString(Convert.ToChar(0xE015, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the Left arrow key.
        /// </summary>
        public static readonly string ArrowDown = Convert.ToString(Convert.ToChar(0xE015, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture); // alias

        /// <summary>
        ///     Represents the Insert key.
        /// </summary>
        public static readonly string Insert = Convert.ToString(Convert.ToChar(0xE016, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the Delete key.
        /// </summary>
        public static readonly string Delete = Convert.ToString(Convert.ToChar(0xE017, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the semi-colon key.
        /// </summary>
        public static readonly string Semicolon = Convert.ToString(Convert.ToChar(0xE018, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the equal sign key.
        /// </summary>
        public static readonly string Equal = Convert.ToString(Convert.ToChar(0xE019, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        // Number pad keys
        /// <summary>
        ///     Represents the number pad 0 key.
        /// </summary>
        public static readonly string NumberPad0 =
            Convert.ToString(Convert.ToChar(0xE01A, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the number pad 1 key.
        /// </summary>
        public static readonly string NumberPad1 =
            Convert.ToString(Convert.ToChar(0xE01B, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the number pad 2 key.
        /// </summary>
        public static readonly string NumberPad2 =
            Convert.ToString(Convert.ToChar(0xE01C, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the number pad 3 key.
        /// </summary>
        public static readonly string NumberPad3 =
            Convert.ToString(Convert.ToChar(0xE01D, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the number pad 4 key.
        /// </summary>
        public static readonly string NumberPad4 =
            Convert.ToString(Convert.ToChar(0xE01E, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the number pad 5 key.
        /// </summary>
        public static readonly string NumberPad5 =
            Convert.ToString(Convert.ToChar(0xE01F, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the number pad 6 key.
        /// </summary>
        public static readonly string NumberPad6 =
            Convert.ToString(Convert.ToChar(0xE020, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the number pad 7 key.
        /// </summary>
        public static readonly string NumberPad7 =
            Convert.ToString(Convert.ToChar(0xE021, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the number pad 8 key.
        /// </summary>
        public static readonly string NumberPad8 =
            Convert.ToString(Convert.ToChar(0xE022, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the number pad 9 key.
        /// </summary>
        public static readonly string NumberPad9 =
            Convert.ToString(Convert.ToChar(0xE023, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the number pad multiplication key.
        /// </summary>
        public static readonly string Multiply = Convert.ToString(Convert.ToChar(0xE024, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the number pad addition key.
        /// </summary>
        public static readonly string Add = Convert.ToString(Convert.ToChar(0xE025, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the number pad thousands separator key.
        /// </summary>
        public static readonly string Separator = Convert.ToString(Convert.ToChar(0xE026, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the number pad subtraction key.
        /// </summary>
        public static readonly string Subtract = Convert.ToString(Convert.ToChar(0xE027, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the number pad decimal separator key.
        /// </summary>
        public static readonly string Decimal = Convert.ToString(Convert.ToChar(0xE028, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the number pad division key.
        /// </summary>
        public static readonly string Divide = Convert.ToString(Convert.ToChar(0xE029, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        // Function keys
        /// <summary>
        ///     Represents the function key F1.
        /// </summary>
        public static readonly string F1 = Convert.ToString(Convert.ToChar(0xE031, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the function key F2.
        /// </summary>
        public static readonly string F2 = Convert.ToString(Convert.ToChar(0xE032, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the function key F3.
        /// </summary>
        public static readonly string F3 = Convert.ToString(Convert.ToChar(0xE033, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the function key F4.
        /// </summary>
        public static readonly string F4 = Convert.ToString(Convert.ToChar(0xE034, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the function key F5.
        /// </summary>
        public static readonly string F5 = Convert.ToString(Convert.ToChar(0xE035, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the function key F6.
        /// </summary>
        public static readonly string F6 = Convert.ToString(Convert.ToChar(0xE036, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the function key F7.
        /// </summary>
        public static readonly string F7 = Convert.ToString(Convert.ToChar(0xE037, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the function key F8.
        /// </summary>
        public static readonly string F8 = Convert.ToString(Convert.ToChar(0xE038, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the function key F9.
        /// </summary>
        public static readonly string F9 = Convert.ToString(Convert.ToChar(0xE039, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the function key F10.
        /// </summary>
        public static readonly string F10 = Convert.ToString(Convert.ToChar(0xE03A, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the function key F11.
        /// </summary>
        public static readonly string F11 = Convert.ToString(Convert.ToChar(0xE03B, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the function key F12.
        /// </summary>
        public static readonly string F12 = Convert.ToString(Convert.ToChar(0xE03C, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the function key META.
        /// </summary>
        public static readonly string Meta = Convert.ToString(Convert.ToChar(0xE03D, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        /// <summary>
        ///     Represents the function key COMMAND.
        /// </summary>
        public static readonly string Command = Convert.ToString(Convert.ToChar(0xE03D, CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        //System.Windows.Clipboard.SetText(string.Join(Environment.NewLine, typeof(Zu.AsyncWebDriver.Keys).GetFields().Select(v => $"\t\t\t{{ (char)0x{((int)(((string)v.GetValue(null))[0])).ToString("X")}, 0x00 }}, // {v.Name} ")))
        //https://msdn.microsoft.com/en-us/En-en/library/windows/desktop/dd375731(v=vs.85).aspx
        public static Dictionary<char, int> KeyToVirtualKeyCode = new Dictionary<char, int>
        {
            { (char)0xE000, 0x00 }, // Null 
			{ (char)0xE001, 0x03 }, // Cancel 
			{ (char)0xE002, 0x00 }, // Help 
			{ (char)0xE003, 0x08 }, // Backspace 
			{ (char)0xE004, 0x09 }, // Tab 
			{ (char)0xE005, 0x0C }, // Clear 
			{ (char)0xE006, 0x0D }, // Return 
			{ (char)0xE007, 0x0D }, // Enter 
			{ (char)0xE008, 0x10 }, // Shift 
			//{ (char)0xE008, 0x10 }, // LeftShift 
			{ (char)0xE009, 0x11 }, // Control 
			//{ (char)0xE009, 0x11 }, // LeftControl 
			{ (char)0xE00A, 0x12 }, // Alt 
			//{ (char)0xE00A, 0x12 }, // LeftAlt 
			{ (char)0xE00B, 0x13 }, // Pause 
			{ (char)0xE00C, 0x1B }, // Escape 
			{ (char)0xE00D, 0x20 }, // Space 
			{ (char)0xE00E, 0x21 }, // PageUp 
			{ (char)0xE00F, 0x22 }, // PageDown 
			{ (char)0xE010, 0x23 }, // End 
			{ (char)0xE011, 0x24 }, // Home 
			{ (char)0xE012, 0x25 }, // Left 
			//{ (char)0xE012, 0x25 }, // ArrowLeft 
			{ (char)0xE013, 0x26 }, // Up 
			//{ (char)0xE013, 0x26 }, // ArrowUp 
			{ (char)0xE014, 0x27 }, // Right 
			//{ (char)0xE014, 0x27 }, // ArrowRight 
			{ (char)0xE015, 0x28 }, // Down 
			//{ (char)0xE015, 0x28 }, // ArrowDown 
			{ (char)0xE016, 0x2D }, // Insert 
			{ (char)0xE017, 0x2E }, // Delete 
			{ (char)0xE018, 0x00 }, // Semicolon 
			{ (char)0xE019, 0x00 }, // Equal 
			{ (char)0xE01A, 0x60 }, // NumberPad0 
			{ (char)0xE01B, 0x61 }, // NumberPad1 
			{ (char)0xE01C, 0x62 }, // NumberPad2 
			{ (char)0xE01D, 0x63 }, // NumberPad3 
			{ (char)0xE01E, 0x64 }, // NumberPad4 
			{ (char)0xE01F, 0x65 }, // NumberPad5 
			{ (char)0xE020, 0x66 }, // NumberPad6 
			{ (char)0xE021, 0x67 }, // NumberPad7 
			{ (char)0xE022, 0x68 }, // NumberPad8 
			{ (char)0xE023, 0x69 }, // NumberPad9 
			{ (char)0xE024, 0x6A }, // Multiply 
			{ (char)0xE025, 0x6B }, // Add 
			{ (char)0xE026, 0x6C }, // Separator 
			{ (char)0xE027, 0x6D }, // Subtract 
			{ (char)0xE028, 0x6E }, // Decimal 
			{ (char)0xE029, 0x6F }, // Divide 
			{ (char)0xE031, 0x70 }, // F1 
			{ (char)0xE032, 0x71 }, // F2 
			{ (char)0xE033, 0x72 }, // F3 
			{ (char)0xE034, 0x73 }, // F4 
			{ (char)0xE035, 0x74 }, // F5 
			{ (char)0xE036, 0x75 }, // F6 
			{ (char)0xE037, 0x76 }, // F7 
			{ (char)0xE038, 0x77 }, // F8 
			{ (char)0xE039, 0x78 }, // F9 
			{ (char)0xE03A, 0x79 }, // F10 
			{ (char)0xE03B, 0x7A }, // F11 
			{ (char)0xE03C, 0x7B }, // F12 
			{ (char)0xE03D, 0x00 }, // Meta 
			//(char)0xE03D, 0x00 }, // Command 
        };

     

    }
}