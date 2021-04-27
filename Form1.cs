using System;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Intel.RealSense;

namespace RealSense_Viewer_Custom
{
    public partial class Form1 : Form
    {
        private bool cameraOn = false;
        private bool recordingOn = false;
        private bool depthStreaming = false;

        CamFunctions camera = new CamFunctions();

        //Workers that work on different threads
        private BackgroundWorker depthWorker = new BackgroundWorker();
        private BackgroundWorker recordWorker = new BackgroundWorker();

        //Information about camera settings
        private float distance = 0;

        public delegate void InvokeDelegate();

        public Form1()
        {
            InitializeComponent();

            InitializeDepthWorker();
            InitializeRecordWorker();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// DEPTH FUNCTIONALITY
        /// </summary>

        //Button for this thread.
        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (depthWorker.IsBusy != true)
            {
                depthWorker.RunWorkerAsync();
                buttonStart.Text = "Stop Cam";
            }
            else
            {
                depthWorker.CancelAsync();
                buttonStart.Text = "Start Cam";
            }
        }

        private void InitializeDepthWorker()
        {
            //Initialize begin-work-finish parts for the worker thread.
            depthWorker.DoWork += new DoWorkEventHandler(depthWorker_DoWork);
            depthWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(depthWorker_RunWorkerCompleted);
            depthWorker.ProgressChanged += new ProgressChangedEventHandler(depthWorker_ProgressChanged);

            //Worker actions: can be cancelled; report progress./
            depthWorker.WorkerSupportsCancellation = true;
            depthWorker.WorkerReportsProgress = true;
        }


        private void depthWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            using BackgroundWorker worker = sender as BackgroundWorker;
            {
                //Check if camera is already connected and streaming.
                if (depthStreaming != true)
                {
                    depthStreaming = !depthStreaming;
                    //Update camera connection label.
                    labelConnect.BeginInvoke(new InvokeDelegate(labelConnectInvoke));

                    //Start streaming with default settings.
                    using var pipe = new Pipeline();
                    pipe.Start();

                    while (depthStreaming == true)
                    {
                        //Check for process cancellation.
                        if (depthWorker.CancellationPending == true)
                        {
                            e.Cancel = true;
                            break;
                        }
                        else
                        {
                            //Get camera depth from current frame.
                            using (var frames = pipe.WaitForFrames())
                            using (var depth = frames.DepthFrame)
                            {
                                distance = depth.GetDistance(depth.Width / 2, depth.Height / 2);
                                //Update camera info panel.
                                worker.ReportProgress(1);
                            }
                        }
                    }
                }
                else
                {
                    worker.ReportProgress(0);
                }
            }
        }

        private void depthWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Check progress flags.
            if (e.ProgressPercentage == 1)
            {
                //If flag is 1, output depth.
                labelCameraInfo.Text = string.Format("Distance: {0:N3} meters", distance);
            }
            else
            {
                //Else change output to none;
                labelCameraInfo.Text = "Distance: ---";
            }
        }

        private void depthWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Set depth streaming back to false when method is not used anymore.
            if (depthStreaming != false)
            {
                depthStreaming = !depthStreaming;
            }

            labelConnect.Text = "Connected!";
        }

        /// <summary>
        /// INVOKE ELEMENTS FOR DEPTH STREAMING
        /// </summary>

        private void labelConnectInvoke()
        {
            labelConnect.Text = "Streaming!";
        }

        /// <summary>
        /// TAKE PICTURE FUNCTIONALITY
        /// </summary>

        private void buttonPicture_Click()
        {

        }

        /// <summary>
        /// RECORDING FUNCTIONALITY
        /// </summary>

        private void buttonRecord_Click()
        {
            if(recordWorker.IsBusy != true)
            {
                recordWorker.RunWorkerAsync();
            }
            else
            {
                recordWorker.CancelAsync();
            }
        }

        private void InitializeRecordWorker()
        {
            //Initialize begin-work-finish parts for the worker thread.
            recordWorker.DoWork += new DoWorkEventHandler(recordWorker_DoWork);
            recordWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(recordWorker_RunWorkerCompleted);
            recordWorker.ProgressChanged += new ProgressChangedEventHandler(recordWorker_ProgressChanged);

            //Worker actions: can be cancelled; report progress./
            recordWorker.WorkerSupportsCancellation = true;
            recordWorker.WorkerReportsProgress = true;
        }

        private void recordWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }

        private void recordWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }

        private void recordWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }


        /// <summary>
        /// LOADING FUNCTIONALITY
        /// </summary>

        /// Button 'Load File' click events.
        private void buttonLoad_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// VISUAL FUNCTIONALITY
        /// </summary>

        private void buttonPlay_Click(object sender, EventArgs e)
        {

        }

        private void labelMenu2_Click(Object sender, EventArgs e)
        {

        }
    }
}
