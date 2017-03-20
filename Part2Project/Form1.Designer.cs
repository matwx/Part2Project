﻿namespace Part2Project
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
            this.btnAvManCorrel = new System.Windows.Forms.Button();
            this.txt = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
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
            // btnAvManCorrel
            // 
            this.btnAvManCorrel.Location = new System.Drawing.Point(113, 63);
            this.btnAvManCorrel.Name = "btnAvManCorrel";
            this.btnAvManCorrel.Size = new System.Drawing.Size(95, 48);
            this.btnAvManCorrel.TabIndex = 1;
            this.btnAvManCorrel.Text = "Average manual correlation";
            this.btnAvManCorrel.UseVisualStyleBackColor = true;
            this.btnAvManCorrel.Click += new System.EventHandler(this.btnAvManCorrel1_Click);
            // 
            // txt
            // 
            this.txt.Location = new System.Drawing.Point(14, 117);
            this.txt.Multiline = true;
            this.txt.Name = "txt";
            this.txt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt.Size = new System.Drawing.Size(398, 418);
            this.txt.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(214, 63);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 48);
            this.button1.TabIndex = 6;
            this.button1.Text = "Average intuitive correlation";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(315, 63);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 48);
            this.button2.TabIndex = 7;
            this.button2.Text = "Average efficient correlation";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(214, 9);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(95, 48);
            this.button3.TabIndex = 8;
            this.button3.Text = "Average intuitive Top 10";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(315, 9);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(95, 48);
            this.button4.TabIndex = 9;
            this.button4.Text = "Average efficient Top 10";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(113, 10);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(95, 48);
            this.button5.TabIndex = 10;
            this.button5.Text = "Average manual Top 10";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(14, 63);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(95, 48);
            this.button6.TabIndex = 11;
            this.button6.Text = "Average random Top 10";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 547);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txt);
            this.Controls.Add(this.btnAvManCorrel);
            this.Controls.Add(this.btnReadRecords);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog dlgFolder;
        private System.Windows.Forms.Button btnReadRecords;
        private System.Windows.Forms.Button btnAvManCorrel;
        private System.Windows.Forms.TextBox txt;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;



    }
}

