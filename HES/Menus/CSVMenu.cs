using HES.Models;
using System;

namespace HES.Menus
{
    class CSVMenu : HESMenu
    {
        private const string _RESOURCE = @"*.csv";
        public bool ContainsData { get; set; }

        public void StartOnce(Action loginMenuImpl)
        {
            Start(loginMenuImpl);
            ContainsData = false;
        }

        public void Start(Action loginMenuImpl)
        {
            Start();
            loginMenuImpl();
        }

        public override void Start()
        {
            DefaultMenu();
        }

        private void DefaultMenu()
        {
            HESConsole.Write("---> [", "Data File Detected - Info Loaded", "] <---\n\n", ConsoleColor.Green, alignSize: 80);
        }

        public void GetResource(MainMenu parentMenu)
        {
            // Set Menu Fields Values if there's a csv file in the \IN directory
            HESFile.CreateDefaultDirsIfRequired();
            if (!HESFile.HasFile()) return;

            CSVDTO dto = HESFile.ReadFromFile<CSVDTO>(_RESOURCE);
            parentMenu.SetAdditionalFieldsValues(dto.cifs, dto.startDates, dto.endDates);

            // Checking if data has been loaded successfully
            ContainsData = dto.cifs.Count > 0 ? true : false;
        }
    }
}
