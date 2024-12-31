using HES.Common;
using HES.Menus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

namespace HES
{
    class HESManager
    {
        private MainMenu _MAINMENU;
        private WindowMenu _WINDOWMENU;
        private HESWindow _WINDOW;
        private Instruction _INSTRUCTIONS;

        public HESManager(string windowName = "HES")
        {
            _MAINMENU = new MainMenu();
            _WINDOWMENU = new WindowMenu();
            _WINDOW = new HESWindow(windowName);
            _INSTRUCTIONS = new Instruction();
        }

        public void Start()
        {
            Task.Run(async () => await ResourceLoader.LoadResourcesAsync());

            while (true)
            {
                try
                {
                    _WINDOWMENU.Start();

                    _MAINMENU.Start();

                    _INSTRUCTIONS.SetInstructions(_MAINMENU.GetAllFields()); //Sets the data the user typed

                    _WINDOW.GetWindow(_WINDOWMENU.GetWindowHandlerFromUserInput()); //Gets the window that contains the windowName string

                    StartMultiThreadTask(); //Starts the task as multithread

                    Console.Clear();
                }
                catch (Exception e)
                {
                    new HESException("Something went wrong during the execution of the HES...", e);
                }
                finally
                {
                    _INSTRUCTIONS.Clear();
                }
            }
        }

        private void StartMultiThreadTask()
        {
            HESThreadPool<List<VKObjectContainer>> TPool =
                new HESThreadPool<List<VKObjectContainer>>(1);

            TPool.SetWorkLoad(_INSTRUCTIONS.GetInstructions());
            TPool.StartWork();
            TPool.Join(); //Prevents further execution of the code until the tasks are complete
        }
    }
}
