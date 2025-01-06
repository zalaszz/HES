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
            _DEFAULT_FIELDS = new HashSet<MenuField>() {
                new MenuField() { name = "Username", category = Category.Login },
                new MenuField() { name = "Password", category = Category.Login },
                new MenuField() { name = "Cifs", category = Category.Additional },
                new MenuField() { name = "Start Date", category = Category.Additional },
                new MenuField() { name = "End Date", category = Category.Additional }
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

        public void SetFieldsFromJSON(string jsonFile)
        {
            if (!HESFile.HasFile(jsonFile))
            {
                SetFields(_DEFAULT_FIELDS);
                return;
            }

            MenuDTO dto = HESFile.ReadFromFile<MenuDTO>(jsonFile);
            dto.LoginFields.ForEach(field => LoginFields.Add(new MenuField() { name = field.name, category = Category.Login }));
            dto.AdditionalFields.ForEach(field => AdditionalFields.Add(new MenuField() { name = field.name, category = Category.Additional }));
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

        public void SetFieldsValues(MenuField field)
        { //WIP
            if (field.category.Equals(Category.Login))
            {
                GetField(field.name).value = field.value;
            }
            else
            {
                AdditionalFields.Add(GetField(field.name));
            }
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
    }
}
