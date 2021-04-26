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

            //Check whether a camera is connected.
            if ( camera.cameraConnected() == true )
            {
                labelConnect.Text = "Connected!";

                //Check if distance value has already been created in this form instance.
                if ( camInfo.Count == 1)
                {
                    camInfo[0] = (string.Format("Distance: {0:N3}m\n", camera.getDepth()));
                }
                else
                {
                    camInfo.Add(string.Format("Distance: {0:N3}m\n", camera.getDepth()));
                }

                //Add distance value to the information.
                infoFull += camInfo[0];
            }
            else
            {
                labelConnect.Text = "Not connected!";
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
