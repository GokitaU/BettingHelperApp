using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Office.Interop.Excel;

namespace BettingHelper
{
    class ExcelWriter
    {
        const string ODDS_GAMES_WORKSHEET = "1. Input Odds och matcher";
        const string STRYKTIPSET_EUROPATIPSET_DISTRIBUTION_WORKSHEET = "2. Input Stryk.- & Eu.-tipset";
        const string TOPPTIPSET_DISTRIBUTION_WORKSHEET = "3. Input Topptipset";
        const int HOME_DISTR_COL = 4;
        const int DRAW_DISTR_COL = 5;
        const int AWAY_DISTR_COL = 6;
        const int ROW_OFFSET = 2;
        string filePath;
        Application _excel;

        public ExcelWriter(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new IOException($"Fil med sökväg {filePath} kan inte hittas!");
            }
            this.filePath = filePath;
        }

        public void Close()
        {
            try
            {
                _excel?.Workbooks.Close();
                _excel?.Quit();
            }catch(Exception exc)
            {
                throw exc;
            }
        }

        public void Open()
        {
            if(_excel != null)
            {
                _excel.Quit();
            }
            _excel = new Application();
            _excel.Visible = false;
        }

        public bool WriteGames<T>(List<T> events) where T:IBettingEvent
        {
            if (events == null || events.Count != 13)
            {
                return false;
            }
            Workbook wb = _excel?.Workbooks.Open(filePath);
            Worksheet sheet = wb.Worksheets[ODDS_GAMES_WORKSHEET];

            try
            {
                for (int i = 0; i < events.Count; i++)
                {
                    sheet.Cells[i + ROW_OFFSET, Constants.HOME_TEAM_NAME] = events[i].HomeTeam;
                    sheet.Cells[i + ROW_OFFSET, Constants.AWAY_TEAM_NAME] = events[i].AwayTeam;
                }
                wb.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                wb.Close(0);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);

            }
        }

        public bool WriteDistribution(SvenskaSpelDraw draw)
        {
            return WriteDistribution(draw.DrawEvents, draw.ProductName == "Topptipset" ? TOPPTIPSET_DISTRIBUTION_WORKSHEET : STRYKTIPSET_EUROPATIPSET_DISTRIBUTION_WORKSHEET);
        }

        private bool WriteDistribution(List<SvenskaSpelEvent> events, string worksheet)
        {
            if (events == null || (events.Count != 13 && events.Count != 8))
            {
                return false;
            }
            Workbook wb = _excel?.Workbooks.Open(filePath);
            Worksheet sheet = wb.Worksheets[worksheet];

            try
            {
                for (int i = 0; i < events.Count; i++)
                {
                    sheet.Cells[i + ROW_OFFSET, HOME_DISTR_COL] = events[i].HomeDistribution;
                    sheet.Cells[i + ROW_OFFSET, DRAW_DISTR_COL] = events[i].DrawDistribution;
                    sheet.Cells[i + ROW_OFFSET, AWAY_DISTR_COL] = events[i].AwayDistribution;
                }
                wb.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                wb.Close(0);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
            }
        }

        public bool WriteOdds<T>(List<T> events) where T:IBettingEvent
        {
            if (events == null || (events.Count != 13 && events.Count != 8))
            {
                return false;
            }
            Workbook wb = _excel?.Workbooks.Open(filePath);
            Worksheet sheet = wb.Worksheets[ODDS_GAMES_WORKSHEET];
            try
            {
                for (int i = 0; i < events.Count; i++)
                {
                    sheet.Cells[i + ROW_OFFSET, events[i].HomeOddsCol] = events[i].HomeOdds;
                    sheet.Cells[i + ROW_OFFSET, events[i].DrawOddsCol] = events[i].DrawOdds;
                    sheet.Cells[i + ROW_OFFSET, events[i].AwayOddsCol] = events[i].AwayOdds;
                }
                wb.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                wb.Close(0);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
            }
        } 
    }
}
