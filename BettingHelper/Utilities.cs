using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BettingHelper
{
    class Utilities
    {

        public static SvenskaSpelDraw ClosestDraw(SvenskaSpelDraw d1, SvenskaSpelDraw d2)
        {

            if(d1 == null && d2 != null)
            {
                return d2;
            } else if(d1 != null && d2 == null)
            {
                return d1;
            }
            return d1?.RegCloseTime.CompareTo(d2?.RegCloseTime) == -1 ? d1 : d2;
        }

        public static bool ValidNrOfGames(SvenskaSpelDraw draw)
        {
            return draw.DrawEvents.Count == 0 || ((
                (draw.ProductName.Equals("Europatipset",StringComparison.CurrentCultureIgnoreCase) || draw.ProductName.Equals("Stryktipset",StringComparison.CurrentCultureIgnoreCase)) && draw.DrawEvents.Count == 13) 
                || (draw.ProductName.Equals("Topptipset", StringComparison.CurrentCultureIgnoreCase) && draw.DrawEvents.Count == 8)
                );
        }

        public static bool ValidNrOfGames(List<BetssonEvent> betsson)
        {
            return betsson.Count == 0 || betsson.Count == 13;
        }

        public static string ValidateGame(SvenskaSpelEvent svs, BetssonEvent betsson)
        {
            string firstHome = GetFirstLettersInTeamName(svs.HomeTeam);
            string secondHome = GetFirstLettersInTeamName(betsson.HomeTeam);
            string firstAway = GetFirstLettersInTeamName(svs.AwayTeam);
            string secondAway = GetFirstLettersInTeamName(betsson.AwayTeam);
            string homeInvalidString = (firstHome.Equals(secondHome, StringComparison.CurrentCultureIgnoreCase) ? "\tHemmalag: " + firstHome + " - " + secondHome : null);
            string awayInvalidString = (firstAway.Equals(secondAway, StringComparison.CurrentCultureIgnoreCase) ? "\tBortalag: " + firstAway + " - " + secondAway : null);
            return homeInvalidString != null && awayInvalidString != null ? homeInvalidString + "\n" + awayInvalidString + "\n" : homeInvalidString ?? awayInvalidString;
        }

        private static string GetFirstLettersInTeamName(string team)
        {
            return team.Length < 5 ? team : team.Substring(0, 5);
        }

        public static string ValidateTeamNames(Dictionary<string, SvenskaSpelDraw> svs, List<BetssonEvent> betsson)
        {
            string warningMsg = "";
            SvenskaSpelDraw europa = null;
            SvenskaSpelDraw stryktips = null;
            svs.TryGetValue("Europatipset", out europa);
            svs.TryGetValue("Stryktipset", out stryktips);
            var svsDraw = stryktips ?? europa;
            if(svsDraw.DrawEvents.Count != betsson.Count)
            {
                return null;
            }
            for(int i = 0; i < svsDraw.DrawEvents.Count; i++)
            {
                var svsEvt = svsDraw.DrawEvents[i];
                var betssonEvt = betsson[i];
                var validationWarning = ValidateGame(svsEvt, betssonEvt);
                if(validationWarning != null){
                    warningMsg += $"Lagnamn överensstämmer inte i match nr {i}: \n {validationWarning}";
                }
            }
            return String.IsNullOrEmpty(warningMsg) ? null : warningMsg;
        }

        public static string ValidateNrOfGames(Dictionary<string, SvenskaSpelDraw> svs, List<BetssonEvent> betsson)
        {
            string svsWarning = ValidateNrOfSvsGames(svs);
            string betssonWarning = ValidateNrOfBetssonGames(betsson);
            if(svsWarning != null && betssonWarning != null)
            {
                return Constants.WARNING_CONTENT_PRE_TEXT + svsWarning + "\n" + betssonWarning;
            }
            return svsWarning == null && betssonWarning == null ? null : Constants.WARNING_CONTENT_PRE_TEXT + (svsWarning ?? betssonWarning);
        }

        private static string ValidateNrOfSvsGames(Dictionary<string, SvenskaSpelDraw> svs)
        {
            string warning = "";
            foreach(var draw in svs.Values)
            {
                if (!ValidNrOfGames(draw))
                {
                    warning += $"Antalet matcher som har hämtats för {draw.ProductName} är {draw.DrawEvents.Count} stycken.\n";
                }
            }
            return string.IsNullOrEmpty(warning) ? null : warning.Substring(0, warning.Length-1);
        }

        private static string ValidateNrOfBetssonGames(List<BetssonEvent> betsson)
        {
            return ValidNrOfGames(betsson) ? null : $"Antalet matcher som har hämtats för Betsson är {betsson.Count} stycken.";
        }
    }
}
