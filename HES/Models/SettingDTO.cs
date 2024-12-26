using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using HES.Converters;

namespace HES.Models
{
    class SettingDTO
    {
        [JsonConverter(typeof(JsonToVKCodeConverter))]
        public Dictionary<char, VK_CODE> SpecialChars { get; set; }
        [JsonConverter(typeof(JsonToVKCodeConverter))]
        public Dictionary<char, VK_CODE> SpecialShiftChars { get; set; }
        [JsonConverter(typeof(JsonToVKCodeConverter))]
        public Dictionary<char, VK_CODE> SpecialAltChars { get; set; }
    }
}
