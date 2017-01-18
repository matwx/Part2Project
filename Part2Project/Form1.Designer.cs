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
            this.box = new System.Windows.Forms.TextBox();
            this.btnResizeOriginals = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // box
            // 
            this.box.Location = new System.Drawing.Point(13, 65);
            this.box.Multiline = true;
            this.box.Name = "box";
            this.box.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.box.Size = new System.Drawing.Size(522, 258);
            this.box.TabIndex = 1;
            // 
            // btnResizeOriginals
            // 
            this.btnResizeOriginals.Location = new System.Drawing.Point(13, 13);
            this.btnResizeOriginals.Name = "btnResizeOriginals";
            this.btnResizeOriginals.Size = new System.Drawing.Size(95, 46);
            this.btnResizeOriginals.TabIndex = 2;
            this.btnResizeOriginals.Text = "Resize Originals";
            this.btnResizeOriginals.UseVisualStyleBackColor = true;
            this.btnResizeOriginals.Click += new System.EventHandler(this.btnResizeOriginals_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(114, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 46);
            this.button1.TabIndex = 3;
            this.button1.Text = "RoT sigma sweep";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 335);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnResizeOriginals);
            this.Controls.Add(this.box);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog dlgFolder;
        private System.Windows.Forms.TextBox box;
        private System.Windows.Forms.Button btnResizeOriginals;
        private System.Windows.Forms.Button button1;
    }
}

