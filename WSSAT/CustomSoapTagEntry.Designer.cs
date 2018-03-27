namespace WSSAT
{
    partial class CustomSoapTagEntry
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
            this.txtCustomSoapHeaderTags = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCustomSoapBodyTags = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(22, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(220, 106);
            this.label1.TabIndex = 17;
            this.label1.Text = "Custom Soap:Header Tag(s)\r\n(This XML tags will be sent in <soap:Header> e.g. <tok" +
    "en>xxx</token><pass>xxx</pass>) ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCustomSoapHeaderTags
            // 
            this.txtCustomSoapHeaderTags.Location = new System.Drawing.Point(250, 29);
            this.txtCustomSoapHeaderTags.Margin = new System.Windows.Forms.Padding(4);
            this.txtCustomSoapHeaderTags.Multiline = true;
            this.txtCustomSoapHeaderTags.Name = "txtCustomSoapHeaderTags";
            this.txtCustomSoapHeaderTags.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCustomSoapHeaderTags.Size = new System.Drawing.Size(435, 104);
            this.txtCustomSoapHeaderTags.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(22, 152);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(220, 106);
            this.label2.TabIndex = 19;
            this.label2.Text = "Custom Soap:Body Tag(s)\r\n(This XML tags will be sent in <soap:Body> e.g. <token>x" +
    "xx</token><pass>xxx</pass>) ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCustomSoapBodyTags
            // 
            this.txtCustomSoapBodyTags.Location = new System.Drawing.Point(250, 152);
            this.txtCustomSoapBodyTags.Margin = new System.Windows.Forms.Padding(4);
            this.txtCustomSoapBodyTags.Multiline = true;
            this.txtCustomSoapBodyTags.Name = "txtCustomSoapBodyTags";
            this.txtCustomSoapBodyTags.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCustomSoapBodyTags.Size = new System.Drawing.Size(435, 104);
            this.txtCustomSoapBodyTags.TabIndex = 18;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(579, 339);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(96, 33);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // CustomSoapTagEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(721, 407);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCustomSoapBodyTags);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCustomSoapHeaderTags);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomSoapTagEntry";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Custom Soap Tags";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCustomSoapHeaderTags;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCustomSoapBodyTags;
        private System.Windows.Forms.Button btnSave;
    }
}