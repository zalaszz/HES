using System.Collections.Generic;
using System.Text.Json.Serialization;
using HES.Converters;

namespace HES.Models
{
    class SettingDTO : HESDTO
    {
        [JsonConverter(typeof(JsonToVKCodeConverter))]
        public Dictionary<char, VK_CODE> SpecialChars { get; set; }
        [JsonConverter(typeof(JsonToVKCodeConverter))]
        public Dictionary<char, VK_CODE> SpecialShiftChars { get; set; }
        [JsonConverter(typeof(JsonToVKCodeConverter))]
        public Dictionary<char, VK_CODE> SpecialAltChars { get; set; }
    }
}
