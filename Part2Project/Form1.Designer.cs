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
            this.dlgChooseImage = new System.Windows.Forms.OpenFileDialog();
            this.viewer = new System.Windows.Forms.PictureBox();
            this.btnChooseImage = new System.Windows.Forms.Button();
            this.btnROT = new System.Windows.Forms.Button();
            this.btnBatchROT = new System.Windows.Forms.Button();
            this.dlgChooseFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.btnICFolder = new System.Windows.Forms.Button();
            this.btnIC = new System.Windows.Forms.Button();
            this.viewer2 = new System.Windows.Forms.PictureBox();
            this.btnICDiffMap = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.viewer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer2)).BeginInit();
            this.SuspendLayout();
            // 
            // dlgChooseImage
            // 
            this.dlgChooseImage.FileName = "openFileDialog1";
            this.dlgChooseImage.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgChooseImage_FileOk_1);
            // 
            // viewer
            // 
            this.viewer.Location = new System.Drawing.Point(12, 53);
            this.viewer.Name = "viewer";
            this.viewer.Size = new System.Drawing.Size(320, 240);
            this.viewer.TabIndex = 17;
            this.viewer.TabStop = false;
            // 
            // btnChooseImage
            // 
            this.btnChooseImage.Location = new System.Drawing.Point(12, 12);
            this.btnChooseImage.Name = "btnChooseImage";
            this.btnChooseImage.Size = new System.Drawing.Size(86, 37);
            this.btnChooseImage.TabIndex = 16;
            this.btnChooseImage.Text = "Choose Image";
            this.btnChooseImage.UseVisualStyleBackColor = true;
            this.btnChooseImage.Click += new System.EventHandler(this.btnChooseImage_Click_1);
            // 
            // btnROT
            // 
            this.btnROT.Location = new System.Drawing.Point(104, 10);
            this.btnROT.Name = "btnROT";
            this.btnROT.Size = new System.Drawing.Size(86, 37);
            this.btnROT.TabIndex = 27;
            this.btnROT.Text = "Rule Of Thirds";
            this.btnROT.UseVisualStyleBackColor = true;
            this.btnROT.Visible = false;
            this.btnROT.Click += new System.EventHandler(this.btnROT_Click);
            // 
            // btnBatchROT
            // 
            this.btnBatchROT.Location = new System.Drawing.Point(572, 10);
            this.btnBatchROT.Name = "btnBatchROT";
            this.btnBatchROT.Size = new System.Drawing.Size(86, 37);
            this.btnBatchROT.TabIndex = 28;
            this.btnBatchROT.Text = "RoT Folder";
            this.btnBatchROT.UseVisualStyleBackColor = true;
            this.btnBatchROT.Click += new System.EventHandler(this.btnBatchROT_Click);
            // 
            // btnICFolder
            // 
            this.btnICFolder.Location = new System.Drawing.Point(480, 10);
            this.btnICFolder.Name = "btnICFolder";
            this.btnICFolder.Size = new System.Drawing.Size(86, 37);
            this.btnICFolder.TabIndex = 29;
            this.btnICFolder.Text = "Intensity Contrast Folder";
            this.btnICFolder.UseVisualStyleBackColor = true;
            this.btnICFolder.Click += new System.EventHandler(this.btnICFolder_Click);
            // 
            // btnIC
            // 
            this.btnIC.Location = new System.Drawing.Point(196, 10);
            this.btnIC.Name = "btnIC";
            this.btnIC.Size = new System.Drawing.Size(86, 37);
            this.btnIC.TabIndex = 30;
            this.btnIC.Text = "Intensity Contrast";
            this.btnIC.UseVisualStyleBackColor = true;
            this.btnIC.Visible = false;
            this.btnIC.Click += new System.EventHandler(this.btnIC_Click);
            // 
            // viewer2
            // 
            this.viewer2.Location = new System.Drawing.Point(338, 53);
            this.viewer2.Name = "viewer2";
            this.viewer2.Size = new System.Drawing.Size(320, 240);
            this.viewer2.TabIndex = 31;
            this.viewer2.TabStop = false;
            // 
            // btnICDiffMap
            // 
            this.btnICDiffMap.Location = new System.Drawing.Point(288, 10);
            this.btnICDiffMap.Name = "btnICDiffMap";
            this.btnICDiffMap.Size = new System.Drawing.Size(95, 37);
            this.btnICDiffMap.TabIndex = 32;
            this.btnICDiffMap.Text = "Intensity Difference Map";
            this.btnICDiffMap.UseVisualStyleBackColor = true;
            this.btnICDiffMap.Visible = false;
            this.btnICDiffMap.Click += new System.EventHandler(this.btnICDiffMap_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(389, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(86, 37);
            this.btnSave.TabIndex = 33;
            this.btnSave.Text = "Save Both";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 311);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnICDiffMap);
            this.Controls.Add(this.viewer2);
            this.Controls.Add(this.btnIC);
            this.Controls.Add(this.btnICFolder);
            this.Controls.Add(this.btnBatchROT);
            this.Controls.Add(this.btnROT);
            this.Controls.Add(this.viewer);
            this.Controls.Add(this.btnChooseImage);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.viewer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog dlgChooseImage;
        private System.Windows.Forms.PictureBox viewer;
        private System.Windows.Forms.Button btnChooseImage;
        private System.Windows.Forms.Button btnROT;
        private System.Windows.Forms.Button btnBatchROT;
        private System.Windows.Forms.FolderBrowserDialog dlgChooseFolder;
        private System.Windows.Forms.Button btnICFolder;
        private System.Windows.Forms.Button btnIC;
        private System.Windows.Forms.PictureBox viewer2;
        private System.Windows.Forms.Button btnICDiffMap;
        private System.Windows.Forms.Button btnSave;
    }
}

