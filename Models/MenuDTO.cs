using HES.Menus.Fields;
using System.Collections.Generic;

namespace HES.Models
{
    class MenuDTO : HESDTO
    {
        public List<MenuField> LoginFields { get; set; }
        public List<MenuField> AdditionalFields { get; set; }
    }
}
