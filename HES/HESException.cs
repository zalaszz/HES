using Figgle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HES
{
    class HESException : Exception
    {
        public HESException(){ }

        public HESException(string message) : base(message)
        {
            DisplayError(() => {
                Console.WriteLine($"{message}\n");
            }, null); // Only HESExceptions
        }

        public HESException(Exception inner)
        {
            DisplayError(() => {
                Console.WriteLine($"{inner}\n");
            }, inner); // All other exceptions
        }

        public HESException(string message, Exception inner) : base(message, inner)
        {
            DisplayError(() => {
                Console.WriteLine(message);
                Console.WriteLine($"{inner}\n");
            }, inner);
        }

        private void DisplayError(Action errorAction, Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{FiggleFonts.Small.Render(" E R R O R")}");
            if(e is HESException)
                errorAction();
            else
                Console.WriteLine($"{e}\n");
            Console.ResetColor();
        }
    }
}
