using HES.Menus.Fields;
using System.Collections.Generic;

/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

namespace HES.Models
{
    class MenuDTO : HESDTO
    {
        public List<MenuField> LoginFields { get; set; }
        public List<MenuField> AdditionalFields { get; set; }
    }
}
