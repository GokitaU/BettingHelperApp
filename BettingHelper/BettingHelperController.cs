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
                var svsData = await svsTask;
                var betssonData = await betssonTask;
                ProcessData(svsData,betssonData);
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

        public void ProcessData<T>(Dictionary<string,SvenskaSpelDraw> svsData, List<T> betsson) where T : IBettingEvent
        {
            if (svsData == null)
            {
                window.SetSVSOddsLabelText(Constants.ODDS_DOWNLOAD_ERROR_MSG);
                window.SetBetssonOddsLabelText("Laddning avbruten eftersom det inte gick att hämta data från Svenska Spel.");
                return;
            }
            if (svsData.Count == 0)
            {
                window.SetSVSOddsLabelText("Det fanns inga data att hämta från Svenska spel!");
                window.SetBetssonOddsLabelText("Laddning avbruten eftersom det inte gick att hämta data från Svenska Spel.");
                return;
            }
            SvenskaSpelDraw europaTipset = null;
            SvenskaSpelDraw strykTipset = null;
            SvenskaSpelDraw toppTipset = null;
            svsData.TryGetValue("Europatipset", out europaTipset);
            svsData.TryGetValue("Stryktipset", out strykTipset);
            svsData.TryGetValue("Topptipset", out toppTipset);
            SvenskaSpelDraw stryktipsEuropatips = Utilities.ClosestDraw(europaTipset, strykTipset);
            backgroundWorker.RunWorkerAsync(argument: new Tuple<SvenskaSpelDraw, SvenskaSpelDraw, List<T>, string>(stryktipsEuropatips,toppTipset,betsson,_excelPath));
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

        private void ProcessSVSData(Dictionary<string, SvenskaSpelDraw> svsData)
        {
            if (svsData == null)
            {
                window.SetSVSOddsLabelText(Constants.ODDS_DOWNLOAD_ERROR_MSG);
                window.SetBetssonOddsLabelText("Laddning avbruten eftersom det inte gick att hämta data från Svenska Spel.");
                return;
            }
            if (svsData.Count == 0)
            {
                window.SetSVSOddsLabelText("Det fanns inga data att hämta från Svenska spel!");
                window.SetBetssonOddsLabelText("Laddning avbruten eftersom det inte gick att hämta data från Svenska Spel.");
                return;
            }
            SvenskaSpelDraw europaTipset = null;
            SvenskaSpelDraw strykTipset = null;
            SvenskaSpelDraw toppTipset = null;
            svsData.TryGetValue("Europatipset", out europaTipset);
            svsData.TryGetValue("Stryktipset", out strykTipset);
            svsData.TryGetValue("Topptipset", out toppTipset);
            SvenskaSpelDraw stryktipsEuropatips = Utilities.ClosestDraw(europaTipset, strykTipset);

            bool distr = ProcessSVSDistribution(stryktipsEuropatips, toppTipset);
            //bool teams = ProcessTeams(stryktipsEuropatips);
           // return odds && distr && teams;
        }

        private void RunExcelWork(object sender, DoWorkEventArgs a)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            Tuple<SvenskaSpelDraw, SvenskaSpelDraw, List<BetssonEvent>, string> data = (Tuple<SvenskaSpelDraw,SvenskaSpelDraw,List<BetssonEvent>,string>) a.Argument;
            ExcelWriter writer = new ExcelWriter(data.Item4);
            writer.Open();
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
            // window.SetSVSOddsLabelText(writeSuccess ? Constants.LOAD_COMPLETE_MSG : Constants.EXCEL_WRITE_ODDS_ERROR_MSG);
        }

        private void WorkComplete(object sender, RunWorkerCompletedEventArgs arg)
        {
            Dictionary<ExcelWriteResult,bool> result = (Dictionary<ExcelWriteResult, bool>)arg.Result;

            //window.SetSVSOddsLabelText(writeSuccess ? Constants.LOAD_COMPLETE_MSG : Constants.EXCEL_WRITE_ODDS_ERROR_MSG);
            //window.ToggleLoadButton(true);
            //string warningNrOfGames = Utilities.ValidateNrOfGames(svsData,betssonData);
            //string warningTeamNames = Utilities.ValidateTeamNames(svsData,betssonData);
            //if(warningNrOfGames != null)
            //{
            //    window.ShowWarningMessage(Constants.WARNING_TITLE, warningNrOfGames);
            //}
            //if(warningTeamNames != null)
            //{
            //    window.ShowWarningMessage(Constants.WARNING_TITLE, warningTeamNames);
            //}
            window.SetSVSOddsLabelText(Constants.LOAD_COMPLETE_MSG);

        }

        private void ProcessBetssonData(List<BetssonEvent> betssonData)
        {
            if (betssonData == null)
            {
                window.SetBetssonOddsLabelText(Constants.ODDS_DOWNLOAD_ERROR_MSG);
                return;
            }
            if (betssonData.Count == 0)
            {
                window.SetBetssonOddsLabelText(Constants.NO_ODDS_ERROR_MSG);
                return;
            }
            WriteBetssonDataToExcel(betssonData);
        }

        private bool ProcessTeams(SvenskaSpelDraw draw, ExcelWriter writer)
        {
            //window.SetTeamsLoadLabelText(Constants.WRITING_TEAMS_TO_EXCEL_MSG);
            //if (!writer.WriteGames(draw.DrawEvents))
            //{
            //    window.SetTeamsLoadLabelText(Constants.EXCEL_WRITE_TEAMS_ERROR_MSG);
            //    return false;
            //}
            //else
            //{
            //    window.SetTeamsLoadLabelText(Constants.LOAD_COMPLETE_MSG);
            //}
            writer.WriteGames(draw.DrawEvents);
            return true;
        }

        private bool ProcessSVSDistribution(SvenskaSpelDraw europatipsStryktips, SvenskaSpelDraw topptips)
        {
            bool stryktipsEuropatipsSuccess = true;
            bool topptipsSuccess = true;
            window.SetStryktipsetDistributionLabelText(Constants.WRITING_DISTRIBUTION_TO_EXCEL_MSG);
            //if (!writer.WriteDistribution(europatipsStryktips))
            //{
            //    window.SetStryktipsetDistributionLabelText(Constants.EXCEL_WRITE_DISTRIBUTION_ERROR_MSG);
            //    stryktipsEuropatipsSuccess = false;
            //}
            //else
            //{
            //    window.SetStryktipsetDistributionLabelText(Constants.LOAD_COMPLETE_MSG);
            //}
            //window.SetTopptipsetDistributionLabelText(Constants.WRITING_DISTRIBUTION_TO_EXCEL_MSG);
            //if (!writer.WriteDistribution(topptips))
            //{
            //    window.SetTopptipsetDistributionLabelText(Constants.EXCEL_WRITE_DISTRIBUTION_ERROR_MSG);
            //    topptipsSuccess = false;
            //}
            //else
            //{
            //    window.SetTopptipsetDistributionLabelText(Constants.LOAD_COMPLETE_MSG);
            //}
            return stryktipsEuropatipsSuccess && topptipsSuccess;
        }

        private bool WriteBetssonDataToExcel(List<BetssonEvent> evts)
        {
            window.SetBetssonOddsLabelText(Constants.WRITING_ODDS_TO_EXCEL_MSG);
            try
            {
                //bool writeResult = writer.WriteOdds(evts);
                //writer.Close();
                // window.SetBetssonOddsLabelText(writeResult ? Constants.LOAD_COMPLETE_MSG : Constants.EXCEL_WRITE_ODDS_ERROR_MSG);
                return true;//writeResult;
            }
            catch (COMException)
            {
                window.SetBetssonOddsLabelText(Constants.EXCEL_WRITE_ODDS_ERROR_MSG);
                return false;
            }
        }
    }
}
