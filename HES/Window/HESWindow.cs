using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

namespace HES
{
    class HESWindow
    {
        /* Enumerar as windows */
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, int lParam);
        /* Conseguir o titulo da window */
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        /* Definir janela como ativa */
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        /* Apresentar a window */
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private delegate bool EnumWindowsProc(int hwd, int lParam);

        private const int SW_NORMAL = 1;
        private string _WINDOWNAME;

        public HESWindow(string windowName)
        {
            _WINDOWNAME = windowName;
            HESDefaultWindowSettings();
        }

        public void GetWindow(int hwd)
        {
            ShowWindow(new IntPtr(hwd), SW_NORMAL); // Restore window
            SetForegroundWindow(new IntPtr(hwd)); // Focus window            
            Thread.Sleep(250);
        }

        public static Dictionary<int, StringBuilder> GetAllWindows()
        {
            Dictionary<int, StringBuilder> windows = new Dictionary<int, StringBuilder>();
            EnumWindows((int hwd, int lPAram) =>
            {
                StringBuilder sb = new StringBuilder(1024);
                GetWindowText(new IntPtr(hwd), sb, sb.Capacity);
                if (sb.ToString().Length > 0)
                {
                    windows.Add(hwd, sb);
                }
                return true;
            }, 0);
            return windows;
        }

        public void HESDefaultWindowSettings()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = versionInfo.FileVersion;
            Console.Title = $"HES - Haitong Extraction System {version}";
        }
    }
}
