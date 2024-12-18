/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

namespace HES
{
    public class VKObject
    {
        private ushort vk;
        private ushort flag;
        private char character;
        private bool isUpperCase;
        private bool isSpecialCharacter;

        public VKObject(char character, VK_CODE flag, bool isSpecialCharacter, bool isUpperCase)
        {
            this.character = character;
            this.flag = VirtualKeys.ConvertVKToUshort(flag);
            this.isUpperCase = isUpperCase;
            this.isSpecialCharacter = isSpecialCharacter;
            vk = 0;
        }

        public VKObject(VK_CODE vkcode, VK_CODE flag)
        {
            vk = VirtualKeys.ConvertVKToUshort(vkcode);
            this.flag = VirtualKeys.ConvertVKToUshort(flag);
            character = ' ';
            isSpecialCharacter = false;
            isUpperCase = false;
        }

        public ushort GetKey()
        {
            return ContainsVirtualKey() ? vk : character;
        }

        public bool ContainsVirtualKey()
        {
            return vk.Equals(0) ? false : true;
        }

        public bool ContainsCharKey()
        {
            return character.Equals(' ') ? false : true;
        }

        public ushort GetFlag()
        {
            return flag;
        }

        public bool ContainsFlag()
        {
            return GetFlag() == 0 ? false : true;
        }

        public bool IsSpecialCharacter()
        {
            return isSpecialCharacter;
        }

        public bool IsUpperCase()
        {
            return isUpperCase;
        }
    }
}
