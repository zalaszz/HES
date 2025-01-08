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
        private WindowMenu _WINDOWMENU;
        private HESWindow _WINDOW;

        public HESManager()
        {
            _WINDOW = new HESWindow();
            _WINDOWMENU = new WindowMenu();
        }

        public void Start()
        {
            //Task.Run(async () => await ResourceManager.LoadResourcesAsync());
            ResourceManager.LoadResources();

            while (true)
            {
                try
                {
                    _WINDOWMENU.Start(); // Start the Menu with all the windows

                    ResourceManager.GetInstance<MainMenu>().Start(); // Start the Main Menu

                    ResourceManager.GetInstance<Instruction>().SetInstructions(ResourceManager.GetInstance<MainMenu>().GetMenuFieldContainer()); //Sets the data the user typed

                    _WINDOW.GetWindow(_WINDOWMENU.GetWindowHandlerFromUserInput()); //Gets the window chosen by the user

                    StartMultiThreadTask(); //Starts the task as multithread

                    Console.Clear();
                }
                catch (Exception e)
                {
                    new HESException("Something went wrong during the execution of the HES...", e);
                }
                finally
                {
                    ResourceManager.GetInstance<Instruction>().Clear();
                }
            }
        }

        private void StartMultiThreadTask()
        {
            HESThreadPool<List<VKObjectContainer>> TPool =
                new HESThreadPool<List<VKObjectContainer>>(1);

            TPool.SetWorkLoad(ResourceManager.GetInstance<Instruction>().GetInstructions());
            TPool.StartWork();
            TPool.Join(); //Prevents further execution of the code until the tasks are complete
        }
    }
}
