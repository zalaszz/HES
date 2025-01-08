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
            //foreach (var instruction in _dto.Instructions)
            //{
            //    if (instruction.ValueKind == JsonValueKind.Object && instruction.TryGetProperty("loop", out var loop))
            //    {
            //        Console.WriteLine("Loop:");
            //        foreach (var item in loop.EnumerateArray())
            //        {
            //            Console.WriteLine($"  {item.GetString()}");
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine(instruction.GetString());
            //    }
            //}


            //foreach (var item in _dto.Instructions)
            //{
            //    if (item is JsonElement ele && ele.ValueKind.Equals(JsonValueKind.Object))
            //    {
            //        Console.WriteLine(item.TryGetProperty("loop", out JsonElement loop));
            //        continue;
            //    }

            //    Console.WriteLine(item.GetString());
            //}

            //Console.ReadLine();

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
                finalInstructions.Add(VirtualKeys.SetVKs((string)field.GetValue(), 0));
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
