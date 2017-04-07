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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSigma = new System.Windows.Forms.TextBox();
            this.txtK = new System.Windows.Forms.TextBox();
            this.btnGBIS = new System.Windows.Forms.Button();
            this.viewer2 = new System.Windows.Forms.PictureBox();
            this.viewer = new System.Windows.Forms.PictureBox();
            this.btnChooseImage = new System.Windows.Forms.Button();
            this.btnSegSaliency = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.dlgFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.btnHighlightSmall = new System.Windows.Forms.Button();
            this.btnRenormalise = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.viewer2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer)).BeginInit();
            this.SuspendLayout();
            // 
            // dlgChooseImage
            // 
            this.dlgChooseImage.FileName = "openFileDialog1";
            this.dlgChooseImage.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgChooseImage_FileOk_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(486, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "sigma:";
            this.label2.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(411, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "k:";
            this.label1.Visible = false;
            // 
            // txtSigma
            // 
            this.txtSigma.Location = new System.Drawing.Point(523, 19);
            this.txtSigma.Name = "txtSigma";
            this.txtSigma.Size = new System.Drawing.Size(53, 20);
            this.txtSigma.TabIndex = 21;
            this.txtSigma.Text = "0.6";
            this.txtSigma.Visible = false;
            // 
            // txtK
            // 
            this.txtK.Location = new System.Drawing.Point(427, 19);
            this.txtK.Name = "txtK";
            this.txtK.Size = new System.Drawing.Size(53, 20);
            this.txtK.TabIndex = 20;
            this.txtK.Text = "125";
            this.txtK.Visible = false;
            // 
            // btnGBIS
            // 
            this.btnGBIS.Location = new System.Drawing.Point(73, 10);
            this.btnGBIS.Name = "btnGBIS";
            this.btnGBIS.Size = new System.Drawing.Size(57, 37);
            this.btnGBIS.TabIndex = 19;
            this.btnGBIS.Text = "Saliency Map";
            this.btnGBIS.UseVisualStyleBackColor = true;
            this.btnGBIS.Visible = false;
            this.btnGBIS.Click += new System.EventHandler(this.btnGBIS_Click_1);
            // 
            // viewer2
            // 
            this.viewer2.Location = new System.Drawing.Point(348, 53);
            this.viewer2.Name = "viewer2";
            this.viewer2.Size = new System.Drawing.Size(320, 240);
            this.viewer2.TabIndex = 18;
            this.viewer2.TabStop = false;
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
            this.btnChooseImage.Size = new System.Drawing.Size(55, 37);
            this.btnChooseImage.TabIndex = 16;
            this.btnChooseImage.Text = "Choose Image";
            this.btnChooseImage.UseVisualStyleBackColor = true;
            this.btnChooseImage.Click += new System.EventHandler(this.btnChooseImage_Click_1);
            // 
            // btnSegSaliency
            // 
            this.btnSegSaliency.Location = new System.Drawing.Point(136, 10);
            this.btnSegSaliency.Name = "btnSegSaliency";
            this.btnSegSaliency.Size = new System.Drawing.Size(86, 37);
            this.btnSegSaliency.TabIndex = 26;
            this.btnSegSaliency.Text = "Segment Saliency Map";
            this.btnSegSaliency.UseVisualStyleBackColor = true;
            this.btnSegSaliency.Visible = false;
            this.btnSegSaliency.Click += new System.EventHandler(this.btnSegSaliency_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(582, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 37);
            this.button1.TabIndex = 27;
            this.button1.Text = "Save Both";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnHighlightSmall
            // 
            this.btnHighlightSmall.Location = new System.Drawing.Point(307, 10);
            this.btnHighlightSmall.Name = "btnHighlightSmall";
            this.btnHighlightSmall.Size = new System.Drawing.Size(98, 37);
            this.btnHighlightSmall.TabIndex = 28;
            this.btnHighlightSmall.Text = "Highlight Ignored Segments";
            this.btnHighlightSmall.UseVisualStyleBackColor = true;
            this.btnHighlightSmall.Visible = false;
            this.btnHighlightSmall.Click += new System.EventHandler(this.btnHighlightSmall_Click);
            // 
            // btnRenormalise
            // 
            this.btnRenormalise.Location = new System.Drawing.Point(228, 10);
            this.btnRenormalise.Name = "btnRenormalise";
            this.btnRenormalise.Size = new System.Drawing.Size(73, 37);
            this.btnRenormalise.TabIndex = 29;
            this.btnRenormalise.Text = "Renormalise to Ignore";
            this.btnRenormalise.UseVisualStyleBackColor = true;
            this.btnRenormalise.Visible = false;
            this.btnRenormalise.Click += new System.EventHandler(this.btnRenormalise_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 311);
            this.Controls.Add(this.btnRenormalise);
            this.Controls.Add(this.btnHighlightSmall);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSegSaliency);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSigma;
        private System.Windows.Forms.TextBox txtK;
        private System.Windows.Forms.Button btnGBIS;
        private System.Windows.Forms.PictureBox viewer2;
        private System.Windows.Forms.PictureBox viewer;
        private System.Windows.Forms.Button btnChooseImage;
        private System.Windows.Forms.Button btnSegSaliency;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FolderBrowserDialog dlgFolder;
        private System.Windows.Forms.Button btnHighlightSmall;
        private System.Windows.Forms.Button btnRenormalise;
    }
}

