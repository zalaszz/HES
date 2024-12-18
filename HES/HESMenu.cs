using Figgle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

namespace HES
{
    class HESMenu
    {
        private Dictionary<string, string> _fields = new Dictionary<string, string>() {
            { "Username", "" },
            { "Password", "" },
            { "Cifs", "" },
            { "Start Date", "" },
            { "End Date", "" },
        };

        public delegate void InterceptUserKeystrokesImpl(ref string data, ConsoleKeyInfo key);
        private const string _DATAFILENAME = "data.csv";
        private const string _BANNERSTRING = "HES";
        private HESFile _hESFile;

        public HESMenu()
        {
            _hESFile = new HESFile(_DATAFILENAME);
        }

        public void Start()
        {
            Banner();
            Header();

            if (_hESFile.HasInDir() && _hESFile.HasFile())
            {
                CSVFileMenu();
                return;
            }

            DefaultMenu();
        }

        private void Banner()
        {
            for (int i = 0; i < 1; i++)
            {
                int color = new Random().Next(1, 15) + Convert.ToInt16(0 + DateTime.UtcNow.Second.ToString().Substring(0, 1));
                int color1 = new Random().Next(1, 15) - Convert.ToInt16(0 + DateTime.UtcNow.Second.ToString().Substring(1)) + 1;
                int color2 = new Random().Next(1, 15);

                for (int j = 0; j < FiggleFonts.Isometric2.Render(_BANNERSTRING[i].ToString()).Split(new[] { "\r\n" }, StringSplitOptions.None).Length; j++)
                {
                    string firstLetterLine = FiggleFonts.Isometric2.Render(_BANNERSTRING[i].ToString()).Split(new[] { "\r\n" }, StringSplitOptions.None)[j];
                    string secondLetterLine = FiggleFonts.Isometric2.Render(_BANNERSTRING[i + 1].ToString()).Split(new[] { "\r\n" }, StringSplitOptions.None)[j];
                    string thirdLetterLine = FiggleFonts.Isometric2.Render(_BANNERSTRING[i + 2].ToString()).Split(new[] { "\r\n" }, StringSplitOptions.None)[j];

                    HESConsole.Write(string.Format("{0,33}", firstLetterLine), (ConsoleColor)(color > 15 ? 15 : color));
                    HESConsole.Write($"{secondLetterLine}", (ConsoleColor)(color1 < 1 ? 1 : color1));
                    HESConsole.Write($"{thirdLetterLine} \r\n", (ConsoleColor)color2);
                }
            }
        }

        private void Header()
        {
            var assembly = Assembly.GetExecutingAssembly().GetName();
            string version = assembly.Version.ToString();
            string productName = assembly.Name;
            string fullString = $"Product Name: {productName} - Version: {version} ";

            HESConsole.Write("Product Name: ", productName, " — ", ConsoleColor.Blue, alignSize: 80, stringLength: fullString.Length);
            HESConsole.Write("Version: ", version, " \n", ConsoleColor.DarkYellow);

            HESConsole.Write("Built with ", "love", " by zalasz\n", ConsoleColor.DarkRed, alignSize: 80);

            HESConsole.Write("Source code (git repo) ", "https://github.com/zalaszz/HES", "\n\n", ConsoleColor.Cyan, alignSize: 80);
        }

        private void CSVFileMenu()
        {
            HESConsole.Write("---> [", "Data File Detected - Reading data", "] <---\n\n", ConsoleColor.Green, alignSize: 80);

            ProfileLogin();
            
            SetDataFields(_hESFile.ReadCsvData());
        }

        public void SetDataFields(List<string> data)
        {
            for (int i = 2; i < _fields.Count; i++) // Starting from number 2 because we don't want to set the login fields
            {
                _fields[_fields.ElementAt(i).Key] = data[i - 2]; // Minus 2 so we can start from the beginning of the list
            }
        }

        private void ProfileLogin()
        {
            for (int i = 0; i < 2; i++) // Less than 2 because we only have 2 fields for the login User and Password
            {
                HESConsole.Write(String.Format("{0}", _fields.ElementAt(i).Key.ToLower()), ConsoleColor.Magenta);
                HESConsole.Write("> ", ConsoleColor.Cyan);
                if (_fields.ElementAt(i).Key.Contains("assword")) // Removing the 'P' from the word "Password" so it can identify with either 'p' or 'P'
                {
                    _fields[_fields.ElementAt(i).Key] = InterceptUserKeystrokes(HidePasswordCredentialsImpl);
                    Console.Write("\n");
                    break;
                }

                _fields[_fields.ElementAt(i).Key] = Console.ReadLine();
            }
        }

        private void DefaultMenu()
        {
            ProfileLogin();

            for (int i = 2; i < _fields.Count; i++)
            {
                HESConsole.Write(String.Format("{0}", _fields.ElementAt(i).Key.ToLower()), ConsoleColor.Magenta);
                HESConsole.Write("> ", ConsoleColor.Cyan);
                if (_fields.ElementAt(i).Key.Contains("Date"))
                {
                    _fields[_fields.ElementAt(i).Key] = InterceptUserKeystrokes(TtoCurrentDateImpl);
                    Console.Write("\n");
                }
                else if (_fields.ElementAt(i).Key.Contains("Cifs"))
                {
                    _fields[_fields.ElementAt(i).Key] = InterceptUserKeystrokes(AllowOnlyNumbersImpl);
                    Console.Write("\n");
                }
                else
                    _fields[_fields.ElementAt(i).Key] = Console.ReadLine();
            }
        }

        private string InterceptUserKeystrokes(InterceptUserKeystrokesImpl implementation)
        {
            string data = "";
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                    break;

                implementation(ref data, key);
            }

            return data;
        }

        private void HidePasswordCredentialsImpl(ref string data, ConsoleKeyInfo key)
        {
            if (key.Key.Equals(ConsoleKey.Backspace) && data.Length > 0)
            {
                data = data.Substring(0, data.Length - 1);
            }
            else
                data += key.KeyChar;
        }

        private void TtoCurrentDateImpl(ref string data, ConsoleKeyInfo key)
        {
            if (key.Key.Equals(ConsoleKey.Backspace) && data.Length > 0)
            {
                data = data.Substring(0, data.Length - 1);
                Console.Write("\b \b");
            }
            else if (key.Key.Equals(ConsoleKey.T))
            {
                data += DateTime.UtcNow.Date.ToString("yyyy.MM.dd");
                Console.Write(DateTime.UtcNow.Date.ToString("yyyy.MM.dd"));
            }
            else if (!char.IsLetter(key.KeyChar) && !key.KeyChar.Equals('\b'))
            {
                data += key.KeyChar;
                Console.Write(key.KeyChar);
            }
        }

        private void AllowOnlyNumbersImpl(ref string data, ConsoleKeyInfo key)
        {
            if (key.Key.Equals(ConsoleKey.Backspace) && data.Length > 0)
            {
                data = data.Substring(0, data.Length - 1);
                Console.Write("\b \b");
            }
            else if (char.IsDigit(key.KeyChar) || key.Key.Equals(ConsoleKey.Spacebar))
            {
                data += key.KeyChar;
                Console.Write(key.KeyChar);
            }
        }

        public Dictionary<string, string> GetFields()
        {
            return _fields;
        }
    }
}
