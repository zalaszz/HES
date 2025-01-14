using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

namespace HES.Converters
{
    class JsonToVKCodeConverter : JsonConverter<Dictionary<char, VK_CODE>>
    {
        public override Dictionary<char, VK_CODE> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Dictionary<char, VK_CODE> dictionary;

            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                dictionary = doc.RootElement
                       .EnumerateObject()
                       .ToDictionary(childNode => char.Parse(childNode.Name), childNode => (VK_CODE)Enum.Parse(typeof(VK_CODE), childNode.Value.ToString()));
            }

            return dictionary;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<char, VK_CODE> value, JsonSerializerOptions options) { }
    }
}
