using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Figgle;

namespace HES
{
    static class HESConsole
    {
        public static void WriteLine(string value, ConsoleColor fgColor, ConsoleColor bgColor = 0, int alignSize = 0)
        {
            SetWriteLineColor(Console.WriteLine, value, fgColor, bgColor, alignSize, value.Length);
        }

        public static void Write(string value, ConsoleColor fgColor, ConsoleColor bgColor = 0, int alignSize = 0)
        {
            SetWriteLineColor(Console.Write, value, fgColor, bgColor, alignSize, value.Length);
        }

        private static void SetWriteLineColor(Action<string> DisplayText, string value, 
            ConsoleColor fgColor, ConsoleColor bgColor, int alignSize, int stringLength)
        {
            string padding = GetLeftPadToAlignCenter(stringLength, alignSize);

            SetLineBackgroundColor(bgColor);
            Console.ForegroundColor = fgColor;
            DisplayText(padding + value);
            Console.ResetColor();
        }

        private static void SetLineBackgroundColor(ConsoleColor bgColor)
        {
            if (bgColor.Equals(0))
                Console.ResetColor();
            else
                Console.BackgroundColor = bgColor;
        }

        public static void WriteLine(string startValue, string value, 
            string endValue, ConsoleColor fgColor, ConsoleColor bgColor = 0, 
            int alignSize = 0, int stringLength = 0)
        {
            SetComposedWriteLineColor(Console.WriteLine, startValue, value, endValue, fgColor, bgColor, alignSize, stringLength);
        }

        public static void Write(string startValue, string value, 
            string endValue, ConsoleColor fgColor, ConsoleColor bgColor = 0, 
            int alignSize = 0, int stringLength = 0)
        {
            SetComposedWriteLineColor(Console.Write, startValue, value, endValue, fgColor, bgColor, alignSize, stringLength);
        }

        private static void SetComposedWriteLineColor(Action<string> DisplayText, string startValue, 
            string value, string endValue, ConsoleColor fgColor, 
            ConsoleColor bgColor, int alignSize, int stringLength)
        {
            string fullText = $"{startValue}{value}{endValue}";
            int length = stringLength.Equals(0) ? fullText.Length : stringLength;
            string padding = GetLeftPadToAlignCenter(length, alignSize);

            SetLineBackgroundColor(bgColor);
            DisplayText(padding + startValue);
            Console.ForegroundColor = fgColor;
            DisplayText(value);
            Console.ResetColor();
            DisplayText(endValue);
        }

        private static string GetLeftPadToAlignCenter(int stringLength, int size)
        {
            return size.Equals(0) ? new string(' ', 0) : new string(' ', (size - stringLength) / 2);
        }
    }
}
