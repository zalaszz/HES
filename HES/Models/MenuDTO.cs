using System.Collections.Generic;

namespace HES.Models
{
    class MenuDTO
    {
        public List<Dictionary<string, string>> LoginFields { get; set; }
        public List<Dictionary<string, string>> AdditionalFields { get; set; }
    }
}
