using System.Collections.Generic;

namespace HES.Models
{
    class SettingDTO
    {
        public Dictionary<string, VK_CODE> SpecialChars { get; set; }
        public Dictionary<string, VK_CODE> SpecialShiftChars { get; set; }
        public Dictionary<string, VK_CODE> SpecialAltChars { get; set; }
    }
}
