using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BettingHelper
{
    class BettingHelperController
    {
        BettingHelperWindow window;
        BettingDataLoader dataLoader;
        string _excelPath;
        BackgroundWorker backgroundWorker;
        SvenskaSpelDraw stryktipsetEuropatipset;
        SvenskaSpelDraw topptipset;

        public BettingHelperController(BettingHelperWindow window)
        {
            this.window = window ?? throw new ArgumentException("Window must not be null.");
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(RunExcelWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkComplete);

        }

        public async void LoadButtonClicked(BettingHelperWindow window)
        {
            window.SetTextForAllStatusLabels("Hämtar data...");
            window.ToggleLoadButton(false);

            var svsTask = dataLoader?.DownloadSvenskaSpelData();
            try
            {
                Dictionary<string, SvenskaSpelDraw> svsData = await svsTask;
                ProcessData(svsData);
            }
            catch (OperationCanceledException exc)
            {
                window.ToggleLoadButton(true);
                window.ShowErrorMessage(window, "Timeout", "Kunde inte hämta data från spelbolag.");
            }
            catch (Exception ex)
            {
                window.ToggleLoadButton(true);
                window.ShowErrorMessage(window, "Fel!", "Ett oväntat fel har inträffat!");
            }
        }

        public void ProcessData(Dictionary<string, SvenskaSpelDraw> svsData)
        {
            if (!EnsureSVSData(svsData))
            {
                return;
            }
            SvenskaSpelDraw europaTipset = null;
            SvenskaSpelDraw strykTipset = null;
            SvenskaSpelDraw toppTipset = null;
            svsData.TryGetValue("Europatipset", out europaTipset);
            svsData.TryGetValue("Stryktipset", out strykTipset);
            svsData.TryGetValue("Topptipset", out toppTipset);
            if(europaTipset == null && strykTipset == null)
            {
                window.ShowErrorMessage(window, "Error","No Stryktipset or Europatipset present");
                window.ToggleLoadButton(true);
                return;
            }
            SvenskaSpelDraw stryktipsEuropatips = Utilities.ClosestDraw(europaTipset, strykTipset);
            this.stryktipsetEuropatipset = stryktipsEuropatips;
            this.topptipset = toppTipset;
            window.SetTeamsLoadLabelText(Constants.WRITING_TEAMS_TO_EXCEL_MSG);
            window.SetSVSOddsLabelText(Constants.WRITING_ODDS_TO_EXCEL_MSG);
            window.SetStryktipsetDistributionLabelText(Constants.WRITING_DISTRIBUTION_TO_EXCEL_MSG);
            window.SetTopptipsetDistributionLabelText(Constants.WRITING_DISTRIBUTION_TO_EXCEL_MSG);
            backgroundWorker.RunWorkerAsync(argument: new Tuple<SvenskaSpelDraw, SvenskaSpelDraw, string>(stryktipsEuropatips, toppTipset, _excelPath));
        }

        private bool EnsureSVSData(Dictionary<string, SvenskaSpelDraw> svsData)
        {
            if (svsData == null)
            {
                window.SetSVSOddsLabelText(Constants.ODDS_DOWNLOAD_ERROR_MSG);
                return false;
            }
            if (svsData.Count == 0)
            {
                window.SetSVSOddsLabelText("Det fanns inga data att hämta från Svenska spel!");
                return false;
            }
            return true;
        }

        public void OpenFileClicked(BettingHelperWindow window)
        {
            string excelPath = window.ShowOpenFileDialog();
            if (excelPath == null)
            {
                return;
            }
            try
            {
                dataLoader = new BettingDataLoader(excelPath);
                window.TogglePickFileButton(false);
                window.SetPickFileButtonText("Öppnar fil...");
                window.SetPickFileButtonText("Öppna Excel-fil");
                window.SetStatusLabelText($"Laddad Excel-fil: {excelPath}");
                window.ToggleLoadButton(true);
                window.TogglePickFileButton(true);
                _excelPath = excelPath;
            }
            catch (IOException exc)
            {
                window.ShowErrorMessage(window, "Fel!", exc.Message);
            }
        }

        private void RunExcelWork(object sender, DoWorkEventArgs a)
        {
            Tuple<SvenskaSpelDraw, SvenskaSpelDraw, string> data = (Tuple<SvenskaSpelDraw, SvenskaSpelDraw, string>)a.Argument;
            ExcelWriter writer = new ExcelWriter(data.Item3);
            BackgroundWorker worker = (BackgroundWorker)sender;
            try
            {
                writer.Open();
            }
            finally
            {
                writer.Close();
            }
            bool writeSvsOdds = writer.WriteOdds(data.Item1.BettingEvents);
            bool writeSvsDistribution = writer.WriteDistribution(data.Item1);
            bool writeTopptipsDistribution = writer.WriteDistribution(data.Item2);
            bool teams = writer.WriteGames(data.Item1.BettingEvents);
            a.Result = new Dictionary<ExcelWriteResult, bool>
            {
                { ExcelWriteResult.SVS_ODDS, writeSvsOdds},
                { ExcelWriteResult.SVS_DISTRIBUTION, writeSvsDistribution },
                { ExcelWriteResult.TOPPTIPS_DISTRIBUTION,writeTopptipsDistribution },
                { ExcelWriteResult.TEAMS, teams }
            };
        }

        private void WorkComplete(object sender, RunWorkerCompletedEventArgs arg)
        {
            Dictionary<ExcelWriteResult, bool> result = (Dictionary<ExcelWriteResult, bool>) arg.Result;

            window.SetSVSOddsLabelText(result[ExcelWriteResult.SVS_ODDS] ? Constants.LOAD_COMPLETE_MSG : Constants.EXCEL_WRITE_ODDS_ERROR_MSG);
            window.SetStryktipsetDistributionLabelText(result[ExcelWriteResult.SVS_DISTRIBUTION] ? Constants.LOAD_COMPLETE_MSG : Constants.EXCEL_WRITE_DISTRIBUTION_ERROR_MSG);
            window.SetTeamsLoadLabelText(result[ExcelWriteResult.TEAMS] ? Constants.LOAD_COMPLETE_MSG : Constants.EXCEL_WRITE_TEAMS_ERROR_MSG);
            window.SetTopptipsetDistributionLabelText(result[ExcelWriteResult.TOPPTIPS_DISTRIBUTION] ? Constants.LOAD_COMPLETE_MSG : Constants.EXCEL_WRITE_DISTRIBUTION_ERROR_MSG);
            string warningNrOfGames = Utilities.ValidateNrOfGames(stryktipsetEuropatipset);
            string warningTeamNamesTopptipset = Utilities.ValidateTeamNames(stryktipsetEuropatipset,topptipset);
            if (warningNrOfGames != null)
            {
                window.ShowWarningMessage(Constants.WARNING_TITLE, warningNrOfGames);
            }
            if (warningTeamNamesTopptipset != null)
            {
                window.ShowWarningMessage(Constants.WARNING_TITLE, warningTeamNamesTopptipset);
            }
            window.ToggleLoadButton(true);
        }

    }
}
