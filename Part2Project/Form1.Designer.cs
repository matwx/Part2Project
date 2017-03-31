namespace Part2Project
{
    partial class Form1
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
            this.dlgFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.btnMultiThreaded = new System.Windows.Forms.Button();
            this.txt = new System.Windows.Forms.TextBox();
            this.btnSingleThreaded = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnMultiThreaded
            // 
            this.btnMultiThreaded.Location = new System.Drawing.Point(12, 12);
            this.btnMultiThreaded.Name = "btnMultiThreaded";
            this.btnMultiThreaded.Size = new System.Drawing.Size(94, 44);
            this.btnMultiThreaded.TabIndex = 0;
            this.btnMultiThreaded.Text = "Multi-threaded";
            this.btnMultiThreaded.UseVisualStyleBackColor = true;
            // 
            // txt
            // 
            this.txt.Location = new System.Drawing.Point(112, 12);
            this.txt.Multiline = true;
            this.txt.Name = "txt";
            this.txt.Size = new System.Drawing.Size(292, 468);
            this.txt.TabIndex = 1;
            // 
            // btnSingleThreaded
            // 
            this.btnSingleThreaded.Location = new System.Drawing.Point(12, 62);
            this.btnSingleThreaded.Name = "btnSingleThreaded";
            this.btnSingleThreaded.Size = new System.Drawing.Size(94, 44);
            this.btnSingleThreaded.TabIndex = 2;
            this.btnSingleThreaded.Text = "Single-threaded";
            this.btnSingleThreaded.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 614);
            this.Controls.Add(this.btnSingleThreaded);
            this.Controls.Add(this.txt);
            this.Controls.Add(this.btnMultiThreaded);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog dlgFolder;
        private System.Windows.Forms.Button btnMultiThreaded;
        private System.Windows.Forms.TextBox txt;
        private System.Windows.Forms.Button btnSingleThreaded;



    }
}

