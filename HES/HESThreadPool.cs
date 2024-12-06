using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

namespace HES
{
    class HESThreadPool<T>
    {
        Thread[] workers;
        private const int _SLEEPTIME = 50;
        Object lockObj = new Object();
        private List<VKObjectContainer> data;

        public HESThreadPool(int numWorkers)
        {
            workers = new Thread[numWorkers];
        }

        public void StartWork()
        {
            for (int i = 0; i < workers.Length; i++)
            {
                this.workers[i] = new Thread(new ThreadStart(Work));
                this.workers[i].Start();
            }

            LoadingBar();
        }

        private void Work()
        {
            lock (lockObj)
            {
                while (data.Count > 0)
                {
                    SendInputs.PressKey(this.data.ElementAt(0));
                    this.data.RemoveAt(0);
                    Thread.Sleep(_SLEEPTIME);
                }
            }
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

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nExecuting task...");
            Console.ForegroundColor = ConsoleColor.White;

            for (int i = data.Count; i >= 0; i--)
            {
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
                    progress = (count / (double)dataCount) * 100;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"\rProgress {bar} {progress.ToString("F2")}% | [{count}/{dataCount}]");
                Console.ResetColor();

                count++;
                
                if (i.Equals(0))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nComplete!");
                    Console.ResetColor();
                    Console.ReadKey();
                }


                Thread.Sleep(_SLEEPTIME);
            }
        }
    }
}
