
using System;
using System.Collections.Generic;
using System.Linq;

namespace HES.Menus
{
    class WindowMenu : HESMenu
    {
        public string UserInput { get; set; }
        public override void Start()
        {
            base.Start();

            if (GetAllFields().Count <= 0)
            {
                SetAllFieldsValues(SetAllFieldsValuesImpl);
            }

            DefaultMenu();
        }

        private void DefaultMenu()
        {
            PrintFieldsToConsole((field, index) => {
                string windowText = $"{field.Value.ToLower()}";
                string label = ((index + 1) % 2).Equals(0) ? $"{windowText}\n" : String.Format("{0,-40}", $"{windowText}");
                HESConsole.Write("[", $"{index}", "] ", ConsoleColor.Green);
                HESConsole.Write(label, ConsoleColor.White);
                if (index.Equals(GetAllFields().Count - 1))
                {
                    HESConsole.Write("\n", "Choose a window", "> ", ConsoleColor.White);
                }
            }, false);
            UserInput = InterceptUserKeystrokes(AllowOnlyNumbersImpl);
        }

        private void SetAllFieldsValuesImpl(Dictionary<string, string> fields)
        {
            for (int i = 0; i < HESWindow.GetAllWindows().Count; i++)
            {
                string windowText = HESWindow.GetAllWindows().ElementAt(i).Value.ToString();
                string value = windowText.Length > 35 ? $"{windowText.Substring(0, 35)}..." : windowText;
                fields.Add(HESWindow.GetAllWindows().ElementAt(i).Key.ToString(), value);
            }
        }

        public int GetWindowHandlerFromUserInput()
        {
            return int.Parse(fields.ElementAt(int.Parse(UserInput)).Key);
        }
    }
}
