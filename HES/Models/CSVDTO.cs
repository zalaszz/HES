using System.Collections.Generic;

/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

namespace HES.Models
{
    class CSVDTO : HESDTO
    {
        public List<string> cifs { get; set; } = new List<string>();
        public List<string> startDates { get; set; } = new List<string>();
        public List<string> endDates { get; set; } = new List<string>();
    }
}
