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
            this.viewer1 = new System.Windows.Forms.PictureBox();
            this.viewer2 = new System.Windows.Forms.PictureBox();
            this.btnSelectImage = new System.Windows.Forms.Button();
            this.dlgImage = new System.Windows.Forms.OpenFileDialog();
            this.viewer3 = new System.Windows.Forms.PictureBox();
            this.btnEdge = new System.Windows.Forms.Button();
            this.btnEdge2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.viewer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer3)).BeginInit();
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
            this.btnEdge.Location = new System.Drawing.Point(93, 12);
            this.btnEdge.Name = "btnEdge";
            this.btnEdge.Size = new System.Drawing.Size(75, 39);
            this.btnEdge.TabIndex = 4;
            this.btnEdge.Text = "Edge Maps";
            this.btnEdge.UseVisualStyleBackColor = true;
            this.btnEdge.Click += new System.EventHandler(this.btnEdge1_Click);
            // 
            // btnEdge2
            // 
            this.btnEdge2.Location = new System.Drawing.Point(174, 12);
            this.btnEdge2.Name = "btnEdge2";
            this.btnEdge2.Size = new System.Drawing.Size(75, 39);
            this.btnEdge2.TabIndex = 5;
            this.btnEdge2.Text = "Edge 2";
            this.btnEdge2.UseVisualStyleBackColor = true;
            this.btnEdge2.Click += new System.EventHandler(this.btnEdge2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 587);
            this.Controls.Add(this.btnEdge2);
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox viewer1;
        private System.Windows.Forms.PictureBox viewer2;
        private System.Windows.Forms.Button btnSelectImage;
        private System.Windows.Forms.OpenFileDialog dlgImage;
        private System.Windows.Forms.PictureBox viewer3;
        private System.Windows.Forms.Button btnEdge;
        private System.Windows.Forms.Button btnEdge2;


    }
}

