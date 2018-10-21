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
        public string ProductName;
        public bool HasOdds => !DrawEvents.Any(evt => evt.Odds == null); 
    }
}
