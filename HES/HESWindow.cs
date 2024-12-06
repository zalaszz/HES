using System;
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
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, int lParam);
        /* Conseguir o titulo da window */
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        /* Definir janela como ativa */
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        /* Apresentar a window */
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public delegate bool EnumWindowsProc(int hwd, int lParam);

        private const int SW_NORMAL = 1;
        private string _WINDOWNAME;

        public HESWindow(string windowName)
        {
            _WINDOWNAME = windowName;
            HESDefaultSettings();
        }

        public void GetTargetWindow()
        {
            EnumWindows((int hwd, int lPAram) =>
            {
                StringBuilder sb = new StringBuilder(1024);
                GetWindowText(new IntPtr(hwd), sb, sb.Capacity);
                if (sb.ToString().Contains(_WINDOWNAME))
                {
                    ShowWindow(new IntPtr(hwd), SW_NORMAL); // Restaurar a window
                    SetForegroundWindow(new IntPtr(hwd)); // Focar window
                }
                return true;
            }, 0);

            Thread.Sleep(250);
        }

        public void HESDefaultSettings()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = versionInfo.FileVersion;
            Console.Title = $"HES - Haitong Extraction System {version}";
        }
    }
}
