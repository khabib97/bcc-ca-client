using System;
namespace BCC_CA_App_Service
{
    partial class MessagePrompt
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
        private void InitializeComponent(String _labelText, String _captionText)
        {
            this.infoLabel = new System.Windows.Forms.Label();
            this.exitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(26, 28);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(46, 17);
            this.infoLabel.TabIndex = 1;
            this.infoLabel.Text = _labelText;
            this.infoLabel.Click += new System.EventHandler(this.infoLabel_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(377, 66);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 30);
            this.exitButton.TabIndex = 2;
            this.exitButton.Text = "exit\r\n";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // Prompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 143);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.infoLabel);
            this.Name = "inputPrompt";
            this.Text = _captionText;
            this.Load += new System.EventHandler(this.Prompt_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Button exitButton;
    }
}