namespace BCC_CA_App_Service.App
{
    partial class Installer
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
            this.fileLoc = new System.Windows.Forms.Label();
            this.submitBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // fileLoc
            // 
            this.fileLoc.AutoSize = true;
            this.fileLoc.Location = new System.Drawing.Point(169, 140);
            this.fileLoc.Name = "fileLoc";
            this.fileLoc.Size = new System.Drawing.Size(0, 13);
            this.fileLoc.TabIndex = 3;
            // 
            // submitBtn
            // 
            this.submitBtn.Location = new System.Drawing.Point(81, 130);
            this.submitBtn.Name = "submitBtn";
            this.submitBtn.Size = new System.Drawing.Size(100, 23);
            this.submitBtn.TabIndex = 4;
            this.submitBtn.Text = "Install";
            this.submitBtn.UseVisualStyleBackColor = true;
            // 
            // Installer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.submitBtn);
            this.Controls.Add(this.fileLoc);
            this.Name = "Installer";
            this.Text = "Installer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label fileLoc;
        private System.Windows.Forms.Button submitBtn;
    }
}