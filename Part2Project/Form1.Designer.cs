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
            this.btnReadRecords = new System.Windows.Forms.Button();
            this.btnAvManCorrel1 = new System.Windows.Forms.Button();
            this.btnAvManCorrel2 = new System.Windows.Forms.Button();
            this.btnAvManCorrel3 = new System.Windows.Forms.Button();
            this.btnAvManCorrel4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnReadRecords
            // 
            this.btnReadRecords.Location = new System.Drawing.Point(12, 12);
            this.btnReadRecords.Name = "btnReadRecords";
            this.btnReadRecords.Size = new System.Drawing.Size(95, 45);
            this.btnReadRecords.TabIndex = 0;
            this.btnReadRecords.Text = "Load all records from folder";
            this.btnReadRecords.UseVisualStyleBackColor = true;
            this.btnReadRecords.Click += new System.EventHandler(this.btnReadRecords_Click);
            // 
            // btnAvManCorrel1
            // 
            this.btnAvManCorrel1.Location = new System.Drawing.Point(12, 63);
            this.btnAvManCorrel1.Name = "btnAvManCorrel1";
            this.btnAvManCorrel1.Size = new System.Drawing.Size(95, 48);
            this.btnAvManCorrel1.TabIndex = 1;
            this.btnAvManCorrel1.Text = "Average manual correlation for D1";
            this.btnAvManCorrel1.UseVisualStyleBackColor = true;
            // 
            // btnAvManCorrel2
            // 
            this.btnAvManCorrel2.Location = new System.Drawing.Point(113, 63);
            this.btnAvManCorrel2.Name = "btnAvManCorrel2";
            this.btnAvManCorrel2.Size = new System.Drawing.Size(95, 48);
            this.btnAvManCorrel2.TabIndex = 2;
            this.btnAvManCorrel2.Text = "Average manual correlation for D2";
            this.btnAvManCorrel2.UseVisualStyleBackColor = true;
            // 
            // btnAvManCorrel3
            // 
            this.btnAvManCorrel3.Location = new System.Drawing.Point(214, 63);
            this.btnAvManCorrel3.Name = "btnAvManCorrel3";
            this.btnAvManCorrel3.Size = new System.Drawing.Size(95, 48);
            this.btnAvManCorrel3.TabIndex = 3;
            this.btnAvManCorrel3.Text = "Average manual correlation for D3";
            this.btnAvManCorrel3.UseVisualStyleBackColor = true;
            // 
            // btnAvManCorrel4
            // 
            this.btnAvManCorrel4.Location = new System.Drawing.Point(315, 63);
            this.btnAvManCorrel4.Name = "btnAvManCorrel4";
            this.btnAvManCorrel4.Size = new System.Drawing.Size(95, 48);
            this.btnAvManCorrel4.TabIndex = 4;
            this.btnAvManCorrel4.Text = "Average manual correlation for D4";
            this.btnAvManCorrel4.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 300);
            this.Controls.Add(this.btnAvManCorrel4);
            this.Controls.Add(this.btnAvManCorrel3);
            this.Controls.Add(this.btnAvManCorrel2);
            this.Controls.Add(this.btnAvManCorrel1);
            this.Controls.Add(this.btnReadRecords);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog dlgFolder;
        private System.Windows.Forms.Button btnReadRecords;
        private System.Windows.Forms.Button btnAvManCorrel1;
        private System.Windows.Forms.Button btnAvManCorrel2;
        private System.Windows.Forms.Button btnAvManCorrel3;
        private System.Windows.Forms.Button btnAvManCorrel4;



    }
}

