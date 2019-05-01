using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BettingHelper
{
    class Utilities
    {

        public static SvenskaSpelDraw ClosestDraw(SvenskaSpelDraw d1, SvenskaSpelDraw d2)
        {
            if(d1 == null && d2 == null)
            {
                return null;
            }
            if(d1 == null && d2 != null)
            {
                return d2;
            } else if(d1 != null && d2 == null)
            {
                return d1;
            } else if(d1 == null && d2 != null)
            {
                return d2;
            }
            var s = d1?.RegCloseTime < d2.RegCloseTime ? d1 : d2;
            return s;
        }

        public static bool ValidNrOfGames(SvenskaSpelDraw draw)
        {
            return draw.BettingEvents.Count == 0 || ((
                (draw.ProductName.Equals("Europatipset",StringComparison.CurrentCultureIgnoreCase) || draw.ProductName.Equals("Stryktipset",StringComparison.CurrentCultureIgnoreCase)) && draw.BettingEvents.Count == 13) 
                || (draw.ProductName.Equals("Topptipset", StringComparison.CurrentCultureIgnoreCase) && draw.BettingEvents.Count == 8)
                );
        }

        public static string ValidateGame(IBettingEvent firstEvt, IBettingEvent secondEvt)
        {
            string firstHome = GetFirstLettersInTeamName(firstEvt.HomeTeam);
            string secondHome = GetFirstLettersInTeamName(secondEvt.HomeTeam);
            string firstAway = GetFirstLettersInTeamName(firstEvt.AwayTeam);
            string secondAway = GetFirstLettersInTeamName(secondEvt.AwayTeam);
            string homeInvalidString = (!firstHome.Equals(secondHome, StringComparison.CurrentCultureIgnoreCase) ? firstEvt.HomeTeam + " - " + firstEvt.AwayTeam : null);
            string awayInvalidString = (!firstAway.Equals(secondAway, StringComparison.CurrentCultureIgnoreCase) ? secondEvt.HomeTeam + " - " + secondEvt.AwayTeam : null);
            return homeInvalidString != null && awayInvalidString != null ? homeInvalidString + "\n\t    " + awayInvalidString : 
                (homeInvalidString != null ? homeInvalidString + "\n" : 
                awayInvalidString != null ? awayInvalidString + "\n" : null);
        }

        private static string GetFirstLettersInTeamName(string team)
        {
            return team.Length < 5 ? team : team.Replace(" ", "").Substring(0, 5);
        }

        private static string GetMisMatchingGameNames(List<IBettingEvent> first, List<IBettingEvent> second)
        {
            string warningMsg = "";
            for (int i = 0; i < second.Count; i++)
            {
                var firstEvt = first[i];
                var secondEvt = second[i];
                var validationWarning = ValidateGame(firstEvt, secondEvt);
                if (validationWarning != null)
                {
                    warningMsg += $"Match nr {i + 1}: {validationWarning}\n";
                }
            }
            return warningMsg;
        }

        public static string ValidateTeamNames(SvenskaSpelDraw svs, SvenskaSpelDraw topptips)
        {
            string warningMsg = "";
            string prefixMsg = $"Lagnamnen för nedanstående matcher skiljer sig åt mellan {svs.ProductName} och Topptipset! \n\n";
            if (svs == null || topptips == null || svs.BettingEvents.Count < topptips.BettingEvents.Count)
            {
                return null;
            }
            warningMsg += GetMisMatchingGameNames(svs.BettingEvents, topptips.BettingEvents);
            return String.IsNullOrEmpty(warningMsg) ? null : prefixMsg + warningMsg;
        }

        public static string ValidateNrOfGames(SvenskaSpelDraw svs)
        {
            string svsWarning = ValidateNrOfSvsGames(svs);            
            if(svsWarning != null)
            {
                return Constants.WARNING_CONTENT_PRE_TEXT + svsWarning;
            }
            return svsWarning == null ? null : Constants.WARNING_CONTENT_PRE_TEXT + svsWarning;
        }

        private static string ValidateNrOfSvsGames(SvenskaSpelDraw draw)
        {
            string warning = "";
            if (!ValidNrOfGames(draw))
            {
                warning += $"Antalet matcher som har hämtats för {draw.ProductName} är {draw.BettingEvents.Count} stycken.\n";
            }
            return string.IsNullOrEmpty(warning) ? null : warning.Substring(0, warning.Length-1);
        }
    }
}
