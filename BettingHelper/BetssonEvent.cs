using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingHelper
{
    class BetssonEvent : IBettingEvent
    {
        public string Id;
        public string Name;
        public decimal WonOdds;
        public decimal DrawOdds;
        public decimal LostOdds;
        public DateTime DeadlineDate;
        public string HomeTeam => Name.Substring(0, Name.IndexOf(" - "));
        public string AwayTeam => Name.Substring(Name.IndexOf(" - ") + 3);
        public int HomeOddsCol => 9;
        public int DrawOddsCol => 10;
        public int AwayOddsCol => 11;
        public decimal HomeOdds => WonOdds;
        public decimal AwayOdds => LostOdds;
        decimal IBettingEvent.DrawOdds => DrawOdds;
    }
}
