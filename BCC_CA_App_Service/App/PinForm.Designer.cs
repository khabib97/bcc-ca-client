namespace BCC_CA_App_Service.App
{
    partial class PinForm
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
            this.pinLbl = new System.Windows.Forms.Label();
            this.pinTxt = new System.Windows.Forms.TextBox();
            this.subBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pinLbl
            // 
            this.pinLbl.AutoSize = true;
            this.pinLbl.Location = new System.Drawing.Point(27, 115);
            this.pinLbl.Name = "pinLbl";
            this.pinLbl.Size = new System.Drawing.Size(77, 13);
            this.pinLbl.TabIndex = 0;
            this.pinLbl.Text = "Smart Card Pin";
            // 
            // pinTxt
            // 
            this.pinTxt.Location = new System.Drawing.Point(148, 112);
            this.pinTxt.Name = "pinTxt";
            this.pinTxt.Size = new System.Drawing.Size(100, 20);
            this.pinTxt.TabIndex = 1;
            // 
            // subBtn
            // 
            this.subBtn.Location = new System.Drawing.Point(148, 213);
            this.subBtn.Name = "subBtn";
            this.subBtn.Size = new System.Drawing.Size(100, 23);
            this.subBtn.TabIndex = 2;
            this.subBtn.Text = "Submit";
            this.subBtn.UseVisualStyleBackColor = true;
            this.subBtn.Click += new System.EventHandler(this.subBtn_Click);
            // 
            // PinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.subBtn);
            this.Controls.Add(this.pinTxt);
            this.Controls.Add(this.pinLbl);
            this.Name = "PinForm";
            this.Text = "PinForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label pinLbl;
        private System.Windows.Forms.TextBox pinTxt;
        private System.Windows.Forms.Button subBtn;
    }
}