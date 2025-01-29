using System.Linq;
using System.Runtime.InteropServices;

namespace HES.InputHandling
{
    class HESHook
    {
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        public static bool IsInteruptShortcutPressed()
        {
            return IsShortcutPressed(VK_CODE.F6);
        }

        private static bool IsShortcutPressed(params VK_CODE[] vkcode)
        {
            return vkcode
                .ToList()
                .All(vk => (GetAsyncKeyState(VirtualKeys.ConvertVKToUshort(vk)) & 0x8000) != 0); // Checks if the key has been pressed with bitwise
        }
    }
}
