using System.Collections.Generic;

namespace HES.Models
{
    class CSVDTO : HESDTO
    {
        public List<string> cifs { get; set; }
        public List<string> startDates { get; set; }
        public List<string> endDates { get; set; }

    }
}
