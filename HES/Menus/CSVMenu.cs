using HES.Interfaces;
using HES.Models;
using System;

namespace HES.Menus
{
    class CSVMenu : HESMenu, IResourceProvider
    {
        private const string _RESOURCE = @"*.csv";

        public void Start(Action loginMenuImpl)
        {
            this.Start();
            loginMenuImpl();
        }

        public override void Start()
        {
            base.Start();
            DefaultMenu();
        }

        private void DefaultMenu()
        {
            HESConsole.Write("---> [", "Data File Detected - Info Loaded", "] <---\n\n", ConsoleColor.Green, alignSize: 80);
        }

        public void GetResource()
        {
            // Set Menu Fields Values if there's a csv file in the \IN directory
            HESFile.CreateDefaultDirsIfRequired();
            if (!HESFile.HasFile()) return;

            CSVDTO dto = HESFile.ReadFromFile<CSVDTO>(_RESOURCE);
            SetAdditionalFieldsValues(dto.cifs, dto.startDates, dto.endDates);
        }
    }
}
