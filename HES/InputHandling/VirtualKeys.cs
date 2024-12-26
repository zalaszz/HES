using HES.Interfaces;
using HES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

namespace HES
{
    public enum VK_CODE
    {
        ENTER = 0x0D,
        AT_SIGN = 0xC0,
        CAPSLOCK = 0x14,
        BACKSPACE = 0x08,
        CTRL = 0x11,
        ALT = 0x12,
        LALT = 0xA4,
        RALT = 0xA5,
        SPACEBAR = 0x20,
        LSHIFT = 0xA0,
        PERIOD = 0xBE,
        COMMA = 0xBC,
        DIVIDE = 0x6F,
        MULTIPLY = 0x6A,
        PLUS = 0xBB,
        SEPARATOR = 0x6C,
        MINUS = 0xBD,
        END = 0x23,
        UP_ARROW = 0x26,
        DOWN_ARROW = 0x28,
        VK_0 = 0x30,
        VK_1 = 0x31,
        VK_2 = 0x32,
        VK_3 = 0x33,
        VK_4 = 0x34,
        VK_5 = 0x35,
        VK_6 = 0x36,
        VK_7 = 0x37,
        VK_8 = 0x38,
        VK_9 = 0x39,
        VK_A = 0x41,
        VK_B = 0x42,
        VK_C = 0x43,
        VK_D = 0x44,
        VK_E = 0x45,
        VK_F = 0x46,
        VK_G = 0x47,
        VK_H = 0x48,
        VK_I = 0x49,
        VK_J = 0x4A,
        VK_K = 0x4B,
        VK_L = 0x4C,
        VK_M = 0x4D,
        VK_N = 0x4E,
        VK_O = 0x4F,
        VK_P = 0x50,
        VK_Q = 0x51,
        VK_R = 0x52,
        VK_S = 0x53,
        VK_T = 0x54,
        VK_U = 0x55,
        VK_V = 0x56,
        VK_W = 0x57,
        VK_X = 0x58,
        VK_Y = 0x59,
        VK_Z = 0x5A,
        F11 = 0x7A,
    }

    public class VirtualKeys : IResourceProvider
    {
        private static Dictionary<char, VK_CODE> SpecialChars, SpecialShiftChars, SpecialAltChars = new Dictionary<char, VK_CODE>();

        private const string _RESOURCE = @"\Resources\settings.json";

        public static VKObjectContainer SetVKs<T>(T instruction, VK_CODE flag)
        {
            return SetVKs(new List<T> { instruction }, flag);
        }

        public static VKObjectContainer SetVKs<T>(List<T> instructions, VK_CODE flag)
        {
            VKObjectContainer vKObject = new VKObjectContainer();

            if (typeof(List<T>).Equals(typeof(List<string>)))
                instructions.ForEach(instruction => MapToVKObject(instruction.ToString(), ref vKObject, flag));
            else
                instructions.ForEach(instruction => MapToVKObject((VK_CODE)(object)instruction, ref vKObject, flag));

            return vKObject;
        }

        private static void MapToVKObject<T>(T objToMap, ref VKObjectContainer vKObject, VK_CODE flag)
        {
            if (typeof(T).Equals(typeof(string))) // If the objToMap is text call the String Implementation
            {
                MapToVKObjectStrImpl(objToMap.ToString(), ref vKObject, flag);
                return;
            }

            MapToVKObjectVKImpl((VK_CODE)(object)objToMap, ref vKObject, flag); // Otherwise use VK Implementation because we're sending a VK_CODE 
        }

        private static void MapToVKObjectStrImpl(string str, ref VKObjectContainer vKObject, VK_CODE flag)
        {
            foreach (char c in str)
            {
                if (SpecialChars.ContainsKey(c))
                {
                    vKObject.Add((char)SpecialChars[c], 0, true);
                    continue;
                }

                if (SpecialShiftChars.ContainsKey(c))
                {
                    vKObject.Add((char)SpecialShiftChars[c], VK_CODE.LSHIFT, true);
                    continue;
                }

                if (SpecialAltChars.ContainsKey(c))
                {
                    vKObject.Add((char)SpecialAltChars[c], VK_CODE.RALT, true);
                    continue;
                }

                MapVkCodes((char ch, VK_CODE key, VKObjectContainer vkObj, VK_CODE flg) => {
                    char vkChar = (char)key;
                    char cUpper = char.ToUpper(ch);
                    if (cUpper == vkChar)
                    {
                        vkObj.Add(ch, flg, false);
                    }
                }, ref vKObject, c, flag);
            }
        }

        private static void MapToVKObjectVKImpl(VK_CODE vkcode, ref VKObjectContainer vKObject, VK_CODE flag)
        {
            MapVkCodes((VK_CODE vkcod, VK_CODE key, VKObjectContainer vkObj, VK_CODE flg) => {

                if (key == vkcod)
                {
                    vkObj.Add(vkcod, flg);
                }

            }, ref vKObject, vkcode, flag);
        }

        private static void MapVkCodes<T1>(Action<T1, VK_CODE, VKObjectContainer, VK_CODE> implementation, ref VKObjectContainer vKObject, T1 vkcode, VK_CODE flag)
        {
            foreach (VK_CODE key in Enum.GetValues(typeof(VK_CODE)))
            {
                implementation((T1)(object)vkcode, key, vKObject, flag);
            }
        }

        public static ushort ConvertVKToUshort(VK_CODE vkcode)
        {
            return ConvertVKToUshort(new List<VK_CODE> { vkcode })[0];
        }

        public static List<ushort> ConvertVKToUshort(List<VK_CODE> vkcodes)
        {
            return vkcodes
                .Select(vkcode => (ushort)vkcode)
                .ToList();
        }

        public void GetResource()
        {
            try
            {
                SettingDTO dto = (SettingDTO)HESFile.ReadFromFile<SettingDTO>(_RESOURCE);

                //Dictionary<string, Dictionary<string, VK_CODE>> resources = element
                //    .EnumerateObject()
                //    .ToDictionary(node => node.Name, node => node.Value.EnumerateObject()
                //    .ToDictionary(childNode => childNode.Name, childNode => (VK_CODE)Enum.Parse(typeof(VK_CODE), childNode.Value.ToString())));

                SpecialChars = dto.SpecialChars;
                SpecialShiftChars = dto.SpecialShiftChars;
                SpecialAltChars = dto.SpecialAltChars;
            }
            catch (Exception e)
            {
                new HESException("Settings file could not be loaded, HES might not work as expected...", e);
            }
        }
    }
}
