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
        private InstructionDTO _dto;

        public void SetInstructions(MenuFieldsContainer instructions) // MENUDTO here?
        {
            for (int u = 0; u < _dto.Instructions.Count; u++)
            {
                JsonElement item = _dto.Instructions[u];
                if (item is JsonElement element && element.ValueKind.Equals(JsonValueKind.String))
                {
                    if (item.GetString().Contains("field:"))
                    {
                        if(instructions.GetField(item.GetString().Replace("field:", "")).IsMultiType())
                        {
                            List<string> listInstruction = instructions.GetField(item.GetString().Replace("field:", "")).GetValue() as List<string>;
                            finalInstructions.Add(VirtualKeys.SetVKs(listInstruction[u], 0)); // Loop here because it is a multivalue field
                            continue;
                        }

                        string stringInstruction = instructions.GetField(item.GetString().Replace("field:", "")).GetValue() as string;
                        finalInstructions.Add(VirtualKeys.SetVKs(stringInstruction, 0));
                        continue;
                    }
                    else if (item.GetString().Contains("virtualkey:"))
                    {
                        Enum.TryParse(item.GetString().Replace("virtualkey:", ""), out VK_CODE result);
                        finalInstructions.Add(VirtualKeys.SetVKs(result, 0));
                        continue;
                    }
                    else
                    {
                        finalInstructions.Add(VirtualKeys.SetVKs(item.GetString(), 0));
                        continue;
                    }
                }else if (item is JsonElement loopElement && loopElement.ValueKind.Equals(JsonValueKind.Object)) {
                    if (!loopElement.TryGetProperty("loop", out JsonElement loop)) continue; // If it can't get the element loop skips to the next iteration

                    int numIterations = 1;
                    if (loopElement.TryGetProperty("iterations", out JsonElement iterationsProp))
                    {
                        if (int.TryParse(iterationsProp.GetString(), out int parsedIteration))
                        {
                            numIterations = parsedIteration;
                        }
                        else if (iterationsProp.GetString().Contains("field:") && instructions.GetField(iterationsProp.GetString().Replace("field:", "")).IsMultiType())
                        {
                            numIterations = (instructions.GetField(iterationsProp.GetString().Replace("field:", "")).GetValue() as List<string>).Count;
                        }
                    }

                    for (int i = 0; i < numIterations; i++)
                    {
                        foreach (var loopItem in loop.EnumerateArray())
                        {
                            if (loopItem.GetString().Contains("field:"))
                            {
                                if (instructions.GetField(loopItem.GetString().Replace("field:", "")).IsMultiType())
                                {
                                    List<string> listInstruction = instructions.GetField(loopItem.GetString().Replace("field:", "")).GetValue() as List<string>;
                                    finalInstructions.Add(VirtualKeys.SetVKs(listInstruction[i], 0)); // Loop here as well
                                    continue;
                                }

                                string stringInstruction = instructions.GetField(loopItem.GetString().Replace("field:", "")).GetValue() as string;
                                finalInstructions.Add(VirtualKeys.SetVKs(stringInstruction, 0));
                                continue;

                            }else if (loopItem.GetString().Contains("virtualkey:"))
                            {
                                if (Enum.TryParse(loopItem.GetString().Replace("virtualkey:", ""), out VK_CODE result))
                                {
                                    finalInstructions.Add(VirtualKeys.SetVKs(result, 0));
                                }
                                continue;
                            }

                            finalInstructions.Add(VirtualKeys.SetVKs(loopItem.GetString(), 0));
                        }
                    }
                }
            }
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
