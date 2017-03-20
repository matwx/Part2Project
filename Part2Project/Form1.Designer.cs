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
            this.btnGBIS = new System.Windows.Forms.Button();
            this.viewer2 = new System.Windows.Forms.PictureBox();
            this.viewer = new System.Windows.Forms.PictureBox();
            this.btnChooseImage = new System.Windows.Forms.Button();
            this.txtK = new System.Windows.Forms.TextBox();
            this.txtSigma = new System.Windows.Forms.TextBox();
            this.cmboDisplayType = new System.Windows.Forms.ComboBox();
            this.cmboEdgeWeights = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.dlgFolder = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.viewer2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer)).BeginInit();
            this.SuspendLayout();
            // 
            // dlgChooseImage
            // 
            this.dlgChooseImage.FileName = "openFileDialog1";
            this.dlgChooseImage.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgChooseImage_FileOk);
            // 
            // btnGBIS
            // 
            this.btnGBIS.Location = new System.Drawing.Point(104, 11);
            this.btnGBIS.Name = "btnGBIS";
            this.btnGBIS.Size = new System.Drawing.Size(86, 37);
            this.btnGBIS.TabIndex = 7;
            this.btnGBIS.Text = "Segment Image";
            this.btnGBIS.UseVisualStyleBackColor = true;
            this.btnGBIS.Visible = false;
            this.btnGBIS.Click += new System.EventHandler(this.btnGBIS_Click);
            // 
            // viewer2
            // 
            this.viewer2.Location = new System.Drawing.Point(338, 53);
            this.viewer2.Name = "viewer2";
            this.viewer2.Size = new System.Drawing.Size(320, 240);
            this.viewer2.TabIndex = 6;
            this.viewer2.TabStop = false;
            // 
            // viewer
            // 
            this.viewer.Location = new System.Drawing.Point(12, 53);
            this.viewer.Name = "viewer";
            this.viewer.Size = new System.Drawing.Size(320, 240);
            this.viewer.TabIndex = 5;
            this.viewer.TabStop = false;
            // 
            // btnChooseImage
            // 
            this.btnChooseImage.Location = new System.Drawing.Point(12, 12);
            this.btnChooseImage.Name = "btnChooseImage";
            this.btnChooseImage.Size = new System.Drawing.Size(86, 37);
            this.btnChooseImage.TabIndex = 4;
            this.btnChooseImage.Text = "Choose Image";
            this.btnChooseImage.UseVisualStyleBackColor = true;
            this.btnChooseImage.Click += new System.EventHandler(this.btnChooseImage_Click);
            // 
            // txtK
            // 
            this.txtK.Location = new System.Drawing.Point(212, 21);
            this.txtK.Name = "txtK";
            this.txtK.Size = new System.Drawing.Size(53, 20);
            this.txtK.TabIndex = 8;
            this.txtK.Text = "400";
            this.txtK.Visible = false;
            // 
            // txtSigma
            // 
            this.txtSigma.Location = new System.Drawing.Point(308, 21);
            this.txtSigma.Name = "txtSigma";
            this.txtSigma.Size = new System.Drawing.Size(53, 20);
            this.txtSigma.TabIndex = 9;
            this.txtSigma.Text = "0.0";
            this.txtSigma.Visible = false;
            // 
            // cmboDisplayType
            // 
            this.cmboDisplayType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboDisplayType.FormattingEnabled = true;
            this.cmboDisplayType.Items.AddRange(new object[] {
            "Random",
            "Average"});
            this.cmboDisplayType.Location = new System.Drawing.Point(408, 20);
            this.cmboDisplayType.Name = "cmboDisplayType";
            this.cmboDisplayType.Size = new System.Drawing.Size(65, 21);
            this.cmboDisplayType.TabIndex = 10;
            this.cmboDisplayType.Visible = false;
            // 
            // cmboEdgeWeights
            // 
            this.cmboEdgeWeights.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboEdgeWeights.FormattingEnabled = true;
            this.cmboEdgeWeights.Items.AddRange(new object[] {
            "IntensityDiff",
            "CIELabDist",
            "CIEDE2000",
            "Hybrid",
            "HybridInterpolation",
            "NotZero"});
            this.cmboEdgeWeights.Location = new System.Drawing.Point(528, 21);
            this.cmboEdgeWeights.Name = "cmboEdgeWeights";
            this.cmboEdgeWeights.Size = new System.Drawing.Size(130, 21);
            this.cmboEdgeWeights.TabIndex = 11;
            this.cmboEdgeWeights.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(196, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "k:";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(271, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "sigma:";
            this.label2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(367, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 26);
            this.label3.TabIndex = 14;
            this.label3.Text = "Display\r\nType:";
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(479, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 26);
            this.label4.TabIndex = 15;
            this.label4.Text = "Edge\r\nWeights:";
            this.label4.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(664, 11);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(46, 37);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 305);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmboEdgeWeights);
            this.Controls.Add(this.cmboDisplayType);
            this.Controls.Add(this.txtSigma);
            this.Controls.Add(this.txtK);
            this.Controls.Add(this.btnGBIS);
            this.Controls.Add(this.viewer2);
            this.Controls.Add(this.viewer);
            this.Controls.Add(this.btnChooseImage);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.viewer2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog dlgChooseImage;
        private System.Windows.Forms.Button btnGBIS;
        private System.Windows.Forms.PictureBox viewer2;
        private System.Windows.Forms.PictureBox viewer;
        private System.Windows.Forms.Button btnChooseImage;
        private System.Windows.Forms.TextBox txtK;
        private System.Windows.Forms.TextBox txtSigma;
        private System.Windows.Forms.ComboBox cmboDisplayType;
        private System.Windows.Forms.ComboBox cmboEdgeWeights;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.FolderBrowserDialog dlgFolder;
    }
}

