namespace WSSAT
{
    partial class MainForm
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
            this.lvResult = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkXMLReport = new System.Windows.Forms.CheckBox();
            this.btnScanREST = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnCustomSoapTags = new System.Windows.Forms.Button();
            this.chkInfoDisclosure = new System.Windows.Forms.CheckBox();
            this.chkDynamicScan = new System.Windows.Forms.CheckBox();
            this.chkStaticScan = new System.Windows.Forms.CheckBox();
            this.chkDebug = new System.Windows.Forms.CheckBox();
            this.btnScan = new System.Windows.Forms.Button();
            this.lblSelectedFileName = new System.Windows.Forms.Label();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.scanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scanRestServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addCustomSoapTagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addCustomRequestHeaderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvResult
            // 
            this.lvResult.BackColor = System.Drawing.Color.Black;
            this.lvResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvResult.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lvResult.ForeColor = System.Drawing.Color.White;
            this.lvResult.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvResult.LabelWrap = false;
            this.lvResult.Location = new System.Drawing.Point(0, 182);
            this.lvResult.Margin = new System.Windows.Forms.Padding(4);
            this.lvResult.Name = "lvResult";
            this.lvResult.ShowItemToolTips = true;
            this.lvResult.Size = new System.Drawing.Size(1159, 555);
            this.lvResult.TabIndex = 0;
            this.lvResult.UseCompatibleStateImageBehavior = false;
            this.lvResult.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 750;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Black;
            this.groupBox1.Controls.Add(this.chkXMLReport);
            this.groupBox1.Controls.Add(this.btnScanREST);
            this.groupBox1.Controls.Add(this.btnAbout);
            this.groupBox1.Controls.Add(this.btnCustomSoapTags);
            this.groupBox1.Controls.Add(this.chkInfoDisclosure);
            this.groupBox1.Controls.Add(this.chkDynamicScan);
            this.groupBox1.Controls.Add(this.chkStaticScan);
            this.groupBox1.Controls.Add(this.chkDebug);
            this.groupBox1.Controls.Add(this.btnScan);
            this.groupBox1.Controls.Add(this.lblSelectedFileName);
            this.groupBox1.Controls.Add(this.btnOpenFile);
            this.groupBox1.Controls.Add(this.menuStrip1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(1159, 182);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // chkXMLReport
            // 
            this.chkXMLReport.AutoSize = true;
            this.chkXMLReport.Checked = true;
            this.chkXMLReport.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkXMLReport.ForeColor = System.Drawing.Color.White;
            this.chkXMLReport.Location = new System.Drawing.Point(744, 145);
            this.chkXMLReport.Margin = new System.Windows.Forms.Padding(4);
            this.chkXMLReport.Name = "chkXMLReport";
            this.chkXMLReport.Size = new System.Drawing.Size(151, 21);
            this.chkXMLReport.TabIndex = 18;
            this.chkXMLReport.Text = "Create XML Report";
            this.chkXMLReport.UseVisualStyleBackColor = true;
            // 
            // btnScanREST
            // 
            this.btnScanREST.BackColor = System.Drawing.Color.Black;
            this.btnScanREST.Location = new System.Drawing.Point(13, 140);
            this.btnScanREST.Margin = new System.Windows.Forms.Padding(4);
            this.btnScanREST.Name = "btnScanREST";
            this.btnScanREST.Size = new System.Drawing.Size(196, 28);
            this.btnScanREST.TabIndex = 16;
            this.btnScanREST.Text = "Scan REST Service";
            this.btnScanREST.UseVisualStyleBackColor = false;
            this.btnScanREST.Click += new System.EventHandler(this.btnScanREST_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.BackColor = System.Drawing.Color.Black;
            this.btnAbout.Location = new System.Drawing.Point(944, 140);
            this.btnAbout.Margin = new System.Windows.Forms.Padding(4);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(208, 28);
            this.btnAbout.TabIndex = 15;
            this.btnAbout.Text = "About WSSAT";
            this.btnAbout.UseVisualStyleBackColor = false;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnCustomSoapTags
            // 
            this.btnCustomSoapTags.BackColor = System.Drawing.Color.Black;
            this.btnCustomSoapTags.Location = new System.Drawing.Point(944, 97);
            this.btnCustomSoapTags.Margin = new System.Windows.Forms.Padding(4);
            this.btnCustomSoapTags.Name = "btnCustomSoapTags";
            this.btnCustomSoapTags.Size = new System.Drawing.Size(208, 28);
            this.btnCustomSoapTags.TabIndex = 14;
            this.btnCustomSoapTags.Text = "Add Custom Soap Tags";
            this.btnCustomSoapTags.UseVisualStyleBackColor = false;
            this.btnCustomSoapTags.Click += new System.EventHandler(this.btnCustomSoapTags_Click);
            // 
            // chkInfoDisclosure
            // 
            this.chkInfoDisclosure.AutoSize = true;
            this.chkInfoDisclosure.Checked = true;
            this.chkInfoDisclosure.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInfoDisclosure.ForeColor = System.Drawing.Color.White;
            this.chkInfoDisclosure.Location = new System.Drawing.Point(744, 117);
            this.chkInfoDisclosure.Margin = new System.Windows.Forms.Padding(4);
            this.chkInfoDisclosure.Name = "chkInfoDisclosure";
            this.chkInfoDisclosure.Size = new System.Drawing.Size(170, 21);
            this.chkInfoDisclosure.TabIndex = 13;
            this.chkInfoDisclosure.Text = "Information Disclosure";
            this.chkInfoDisclosure.UseVisualStyleBackColor = true;
            // 
            // chkDynamicScan
            // 
            this.chkDynamicScan.AutoSize = true;
            this.chkDynamicScan.Checked = true;
            this.chkDynamicScan.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDynamicScan.ForeColor = System.Drawing.Color.White;
            this.chkDynamicScan.Location = new System.Drawing.Point(744, 89);
            this.chkDynamicScan.Margin = new System.Windows.Forms.Padding(4);
            this.chkDynamicScan.Name = "chkDynamicScan";
            this.chkDynamicScan.Size = new System.Drawing.Size(120, 21);
            this.chkDynamicScan.TabIndex = 12;
            this.chkDynamicScan.Text = "Dynamic Scan";
            this.chkDynamicScan.UseVisualStyleBackColor = true;
            // 
            // chkStaticScan
            // 
            this.chkStaticScan.AutoSize = true;
            this.chkStaticScan.Checked = true;
            this.chkStaticScan.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStaticScan.ForeColor = System.Drawing.Color.White;
            this.chkStaticScan.Location = new System.Drawing.Point(744, 60);
            this.chkStaticScan.Margin = new System.Windows.Forms.Padding(4);
            this.chkStaticScan.Name = "chkStaticScan";
            this.chkStaticScan.Size = new System.Drawing.Size(101, 21);
            this.chkStaticScan.TabIndex = 11;
            this.chkStaticScan.Text = "Static Scan";
            this.chkStaticScan.UseVisualStyleBackColor = true;
            // 
            // chkDebug
            // 
            this.chkDebug.AutoSize = true;
            this.chkDebug.Checked = true;
            this.chkDebug.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDebug.ForeColor = System.Drawing.Color.White;
            this.chkDebug.Location = new System.Drawing.Point(968, 57);
            this.chkDebug.Margin = new System.Windows.Forms.Padding(4);
            this.chkDebug.Name = "chkDebug";
            this.chkDebug.Size = new System.Drawing.Size(72, 21);
            this.chkDebug.TabIndex = 10;
            this.chkDebug.Text = "Debug";
            this.chkDebug.UseVisualStyleBackColor = true;
            // 
            // btnScan
            // 
            this.btnScan.BackColor = System.Drawing.Color.Black;
            this.btnScan.Location = new System.Drawing.Point(13, 97);
            this.btnScan.Margin = new System.Windows.Forms.Padding(4);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(196, 28);
            this.btnScan.TabIndex = 9;
            this.btnScan.Text = "Scan SOAP Service(s)";
            this.btnScan.UseVisualStyleBackColor = false;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // lblSelectedFileName
            // 
            this.lblSelectedFileName.AutoSize = true;
            this.lblSelectedFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSelectedFileName.ForeColor = System.Drawing.Color.Tomato;
            this.lblSelectedFileName.Location = new System.Drawing.Point(253, 67);
            this.lblSelectedFileName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSelectedFileName.Name = "lblSelectedFileName";
            this.lblSelectedFileName.Size = new System.Drawing.Size(0, 17);
            this.lblSelectedFileName.TabIndex = 8;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.BackColor = System.Drawing.Color.Black;
            this.btnOpenFile.Location = new System.Drawing.Point(13, 60);
            this.btnOpenFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(196, 28);
            this.btnOpenFile.TabIndex = 7;
            this.btnOpenFile.Text = "Select WSDL List File";
            this.btnOpenFile.UseVisualStyleBackColor = false;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scanToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(3, 17);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1153, 28);
            this.menuStrip1.TabIndex = 17;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // scanToolStripMenuItem
            // 
            this.scanToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scanRestServiceToolStripMenuItem,
            this.addCustomSoapTagsToolStripMenuItem,
            this.addCustomRequestHeaderToolStripMenuItem});
            this.scanToolStripMenuItem.Name = "scanToolStripMenuItem";
            this.scanToolStripMenuItem.Size = new System.Drawing.Size(52, 24);
            this.scanToolStripMenuItem.Text = "Scan";
            // 
            // scanRestServiceToolStripMenuItem
            // 
            this.scanRestServiceToolStripMenuItem.Name = "scanRestServiceToolStripMenuItem";
            this.scanRestServiceToolStripMenuItem.Size = new System.Drawing.Size(276, 26);
            this.scanRestServiceToolStripMenuItem.Text = "Scan Rest Service";
            this.scanRestServiceToolStripMenuItem.Click += new System.EventHandler(this.scanRestServiceToolStripMenuItem_Click);
            // 
            // addCustomSoapTagsToolStripMenuItem
            // 
            this.addCustomSoapTagsToolStripMenuItem.Name = "addCustomSoapTagsToolStripMenuItem";
            this.addCustomSoapTagsToolStripMenuItem.Size = new System.Drawing.Size(276, 26);
            this.addCustomSoapTagsToolStripMenuItem.Text = "Add Custom Soap Tags";
            this.addCustomSoapTagsToolStripMenuItem.Click += new System.EventHandler(this.addCustomSoapTagsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(125, 26);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // addCustomRequestHeaderToolStripMenuItem
            // 
            this.addCustomRequestHeaderToolStripMenuItem.Name = "addCustomRequestHeaderToolStripMenuItem";
            this.addCustomRequestHeaderToolStripMenuItem.Size = new System.Drawing.Size(276, 26);
            this.addCustomRequestHeaderToolStripMenuItem.Text = "Add Custom Request Header";
            this.addCustomRequestHeaderToolStripMenuItem.Click += new System.EventHandler(this.addCustomRequestHeaderToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1159, 737);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lvResult);
            this.ForeColor = System.Drawing.Color.White;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "WEB SERVICE SECURITY ASSESSMENT TOOL (WSSAT)";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ListView lvResult;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkDynamicScan;
        private System.Windows.Forms.CheckBox chkStaticScan;
        private System.Windows.Forms.CheckBox chkDebug;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Label lblSelectedFileName;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.CheckBox chkInfoDisclosure;
        private System.Windows.Forms.Button btnCustomSoapTags;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button btnScanREST;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem scanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scanRestServiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addCustomSoapTagsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkXMLReport;
        private System.Windows.Forms.ToolStripMenuItem addCustomRequestHeaderToolStripMenuItem;
    }
}

