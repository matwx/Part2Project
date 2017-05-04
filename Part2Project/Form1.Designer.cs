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
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.viewer = new System.Windows.Forms.PictureBox();
            this.graph1 = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnChooseColours = new System.Windows.Forms.Button();
            this.graph2 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.viewer)).BeginInit();
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
            this.txt.Location = new System.Drawing.Point(12, 171);
            this.txt.Multiline = true;
            this.txt.Name = "txt";
            this.txt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt.Size = new System.Drawing.Size(499, 504);
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
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(14, 117);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(95, 48);
            this.button7.TabIndex = 12;
            this.button7.Text = "Original correlation with manual";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(416, 9);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(95, 48);
            this.button8.TabIndex = 14;
            this.button8.Text = "Average Stage3 Top 10";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(416, 63);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(95, 48);
            this.button9.TabIndex = 13;
            this.button9.Text = "Average Stage3 correlation";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(113, 117);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(95, 48);
            this.button10.TabIndex = 15;
            this.button10.Text = "Average Seg Efficient Feature values";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(214, 117);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(95, 48);
            this.button11.TabIndex = 16;
            this.button11.Text = "Average Seg Intuitive Feature values";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(416, 117);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(95, 48);
            this.button12.TabIndex = 18;
            this.button12.Text = "Average No Seg Intuitive Feature values";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(315, 117);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(95, 48);
            this.button13.TabIndex = 17;
            this.button13.Text = "Average No Seg Efficient Feature values";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // viewer
            // 
            this.viewer.Location = new System.Drawing.Point(517, 12);
            this.viewer.Name = "viewer";
            this.viewer.Size = new System.Drawing.Size(512, 512);
            this.viewer.TabIndex = 19;
            this.viewer.TabStop = false;
            // 
            // graph1
            // 
            this.graph1.Location = new System.Drawing.Point(618, 530);
            this.graph1.Name = "graph1";
            this.graph1.Size = new System.Drawing.Size(95, 48);
            this.graph1.TabIndex = 20;
            this.graph1.Text = "D1 and 3 Seg Man-Efficient Graph";
            this.graph1.UseVisualStyleBackColor = true;
            this.graph1.Click += new System.EventHandler(this.graph1_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(820, 530);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(95, 48);
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnChooseColours
            // 
            this.btnChooseColours.Location = new System.Drawing.Point(517, 530);
            this.btnChooseColours.Name = "btnChooseColours";
            this.btnChooseColours.Size = new System.Drawing.Size(95, 48);
            this.btnChooseColours.TabIndex = 22;
            this.btnChooseColours.Text = "Choose Colours";
            this.btnChooseColours.UseVisualStyleBackColor = true;
            this.btnChooseColours.Click += new System.EventHandler(this.btnChooseColours_Click);
            // 
            // graph2
            // 
            this.graph2.Location = new System.Drawing.Point(719, 530);
            this.graph2.Name = "graph2";
            this.graph2.Size = new System.Drawing.Size(95, 48);
            this.graph2.TabIndex = 23;
            this.graph2.Text = "D1 and 3 N-Seg Man-Efficient Graph";
            this.graph2.UseVisualStyleBackColor = true;
            this.graph2.Click += new System.EventHandler(this.graph2_Click);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(618, 584);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(95, 48);
            this.button14.TabIndex = 24;
            this.button14.Text = "D13SegEffGraph w/o broken";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1185, 687);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.graph2);
            this.Controls.Add(this.btnChooseColours);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.graph1);
            this.Controls.Add(this.viewer);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button7);
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
            ((System.ComponentModel.ISupportInitialize)(this.viewer)).EndInit();
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
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.PictureBox viewer;
        private System.Windows.Forms.Button graph1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnChooseColours;
        private System.Windows.Forms.Button graph2;
        private System.Windows.Forms.Button button14;



    }
}

