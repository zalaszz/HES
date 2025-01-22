using HES.InputHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

namespace HES
{
    class HESThreadPool<T>
    {
        Thread[] workers;
        private const int _SLEEPTIME = 100;
        Object lockObj = new Object();
        private List<VKObjectContainer> data;
        private bool _isHeavyTask = false;

        public HESThreadPool(int numWorkers)
        {
            workers = new Thread[numWorkers];
        }

        public void StartWork()
        {
            for (int i = 0; i < workers.Length; i++)
            {
                //workers[i] = Task.Run(() => Work());
                this.workers[i] = new Thread(new ThreadStart(Work));
                this.workers[i].Priority = ThreadPriority.Highest;
                this.workers[i].Start();
            }

            TaskProgress();
        }

        private void TaskProgress()
        {
            if (data.Count > 150)
                _isHeavyTask = true;
            
            switch (_isHeavyTask)
            {
                case true:
                    LoadingBar();
                    break;
                case false:
                    LightTask();
                    break;
            }
        }

        private void LightTask()
        {
            HESConsole.Write("\nExecuting task...", ConsoleColor.DarkYellow);
        }

        private void Work()
        {
            while (true)
            {
                VKObjectContainer container = null;

                lock (lockObj)
                {
                    if (data.Count > 0)
                    {
                        container = data[0];
                        this.data.RemoveAt(0);
                    }
                }

                if (IsCancelledByUser())
                {
                    HESConsole.Write("\nThe task has been cancelled!", ConsoleColor.DarkRed);
                    Console.ReadKey();
                    break;
                }



                if (container == null)
                    break;

                SendInputs.PressKey(container);
                Task.Delay(_SLEEPTIME).Wait();
            }
        }

        private bool IsCancelledByUser()
        {
            return HESHook.IsInteruptShortcutPressed();
        }

        public void SetWorkLoad(T data)
        {
            this.data = (List<VKObjectContainer>)(object)data;
        }

        public void Join()
        {
            for (int i = 0; i < workers.Length; i++)
            {
                this.workers[i].Join();
            }

            if (data.Count.Equals(0) && _isHeavyTask.Equals(false))
            {
                HESConsole.Write("\nThe task has been successfully completed!", ConsoleColor.Green);
                Console.ReadKey();
            }
        }

        public void LoadingBar()
        {
            double percentageForEachTask = data.Count / 100; // 38,67 Percentage for each task
            int numBars = 40; 
            double percentagePerBar = data.Count / numBars; // 193,35 Data per bar

            StringBuilder bar = new StringBuilder("<|▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒|>");
            double progress = 0;

            int count = 0;
            int dataCount = data.Count;

            for (int i = data.Count; i >= 0; i--)
            {
                if (IsCancelledByUser())
                    break;

                if ((i % percentagePerBar).Equals(percentageForEachTask))
                {
                    int firstSpaceIndex = bar.ToString().IndexOf('▒');
                    if (!firstSpaceIndex.Equals(-1))  // Se houver um espaço
                    {
                        bar[firstSpaceIndex] = '█';  // Substitui o espaço por '█'
                    }
                }

                if((i % 10).Equals(int.Parse(percentageForEachTask.ToString().Substring(percentageForEachTask.ToString().Length - 1))))
                    //progress = progress >= 100 ? 100 : progress + (0.093 * (100 / percentageForEachTask));
                    progress = (count / (double)dataCount) * 100 > 99 ? 100 : (count / (double)dataCount) * 100;

                string displayProgress = $"\rProgress {bar} {progress.ToString("F2")}% | [{count}/{dataCount}]";
                HESConsole.Write(displayProgress, ConsoleColor.Green);
                count++;
                
                if (i.Equals(0))
                {
                    HESConsole.Write("\n[", ConsoleColor.Green, ConsoleColor.White);
                    HESConsole.Write("COMPLETE", ConsoleColor.Green, ConsoleColor.White, alignSize: displayProgress.Length - "COMPLETE".Length - 2);
                    HESConsole.Write("]", ConsoleColor.Green, ConsoleColor.White, alignSize: displayProgress.Length);
                    Console.ReadKey();
                }


                Thread.Sleep(_SLEEPTIME);
            }
        }
    }
}
