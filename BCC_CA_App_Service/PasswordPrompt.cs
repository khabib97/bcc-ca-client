namespace BCC_CA_App_Service
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="PasswordPrompt" />
    /// </summary>
    public partial  class PasswordPrompt : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordPrompt"/> class.
        /// </summary>
        protected PasswordPrompt(String _labelText, String _captionText)
        {
            InitializeComponent(_labelText, _captionText);
        }

        /// <summary>
        /// The passwordLabel_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void passwordLabel_Click(object sender, EventArgs e)
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
        private void enterButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// The ShowDialog
        /// </summary>
        /// <param name="text">The text<see cref="string"/></param>
        /// <param name="caption">The caption<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string ShowDialog(string label, string caption)         {
            PasswordPrompt prompt = new PasswordPrompt(label, caption);
            return prompt.ShowDialog() == DialogResult.OK ? prompt.passwordBox.Text : "";
        }
    }
}
