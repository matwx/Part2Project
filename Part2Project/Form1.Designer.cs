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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 305);
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
    }
}

