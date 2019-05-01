using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingHelper
{
    public interface IBettingEvent
    {
        string HomeTeam { get; }
        string AwayTeam { get; }
        decimal HomeOdds { get; }
        decimal DrawOdds { get; }
        decimal AwayOdds { get; }
        decimal HomeDistribution { get; }
        decimal DrawDistribution { get; }
        decimal AwayDistribution { get; }
        int HomeOddsCol { get; }
        int DrawOddsCol { get; }
        int AwayOddsCol { get; }
    }
}
