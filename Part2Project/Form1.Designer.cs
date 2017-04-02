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
            this.dlgFile = new System.Windows.Forms.OpenFileDialog();
            this.viewer1 = new System.Windows.Forms.PictureBox();
            this.btnSelectImage = new System.Windows.Forms.Button();
            this.viewer2 = new System.Windows.Forms.PictureBox();
            this.btnBackDistractHist = new System.Windows.Forms.Button();
            this.viewer3 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.viewer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer3)).BeginInit();
            this.SuspendLayout();
            // 
            // dlgFile
            // 
            this.dlgFile.FileName = "openFileDialog1";
            this.dlgFile.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgFile_FileOk);
            // 
            // viewer1
            // 
            this.viewer1.Location = new System.Drawing.Point(12, 73);
            this.viewer1.Name = "viewer1";
            this.viewer1.Size = new System.Drawing.Size(320, 240);
            this.viewer1.TabIndex = 0;
            this.viewer1.TabStop = false;
            // 
            // btnSelectImage
            // 
            this.btnSelectImage.Location = new System.Drawing.Point(12, 12);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(81, 55);
            this.btnSelectImage.TabIndex = 1;
            this.btnSelectImage.Text = "Select Image";
            this.btnSelectImage.UseVisualStyleBackColor = true;
            this.btnSelectImage.Click += new System.EventHandler(this.btnSelectImage_Click);
            // 
            // viewer2
            // 
            this.viewer2.Location = new System.Drawing.Point(338, 73);
            this.viewer2.Name = "viewer2";
            this.viewer2.Size = new System.Drawing.Size(512, 512);
            this.viewer2.TabIndex = 2;
            this.viewer2.TabStop = false;
            // 
            // btnBackDistractHist
            // 
            this.btnBackDistractHist.Location = new System.Drawing.Point(99, 12);
            this.btnBackDistractHist.Name = "btnBackDistractHist";
            this.btnBackDistractHist.Size = new System.Drawing.Size(78, 55);
            this.btnBackDistractHist.TabIndex = 3;
            this.btnBackDistractHist.Text = "Background Distraction Histogram";
            this.btnBackDistractHist.UseVisualStyleBackColor = true;
            this.btnBackDistractHist.Click += new System.EventHandler(this.btnBackDistractHist_Click);
            // 
            // viewer3
            // 
            this.viewer3.Location = new System.Drawing.Point(12, 345);
            this.viewer3.Name = "viewer3";
            this.viewer3.Size = new System.Drawing.Size(320, 240);
            this.viewer3.TabIndex = 4;
            this.viewer3.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(335, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 329);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "label2";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(183, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(78, 55);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save Images";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 604);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.viewer3);
            this.Controls.Add(this.btnBackDistractHist);
            this.Controls.Add(this.viewer2);
            this.Controls.Add(this.btnSelectImage);
            this.Controls.Add(this.viewer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.viewer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog dlgFolder;
        private System.Windows.Forms.OpenFileDialog dlgFile;
        private System.Windows.Forms.PictureBox viewer1;
        private System.Windows.Forms.Button btnSelectImage;
        private System.Windows.Forms.PictureBox viewer2;
        private System.Windows.Forms.Button btnBackDistractHist;
        private System.Windows.Forms.PictureBox viewer3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;



    }
}

