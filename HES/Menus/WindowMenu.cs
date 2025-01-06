﻿
using HES.Menus.Fields;
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

            if (GetMenuFieldContainer().CountAllFields() <= 0)
            {
                SetAllFieldsValues(SetAllFieldsValuesImpl);
            }

            DefaultMenu();
            Console.Clear();
        }

        private void SetAllFieldsValuesImpl(MenuFieldsContainer fields)
        {
            for (int i = 0; i < HESWindow.GetAllWindows().Count; i++)
            {
                string windowText = HESWindow.GetAllWindows().ElementAt(i).Value.ToString();
                string value = windowText.Length > 35 ? $"{windowText.Substring(0, 35)}..." : windowText;
                string hwd = HESWindow.GetAllWindows().ElementAt(i).Key.ToString();
                MenuField field = new MenuField() { name = hwd, category = Category.Additional };
                field.SetValue(value);

                fields.AdditionalFields.Add(field);
            }
        }

        private void DefaultMenu()
        {
            PrintFieldsToConsole((field, index) => {
                string windowText = $"{field.GetValue<string>().ToLower()}";
                string label = ((index + 1) % 2).Equals(0) ? $"{windowText}\n" : String.Format("{0,-40}", $"{windowText}");
                HESConsole.Write("[", $"{index}", "] ", ConsoleColor.Green);
                HESConsole.Write(label, ConsoleColor.White);
                if (index.Equals(GetMenuFieldContainer().CountAllFields() - 1)) // Check if this is the last iteration
                {
                    HESConsole.Write("\n", "Choose a window", "> ", (ConsoleColor)new Random().Next(1, 15));
                }
            }, false);
            UserInput = InterceptUserKeystrokes(AllowOnlyNumbersImpl);
        }

        public int GetWindowHandlerFromUserInput()
        {
            return int.Parse(GetFields().ElementAt(int.Parse(UserInput)).GetValue<string>());
        }
    }
}
