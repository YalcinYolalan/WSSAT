using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSSAT
{
    public partial class CustomRequestHeaderEntry : Form
    {
        public CustomRequestHeaderEntry()
        {
            InitializeComponent();
        }

        public CustomRequestHeaderEntry(string CustomRequestHeader, string UserAgentHeader)
        {
            InitializeComponent();
            txtUserAgent.Text = UserAgentHeader;
            txtCustomRequestHeader.Text = CustomRequestHeader;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            MainForm.CustomRequestHeader = txtCustomRequestHeader.Text.Trim();
            MainForm.UserAgentHeader = txtUserAgent.Text.Trim();

            this.Close();
        }
    }
}
