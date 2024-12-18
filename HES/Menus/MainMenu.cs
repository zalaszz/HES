﻿
using HES.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HES.Menus
{
    class MainMenu : HESMenu, IResourceProvider
    {
        private const string _RESOURCE = @"data.csv";
        private const string _RESOURCE_IN_DIR = @"\in\";
        private const string _RESOURCE_OUT_DIR = @"\out\";

        public MainMenu():base("Username", "Password", "Cifs", "Start Date", "End Date") { }

        public override void Start()
        {
            base.Start();

            if (HESFile.HasDir(_RESOURCE_IN_DIR) && HESFile.HasFile(_RESOURCE_IN_DIR + _RESOURCE))
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

        public void SetAdditionalFieldsValues(List<string> data)
        {
            for (int i = 2; i < fields.Count; i++) // Starting from number 2 because we don't want to set the login fields
            {
                fields[fields.ElementAt(i).Key] = data[i - 2]; // Minus 2 so we can start from the beginning of the list
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

        public Dictionary<string, string> GetFields()
        {
            return fields;
        }

        public void GetResource()
        {
            if (!HESFile.HasDir(_RESOURCE_IN_DIR) && !HESFile.HasFile(_RESOURCE_IN_DIR + _RESOURCE))
            {
                HESFile.CreateDirIfRequired(_RESOURCE_IN_DIR);
                HESFile.CreateDirIfRequired(_RESOURCE_OUT_DIR);
                return;
            }

            SetAdditionalFieldsValues(HESFile.ReadCsvData(HESFile.GetPath() + _RESOURCE));
        }
    }
}
