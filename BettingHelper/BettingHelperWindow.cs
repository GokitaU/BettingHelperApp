using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BettingHelper
{
    public partial class BettingHelperWindow : Form
    {
        BettingHelperController _controller;
        internal BettingHelperController Controller {
            get { return _controller; }
            set { _controller = value; }
        }

        public BettingHelperWindow()
        {
            InitializeComponent();
            MaximizeBox = false;
            openFileDialog.Filter = "Excel-filer|*.xls;*.xlsx;*.xlsm";
            loadDataBtn.Enabled = false;

        }

        private void openFile_Click(object sender, EventArgs e)
        {
            Controller.OpenFileClicked(this);
        }

        private async void loadData_Click(object sender, EventArgs e)
        {
            Controller.LoadButtonClicked(this);
        }

        private void BettingHelperWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            _controller.WindowIsClosing();
        }

        public void SetBetssonOddsLabelText(string txt)
        {
            statusLblOddsBetsson.Text = txt;
        }

        public void SetSVSOddsLabelText(string txt)
        {
            statusLblOddsSvs.Text = txt;
        }

        public void ToggleLoadButton(bool state)
        {
            loadDataBtn.Enabled = state;
        }

        public void ToggleLoadButton()
        {
            loadDataBtn.Enabled = !loadDataBtn.Enabled;
        }

        public void TogglePickFileButton(bool state)
        {
            pickFileButton.Enabled = state;
        }

        public void SetTeamsLoadLabelText(string txt)
        {
            teamLoadLbl.Text = txt;
        }

        public void SetPickFileButtonText(string txt)
        {
            pickFileButton.Text = txt;
        }

        public void SetStatusLabelText(string txt)
        {
            statusLbl.Text = txt;
        }

        public void SetStryktipsetDistributionLabelText(string txt)
        {
            stryktipsDistributionLbl.Text = txt;
        }

        public void SetTopptipsetDistributionLabelText(string txt)
        {
            topptipsDistrLbl.Text = txt;
        }

        public string ShowOpenFileDialog()
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            return null;
        }

        public void SetTextForAllStatusLabels(string txt)
        {
            SetStryktipsetDistributionLabelText(txt);
            SetTeamsLoadLabelText(txt);
            SetSVSOddsLabelText(txt);
            SetBetssonOddsLabelText(txt);
            SetTopptipsetDistributionLabelText(txt);
        }

        public void ShowWarningMessage(string title, string message)
        {
            MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void ShowErrorMessage(Form window, string title, string msg)
        {
            MessageBox.Show(window, msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
