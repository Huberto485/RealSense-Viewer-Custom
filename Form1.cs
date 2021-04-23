using System;
using System.Windows.Forms;

namespace RealSense_Viewer_Custom
{
    public partial class Form1 : Form
    {

        CamFunctions camera = new CamFunctions();
        bool camOn = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// Button 'Load File' on click events.
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            labelFileName.Text = camera.fileName();
            buttonLoad.Text = "Loaded";
        }

        private void buttonPicture_Click(object sender, EventArgs e)
        {

            var distance = camera.tryCam();
            labelConnect.Text = "Distance: " + Math.Round(distance, 3) + "m";
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        
    }
}
