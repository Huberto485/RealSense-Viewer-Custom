using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RealSense_Viewer_Custom
{
    public partial class Form1 : Form
    {

        CamFunctions camera = new CamFunctions();

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

        private void labelFileName_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
