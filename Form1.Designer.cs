
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
            this.buttonPicture = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.labelFileName = new System.Windows.Forms.Label();
            this.labelMenu2 = new System.Windows.Forms.Label();
            this.labelConnect = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.labelDistance = new System.Windows.Forms.Label();
            this.buttonStream = new System.Windows.Forms.Button();
            this.labelPixel = new System.Windows.Forms.Label();
            this.labelDistancePixel = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.labelCameraBox = new System.Windows.Forms.Label();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.labelListOfFiles = new System.Windows.Forms.Label();
            this.labelMaxValue = new System.Windows.Forms.Label();
            this.labelMinValue = new System.Windows.Forms.Label();
            this.pictureDepthBar = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureDepthBar)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.pictureBox1.Location = new System.Drawing.Point(344, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(640, 480);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseHover += new System.EventHandler(this.pictureBox1_MouseHover);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pixtureBox1_MouseMove);
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
            // 
            // buttonPlay
            // 
            this.buttonPlay.BackColor = System.Drawing.SystemColors.Control;
            this.buttonPlay.Location = new System.Drawing.Point(627, 513);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(75, 23);
            this.buttonPlay.TabIndex = 2;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.UseVisualStyleBackColor = false;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonRecord
            // 
            this.buttonRecord.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRecord.Location = new System.Drawing.Point(901, 513);
            this.buttonRecord.Name = "buttonRecord";
            this.buttonRecord.Size = new System.Drawing.Size(75, 23);
            this.buttonRecord.TabIndex = 3;
            this.buttonRecord.Text = "Record";
            this.buttonRecord.UseVisualStyleBackColor = false;
            this.buttonRecord.Click += new System.EventHandler(this.buttonRecord_Click);
            // 
            // buttonPicture
            // 
            this.buttonPicture.BackColor = System.Drawing.SystemColors.Control;
            this.buttonPicture.Location = new System.Drawing.Point(352, 513);
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
            this.buttonLoad.Location = new System.Drawing.Point(22, 54);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 6;
            this.buttonLoad.Text = "Load File";
            this.buttonLoad.UseVisualStyleBackColor = false;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // labelFileName
            // 
            this.labelFileName.AutoSize = true;
            this.labelFileName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(106)))), ((int)(((byte)(200)))));
            this.labelFileName.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelFileName.Location = new System.Drawing.Point(103, 58);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(94, 15);
            this.labelFileName.TabIndex = 7;
            this.labelFileName.Text = "No File Selected!";
            // 
            // labelMenu2
            // 
            this.labelMenu2.AutoSize = true;
            this.labelMenu2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.labelMenu2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelMenu2.Location = new System.Drawing.Point(13, 181);
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
            this.labelConnect.Location = new System.Drawing.Point(102, 208);
            this.labelConnect.Name = "labelConnect";
            this.labelConnect.Size = new System.Drawing.Size(91, 15);
            this.labelConnect.TabIndex = 9;
            this.labelConnect.Text = "Not Connected!";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(106)))), ((int)(((byte)(200)))));
            this.pictureBox2.Location = new System.Drawing.Point(12, 197);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(300, 102);
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
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
            this.pictureBox5.Size = new System.Drawing.Size(300, 118);
            this.pictureBox5.TabIndex = 13;
            this.pictureBox5.TabStop = false;
            // 
            // labelDistance
            // 
            this.labelDistance.AutoSize = true;
            this.labelDistance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(106)))), ((int)(((byte)(200)))));
            this.labelDistance.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelDistance.Location = new System.Drawing.Point(21, 270);
            this.labelDistance.Name = "labelDistance";
            this.labelDistance.Size = new System.Drawing.Size(144, 15);
            this.labelDistance.TabIndex = 14;
            this.labelDistance.Text = "Centre-Point Distance: ---";
            // 
            // buttonStream
            // 
            this.buttonStream.BackColor = System.Drawing.SystemColors.Control;
            this.buttonStream.Location = new System.Drawing.Point(21, 204);
            this.buttonStream.Name = "buttonStream";
            this.buttonStream.Size = new System.Drawing.Size(75, 23);
            this.buttonStream.TabIndex = 15;
            this.buttonStream.Text = "Start Cam";
            this.buttonStream.UseVisualStyleBackColor = false;
            this.buttonStream.Click += new System.EventHandler(this.buttonStream_Click);
            // 
            // labelPixel
            // 
            this.labelPixel.AutoSize = true;
            this.labelPixel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(106)))), ((int)(((byte)(200)))));
            this.labelPixel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelPixel.Location = new System.Drawing.Point(22, 242);
            this.labelPixel.Name = "labelPixel";
            this.labelPixel.Size = new System.Drawing.Size(71, 15);
            this.labelPixel.TabIndex = 16;
            this.labelPixel.Text = "Pixel: ---.---";
            // 
            // labelDistancePixel
            // 
            this.labelDistancePixel.AutoSize = true;
            this.labelDistancePixel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(106)))), ((int)(((byte)(200)))));
            this.labelDistancePixel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelDistancePixel.Location = new System.Drawing.Point(102, 242);
            this.labelDistancePixel.Name = "labelDistancePixel";
            this.labelDistancePixel.Size = new System.Drawing.Size(73, 15);
            this.labelDistancePixel.TabIndex = 17;
            this.labelDistancePixel.Text = "Distance: ---";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // labelCameraBox
            // 
            this.labelCameraBox.AutoSize = true;
            this.labelCameraBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.labelCameraBox.Location = new System.Drawing.Point(595, 265);
            this.labelCameraBox.Name = "labelCameraBox";
            this.labelCameraBox.Size = new System.Drawing.Size(139, 15);
            this.labelCameraBox.TabIndex = 18;
            this.labelCameraBox.Text = "Camera is not streaming!";
            // 
            // pictureBox6
            // 
            this.pictureBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(106)))), ((int)(((byte)(200)))));
            this.pictureBox6.Location = new System.Drawing.Point(344, 506);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(640, 38);
            this.pictureBox6.TabIndex = 19;
            this.pictureBox6.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.pictureBox3.Location = new System.Drawing.Point(12, 179);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(300, 19);
            this.pictureBox3.TabIndex = 11;
            this.pictureBox3.TabStop = false;
            // 
            // labelListOfFiles
            // 
            this.labelListOfFiles.AutoSize = true;
            this.labelListOfFiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(106)))), ((int)(((byte)(200)))));
            this.labelListOfFiles.Location = new System.Drawing.Point(22, 92);
            this.labelListOfFiles.Name = "labelListOfFiles";
            this.labelListOfFiles.Size = new System.Drawing.Size(0, 15);
            this.labelListOfFiles.TabIndex = 20;
            // 
            // labelMaxValue
            // 
            this.labelMaxValue.AutoSize = true;
            this.labelMaxValue.Location = new System.Drawing.Point(190, 333);
            this.labelMaxValue.Name = "labelMaxValue";
            this.labelMaxValue.Size = new System.Drawing.Size(51, 15);
            this.labelMaxValue.TabIndex = 21;
            this.labelMaxValue.Text = "Max: ---";
            // 
            // labelMinValue
            // 
            this.labelMinValue.AutoSize = true;
            this.labelMinValue.Location = new System.Drawing.Point(190, 529);
            this.labelMinValue.Name = "labelMinValue";
            this.labelMinValue.Size = new System.Drawing.Size(49, 15);
            this.labelMinValue.TabIndex = 22;
            this.labelMinValue.Text = "Min: ---";
            // 
            // pictureDepthBar
            // 
            this.pictureDepthBar.ImageLocation = "";
            this.pictureDepthBar.InitialImage = null;
            this.pictureDepthBar.Location = new System.Drawing.Point(125, 333);
            this.pictureDepthBar.Name = "pictureDepthBar";
            this.pictureDepthBar.Size = new System.Drawing.Size(59, 211);
            this.pictureDepthBar.TabIndex = 23;
            this.pictureDepthBar.TabStop = false;
            this.pictureDepthBar.WaitOnLoad = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(1008, 592);
            this.Controls.Add(this.pictureDepthBar);
            this.Controls.Add(this.labelMinValue);
            this.Controls.Add(this.labelMaxValue);
            this.Controls.Add(this.labelListOfFiles);
            this.Controls.Add(this.labelCameraBox);
            this.Controls.Add(this.labelDistancePixel);
            this.Controls.Add(this.labelPixel);
            this.Controls.Add(this.buttonStream);
            this.Controls.Add(this.labelDistance);
            this.Controls.Add(this.labelConnect);
            this.Controls.Add(this.labelMenu2);
            this.Controls.Add(this.labelFileName);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.buttonPicture);
            this.Controls.Add(this.buttonRecord);
            this.Controls.Add(this.buttonPlay);
            this.Controls.Add(this.labelMenu1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox6);
            this.Name = "Form1";
            this.Text = "RealSense Viewer Custom";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureDepthBar)).EndInit();
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
        private System.Windows.Forms.Button buttonPicture;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button buttonStream;

        /// Labels
        private System.Windows.Forms.Label labelMenu1;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.Label labelMenu2;
        private System.Windows.Forms.Label labelConnect;
        private System.Windows.Forms.Label labelDistance;
        private System.Windows.Forms.Label labelPixel;

        /// Images
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Label labelDistancePixel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label labelCameraBox;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label labelListOfFiles;
        private System.Windows.Forms.Label labelMaxValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureDepthBar;
        private System.Windows.Forms.Label labelMinValue;
    }
}

