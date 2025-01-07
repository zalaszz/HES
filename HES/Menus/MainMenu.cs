using HES.Interfaces;
using HES.Menus.Fields;
using System;
using System.Linq;

namespace HES.Menus
{
    class MainMenu : HESMenu, IResourceProvider
    {
        private const string _RESOURCE = @"\Resources\menu.json";

        public override void Start()
        {
            base.Start();

            HESFile.CreateDefaultDirsIfRequired();

            if (HESFile.HasFile())
            {
                new CSVMenu().Start(ProfileLogin);
                return;
            }

            DefaultMenu();
        }

        private void ProfileLogin()
        {
            for (int i = 0; i < GetMenuFieldContainer().CountLoginFields(); i++) // Less than 2 because we only have 2 fields for the login User and Password
            {
                MenuField field = GetMenuFieldContainer().LoginFields.ElementAt(i);
                HESConsole.Write(String.Format("{0}", field.name.ToLower()), ConsoleColor.Magenta);
                HESConsole.Write("> ", ConsoleColor.Cyan);
                if (field.name.Contains("assword")) // Removing the 'P' from the word "Password" so it can identify with either 'p' or 'P'
                {
                    field.SetValue(InterceptUserKeystrokes(HidePasswordCredentialsImpl));
                    Console.Write("\n");
                    break;
                }

                field.SetValue(Console.ReadLine());
            }
        }

        private void DefaultMenu()
        {
            ProfileLogin();

            for (int i = 0; i < GetMenuFieldContainer().CountAdditionalFields(); i++)
            {
                MenuField field = GetMenuFieldContainer().AdditionalFields.ElementAt(i);
                HESConsole.Write(String.Format("{0}", field.name.ToLower()), ConsoleColor.Magenta);
                HESConsole.Write("> ", ConsoleColor.Cyan);
                if (field.name.Contains("Date"))
                {
                   field.SetValue(InterceptUserKeystrokes(TtoCurrentDateImpl));
                    Console.Write("\n");
                }
                else if (field.name.Contains("Cifs"))
                {
                    field.SetValue(InterceptUserKeystrokes(AllowOnlyNumbersImpl));
                    Console.Write("\n");
                }
                else
                    field.SetValue(Console.ReadLine());
            }
        }

        public void GetResource()
        {
            // Set Menu Fields Placeholders if there's a menu.json file
            GetMenuFieldContainer().SetFieldsFromJSON(_RESOURCE);
        }
    }
}
