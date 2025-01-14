using System;
using System.Collections.Generic;
using System.Text.Json;

/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

namespace HES.Models
{
    class InstructionDTO : HESDTO
    {
        public List<JsonElement> Instructions { get; set; } = new List<JsonElement>();
        //private List<string> _loop;
        //public List<string> Loop { 
        //    get {
        //        if (_loop.Equals(null))
        //        {
        //            _loop = ConfigureProperties();
        //        }
        //        return _loop;
        //    } 
        //    set => _loop = value;
        //}

        protected override List<string> ConfigureProperties()
        {
            List<string> localLoop = new List<string>();
            foreach (var instruction in Instructions)
            {
                if (instruction.ValueKind == JsonValueKind.Object && instruction.TryGetProperty("loop", out var loop))
                {
                    foreach (var item in loop.EnumerateArray())
                    {
                        localLoop.Add(item.GetString());
                    }
                }
            }

            return localLoop;
        }
    }
}
