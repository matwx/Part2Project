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
            this.btnSegSaliency = new System.Windows.Forms.Button();
            this.btnROT = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.viewer2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer)).BeginInit();
            this.SuspendLayout();
            // 
            // dlgChooseImage
            // 
            this.dlgChooseImage.FileName = "openFileDialog1";
            this.dlgChooseImage.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgChooseImage_FileOk_1);
            // 
            // btnGBIS
            // 
            this.btnGBIS.Location = new System.Drawing.Point(104, 11);
            this.btnGBIS.Name = "btnGBIS";
            this.btnGBIS.Size = new System.Drawing.Size(86, 37);
            this.btnGBIS.TabIndex = 19;
            this.btnGBIS.Text = "Saliency Map";
            this.btnGBIS.UseVisualStyleBackColor = true;
            this.btnGBIS.Visible = false;
            this.btnGBIS.Click += new System.EventHandler(this.btnGBIS_Click_1);
            // 
            // viewer2
            // 
            this.viewer2.Location = new System.Drawing.Point(338, 53);
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
            this.btnChooseImage.Size = new System.Drawing.Size(86, 37);
            this.btnChooseImage.TabIndex = 16;
            this.btnChooseImage.Text = "Choose Image";
            this.btnChooseImage.UseVisualStyleBackColor = true;
            this.btnChooseImage.Click += new System.EventHandler(this.btnChooseImage_Click_1);
            // 
            // btnSegSaliency
            // 
            this.btnSegSaliency.Location = new System.Drawing.Point(196, 10);
            this.btnSegSaliency.Name = "btnSegSaliency";
            this.btnSegSaliency.Size = new System.Drawing.Size(86, 37);
            this.btnSegSaliency.TabIndex = 26;
            this.btnSegSaliency.Text = "Segment Saliency Map";
            this.btnSegSaliency.UseVisualStyleBackColor = true;
            this.btnSegSaliency.Visible = false;
            this.btnSegSaliency.Click += new System.EventHandler(this.btnSegSaliency_Click);
            // 
            // btnROT
            // 
            this.btnROT.Location = new System.Drawing.Point(288, 10);
            this.btnROT.Name = "btnROT";
            this.btnROT.Size = new System.Drawing.Size(86, 37);
            this.btnROT.TabIndex = 27;
            this.btnROT.Text = "Rule Of Thirds";
            this.btnROT.UseVisualStyleBackColor = true;
            this.btnROT.Visible = false;
            this.btnROT.Click += new System.EventHandler(this.btnROT_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 311);
            this.Controls.Add(this.btnROT);
            this.Controls.Add(this.btnSegSaliency);
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

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog dlgChooseImage;
        private System.Windows.Forms.Button btnGBIS;
        private System.Windows.Forms.PictureBox viewer2;
        private System.Windows.Forms.PictureBox viewer;
        private System.Windows.Forms.Button btnChooseImage;
        private System.Windows.Forms.Button btnSegSaliency;
        private System.Windows.Forms.Button btnROT;
    }
}

