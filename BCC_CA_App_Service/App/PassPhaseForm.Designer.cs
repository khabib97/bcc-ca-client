namespace BCC_CA_App_Service.App
{
    partial class PassPhaseForm
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
            this.passLbl = new System.Windows.Forms.Label();
            this.passField = new System.Windows.Forms.TextBox();
            this.submitBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // passLbl
            // 
            this.passLbl.AutoSize = true;
            this.passLbl.Location = new System.Drawing.Point(40, 133);
            this.passLbl.Name = "passLbl";
            this.passLbl.Size = new System.Drawing.Size(63, 13);
            this.passLbl.TabIndex = 0;
            this.passLbl.Text = "Pass Phase";
            // 
            // passField
            // 
            this.passField.Location = new System.Drawing.Point(151, 130);
            this.passField.Name = "passField";
            this.passField.Size = new System.Drawing.Size(100, 20);
            this.passField.TabIndex = 2;
            // 
            // submitBtn
            // 
            this.submitBtn.Location = new System.Drawing.Point(151, 214);
            this.submitBtn.Name = "submitBtn";
            this.submitBtn.Size = new System.Drawing.Size(100, 23);
            this.submitBtn.TabIndex = 4;
            this.submitBtn.Text = "Next";
            this.submitBtn.UseVisualStyleBackColor = true;
            this.submitBtn.Click += new System.EventHandler(this.submitBtn_Click);
            // 
            // UserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.submitBtn);
            this.Controls.Add(this.passField);
            this.Controls.Add(this.passLbl);
            this.Name = "UserForm";
            this.Text = "UserForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label passLbl;
        private System.Windows.Forms.TextBox passField;
        private System.Windows.Forms.Button submitBtn;
    }
}