namespace BettingHelper
{
    partial class BettingHelperWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loadDataBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl4 = new System.Windows.Forms.Label();
            this.pickFileButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.statusLblOddsSvs = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.stryktipsDistributionLbl = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.teamLoadLbl = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbl5 = new System.Windows.Forms.Label();
            this.topptipsDistrLbl = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // loadDataBtn
            // 
            this.loadDataBtn.BackColor = System.Drawing.Color.Transparent;
            this.loadDataBtn.Location = new System.Drawing.Point(32, 133);
            this.loadDataBtn.Name = "loadDataBtn";
            this.loadDataBtn.Size = new System.Drawing.Size(115, 38);
            this.loadDataBtn.TabIndex = 0;
            this.loadDataBtn.Text = "Ladda speldata";
            this.loadDataBtn.UseVisualStyleBackColor = false;
            this.loadDataBtn.Click += new System.EventHandler(this.loadData_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(250, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 18);
            this.label2.TabIndex = 6;
            // 
            // lbl4
            // 
            this.lbl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl4.Location = new System.Drawing.Point(4, 39);
            this.lbl4.Name = "lbl4";
            this.lbl4.Size = new System.Drawing.Size(268, 37);
            this.lbl4.TabIndex = 8;
            this.lbl4.Text = "Spelfördelning Stryktipset/Europatipset";
            this.lbl4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pickFileButton
            // 
            this.pickFileButton.BackColor = System.Drawing.Color.Transparent;
            this.pickFileButton.Location = new System.Drawing.Point(32, 75);
            this.pickFileButton.Name = "pickFileButton";
            this.pickFileButton.Size = new System.Drawing.Size(115, 38);
            this.pickFileButton.TabIndex = 9;
            this.pickFileButton.Text = "Öppna Excel-fil";
            this.pickFileButton.UseVisualStyleBackColor = false;
            this.pickFileButton.Click += new System.EventHandler(this.openFile_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.tableLayoutPanel);
            this.groupBox1.Location = new System.Drawing.Point(171, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox1.Size = new System.Drawing.Size(511, 209);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Laddningar";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.84507F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.66287F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.79043F));
            this.tableLayoutPanel.Controls.Add(this.statusLblOddsSvs, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.topptipsDistrLbl, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.label1, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.lbl5, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.lbl4, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.label7, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.stryktipsDistributionLbl, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.label8, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.teamLoadLbl, 1, 4);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(10, 23);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(491, 176);
            this.tableLayoutPanel.TabIndex = 11;
            this.tableLayoutPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel_Paint);
            // 
            // statusLblOddsSvs
            // 
            this.statusLblOddsSvs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusLblOddsSvs.Location = new System.Drawing.Point(279, 115);
            this.statusLblOddsSvs.Name = "statusLblOddsSvs";
            this.statusLblOddsSvs.Size = new System.Drawing.Size(208, 37);
            this.statusLblOddsSvs.TabIndex = 17;
            this.statusLblOddsSvs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(4, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(268, 37);
            this.label1.TabIndex = 11;
            this.label1.Text = "Odds Svenska spel";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(4, 1);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(268, 37);
            this.label6.TabIndex = 12;
            this.label6.Text = "Källa";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(279, 1);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(208, 37);
            this.label7.TabIndex = 13;
            this.label7.Text = "Status";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stryktipsDistributionLbl
            // 
            this.stryktipsDistributionLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stryktipsDistributionLbl.Location = new System.Drawing.Point(279, 39);
            this.stryktipsDistributionLbl.Name = "stryktipsDistributionLbl";
            this.stryktipsDistributionLbl.Size = new System.Drawing.Size(208, 37);
            this.stryktipsDistributionLbl.TabIndex = 14;
            this.stryktipsDistributionLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 153);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(268, 22);
            this.label8.TabIndex = 18;
            this.label8.Text = "Matcher";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // teamLoadLbl
            // 
            this.teamLoadLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.teamLoadLbl.AutoSize = true;
            this.teamLoadLbl.Location = new System.Drawing.Point(279, 153);
            this.teamLoadLbl.Name = "teamLoadLbl";
            this.teamLoadLbl.Size = new System.Drawing.Size(208, 22);
            this.teamLoadLbl.TabIndex = 19;
            this.teamLoadLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "xls";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLbl});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 254);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusStrip1.Size = new System.Drawing.Size(717, 5);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLbl
            // 
            this.statusLbl.Name = "statusLbl";
            this.statusLbl.Size = new System.Drawing.Size(0, 0);
            // 
            // lbl5
            // 
            this.lbl5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl5.Location = new System.Drawing.Point(4, 77);
            this.lbl5.Name = "lbl5";
            this.lbl5.Size = new System.Drawing.Size(268, 37);
            this.lbl5.TabIndex = 7;
            this.lbl5.Text = "Spelfördelning Topptipset";
            this.lbl5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // topptipsDistrLbl
            // 
            this.topptipsDistrLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topptipsDistrLbl.Location = new System.Drawing.Point(279, 77);
            this.topptipsDistrLbl.Name = "topptipsDistrLbl";
            this.topptipsDistrLbl.Size = new System.Drawing.Size(208, 37);
            this.topptipsDistrLbl.TabIndex = 15;
            this.topptipsDistrLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BettingHelperWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(717, 259);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pickFileButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.loadDataBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "BettingHelperWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Betting Helper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BettingHelperWindow_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loadDataBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl4;
        private System.Windows.Forms.Button pickFileButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label statusLblOddsSvs;
        private System.Windows.Forms.Label stryktipsDistributionLbl;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLbl;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label teamLoadLbl;
        private System.Windows.Forms.Label topptipsDistrLbl;
        private System.Windows.Forms.Label lbl5;
    }
}

