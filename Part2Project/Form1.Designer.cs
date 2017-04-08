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
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.viewer1 = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.viewer2 = new System.Windows.Forms.PictureBox();
            this.viewer3 = new System.Windows.Forms.PictureBox();
            this.viewer4 = new System.Windows.Forms.PictureBox();
            this.viewer5 = new System.Windows.Forms.PictureBox();
            this.btnSaveAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.viewer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer5)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 58);
            this.button1.TabIndex = 0;
            this.button1.Text = "Image";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk_1);
            // 
            // viewer1
            // 
            this.viewer1.Location = new System.Drawing.Point(12, 76);
            this.viewer1.Name = "viewer1";
            this.viewer1.Size = new System.Drawing.Size(320, 240);
            this.viewer1.TabIndex = 1;
            this.viewer1.TabStop = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(93, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 58);
            this.button2.TabIndex = 2;
            this.button2.Text = "Folder";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // viewer2
            // 
            this.viewer2.Location = new System.Drawing.Point(338, 76);
            this.viewer2.Name = "viewer2";
            this.viewer2.Size = new System.Drawing.Size(320, 240);
            this.viewer2.TabIndex = 3;
            this.viewer2.TabStop = false;
            // 
            // viewer3
            // 
            this.viewer3.Location = new System.Drawing.Point(664, 76);
            this.viewer3.Name = "viewer3";
            this.viewer3.Size = new System.Drawing.Size(320, 240);
            this.viewer3.TabIndex = 4;
            this.viewer3.TabStop = false;
            // 
            // viewer4
            // 
            this.viewer4.Location = new System.Drawing.Point(12, 322);
            this.viewer4.Name = "viewer4";
            this.viewer4.Size = new System.Drawing.Size(320, 240);
            this.viewer4.TabIndex = 5;
            this.viewer4.TabStop = false;
            // 
            // viewer5
            // 
            this.viewer5.Location = new System.Drawing.Point(338, 322);
            this.viewer5.Name = "viewer5";
            this.viewer5.Size = new System.Drawing.Size(320, 240);
            this.viewer5.TabIndex = 6;
            this.viewer5.TabStop = false;
            // 
            // btnSaveAll
            // 
            this.btnSaveAll.Location = new System.Drawing.Point(174, 12);
            this.btnSaveAll.Name = "btnSaveAll";
            this.btnSaveAll.Size = new System.Drawing.Size(75, 58);
            this.btnSaveAll.TabIndex = 7;
            this.btnSaveAll.Text = "Save All";
            this.btnSaveAll.UseVisualStyleBackColor = true;
            this.btnSaveAll.Click += new System.EventHandler(this.btnSaveAll_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1052, 617);
            this.Controls.Add(this.btnSaveAll);
            this.Controls.Add(this.viewer5);
            this.Controls.Add(this.viewer4);
            this.Controls.Add(this.viewer3);
            this.Controls.Add(this.viewer2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.viewer1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.viewer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewer5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.PictureBox viewer1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox viewer2;
        private System.Windows.Forms.PictureBox viewer3;
        private System.Windows.Forms.PictureBox viewer4;
        private System.Windows.Forms.PictureBox viewer5;
        private System.Windows.Forms.Button btnSaveAll;

    }
}

