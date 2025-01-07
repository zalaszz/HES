using HES.Interfaces;
using HES.Menus.Fields;
using HES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private InstructionDTO _dto;

        public void SetInstructions(MenuFieldsContainer instructions) // MENUDTO here?
        {
            foreach (object item in _dto.Instructions)
            {
                if (item is string val)
                {
                    Console.WriteLine("STRING => " + val);
                }
                else if (item is JsonElement element)
                {
                    // Verificando se o "loop" existe dentro do objeto JsonElement
                    if (element.TryGetProperty("loop", out JsonElement loopValue))
                    {
                        // Aqui podemos percorrer os itens dentro do loop (caso seja um array de strings)
                        Console.WriteLine("OBJECT => loop property found:");
                        foreach (var loopItem in loopValue.EnumerateArray())
                        {
                            Console.WriteLine(loopItem.GetString());  // Acessando e imprimindo o valor de cada item do array
                        }
                    }
                }
            }

            Console.ReadLine();

            finalInstructions.Add(VirtualKeys.SetVKs("./drv", 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));

            SetLoginInstructionsImpl(instructions);

            SetAdditionalFieldsInstructionsImpl(instructions);

            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.F11, 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.F11, 0));
        }

        private void SetLoginInstructionsImpl(MenuFieldsContainer instructions)
        {
            for (int i = 0; i < instructions.CountLoginFields(); i++)
            {
                MenuField field = instructions.LoginFields.ElementAt(i);
                finalInstructions.Add(VirtualKeys.SetVKs(field.GetValue<string>(), 0));
                finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));
            }
        }

        private void SetAdditionalFieldsInstructionsImpl(MenuFieldsContainer instructions)
        {
            for (int i = 0; i < instructions.CountAdditionalFields(); i++)
            {
                MenuField field = instructions.AdditionalFields.ElementAt(i);

                if (i.Equals(0) && field.GetValue() is string value)
                {
                    string[] cifs = value.Split(' ');
                    string[] startDates = ((string)instructions.AdditionalFields.ElementAt(i + 1).GetValue()).Split(' ');
                    string[] endDates = ((string)instructions.AdditionalFields.ElementAt(i + 2).GetValue()).Split(' ');

                    for (int j = 0; j < cifs.Length; j++)
                    {
                        int numStartDates = startDates.Length.Equals(1) ? 0 : j; //If there's only 1 date use it for all cifs
                        int numEndDates = endDates.Length.Equals(1) ? 0 : j;
                        FormStmtSnap(cifs[j], startDates[numStartDates], endDates[numEndDates]);
                    }
                }
            }
        }

        private void FormStmtSnap(string cif, string startDate, string endDate)
        {
            finalInstructions.Add(VirtualKeys.SetVKs("FORMSTMTSNAP", 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.END, 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));

            finalInstructions.Add(VirtualKeys.SetVKs(cif, 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));

            finalInstructions.Add(VirtualKeys.SetVKs("1", 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));

            finalInstructions.Add(VirtualKeys.SetVKs(startDate, 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));

            finalInstructions.Add(VirtualKeys.SetVKs(endDate, 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));

            finalInstructions.Add(VirtualKeys.SetVKs("2", 0));

            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));
            finalInstructions.Add(VirtualKeys.SetVKs(VK_CODE.ENTER, 0));
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
