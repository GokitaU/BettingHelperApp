using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BettingHelper
{
    class BettingHelperController
    {
        BettingHelperWindow window;
        BettingDataLoader dataLoader;
        ExcelWriter writer;

        public BettingHelperController(BettingHelperWindow window)
        {
            this.window = window ?? throw new ArgumentException("Window must not be null.");
        }

        public async void LoadButtonClicked(BettingHelperWindow window)
        {
            window.SetTextForAllStatusLabels("Hämtar data...");
            window.ToggleLoadButton(false);

            var betssonTask = dataLoader?.DownloadBetssonOdds();
            var svsTask = dataLoader?.DownloadSvenskaSpelData();
            try
            {
                await Task.WhenAll(betssonTask, svsTask);
                var svsData = await svsTask;
                var betssonData = await betssonTask;
                ProcessSVSData(svsData);
                ProcessBetssonData(betssonData);
                window.ToggleLoadButton(true);
                string warningNrOfGames = Utilities.ValidateNrOfGames(svsData,betssonData);
                string warningTeamNames = Utilities.ValidateTeamNames(svsData,betssonData);
                if(warningNrOfGames != null)
                {
                    window.ShowWarningMessage(Constants.WARNING_TITLE, warningNrOfGames);
                }
                if(warningTeamNames != null)
                {
                    window.ShowWarningMessage(Constants.WARNING_TITLE, warningTeamNames);
                }
                
            }
            catch (OperationCanceledException exc)
            {
                writer.Close();
                window.ShowErrorMessage(window, "Timeout", "Kunde inte hämta data från spelbolag.");
            }
            catch (Exception ex)
            {
                writer.Close();
                window.ShowErrorMessage(window, "Fel!", "Ett oväntat fel har inträffat!");
            }
        }

        public void WindowIsClosing()
        {
            writer?.Close();
        }

        public void OpenFileClicked(BettingHelperWindow window)
        {
            string excelPath = window.ShowOpenFileDialog();
            if(excelPath == null)
            {
                return;
            }
            try
            {
                dataLoader = new BettingDataLoader(excelPath);
                writer = new ExcelWriter(excelPath);
                window.TogglePickFileButton(false);
                window.SetPickFileButtonText("Öppnar fil...");
                writer.Open();
                window.SetPickFileButtonText("Öppna Excel-fil");
                window.SetStatusLabelText($"Laddad Excel-fil: {excelPath}");
                window.ToggleLoadButton(true);
                window.TogglePickFileButton(true);
            }
            catch (IOException exc)
            {
                window.ShowErrorMessage(window, "Fel!", exc.Message);
            }
        }

        private bool ProcessSVSData(Dictionary<string, SvenskaSpelDraw> svsData)
        {
            if (svsData == null)
            {
                window.SetSVSOddsLabelText(Constants.ODDS_DOWNLOAD_ERROR_MSG);
                window.SetBetssonOddsLabelText("Laddning avbruten eftersom det inte gick att hämta data från Svenska Spel.");
                return false;
            }
            if (svsData.Count == 0)
            {
                window.SetSVSOddsLabelText("Det fanns inga data att hämta från Svenska spel!");
                window.SetBetssonOddsLabelText("Laddning avbruten eftersom det inte gick att hämta data från Svenska Spel.");
                return false;
            }
            SvenskaSpelDraw europaTipset = null;
            SvenskaSpelDraw strykTipset = null;
            SvenskaSpelDraw toppTipset = null;
            svsData.TryGetValue("Europatipset", out europaTipset);
            svsData.TryGetValue("Stryktipset", out strykTipset);
            svsData.TryGetValue("Topptipset", out toppTipset);
            SvenskaSpelDraw stryktipsEuropatips = Utilities.ClosestDraw(europaTipset, strykTipset);
            bool odds = ProcessSVSOdds(stryktipsEuropatips);
            bool distr = ProcessSVSDistribution(stryktipsEuropatips, toppTipset);
            bool teams = ProcessTeams(stryktipsEuropatips);
            return odds && distr && teams;
        }

        private bool ProcessBetssonData(List<BetssonEvent> betssonData)
        {
            if (betssonData == null)
            {
                window.SetBetssonOddsLabelText(Constants.ODDS_DOWNLOAD_ERROR_MSG);
                return false;
            }
            if (betssonData.Count == 0)
            {
                window.SetBetssonOddsLabelText(Constants.NO_ODDS_ERROR_MSG);
                return false;
            }
            return WriteBetssonDataToExcel(betssonData);
        }

        private bool ProcessSVSOdds(SvenskaSpelDraw draw)
        {
            if (!draw.HasOdds)
            {
                window.SetSVSOddsLabelText(Constants.NO_ODDS_ERROR_MSG);
                return false;
            }
            window.SetSVSOddsLabelText(Constants.WRITING_ODDS_TO_EXCEL_MSG);
            if (!writer.WriteOdds(draw.DrawEvents))
            {
                window.SetSVSOddsLabelText(Constants.EXCEL_WRITE_ODDS_ERROR_MSG);
                return false;
            }
            else
            {
                window.SetSVSOddsLabelText(Constants.LOAD_COMPLETE_MSG);
            }
            return true;
        }

        private bool ProcessTeams(SvenskaSpelDraw draw)
        {
            window.SetTeamsLoadLabelText(Constants.WRITING_TEAMS_TO_EXCEL_MSG);
            if (!writer.WriteGames(draw.DrawEvents))
            {
                window.SetTeamsLoadLabelText(Constants.EXCEL_WRITE_TEAMS_ERROR_MSG);
                return false;
            }
            else
            {
                window.SetTeamsLoadLabelText(Constants.LOAD_COMPLETE_MSG);
            }
            return true;
        }

        private bool ProcessSVSDistribution(SvenskaSpelDraw europatipsStryktips, SvenskaSpelDraw topptips)
        {
            bool stryktipsEuropatipsSuccess = true;
            bool topptipsSuccess = true;
            window.SetStryktipsetDistributionLabelText(Constants.WRITING_DISTRIBUTION_TO_EXCEL_MSG);
            if (!writer.WriteDistribution(europatipsStryktips))
            {
                window.SetStryktipsetDistributionLabelText(Constants.EXCEL_WRITE_DISTRIBUTION_ERROR_MSG);
                stryktipsEuropatipsSuccess = false;
            }
            else
            {
                window.SetStryktipsetDistributionLabelText(Constants.LOAD_COMPLETE_MSG);
            }
            window.SetTopptipsetDistributionLabelText(Constants.WRITING_DISTRIBUTION_TO_EXCEL_MSG);
            if (!writer.WriteDistribution(topptips))
            {
                window.SetTopptipsetDistributionLabelText(Constants.EXCEL_WRITE_DISTRIBUTION_ERROR_MSG);
                topptipsSuccess = false;
            }
            else
            {
                window.SetTopptipsetDistributionLabelText(Constants.LOAD_COMPLETE_MSG);
            }
            return stryktipsEuropatipsSuccess && topptipsSuccess;
        }

        private bool WriteBetssonDataToExcel(List<BetssonEvent> evts)
        {
            window.SetBetssonOddsLabelText(Constants.WRITING_ODDS_TO_EXCEL_MSG);
            try
            {
                writer.WriteGames(evts);
                writer.Close();
                window.SetBetssonOddsLabelText(Constants.LOAD_COMPLETE_MSG);
                return true;
            }
            catch (COMException)
            {
                window.SetBetssonOddsLabelText(Constants.EXCEL_WRITE_ODDS_ERROR_MSG);
                return false;
            }
        }
    }
}
