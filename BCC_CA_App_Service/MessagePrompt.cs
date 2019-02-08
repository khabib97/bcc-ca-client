namespace BCC_CA_App_Service
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="MessagePrompt" />
    /// </summary>
    public partial  class MessagePrompt : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordPrompt"/> class.
        /// </summary>
        protected MessagePrompt(String _labelText, String _captionText)
        {
            InitializeComponent(_labelText, _captionText);
        }

        /// <summary>
        /// The passwordLabel_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void infoLabel_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The Prompt_Load
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void Prompt_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The enterButton_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// The ShowDialog
        /// </summary>
        /// <param name="text">The text<see cref="string"/></param>
        /// <param name="caption">The caption<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static void ShowDialog(string label, string caption){
            MessagePrompt prompt = new MessagePrompt(label, caption);
            prompt.ShowDialog();
        }
    }
}
