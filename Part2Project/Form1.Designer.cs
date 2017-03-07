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
            this.viewer1 = new System.Windows.Forms.PictureBox();
            this.viewer2 = new System.Windows.Forms.PictureBox();
            this.btnSelectImage = new System.Windows.Forms.Button();
            this.dlgImage = new System.Windows.Forms.OpenFileDialog();
            this.viewer3 = new System.Windows.Forms.PictureBox();
            this.btnEdge = new System.Windows.Forms.Button();
            this.btnSelectRealEdges = new System.Windows.Forms.Button();
            this.viewer4 = new System.Windows.Forms.PictureBox();
            this.dlgEdges = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.viewer5 = new System.Windows.Forms.PictureBox();
            this.viewer6 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.viewer8 = new System.Windows.Forms.PictureBox();
            this.viewer7 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.blurredNaive = new System.Windows.Forms.Label();
            this.blurredSaliency = new System.Windows.Forms.Label();
            this.btnPR = new System.Windows.Forms.Button();
            this.btnPRFolder = new System.Windows.Forms.Button();
            this.dlgPRFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.btnSalFolder = new System.Windows.Forms.Button();
            this.dlgEdgesFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.btnNaiveFolder = new System.Windows.Forms.Button();
            this.btnRandFolder = new System.Windows.Forms.Button();
            this.btnCGAccuracy = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.viewer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer7)).BeginInit();
            this.SuspendLayout();
            // 
            // viewer1
            // 
            this.viewer1.Location = new System.Drawing.Point(12, 57);
            this.viewer1.Name = "viewer1";
            this.viewer1.Size = new System.Drawing.Size(320, 240);
            this.viewer1.TabIndex = 0;
            this.viewer1.TabStop = false;
            // 
            // viewer2
            // 
            this.viewer2.Location = new System.Drawing.Point(338, 57);
            this.viewer2.Name = "viewer2";
            this.viewer2.Size = new System.Drawing.Size(320, 240);
            this.viewer2.TabIndex = 1;
            this.viewer2.TabStop = false;
            // 
            // btnSelectImage
            // 
            this.btnSelectImage.Location = new System.Drawing.Point(12, 12);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(75, 39);
            this.btnSelectImage.TabIndex = 2;
            this.btnSelectImage.Text = "Select Image";
            this.btnSelectImage.UseVisualStyleBackColor = true;
            this.btnSelectImage.Click += new System.EventHandler(this.btnSelectImage_Click);
            // 
            // dlgImage
            // 
            this.dlgImage.FileName = "openFileDialog1";
            this.dlgImage.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgImage_FileOk);
            // 
            // viewer3
            // 
            this.viewer3.Location = new System.Drawing.Point(664, 57);
            this.viewer3.Name = "viewer3";
            this.viewer3.Size = new System.Drawing.Size(320, 240);
            this.viewer3.TabIndex = 3;
            this.viewer3.TabStop = false;
            // 
            // btnEdge
            // 
            this.btnEdge.Location = new System.Drawing.Point(174, 12);
            this.btnEdge.Name = "btnEdge";
            this.btnEdge.Size = new System.Drawing.Size(75, 39);
            this.btnEdge.TabIndex = 4;
            this.btnEdge.Text = "Edge Maps";
            this.btnEdge.UseVisualStyleBackColor = true;
            this.btnEdge.Click += new System.EventHandler(this.btnEdge1_Click);
            // 
            // btnSelectRealEdges
            // 
            this.btnSelectRealEdges.Location = new System.Drawing.Point(93, 12);
            this.btnSelectRealEdges.Name = "btnSelectRealEdges";
            this.btnSelectRealEdges.Size = new System.Drawing.Size(77, 39);
            this.btnSelectRealEdges.TabIndex = 5;
            this.btnSelectRealEdges.Text = "Select Real Edges";
            this.btnSelectRealEdges.UseVisualStyleBackColor = true;
            this.btnSelectRealEdges.Click += new System.EventHandler(this.btnSelectRealEdges_Click);
            // 
            // viewer4
            // 
            this.viewer4.Location = new System.Drawing.Point(12, 594);
            this.viewer4.Name = "viewer4";
            this.viewer4.Size = new System.Drawing.Size(320, 240);
            this.viewer4.TabIndex = 6;
            this.viewer4.TabStop = false;
            // 
            // dlgEdges
            // 
            this.dlgEdges.FileName = "openFileDialog1";
            this.dlgEdges.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgEdges_FileOk);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(148, 300);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Original";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(453, 300);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Naive Edge Map";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(770, 300);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Saliency Edge Map";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(138, 837);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "True Edges";
            // 
            // viewer5
            // 
            this.viewer5.Location = new System.Drawing.Point(338, 325);
            this.viewer5.Name = "viewer5";
            this.viewer5.Size = new System.Drawing.Size(320, 240);
            this.viewer5.TabIndex = 11;
            this.viewer5.TabStop = false;
            // 
            // viewer6
            // 
            this.viewer6.Location = new System.Drawing.Point(664, 325);
            this.viewer6.Name = "viewer6";
            this.viewer6.Size = new System.Drawing.Size(320, 240);
            this.viewer6.TabIndex = 12;
            this.viewer6.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(433, 568);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Blurred Naive Edge Map";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(758, 568);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(135, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Blurred Saliency Edge Map";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(770, 837);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Blurred True Edges";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // viewer8
            // 
            this.viewer8.Location = new System.Drawing.Point(664, 594);
            this.viewer8.Name = "viewer8";
            this.viewer8.Size = new System.Drawing.Size(320, 240);
            this.viewer8.TabIndex = 15;
            this.viewer8.TabStop = false;
            // 
            // viewer7
            // 
            this.viewer7.Location = new System.Drawing.Point(338, 594);
            this.viewer7.Name = "viewer7";
            this.viewer7.Size = new System.Drawing.Size(320, 240);
            this.viewer7.TabIndex = 17;
            this.viewer7.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(433, 837);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(117, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Normalised True Edges";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(22, 364);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(207, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Pointwise Multipy Blurred True Edges with:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(46, 380);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Blurred Naive:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(34, 393);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(86, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "Blurred Saliency:";
            // 
            // blurredNaive
            // 
            this.blurredNaive.AutoSize = true;
            this.blurredNaive.Location = new System.Drawing.Point(126, 380);
            this.blurredNaive.Name = "blurredNaive";
            this.blurredNaive.Size = new System.Drawing.Size(0, 13);
            this.blurredNaive.TabIndex = 22;
            // 
            // blurredSaliency
            // 
            this.blurredSaliency.AutoSize = true;
            this.blurredSaliency.Location = new System.Drawing.Point(126, 393);
            this.blurredSaliency.Name = "blurredSaliency";
            this.blurredSaliency.Size = new System.Drawing.Size(0, 13);
            this.blurredSaliency.TabIndex = 23;
            // 
            // btnPR
            // 
            this.btnPR.Location = new System.Drawing.Point(785, 12);
            this.btnPR.Name = "btnPR";
            this.btnPR.Size = new System.Drawing.Size(108, 39);
            this.btnPR.TabIndex = 24;
            this.btnPR.Text = "Generate PR Stuff for current images";
            this.btnPR.UseVisualStyleBackColor = true;
            this.btnPR.Click += new System.EventHandler(this.btnPR_Click);
            // 
            // btnPRFolder
            // 
            this.btnPRFolder.Location = new System.Drawing.Point(899, 12);
            this.btnPRFolder.Name = "btnPRFolder";
            this.btnPRFolder.Size = new System.Drawing.Size(75, 39);
            this.btnPRFolder.TabIndex = 25;
            this.btnPRFolder.Text = "PR folder";
            this.btnPRFolder.UseVisualStyleBackColor = true;
            this.btnPRFolder.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSalFolder
            // 
            this.btnSalFolder.Location = new System.Drawing.Point(255, 12);
            this.btnSalFolder.Name = "btnSalFolder";
            this.btnSalFolder.Size = new System.Drawing.Size(85, 39);
            this.btnSalFolder.TabIndex = 26;
            this.btnSalFolder.Text = "Run SalEdges on Folder";
            this.btnSalFolder.UseVisualStyleBackColor = true;
            this.btnSalFolder.Click += new System.EventHandler(this.btnMyTestFolder_Click);
            // 
            // btnNaiveFolder
            // 
            this.btnNaiveFolder.Location = new System.Drawing.Point(346, 12);
            this.btnNaiveFolder.Name = "btnNaiveFolder";
            this.btnNaiveFolder.Size = new System.Drawing.Size(96, 39);
            this.btnNaiveFolder.TabIndex = 27;
            this.btnNaiveFolder.Text = "Run NaiveEdges on Folder";
            this.btnNaiveFolder.UseVisualStyleBackColor = true;
            this.btnNaiveFolder.Click += new System.EventHandler(this.btnNaiveFolder_Click);
            // 
            // btnRandFolder
            // 
            this.btnRandFolder.Location = new System.Drawing.Point(448, 12);
            this.btnRandFolder.Name = "btnRandFolder";
            this.btnRandFolder.Size = new System.Drawing.Size(102, 39);
            this.btnRandFolder.TabIndex = 28;
            this.btnRandFolder.Text = "Compute Random Accuracy";
            this.btnRandFolder.UseVisualStyleBackColor = true;
            this.btnRandFolder.Click += new System.EventHandler(this.btnRandFolder_Click);
            // 
            // btnCGAccuracy
            // 
            this.btnCGAccuracy.Location = new System.Drawing.Point(556, 12);
            this.btnCGAccuracy.Name = "btnCGAccuracy";
            this.btnCGAccuracy.Size = new System.Drawing.Size(85, 39);
            this.btnCGAccuracy.TabIndex = 29;
            this.btnCGAccuracy.Text = "Compute CG Accuracy";
            this.btnCGAccuracy.UseVisualStyleBackColor = true;
            this.btnCGAccuracy.Click += new System.EventHandler(this.btnCGAccuracy_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 863);
            this.Controls.Add(this.btnCGAccuracy);
            this.Controls.Add(this.btnRandFolder);
            this.Controls.Add(this.btnNaiveFolder);
            this.Controls.Add(this.btnSalFolder);
            this.Controls.Add(this.btnPRFolder);
            this.Controls.Add(this.btnPR);
            this.Controls.Add(this.blurredSaliency);
            this.Controls.Add(this.blurredNaive);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.viewer7);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.viewer8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.viewer6);
            this.Controls.Add(this.viewer5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.viewer4);
            this.Controls.Add(this.btnSelectRealEdges);
            this.Controls.Add(this.btnEdge);
            this.Controls.Add(this.viewer3);
            this.Controls.Add(this.btnSelectImage);
            this.Controls.Add(this.viewer2);
            this.Controls.Add(this.viewer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.viewer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer7)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox viewer1;
        private System.Windows.Forms.PictureBox viewer2;
        private System.Windows.Forms.Button btnSelectImage;
        private System.Windows.Forms.OpenFileDialog dlgImage;
        private System.Windows.Forms.PictureBox viewer3;
        private System.Windows.Forms.Button btnEdge;
        private System.Windows.Forms.Button btnSelectRealEdges;
        private System.Windows.Forms.PictureBox viewer4;
        private System.Windows.Forms.OpenFileDialog dlgEdges;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox viewer5;
        private System.Windows.Forms.PictureBox viewer6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox viewer8;
        private System.Windows.Forms.PictureBox viewer7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label blurredNaive;
        private System.Windows.Forms.Label blurredSaliency;
        private System.Windows.Forms.Button btnPR;
        private System.Windows.Forms.Button btnPRFolder;
        private System.Windows.Forms.FolderBrowserDialog dlgPRFolder;
        private System.Windows.Forms.Button btnSalFolder;
        private System.Windows.Forms.FolderBrowserDialog dlgEdgesFolder;
        private System.Windows.Forms.Button btnNaiveFolder;
        private System.Windows.Forms.Button btnRandFolder;
        private System.Windows.Forms.Button btnCGAccuracy;


    }
}

