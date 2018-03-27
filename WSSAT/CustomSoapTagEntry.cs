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
    public partial class CustomSoapTagEntry : Form
    {
        public CustomSoapTagEntry()
        {
            InitializeComponent();
        }

        public CustomSoapTagEntry(string CustomSoapHeaderTags, string CustomSoapBodyTags)
        {
            InitializeComponent();
            txtCustomSoapHeaderTags.Text = CustomSoapHeaderTags;
            txtCustomSoapBodyTags.Text = CustomSoapBodyTags;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            MainForm.CustomSoapHeaderTags = txtCustomSoapHeaderTags.Text.Trim();
            MainForm.CustomSoapBodyTags = txtCustomSoapBodyTags.Text.Trim();

            this.Close();
        }
    }
}
