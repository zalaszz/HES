using Figgle;
using HES.Models;
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
        private const string _BANNERSTRING = "HES";
        protected delegate void InterceptUserKeystrokesImpl(ref string data, ConsoleKeyInfo key);
        protected Dictionary<string, string> fields = new Dictionary<string, string>();

        //public HESMenu(params string[] fields)
        //{
        //    SetMenuFields(fields);
        //}

        public virtual void Start()
        {
            Banner();
            Header();
        }

        private void Banner()
        {
            for (int i = 0; i < 1; i++)
            {
                int color = new Random().Next(1, 15) + Convert.ToInt16(0 + DateTime.UtcNow.Second.ToString().Substring(0, 1));
                int color1 = new Random().Next(1, 15) - Convert.ToInt16(0 + DateTime.UtcNow.Second.ToString().Substring(1)) + 1;
                int color2 = new Random().Next(1, 15);

                for (int j = 0; j < FiggleFonts.Isometric2.Render(_BANNERSTRING[i].ToString()).Split(new[] { "\n" }, StringSplitOptions.None).Length; j++)
                {
                    string firstLetterLine = FiggleFonts.Isometric2.Render(_BANNERSTRING[i].ToString()).Split(new[] { "\r\n" }, StringSplitOptions.None)[j];
                    string secondLetterLine = FiggleFonts.Isometric2.Render(_BANNERSTRING[i + 1].ToString()).Split(new[] { "\r\n" }, StringSplitOptions.None)[j];
                    string thirdLetterLine = FiggleFonts.Isometric2.Render(_BANNERSTRING[i + 2].ToString()).Split(new[] { "\r\n" }, StringSplitOptions.None)[j];

                    HESConsole.Write(string.Format("{0,33}", firstLetterLine), (ConsoleColor)(color > 15 ? 15 : color));
                    HESConsole.Write($"{secondLetterLine}", (ConsoleColor)(color1 < 1 ? 1 : color1));
                    HESConsole.Write($"{thirdLetterLine} \n", (ConsoleColor)color2);
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

        protected void PrintFieldsToConsole(Action<KeyValuePair<string, string>, int> printImpl, bool multiAnswer)
        {
            for (int i = 0; i < fields.Count; i++)
            {
                printImpl(fields.ElementAt(i), i);
                if (fields.ElementAt(i).Key.Contains("Date"))
                {
                    fields[fields.ElementAt(i).Key] = InterceptUserKeystrokes(TtoCurrentDateImpl);
                    Console.Write("\n");
                }
                else if (fields.ElementAt(i).Key.Contains("Cifs"))
                {
                    fields[fields.ElementAt(i).Key] = InterceptUserKeystrokes(AllowOnlyNumbersImpl);
                    Console.Write("\n");
                }
                else if(multiAnswer.Equals(true))
                    fields[fields.ElementAt(i).Key] = Console.ReadLine();
            }
        }

        protected string InterceptUserKeystrokes(InterceptUserKeystrokesImpl implementation)
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

        protected void HidePasswordCredentialsImpl(ref string data, ConsoleKeyInfo key)
        {
            if (key.Key.Equals(ConsoleKey.Backspace) && data.Length > 0)
            {
                data = data.Substring(0, data.Length - 1);
            }
            else
                data += key.KeyChar;
        }

        protected void TtoCurrentDateImpl(ref string data, ConsoleKeyInfo key)
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

        protected void AllowOnlyNumbersImpl(ref string data, ConsoleKeyInfo key)
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

        public void SetAdditionalFieldsValues(List<string> data)
        {
            for (int i = 2; i < fields.Count; i++) // Starting from number 2 because we don't want to set the login fields
            {
                fields[fields.ElementAt(i).Key] = data[i - 2]; // Minus 2 so we can start from the beginning of the list
            }
        }

        public void SetAllFieldsValues(Action<Dictionary<string, string>> setAllFieldsValuesImpl)
        {
            setAllFieldsValuesImpl(fields);
        }

        public Dictionary<string, string> GetAllFields()
        {
            return fields;
        }

        public void SetMenuFieldsFromJson(string jsonFile)
        {
            if (!HESFile.HasFile(jsonFile))
            {
                SetMenuFields("Username", "Password", "Cifs", "Start Date", "End Date");
                return;
            }

            MenuDTO dto = HESFile.ReadFromFile<MenuDTO>(jsonFile);
            dto.LoginFields.ForEach(field => fields.Add(field.name, ""));
            dto.AdditionalFields.ForEach(field => fields.Add(field.name, ""));
        }

        public void SetMenuFields(params string[] fieldsToSet)
        {
            fieldsToSet.ToList().ForEach(field => fields.Add(field, ""));
        }
    }
}
