
using HES.Interfaces;
using HES.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HES.Menus
{
    class MainMenu : HESMenu, IResourceProvider
    {
        private const string _RESOURCE = @"*.csv";
        private const string _RESOURCE_FIELDS = @"\Resources\menu.json";

        public override void Start()
        {
            base.Start();

            HESFile.CreateDefaultDirsIfRequired();

            if (HESFile.HasFile())
            {
                CSVFileMenu();
                return;
            }

            DefaultMenu();
        }

        private void CSVFileMenu()
        {
            HESConsole.Write("---> [", "Data File Detected - Reading data", "] <---\n\n", ConsoleColor.Green, alignSize: 80);

            ProfileLogin();
        }

        private void ProfileLogin()
        {
            Console.WriteLine(fields.Count);
            for (int i = 0; i < 2; i++) // Less than 2 because we only have 2 fields for the login User and Password
            {
                HESConsole.Write(String.Format("{0}", fields.ElementAt(i).Key.ToLower()), ConsoleColor.Magenta);
                HESConsole.Write("> ", ConsoleColor.Cyan);
                if (fields.ElementAt(i).Key.Contains("assword")) // Removing the 'P' from the word "Password" so it can identify with either 'p' or 'P'
                {
                    fields[fields.ElementAt(i).Key] = InterceptUserKeystrokes(HidePasswordCredentialsImpl);
                    Console.Write("\n");
                    break;
                }

                fields[fields.ElementAt(i).Key] = Console.ReadLine();
            }
        }

        private void DefaultMenu()
        {
            ProfileLogin();

            for (int i = 2; i < fields.Count; i++)
            {
                HESConsole.Write(String.Format("{0}", fields.ElementAt(i).Key.ToLower()), ConsoleColor.Magenta);
                HESConsole.Write("> ", ConsoleColor.Cyan);
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
                else
                    fields[fields.ElementAt(i).Key] = Console.ReadLine();
            }
        }

        public void GetResource()
        {
            // Set Menu Fields Placeholders if there's a menu.json file
            SetMenuFieldsFromJson(_RESOURCE_FIELDS);

            // Set Menu Fields Values if there's a csv file in the \IN directory
            HESFile.CreateDefaultDirsIfRequired();
            if (!HESFile.HasFile()) return;

            SetAdditionalFieldsValues(HESFile.ReadFromFile<List<string>>(_RESOURCE));
        }
    }
}
