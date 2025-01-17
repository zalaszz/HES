using HES.Menus.Fields;
using System;
using System.Linq;

/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

namespace HES.Menus
{
    class WindowMenu : HESMenu
    {
        public override void Start()
        {
            base.Start();

            if (GetMenuFieldContainer().CountAllFields() <= 0) // This ensures the menu doesn't repeatedly fetch window data every time it reloads, improving efficiency
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
                string windowTextFormatted = windowText.Length > 35 ? $"{windowText.Substring(0, 35)}..." : windowText;
                string hwd = HESWindow.GetAllWindows().ElementAt(i).Key.ToString();
                MenuField field = new MenuField() { name = windowTextFormatted, category = Category.Additional, type = FieldType.Text };
                field.SetValue(hwd);

                fields.AdditionalFields.Add(field);
            }
        }

        private void DefaultMenu()
        {
            GenerateMenu((field, index) => {
                string windowText = $"{field.name.ToLower()}";
                string label = ((index + 1) % 2).Equals(0) ? $"{windowText}\n" : String.Format("{0,-40}", $"{windowText}");
                HESConsole.Write("[", $"{index}", "] ", ConsoleColor.Green);
                HESConsole.Write(label, ConsoleColor.White);
                if (index.Equals(GetMenuFieldContainer().CountAllFields() - 1)) // Check if this is the last iteration
                {
                    HESConsole.Write("\n", "Choose a window", "> ", (ConsoleColor)new Random().Next(1, 15));
                }
            }, false);
        }

        public int GetWindowHandlerFromUserInput()
        {
            return int.Parse(GetFields().ElementAt(int.Parse(UserSelectedInput)).GetValue() as string);
        }
    }
}
