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
            this.btnDoTest = new System.Windows.Forms.Button();
            this.dlgFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // btnDoTest
            // 
            this.btnDoTest.Location = new System.Drawing.Point(12, 12);
            this.btnDoTest.Name = "btnDoTest";
            this.btnDoTest.Size = new System.Drawing.Size(112, 47);
            this.btnDoTest.TabIndex = 0;
            this.btnDoTest.Text = "Create Orderings for All Features for All Datasets";
            this.btnDoTest.UseVisualStyleBackColor = true;
            this.btnDoTest.Click += new System.EventHandler(this.btnDoTest_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 373);
            this.Controls.Add(this.btnDoTest);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDoTest;
        private System.Windows.Forms.FolderBrowserDialog dlgFolder;



    }
}

