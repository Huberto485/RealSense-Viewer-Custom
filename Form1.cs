using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RealSense_Viewer_Custom
{
    public partial class Form1 : Form
    {

        CamFunctions camera = new CamFunctions();

        //Array to hold information about the camera.
        //camInfo[0] - depth value (meters)
        List<string> camInfo = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// Button 'Load File' click events.
        private void buttonLoad_Click(object sender, EventArgs e)
        {

        }

        /// Button 'Take Picture' click events.
        /// DO: Take a snapshot of the depth sensor metadata from the camera.
        private void buttonPicture_Click(object sender, EventArgs e)
        {

            string infoFull = "";

            var picture = camera.takePicture();
            var distance = camera.getDepth();

            camInfo.Add(string.Format("Distance: {0:N3} meters\n", camera.getDepth()));
            camInfo.Add(string.Format("Distance: {0:N3} meters\n", camera.getDepth()));

            //Check whether a camera is connected.
            if ( camera.cameraConnected() == true )
            {
                labelConnect.Text = "Connected";
                infoFull += camInfo[0];
                infoFull += camInfo[1];

                //Judge whether the object is too close to the camera lens.
                if ( distance <= 0.2 )
                {
                    labelCameraInfo.Text = "Vision obscured!";
                }
                else if ( distance == 9999 )
                {
                    labelCameraInfo.Text = "No camera available!\t hello!";
                }
                else
                {
                    labelCameraInfo.Text = "Distance: " + Math.Round(distance, 3) + "m";
                }
            }
            else
            {
                labelConnect.Text = "Not connected";
            }

            labelCameraInfo.Text = infoFull;

        }

        private void buttonRecord_Click(object sender, EventArgs e)
        {

        }

        private void labelFileName_Click(object sender, EventArgs e)
        {

        }

        private void labelMenu1_Click(object sender, EventArgs e)
        {

        }

        private void labelMenu2_Click(Object sender, EventArgs e)
        {

        }

        private void labelConnect_Click(object sender, EventArgs e)
        {

        }
    }
}
