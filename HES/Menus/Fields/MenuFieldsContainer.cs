using HES.Models;
using System.Collections.Generic;
using System.Linq;

namespace HES.Menus.Fields
{
    class MenuFieldsContainer
    {
        public HashSet<MenuField> LoginFields { get; set; }
        public HashSet<MenuField> AdditionalFields { get; set; }
        private HashSet<MenuField> _DEFAULT_FIELDS { get; set; }

        public MenuFieldsContainer()
        {
            LoginFields = new HashSet<MenuField>();
            AdditionalFields = new HashSet<MenuField>();
            _DEFAULT_FIELDS = new HashSet<MenuField>() {
                new MenuField() { name = "Username", category = Category.Login, type = FieldType.Text },
                new MenuField() { name = "Password", category = Category.Login, type = FieldType.Hidden },
                new MenuField() { name = "Cifs", category = Category.Additional, type = FieldType.MultiNumber },
                new MenuField() { name = "Start Date", category = Category.Additional, type = FieldType.Date },
                new MenuField() { name = "End Date", category = Category.Additional, type = FieldType.Date }
            };
        }

        public MenuField GetField(string name)
        {
            return GetLoginField(name) ?? GetAdditionalField(name); // If the first value is null returns the next one
        }

        public MenuField GetLoginField(string name)
        {
            return LoginFields.FirstOrDefault(field => field.name.Equals(name));
        }

        public MenuField GetAdditionalField(string name)
        {
            return AdditionalFields.FirstOrDefault(field => field.name.Equals(name));
        }

        public HashSet<MenuField> GetAllFields()
        {
            return new HashSet<MenuField>(LoginFields.Union(AdditionalFields));
        }

        public HashSet<MenuField> GetLoginFields()
        {
            return LoginFields;
        }

        public HashSet<MenuField> GetAdditionalFields()
        {
            return AdditionalFields;
        }

        public void SetFieldsFromJSON(string jsonFile)
        {
            if (!HESFile.HasFile(jsonFile))
            {
                SetFields(_DEFAULT_FIELDS);
                return;
            }

            MenuDTO dto = HESFile.ReadFromFile<MenuDTO>(jsonFile);
            dto.LoginFields.ForEach(field => LoginFields.Add(new MenuField() { name = field.name, category = Category.Login, type = field.type }));
            dto.AdditionalFields.ForEach(field => AdditionalFields.Add(new MenuField() { name = field.name, category = Category.Additional, type = field.type }));
        }

        public void SetFields(HashSet<MenuField> fieldsToSet)
        {
            SetFieldsImpl(fieldsToSet);
        }

        public void SetFields(params MenuField[] fieldsToSet)
        {
            SetFieldsImpl(fieldsToSet);
        }

        private void SetFieldsImpl<T>(T fieldsToSet) where T : IEnumerable<MenuField>
        {
            fieldsToSet
                .ToList()
                .ForEach(field => {
                    MenuField convField = (MenuField)(object)field;
                    if (convField.category.Equals(Category.Login)) LoginFields.Add(convField);
                    else AdditionalFields.Add(convField);
                });
        }

        public void SetFieldValue(string name, string value)
        {
            GetField(name).SetValue(value);
        }

        public int CountLoginFields()
        {
            return LoginFields.Count;
        }

        public int CountAdditionalFields()
        {
            return AdditionalFields.Count;
        }

        public int CountAllFields()
        {
            return LoginFields.Count + AdditionalFields.Count;
        }

        public bool ContainsField(string name)
        {
            return GetField(name) != null ? true : false;
        }
    }
}
