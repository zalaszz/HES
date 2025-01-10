using HES.Interfaces;
using HES.Menus.Fields;
using System;
using System.Collections.Generic;

namespace HES.Menus
{
    class MainMenu : HESMenu, IResourceProvider
    {
        private const string _RESOURCE = @"\Resources\menu.json";
        private CSVMenu _subMenu = new CSVMenu();

        public override void Start()
        {
            base.Start();

            HESFile.CreateDefaultDirsIfRequired();

            if (_subMenu.ContainsData) // Checking if the submenu has got any data from the csv file
            {
                _subMenu.StartOnce(ProfileLogin);
                return;
            }

            DefaultMenu();
        }

        private void ProfileLogin()
        {
            SetupMenu(GetMenuFieldContainer().GetLoginFields());
        }

        private void DefaultMenu()
        {
            ProfileLogin();

            SetupMenu(GetMenuFieldContainer().GetAdditionalFields());
        }

        private void SetupMenu(HashSet<MenuField> fields)
        {
            GenerateMenuFromFields(
               fields,
               ">",
               true,
               ConsoleColor.Magenta,
               ConsoleColor.Cyan
           );
        }

        public void GetResource()
        {
            // Set Menu Fields Placeholders if there's a menu.json file
            GetMenuFieldContainer().SetFieldsFromJSON(_RESOURCE);
            // Getting data from csv submenu and setting values to the fields above
            _subMenu.GetResource(this);
        }
    }
}
