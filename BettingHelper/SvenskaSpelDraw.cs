using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingHelper
{
    class SvenskaSpelDraw
    {
        public DateTime RegCloseTime;
        public List<SvenskaSpelEvent> DrawEvents;
        public List<IBettingEvent> BettingEvents => DrawEvents?.Cast<IBettingEvent>().ToList();
        public string ProductName;
    }
}
