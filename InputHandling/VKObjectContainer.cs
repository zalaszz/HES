using System.Collections.Generic;
using System.Linq;

/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

namespace HES
{
    public class VKObjectContainer
    {
        private List<VKObject> vKObjects = new List<VKObject>();

        public void Add(char character, VK_CODE flag, bool isSpecialCharacter)
        {
            vKObjects.Add(new VKObject(char.ToUpper(character), flag, isSpecialCharacter, IsUpperCase(character)));
        }

        public void Add(VK_CODE vkcode, VK_CODE flag)
        {
            vKObjects.Add(new VKObject(vkcode, flag));
        }

        private bool IsUpperCase(char c)
        {
            return char.IsUpper(c);
        }

        public List<VKObject> GetVKObjects()
        {
            return vKObjects;
        }

        public VKObject GetVKObject(int index)
        {
            return vKObjects.ElementAt(index);
        }
    }
}
