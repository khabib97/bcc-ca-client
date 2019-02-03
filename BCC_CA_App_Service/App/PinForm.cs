using System;
using System.Windows.Forms;

namespace BCC_CA_App_Service.App
{
    public partial class PinForm : Form
    {
        public PinForm()
        {
            InitializeComponent();
        }

        private void subBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
