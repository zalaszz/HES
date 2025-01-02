using System.Collections.Generic;

namespace HES.Models
{
    class MenuDTO : HESDTO
    {
        public List<MenuFields> LoginFields { get; set; }
        public List<MenuFields> AdditionalFields { get; set; }

        public class MenuFields
        {
            public string name { get; set; }
            public string type { get; set; }
        }
    }
}
