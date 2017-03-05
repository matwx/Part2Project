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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.viewer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
            // btnSelectRealEdges
            // 
            this.btnSelectRealEdges.Location = new System.Drawing.Point(174, 12);
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
            this.label7.Location = new System.Drawing.Point(433, 837);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(123, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Blurred Naive Edge Map";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(338, 594);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 240);
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 863);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pictureBox1);
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
        private System.Windows.Forms.PictureBox pictureBox1;


    }
}

