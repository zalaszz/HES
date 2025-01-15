using HES.Interfaces;
using HES.Menus.Fields;
using HES.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;

/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

namespace HES
{
    class Instruction : IResourceProvider
    {
        private List<VKObjectContainer> finalInstructions = new List<VKObjectContainer>();

        private const string _RESOURCE = @"\Resources\instructions.json";
        private const string _FIELD_IDENTIFIER = "field:";
        private const string _VK_IDENTIFIER = "virtualkey:";
        private InstructionDTO _dto;

        public void SetInstructions(MenuFieldsContainer fields) // MENUDTO here?
        {
            for (int u = 0; u < _dto.Instructions.Count; u++)
            {
                JsonElement item = _dto.Instructions[u];

                switch (item.ValueKind)
                {
                    case JsonValueKind.String:
                        HandleJsonStringKindInstructions(fields, item.GetString());
                        break;
                    case JsonValueKind.Object:
                        HandleJsonObjectKindInstructions(fields, item);
                        break;
                }
            }
        }

        private void HandleJsonStringKindInstructions(MenuFieldsContainer fields, string itemValue)
        {
            string virtualkeyName = itemValue?.Replace(_VK_IDENTIFIER, "");
            string fieldName = itemValue?.Replace(_FIELD_IDENTIFIER, "");
            MenuField field = fields.GetField(fieldName);

            HandleInstructionsFromElementValue(field, itemValue, virtualkeyName);
        }

        private void HandleJsonObjectKindInstructions(MenuFieldsContainer fields, JsonElement item)
        {
            if (!item.TryGetProperty("loop", out JsonElement loop)) return; // If it can't get the element loop skips to the next iteration

            int numIterations = GetIterationsFromJsonProperty(fields, item);

            for (int i = 0; i < numIterations; i++)
            {
                foreach (var loopItem in loop.EnumerateArray())
                {
                    string loopItemValue = loopItem.GetString();
                    string virtualkeyName = loopItemValue?.Replace(_VK_IDENTIFIER, "");
                    string fieldName = loopItemValue?.Replace(_FIELD_IDENTIFIER, "");
                    MenuField field = fields.GetField(fieldName);

                    HandleInstructionsFromElementValue(field, loopItemValue, virtualkeyName, true, i);
                }
            }
        }

        private int GetIterationsFromJsonProperty(MenuFieldsContainer fields, JsonElement item)
        {
            int numIterations = 1;
            if (item.TryGetProperty("iterations", out JsonElement iterationsProp))
            {
                string iterationsPropValue = iterationsProp.GetString();
                string fieldName = iterationsPropValue?.Replace(_FIELD_IDENTIFIER, "");
                MenuField field = fields.GetField(fieldName);

                if (int.TryParse(iterationsPropValue, out int parsedIteration))
                    numIterations = parsedIteration;
                else if (iterationsPropValue.Contains(_FIELD_IDENTIFIER) && field.IsMultiType())
                    numIterations = (field.GetValue() as List<string>).Count;
            }

            return numIterations;
        }

        private void HandleInstructionsFromElementValue(MenuField field, string elementValue, string virtualkeyName)
        {
            HandleInstructionsFromElementValue(field, elementValue, virtualkeyName, false, 0);
        }

        private void HandleInstructionsFromElementValue(MenuField field, string elementValue, string virtualkeyName, bool isLoop, int index)
        {
            if (elementValue.Contains(_FIELD_IDENTIFIER))
            {
                if (!field.IsMultiType())
                {
                    finalInstructions.Add(VirtualKeys.SetVKs(field.GetValue() as string, 0));
                    return;
                }

                if (!isLoop)
                {
                    (field.GetValue() as List<string>).ForEach(value => finalInstructions.Add(VirtualKeys.SetVKs($"{value} ", 0)));
                    return;
                }

                finalInstructions.Add(VirtualKeys.SetVKs((field.GetValue() as List<string>)[index], 0));
            }
            else if (elementValue.Contains(_VK_IDENTIFIER))
            {
                if (Enum.TryParse(virtualkeyName, out VK_CODE resultVK))
                    finalInstructions.Add(VirtualKeys.SetVKs(resultVK, 0));
            }
            else
                finalInstructions.Add(VirtualKeys.SetVKs(elementValue, 0));
        }

        public List<VKObjectContainer> GetInstructions()
        {
            return finalInstructions;
        }

        public void Clear()
        {
            finalInstructions.Clear();
        }

        public void GetResource()
        {
            _dto = HESFile.ReadFromFile<InstructionDTO>(_RESOURCE);
        }
    }
}
