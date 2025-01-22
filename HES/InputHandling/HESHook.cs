using System.Runtime.InteropServices;

namespace HES.InputHandling
{
    class HESHook
    {
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        public static bool IsInteruptShortcutPressed()
        {
            // Checks if F6 has been pressed with bitwise
            return ((GetAsyncKeyState(VirtualKeys.ConvertVKToUshort(VK_CODE.F6)) & 0x8000) != 0) ? true : false;
        }
    }
}
