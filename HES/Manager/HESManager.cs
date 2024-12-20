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
        private HESMenu _MENU;
        private HESWindow _WINDOW;
        private Instruction _INSTRUCTIONS;

        public HESManager(string windowName = "HES")
        {
            _MENU = new HESMenu();
            _WINDOW = new HESWindow(windowName);
            _INSTRUCTIONS = new Instruction();
        }

        public void Start()
        {
            //LoadResources().Wait();
            Task.Run(async () => await LoadResourcesAsync());

            while (true)
            {
                try
                {
                    _MENU.Start();

                    //_INSTRUCTIONS.SetInstructions(_MENU.GetFields()); //Sets the data the user typed

                    _WINDOW.GetTargetWindow(); //Gets the window that contains the windowName string

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

        private async Task LoadResourcesAsync()
        {
            await Task.Delay(5000);
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
