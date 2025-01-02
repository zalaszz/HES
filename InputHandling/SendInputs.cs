using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

namespace HES
{
    public static class SendInputs
    {
        private const int _INPUT_KEYBOARD = 1;
        private const uint _KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const uint _KEYEVENTF_KEYDOWN = 0x0000;
        private const uint _KEYEVENTF_KEYUP = 0x0002;

        private static Dictionary<VK_CODE, bool> IsModifierKeyPressed = new Dictionary<VK_CODE, bool> {
            { VK_CODE.RALT, false },
            { VK_CODE.LSHIFT, false },
            { VK_CODE.CTRL, false },
            { VK_CODE.CAPSLOCK, false },
        };

        struct INPUT
        {
            public uint type;
            public InputUnion u;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct InputUnion
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;
            [FieldOffset(0)]
            public KEYBDINPUT ki;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            /*Virtual Key code.  Must be from 1-254.  If the dwFlags member specifies KEYEVENTF_UNICODE, wVk must be 0.*/
            public ushort wVk;
            /*A hardware scan code for the key. If dwFlags specifies KEYEVENTF_UNICODE, wScan specifies a Unicode character which is to be sent to the foreground application.*/
            public ushort wScan;
            /*Specifies various aspects of a keystroke.  See the KEYEVENTF_ constants for more information.*/
            public uint dwFlags;
            /*The time stamp for the event, in milliseconds. If this parameter is zero, the system will provide its own time stamp.*/
            public uint time;
            /*An additional value associated with the keystroke. Use the GetMessageExtraInfo function to obtain this information.*/
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        // Função para pressionar e soltar uma tecla
        public static void PressKey(VKObjectContainer keyCodes)
        {
            //Console.WriteLine("\n---------------");
            //Console.WriteLine("Count: " + keyCodes.GetVKObjects().Count);

            INPUT[] inputs;
            List<INPUT> keyCombinationList = new List<INPUT>();

            PressKeyImpl(keyCodes, ref keyCombinationList);

            inputs = keyCombinationList.ToArray();

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));

            //Console.WriteLine("---------------");
        }

        private static void PressKeyImpl(VKObjectContainer keyCodes, ref List<INPUT> keyCombinationList)
        {
            for (int i = 0; i < keyCodes.GetVKObjects().Count; i++)
            {
                //Console.WriteLine(
                //    string.Format("[{0}, {1}, {2}, {3}]", (char)keyCodes.GetVKObject(i).GetKey(),
                //    keyCodes.GetVKObject(i).GetFlag(), keyCodes.GetVKObject(i).IsSpecialCharacter(), keyCodes.GetVKObject(i).IsUpperCase()
                //));

                PressModifierKey(keyCodes.GetVKObject(i), keyCombinationList);

                keyCombinationList.Add(SetInputs(keyCodes.GetVKObject(i).GetKey()));

                PressModifierKey(keyCodes.GetVKObject(i), keyCombinationList);
            }
        }

        private static void PressModifierKey(VKObject keycode, List<INPUT> inputs)
        {
            if (keycode.IsUpperCase())
            {
                inputs.Add(SetInputs(VirtualKeys.ConvertVKToUshort(VK_CODE.LSHIFT)));
            }

            if (keycode.IsSpecialCharacter() && keycode.ContainsFlag())
            {
                inputs.Add(SetInputs(keycode.GetFlag()));
            }

            if (char.IsDigit((char) keycode.GetKey()) && (!keycode.ContainsFlag() && !keycode.IsSpecialCharacter()))
            {
                // Se for número e não tiver flag nem for um special char
                inputs.Add(SetInputs(keycode.GetFlag()));
            }
        }

        private static INPUT SetInputs(ushort code)
        {
            return new INPUT
            {
                type = _INPUT_KEYBOARD,
                u = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = code,
                        wScan = 0,
                        dwFlags = KeyCombinationFlag(code),
                        dwExtraInfo = IntPtr.Zero,
                    }
                }
            };
        }

        private static uint KeyCombinationFlag(ushort vkcode)
        {
            uint flag = 0;
            foreach (var Vkey in IsModifierKeyPressed.ToList())
            {
                if (vkcode == VirtualKeys.ConvertVKToUshort(Vkey.Key))
                {
                    IsModifierKeyPressed[Vkey.Key] = !Vkey.Value;
                    flag = Vkey.Value ? _KEYEVENTF_KEYUP : _KEYEVENTF_KEYDOWN;

                    if (Vkey.Key == VK_CODE.CAPSLOCK) //CAPSLOCK funciona de forma diferente
                    {
                        // A VK_CODE.CAPSLOCK deve ser chamada 2x para ser ativada e desativada
                        flag = Vkey.Value ? _KEYEVENTF_EXTENDEDKEY : _KEYEVENTF_KEYUP;
                    }

                    //Console.WriteLine(Vkey.Value + "|" + Vkey.Key + "|" + flag);
                }
            }

            return flag;

        }
    }
}
