using System;
using System.Windows.Forms;
using WSSAT.DataTypes;

namespace WSSAT
{
    public partial class RESTInfoEntry : Form
    {
        private MainForm mainForm;
        private RESTApi restAPI;

        public RESTInfoEntry()
        {
            InitializeComponent();
        }

        public RESTInfoEntry(MainForm mainForm, RESTApi restAPI)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.restAPI = restAPI;
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            this.mainForm.RestAPIDesc = new RESTApi();
            try
            {
                this.mainForm.RestAPIDesc.Url = new Uri(txtURL.Text.Trim());
            }
            catch 
            {
                MessageBox.Show("Invalid URL");
                return;
            }

            this.mainForm.RestAPIDesc.Method = cmbMethods.Text;
            this.mainForm.RestAPIDesc.PostData = txtPostData.Text.Trim();
            this.mainForm.RestAPIDesc.ContentType = cmbContentTypes.Text;

            if (!string.IsNullOrEmpty(txtUsername.Text) && !string.IsNullOrEmpty(txtPwd.Text))
            {
                this.mainForm.RestAPIDesc.BasicAuthentication = new BasicAuthentication();
                this.mainForm.RestAPIDesc.BasicAuthentication.Username = txtUsername.Text.Trim();
                this.mainForm.RestAPIDesc.BasicAuthentication.Password = txtPwd.Text.Trim();
            }

            this.Hide();
            this.Close();
            this.mainForm.ScanRESTApi();
        }

        private void RESTInfoEntry_Load(object sender, EventArgs e)
        {
            cmbContentTypes.SelectedIndex = 0;
            cmbMethods.SelectedIndex = 0;

            if (restAPI != null)
            {
                txtURL.Text = restAPI.Url.AbsoluteUri;

                cmbMethods.SelectedIndex = cmbMethods.FindString(restAPI.Method);
                txtPostData.Text = restAPI.PostData;
                cmbContentTypes.SelectedIndex = cmbContentTypes.FindString(restAPI.ContentType);

                if (restAPI.BasicAuthentication != null)
                {
                    txtUsername.Text = restAPI.BasicAuthentication.Username;
                    txtPwd.Text = restAPI.BasicAuthentication.Password;
                }
            }
        }
    }
}
