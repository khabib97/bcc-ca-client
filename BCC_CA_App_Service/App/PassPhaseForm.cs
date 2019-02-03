using System;
using System.Windows.Forms;

namespace BCC_CA_App_Service.App
{
    public partial class PassPhaseForm : Form
    {
        PinForm pin = new PinForm();
        String userInputedPassphase = null;


        public PassPhaseForm(String userInputedPassphase)
        {
            InitializeComponent();
            this.userInputedPassphase = userInputedPassphase;
        }


        private void submitBtn_Click(object sender, EventArgs e)
        {
            SecurityHandler.CheckPassPhaseValidity(userInputedPassphase, passField.Text);
            this.Hide();
            pin.Show();
        }

       
    }
}
