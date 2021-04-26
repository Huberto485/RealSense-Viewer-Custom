
namespace RealSense_Viewer_Custom
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelMenu1 = new System.Windows.Forms.Label();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.buttonRecord = new System.Windows.Forms.Button();
            this.buttonPause = new System.Windows.Forms.Button();
            this.buttonPicture = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.labelFileName = new System.Windows.Forms.Label();
            this.labelMenu2 = new System.Windows.Forms.Label();
            this.labelConnect = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.labelCameraInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.GhostWhite;
            this.pictureBox1.Location = new System.Drawing.Point(318, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(678, 536);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // labelMenu1
            // 
            this.labelMenu1.AutoSize = true;
            this.labelMenu1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.labelMenu1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelMenu1.Location = new System.Drawing.Point(13, 29);
            this.labelMenu1.Name = "labelMenu1";
            this.labelMenu1.Size = new System.Drawing.Size(59, 15);
            this.labelMenu1.TabIndex = 1;
            this.labelMenu1.Text = "File Menu";
            this.labelMenu1.Click += new System.EventHandler(this.labelMenu1_Click);
            // 
            // buttonPlay
            // 
            this.buttonPlay.BackColor = System.Drawing.SystemColors.Control;
            this.buttonPlay.Location = new System.Drawing.Point(627, 529);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(75, 23);
            this.buttonPlay.TabIndex = 2;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.UseVisualStyleBackColor = false;
            // 
            // buttonRecord
            // 
            this.buttonRecord.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRecord.Location = new System.Drawing.Point(546, 529);
            this.buttonRecord.Name = "buttonRecord";
            this.buttonRecord.Size = new System.Drawing.Size(75, 23);
            this.buttonRecord.TabIndex = 3;
            this.buttonRecord.Text = "Record";
            this.buttonRecord.UseVisualStyleBackColor = false;
            // 
            // buttonPause
            // 
            this.buttonPause.BackColor = System.Drawing.SystemColors.Control;
            this.buttonPause.Location = new System.Drawing.Point(708, 529);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(75, 23);
            this.buttonPause.TabIndex = 4;
            this.buttonPause.Text = "Pause";
            this.buttonPause.UseVisualStyleBackColor = false;
            // 
            // buttonPicture
            // 
            this.buttonPicture.BackColor = System.Drawing.SystemColors.Control;
            this.buttonPicture.Location = new System.Drawing.Point(331, 529);
            this.buttonPicture.Name = "buttonPicture";
            this.buttonPicture.Size = new System.Drawing.Size(91, 23);
            this.buttonPicture.TabIndex = 5;
            this.buttonPicture.Text = "Take Picture";
            this.buttonPicture.UseVisualStyleBackColor = false;
            this.buttonPicture.Click += new System.EventHandler(this.buttonPicture_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.BackColor = System.Drawing.SystemColors.Control;
            this.buttonLoad.Location = new System.Drawing.Point(22, 52);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 6;
            this.buttonLoad.Text = "Load File";
            this.buttonLoad.UseVisualStyleBackColor = false;
            this.buttonLoad.Click += new System.EventHandler(this.labelMenu2_Click);
            // 
            // labelFileName
            // 
            this.labelFileName.AutoSize = true;
            this.labelFileName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(106)))), ((int)(((byte)(200)))));
            this.labelFileName.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelFileName.Location = new System.Drawing.Point(103, 56);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(68, 15);
            this.labelFileName.TabIndex = 7;
            this.labelFileName.Text = "Current File";
            this.labelFileName.Click += new System.EventHandler(this.labelFileName_Click);
            // 
            // labelMenu2
            // 
            this.labelMenu2.AutoSize = true;
            this.labelMenu2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.labelMenu2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelMenu2.Location = new System.Drawing.Point(13, 224);
            this.labelMenu2.Name = "labelMenu2";
            this.labelMenu2.Size = new System.Drawing.Size(93, 15);
            this.labelMenu2.TabIndex = 8;
            this.labelMenu2.Text = "Camera Settings";
            // 
            // labelConnect
            // 
            this.labelConnect.AutoSize = true;
            this.labelConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(106)))), ((int)(((byte)(200)))));
            this.labelConnect.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelConnect.Location = new System.Drawing.Point(213, 247);
            this.labelConnect.Name = "labelConnect";
            this.labelConnect.Size = new System.Drawing.Size(88, 15);
            this.labelConnect.TabIndex = 9;
            this.labelConnect.Text = "Not Connected";
            this.labelConnect.Click += new System.EventHandler(this.labelConnect_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(106)))), ((int)(((byte)(200)))));
            this.pictureBox2.Location = new System.Drawing.Point(12, 240);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(300, 323);
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.pictureBox3.Location = new System.Drawing.Point(12, 222);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(300, 19);
            this.pictureBox3.TabIndex = 11;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.pictureBox4.Location = new System.Drawing.Point(12, 27);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(300, 19);
            this.pictureBox4.TabIndex = 12;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(106)))), ((int)(((byte)(200)))));
            this.pictureBox5.Location = new System.Drawing.Point(12, 45);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(300, 158);
            this.pictureBox5.TabIndex = 13;
            this.pictureBox5.TabStop = false;
            // 
            // labelCameraInfo
            // 
            this.labelCameraInfo.AutoSize = true;
            this.labelCameraInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(106)))), ((int)(((byte)(200)))));
            this.labelCameraInfo.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelCameraInfo.Location = new System.Drawing.Point(22, 247);
            this.labelCameraInfo.Name = "labelCameraInfo";
            this.labelCameraInfo.Size = new System.Drawing.Size(0, 15);
            this.labelCameraInfo.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(1008, 592);
            this.Controls.Add(this.labelCameraInfo);
            this.Controls.Add(this.labelConnect);
            this.Controls.Add(this.labelMenu2);
            this.Controls.Add(this.labelFileName);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.buttonPicture);
            this.Controls.Add(this.buttonPause);
            this.Controls.Add(this.buttonRecord);
            this.Controls.Add(this.buttonPlay);
            this.Controls.Add(this.labelMenu1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.pictureBox4);
            this.Name = "Form1";
            this.Text = "RealSense Viewer Custom";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        /// <summary>
        /// Window elements of the app.
        /// </summary>

        /// Buttons  
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Button buttonRecord;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.Button buttonPicture;
        private System.Windows.Forms.Button button5;

        /// Labels
        private System.Windows.Forms.Label labelMenu1;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.Label labelMenu2;
        private System.Windows.Forms.Label labelConnect;


        /// Images
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Label labelCameraInfo;
    }
}

