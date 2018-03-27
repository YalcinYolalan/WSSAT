namespace WSSAT
{
    partial class CustomRequestHeaderEntry
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtCustomRequestHeader = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUserAgent = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(13, 61);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 106);
            this.label1.TabIndex = 19;
            this.label1.Text = "Custom Request Header (Cookie: JSESSIONID=3BFFF43C6ED371C48048B44F52DD2770)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCustomRequestHeader
            // 
            this.txtCustomRequestHeader.Location = new System.Drawing.Point(223, 69);
            this.txtCustomRequestHeader.Margin = new System.Windows.Forms.Padding(4);
            this.txtCustomRequestHeader.Multiline = true;
            this.txtCustomRequestHeader.Name = "txtCustomRequestHeader";
            this.txtCustomRequestHeader.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCustomRequestHeader.Size = new System.Drawing.Size(336, 79);
            this.txtCustomRequestHeader.TabIndex = 18;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(463, 176);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(96, 33);
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(13, 16);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 45);
            this.label2.TabIndex = 23;
            this.label2.Text = "User-Agent";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtUserAgent
            // 
            this.txtUserAgent.Location = new System.Drawing.Point(223, 16);
            this.txtUserAgent.Margin = new System.Windows.Forms.Padding(4);
            this.txtUserAgent.Multiline = true;
            this.txtUserAgent.Name = "txtUserAgent";
            this.txtUserAgent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtUserAgent.Size = new System.Drawing.Size(336, 45);
            this.txtUserAgent.TabIndex = 22;
            // 
            // CustomRequestHeaderEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(572, 234);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtUserAgent);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCustomRequestHeader);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomRequestHeaderEntry";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Custom Request Header Entry";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCustomRequestHeader;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUserAgent;
    }
}