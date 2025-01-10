using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HES.Menus.Fields
{
    public enum Category
    {
        Login = 1,
        Additional = 2
    }

    public enum FieldType
    {
        Text,
        MultiText,
        Number,
        MultiNumber,
        Date,
        Hidden
    }

    class MenuField
    {
        public string name { get; set; }
        private string value { get; set; }
        private List<string> multiValues { get; set; } = new List<string>();
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FieldType type { get; set; }
        public Category category { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType()) return false;
            return (obj as MenuField).name == name;
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public void SetValue(List<string> values) => multiValues = values;

        public void SetValue(string value) => this.value = value;

        public object GetValue()
        {
            if (!type.Equals(FieldType.MultiNumber) || !type.Equals(FieldType.MultiText) && !string.IsNullOrEmpty(value)) return value;
            else if (type.Equals(FieldType.MultiNumber) || type.Equals(FieldType.MultiText) && multiValues != null && multiValues.Count > 0) return multiValues;

            throw new HESException("Value appears to be null or empty...");
        }

        [Obsolete("GetValue<T> is deprecated use GetValue() instead.")]
        public T GetValue<T>()
        {
            if (typeof(T).Equals(value.GetType()) && !string.IsNullOrEmpty(value)) return (T)(object)value;
            if (typeof(T).Equals(multiValues.GetType()) && multiValues.Count > 0) return (T)(object)multiValues;

            throw new HESException("Only List<string> and string types can be passed into GetValue() method...");
        }
    }
}
