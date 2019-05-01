using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingHelper
{
    class SvenskaSpelEvent : IBettingEvent
    {
        public string HomeTeam => Match["participants"][0]["name"].ToString();
        public string AwayTeam => Match["participants"][1]["name"].ToString();
        public JObject Odds;
        public JObject SvenskaFolket;
        public decimal HomeOdds => Odds == null ? -1 : decimal.Parse(Odds["one"].ToString());
        public decimal DrawOdds => Odds == null ? -1 : decimal.Parse(Odds["x"].ToString());
        public decimal AwayOdds => Odds == null ? -1 : decimal.Parse(Odds["two"].ToString());
        public JObject Match;
        public decimal HomeDistribution => SvenskaFolket == null ? 0 : decimal.Parse(SvenskaFolket["one"].ToString()) / 100;
        public decimal DrawDistribution => SvenskaFolket == null ? 0 : decimal.Parse(SvenskaFolket["x"].ToString()) / 100;
        public decimal AwayDistribution => SvenskaFolket == null ? 0 : decimal.Parse(SvenskaFolket["two"].ToString()) / 100;
        public int HomeOddsCol => 4;
        public int DrawOddsCol => 5;
        public int AwayOddsCol => 6;
    }
}
