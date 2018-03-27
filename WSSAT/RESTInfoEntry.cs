using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WSSAT.DataTypes;

namespace WSSAT
{
    public partial class RESTInfoEntry : Form
    {
        private MainForm mainForm;

        public RESTInfoEntry()
        {
            InitializeComponent();
        }

        public RESTInfoEntry(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            RESTApi rest = new RESTApi();
            try
            {
                rest.Url = new Uri(txtURL.Text.Trim());
            }
            catch 
            {
                MessageBox.Show("Invalid URL");
                return;
            }

            rest.Method = cmbMethods.Text;
            rest.PostData = txtPostData.Text.Trim();
            rest.ContentType = cmbContentTypes.Text;

            if (!string.IsNullOrEmpty(txtUsername.Text) && !string.IsNullOrEmpty(txtPwd.Text))
            {
                rest.BasicAuthentication = new BasicAuthentication();
                rest.BasicAuthentication.Username = txtUsername.Text.Trim();
                rest.BasicAuthentication.Password = txtPwd.Text.Trim();
            }

            this.Hide();
            this.Close();
            this.mainForm.ScanRESTApi(rest);
        }

        private void RESTInfoEntry_Load(object sender, EventArgs e)
        {
            cmbContentTypes.SelectedIndex = 0;
            cmbMethods.SelectedIndex = 0;
        }
    }
}
