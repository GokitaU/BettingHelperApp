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
        List<IBettingEvent> betsson;

        public BettingHelperController(BettingHelperWindow window)
        {
            this.window = window ?? throw new ArgumentException("Window must not be null.");
            this.backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(RunExcelWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkComplete);

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
                Dictionary<string, SvenskaSpelDraw> svsData = await svsTask;
                List<IBettingEvent> betssonData = await betssonTask;
                ProcessData(svsData, betssonData);
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

        public void ProcessData(Dictionary<string, SvenskaSpelDraw> svsData, List<IBettingEvent> betsson)
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
            SvenskaSpelDraw stryktipsEuropatips = Utilities.ClosestDraw(europaTipset, strykTipset);
            this.stryktipsetEuropatipset = stryktipsEuropatips;
            this.betsson = betsson;
            this.topptipset = toppTipset;
            window.SetTeamsLoadLabelText(Constants.WRITING_TEAMS_TO_EXCEL_MSG);
            window.SetSVSOddsLabelText(Constants.WRITING_ODDS_TO_EXCEL_MSG);
            window.SetBetssonOddsLabelText(Constants.WRITING_ODDS_TO_EXCEL_MSG);
            window.SetStryktipsetDistributionLabelText(Constants.WRITING_DISTRIBUTION_TO_EXCEL_MSG);
            window.SetTopptipsetDistributionLabelText(Constants.WRITING_DISTRIBUTION_TO_EXCEL_MSG);
            backgroundWorker.RunWorkerAsync(argument: new Tuple<SvenskaSpelDraw, SvenskaSpelDraw, List<IBettingEvent>, string>(stryktipsEuropatips, toppTipset, betsson, _excelPath));
        }

        private bool EnsureSVSData(Dictionary<string, SvenskaSpelDraw> svsData)
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
            Tuple<SvenskaSpelDraw, SvenskaSpelDraw, List<IBettingEvent>, string> data = (Tuple<SvenskaSpelDraw, SvenskaSpelDraw, List<IBettingEvent>, string>)a.Argument;
            ExcelWriter writer = new ExcelWriter(data.Item4);
            BackgroundWorker worker = (BackgroundWorker)sender;
            try
            {
                writer.Open();
            }
            finally
            {
                writer.Close();
            }
            bool writeSvsOdds = writer.WriteOdds(data.Item1.DrawEvents);
            bool writeSvsDistribution = writer.WriteDistribution(data.Item1);
            bool writeTopptipsDistribution = writer.WriteDistribution(data.Item2);
            bool writeBetssonOdds = writer.WriteOdds(data.Item3);
            bool teams = writer.WriteGames(data.Item1.DrawEvents);
            a.Result = new Dictionary<ExcelWriteResult, bool>
                {
                    { ExcelWriteResult.SVS_ODDS, writeSvsOdds},
                    { ExcelWriteResult.SVS_DISTRIBUTION, writeSvsDistribution },
                    { ExcelWriteResult.TOPPTIPS_DISTRIBUTION,writeTopptipsDistribution },
                    { ExcelWriteResult.BETSSON_ODDS, writeBetssonOdds },
                    { ExcelWriteResult.TEAMS, teams }
                };
        }

        private void WorkComplete(object sender, RunWorkerCompletedEventArgs arg)
        {
            Dictionary<ExcelWriteResult, bool> result = (Dictionary<ExcelWriteResult, bool>)arg.Result;

            window.SetSVSOddsLabelText(result[ExcelWriteResult.SVS_ODDS] ? Constants.LOAD_COMPLETE_MSG : Constants.EXCEL_WRITE_ODDS_ERROR_MSG);
            window.SetStryktipsetDistributionLabelText(result[ExcelWriteResult.SVS_DISTRIBUTION] ? Constants.LOAD_COMPLETE_MSG : Constants.EXCEL_WRITE_DISTRIBUTION_ERROR_MSG);
            window.SetTeamsLoadLabelText(result[ExcelWriteResult.TEAMS] ? Constants.LOAD_COMPLETE_MSG : Constants.EXCEL_WRITE_TEAMS_ERROR_MSG);
            window.SetBetssonOddsLabelText(result[ExcelWriteResult.BETSSON_ODDS] ? Constants.LOAD_COMPLETE_MSG : Constants.EXCEL_WRITE_ODDS_ERROR_MSG);
            window.SetTopptipsetDistributionLabelText(result[ExcelWriteResult.TOPPTIPS_DISTRIBUTION] ? Constants.LOAD_COMPLETE_MSG : Constants.EXCEL_WRITE_DISTRIBUTION_ERROR_MSG);
            string warningNrOfGames = Utilities.ValidateNrOfGames(stryktipsetEuropatipset, betsson);
            string warningTeamNames = Utilities.ValidateTeamNames(stryktipsetEuropatipset, betsson);
            if (warningNrOfGames != null)
            {
                window.ShowWarningMessage(Constants.WARNING_TITLE, warningNrOfGames);
            }
            if (warningTeamNames != null)
            {
                window.ShowWarningMessage(Constants.WARNING_TITLE, warningTeamNames);
            }
            window.ToggleLoadButton(true);
        }

        private bool ValidateBetssonData(List<BetssonEvent> betssonData)
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
            return true;
        }

    }
}
