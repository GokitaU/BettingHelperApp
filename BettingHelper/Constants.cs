using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingHelper
{
    class Constants
    {
        public const int HOME_TEAM_NAME = 2;
        public const int AWAY_TEAM_NAME = 3;
        public const string DOWNLOADING_ODDS_MSG = "Laddar ner odds...";
        public const string LOAD_COMPLETE_MSG = "Klart!";
        public const string WRITING_ODDS_TO_EXCEL_MSG = "Skriver odds till Excel...";
        public const string WRITING_TEAMS_TO_EXCEL_MSG = "Skriver lag till Excel...";
        public const string ODDS_DOWNLOAD_ERROR_MSG = "Fel vid nedladdning av odds!";
        public const string EXCEL_WRITE_TEAMS_ERROR_MSG = "Kunde inte skriva matcher till Excel";
        public const string EXCEL_WRITE_ODDS_ERROR_MSG = "Kunde inte skriva odds till Excel";
        public const string WRITING_DISTRIBUTION_TO_EXCEL_MSG = "Skriver spelfördelning till Excel...";
        public const string EXCEL_WRITE_DISTRIBUTION_ERROR_MSG = "Kunde inte skriva spelfördelning till Excel";
        public const string NO_ODDS_ERROR_MSG = "Det fanns ingen oddsinformation att hämta.";
        public const string WARNING_TITLE = "Varning!";
        public const string WARNING_CONTENT_PRE_TEXT = "Möjligt fel har påträffats. Kontrollera nedanstående: \n";
        
    }
}
