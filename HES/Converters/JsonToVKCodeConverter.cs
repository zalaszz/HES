using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HES.Converters
{
    class JsonToVKCodeConverter : JsonConverter<Dictionary<char, VK_CODE>>
    {
        public override Dictionary<char, VK_CODE> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dictionary = new Dictionary<char, VK_CODE>();

            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                foreach (var property in doc.RootElement.EnumerateObject())
                {
                    VK_CODE value = (VK_CODE)Enum.Parse(typeof(VK_CODE), property.Value.ToString());
                    dictionary.Add(char.Parse(property.Name), value);
                }
            }

            return dictionary;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<char, VK_CODE> value, JsonSerializerOptions options)
        {
            
        }
    }
}
