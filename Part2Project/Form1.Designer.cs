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
            this.btnBrightnessFolder = new System.Windows.Forms.Button();
            this.btnBrightness = new System.Windows.Forms.Button();
            this.btnSat = new System.Windows.Forms.Button();
            this.btnSatFolder = new System.Windows.Forms.Button();
            this.btnSimplicity = new System.Windows.Forms.Button();
            this.btnSimpFolder = new System.Windows.Forms.Button();
            this.btnSegSal = new System.Windows.Forms.Button();
            this.btnSalientEnoughSegments = new System.Windows.Forms.Button();
            this.btnBoundingBoxes = new System.Windows.Forms.Button();
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
            this.btnROT.Location = new System.Drawing.Point(193, 10);
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
            this.btnBatchROT.Location = new System.Drawing.Point(756, 53);
            this.btnBatchROT.Name = "btnBatchROT";
            this.btnBatchROT.Size = new System.Drawing.Size(86, 37);
            this.btnBatchROT.TabIndex = 28;
            this.btnBatchROT.Text = "RoT Folder";
            this.btnBatchROT.UseVisualStyleBackColor = true;
            this.btnBatchROT.Click += new System.EventHandler(this.btnBatchROT_Click);
            // 
            // btnICFolder
            // 
            this.btnICFolder.Location = new System.Drawing.Point(664, 53);
            this.btnICFolder.Name = "btnICFolder";
            this.btnICFolder.Size = new System.Drawing.Size(86, 37);
            this.btnICFolder.TabIndex = 29;
            this.btnICFolder.Text = "Intensity Contrast Folder";
            this.btnICFolder.UseVisualStyleBackColor = true;
            this.btnICFolder.Click += new System.EventHandler(this.btnICFolder_Click);
            // 
            // btnIC
            // 
            this.btnIC.Location = new System.Drawing.Point(285, 10);
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
            // btnBrightnessFolder
            // 
            this.btnBrightnessFolder.Location = new System.Drawing.Point(756, 96);
            this.btnBrightnessFolder.Name = "btnBrightnessFolder";
            this.btnBrightnessFolder.Size = new System.Drawing.Size(86, 37);
            this.btnBrightnessFolder.TabIndex = 32;
            this.btnBrightnessFolder.Text = "Brightness Folder";
            this.btnBrightnessFolder.UseVisualStyleBackColor = true;
            this.btnBrightnessFolder.Click += new System.EventHandler(this.btnBrightnessFolder_Click);
            // 
            // btnBrightness
            // 
            this.btnBrightness.Location = new System.Drawing.Point(377, 10);
            this.btnBrightness.Name = "btnBrightness";
            this.btnBrightness.Size = new System.Drawing.Size(86, 37);
            this.btnBrightness.TabIndex = 33;
            this.btnBrightness.Text = "Brightness";
            this.btnBrightness.UseVisualStyleBackColor = true;
            this.btnBrightness.Visible = false;
            this.btnBrightness.Click += new System.EventHandler(this.btnBrightness_Click);
            // 
            // btnSat
            // 
            this.btnSat.Location = new System.Drawing.Point(469, 10);
            this.btnSat.Name = "btnSat";
            this.btnSat.Size = new System.Drawing.Size(86, 37);
            this.btnSat.TabIndex = 34;
            this.btnSat.Text = "Saturation";
            this.btnSat.UseVisualStyleBackColor = true;
            this.btnSat.Visible = false;
            this.btnSat.Click += new System.EventHandler(this.btnSat_Click);
            // 
            // btnSatFolder
            // 
            this.btnSatFolder.Location = new System.Drawing.Point(664, 96);
            this.btnSatFolder.Name = "btnSatFolder";
            this.btnSatFolder.Size = new System.Drawing.Size(86, 37);
            this.btnSatFolder.TabIndex = 35;
            this.btnSatFolder.Text = "Saturation Folder";
            this.btnSatFolder.UseVisualStyleBackColor = true;
            this.btnSatFolder.Click += new System.EventHandler(this.btnSatFolder_Click);
            // 
            // btnSimplicity
            // 
            this.btnSimplicity.Location = new System.Drawing.Point(561, 10);
            this.btnSimplicity.Name = "btnSimplicity";
            this.btnSimplicity.Size = new System.Drawing.Size(86, 37);
            this.btnSimplicity.TabIndex = 36;
            this.btnSimplicity.Text = "Simplicity";
            this.btnSimplicity.UseVisualStyleBackColor = true;
            this.btnSimplicity.Visible = false;
            this.btnSimplicity.Click += new System.EventHandler(this.btnSimplicity_Click);
            // 
            // btnSimpFolder
            // 
            this.btnSimpFolder.Location = new System.Drawing.Point(664, 139);
            this.btnSimpFolder.Name = "btnSimpFolder";
            this.btnSimpFolder.Size = new System.Drawing.Size(86, 37);
            this.btnSimpFolder.TabIndex = 37;
            this.btnSimpFolder.Text = "Simplicity Folder";
            this.btnSimpFolder.UseVisualStyleBackColor = true;
            this.btnSimpFolder.Click += new System.EventHandler(this.btnSimpFolder_Click);
            // 
            // btnSegSal
            // 
            this.btnSegSal.Location = new System.Drawing.Point(101, 10);
            this.btnSegSal.Name = "btnSegSal";
            this.btnSegSal.Size = new System.Drawing.Size(86, 37);
            this.btnSegSal.TabIndex = 38;
            this.btnSegSal.Text = "Segment Saliency Map";
            this.btnSegSal.UseVisualStyleBackColor = true;
            this.btnSegSal.Visible = false;
            this.btnSegSal.Click += new System.EventHandler(this.btnSegSal_Click);
            // 
            // btnSalientEnoughSegments
            // 
            this.btnSalientEnoughSegments.Location = new System.Drawing.Point(653, 10);
            this.btnSalientEnoughSegments.Name = "btnSalientEnoughSegments";
            this.btnSalientEnoughSegments.Size = new System.Drawing.Size(86, 37);
            this.btnSalientEnoughSegments.TabIndex = 39;
            this.btnSalientEnoughSegments.Text = "Salient Enough Segs";
            this.btnSalientEnoughSegments.UseVisualStyleBackColor = true;
            this.btnSalientEnoughSegments.Visible = false;
            this.btnSalientEnoughSegments.Click += new System.EventHandler(this.btnSalientEnoughSegments_Click);
            // 
            // btnBoundingBoxes
            // 
            this.btnBoundingBoxes.Location = new System.Drawing.Point(745, 10);
            this.btnBoundingBoxes.Name = "btnBoundingBoxes";
            this.btnBoundingBoxes.Size = new System.Drawing.Size(86, 37);
            this.btnBoundingBoxes.TabIndex = 40;
            this.btnBoundingBoxes.Text = "Bounding Boxes";
            this.btnBoundingBoxes.UseVisualStyleBackColor = true;
            this.btnBoundingBoxes.Visible = false;
            this.btnBoundingBoxes.Click += new System.EventHandler(this.btnBoundingBoxes_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(713, 224);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(86, 37);
            this.btnSave.TabIndex = 41;
            this.btnSave.Text = "Save Both";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 311);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnBoundingBoxes);
            this.Controls.Add(this.btnSalientEnoughSegments);
            this.Controls.Add(this.btnSegSal);
            this.Controls.Add(this.btnSimpFolder);
            this.Controls.Add(this.btnSimplicity);
            this.Controls.Add(this.btnSatFolder);
            this.Controls.Add(this.btnSat);
            this.Controls.Add(this.btnBrightness);
            this.Controls.Add(this.btnBrightnessFolder);
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
        private System.Windows.Forms.Button btnBrightnessFolder;
        private System.Windows.Forms.Button btnBrightness;
        private System.Windows.Forms.Button btnSat;
        private System.Windows.Forms.Button btnSatFolder;
        private System.Windows.Forms.Button btnSimplicity;
        private System.Windows.Forms.Button btnSimpFolder;
        private System.Windows.Forms.Button btnSegSal;
        private System.Windows.Forms.Button btnSalientEnoughSegments;
        private System.Windows.Forms.Button btnBoundingBoxes;
        private System.Windows.Forms.Button btnSave;
    }
}

