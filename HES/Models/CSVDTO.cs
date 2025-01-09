using System.Collections.Generic;

namespace HES.Models
{
    class CSVDTO : HESDTO
    {
        public List<string> cifs { get; set; } = new List<string>();
        public List<string> startDates { get; set; } = new List<string>();
        public List<string> endDates { get; set; } = new List<string>();
    }
}
