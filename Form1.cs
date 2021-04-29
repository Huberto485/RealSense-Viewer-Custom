using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using Intel.RealSense;

namespace RealSense_Viewer_Custom
{
    public partial class Form1 : Form
    {
        private bool cameraOn = false;
        private bool recordingOn = false;
        private bool depthStreaming = false;

        //Workers that work on different threads.
        private BackgroundWorker depthWorker = new BackgroundWorker();
        private BackgroundWorker recordWorker = new BackgroundWorker();

        //Information about camera settings.
        private float distance = 0;

        //Bitmaps for holding depth and color frames.
        public Bitmap depthImage;
        public Bitmap colorImage;

        //New camera information variable class.
        private Context ctx = new Context();

        private delegate void InvokeDelegate();

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
        private void buttonStream_Click(object sender, EventArgs e)
        {
            if (depthWorker.IsBusy != true)
            {
                depthWorker.RunWorkerAsync();
                buttonStream.Text = "Stop Cam";
            }
            else
            {
                depthWorker.CancelAsync();
                buttonStream.Text = "Start Cam";
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
                using var list = ctx.QueryDevices(); 

                //Check if camera is already connected and streaming.
                if (list.Count != 0)
                {
                    if (depthStreaming != true)
                    {
                        depthStreaming = !depthStreaming;
                        //Update camera connection label.
                        labelConnect.BeginInvoke(new InvokeDelegate(labelConnectInvokeOn));

                        //Settings for streaming metadata.
                        using var cfg = new Config();
                        cfg.EnableStream(Stream.Depth, 640, 480);

                        /// <summary>
                        /// FRAME FILTERS
                        /// <summary>

                        //Convert depth to disparity.
                        using var disparityTransform = new DisparityTransform();

                        //Add a depth threshold filter.
                        using var thresholdFilter = new ThresholdFilter();

                        //Add a temporal filter to fill out holes.
                        using var temporalFilter = new TemporalFilter();

                        //Declare a new colorizer class for camera instance.
                        using var colorizer = new Colorizer();

                        //Create a new streaming-ready camera instance and start it.
                        using var pipe = new Pipeline();
                        pipe.Start(cfg);

                        try
                        {
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
                                    using (var frameSet = pipe.WaitForFrames())
                                    using (var frame = frameSet.DepthFrame)
                                    //using (var color = frames.ColorFrame)
                                    {
                                        //Apply filters to the frame.
                                        var filteredFrame = thresholdFilter.Process(frame).DisposeWith(frameSet);
                                        filteredFrame = disparityTransform.Process(filteredFrame).DisposeWith(frameSet);
                                        filteredFrame = temporalFilter.Process(filteredFrame).DisposeWith(frameSet);
                                        filteredFrame = colorizer.Process(filteredFrame).DisposeWith(frameSet);

                                        //Put metadata from depth stream and color stream into their respective Bitmaps.
                                        depthImage = new Bitmap(frame.Width, frame.Height,
                                            1920, System.Drawing.Imaging.PixelFormat.Format24bppRgb, filteredFrame.Data);
                                        //colorImage = new Bitmap(color.Width, color.Height,
                                        //    color.Stride, System.Drawing.Imaging.PixelFormat.Format24bppRgb, color.Data);

                                        distance = frame.GetDistance(frame.Width / 2, frame.Height / 2);
                                        //Update camera info panel.
                                        worker.ReportProgress(1);
                                    }
                                }
                            }
                        }
                        catch (Exception error)
                        {
                            //Cancel thread action, reset button, and show an error.
                            depthWorker.CancelAsync();
                            buttonStream.BeginInvoke(new InvokeDelegate(buttonStreamInvokeOff));
                            MessageBox.Show("Error during runtime: " + error, "Error");
                        }

                        pipe.Stop();
                    }
                    else
                    {
                        worker.ReportProgress(0);
                    }
                }
                else
                {
                    //Cancel thread action, reset button, and reset label.
                    depthWorker.CancelAsync();
                    buttonStream.BeginInvoke(new InvokeDelegate(buttonStreamInvokeOff));
                    labelConnect.BeginInvoke(new InvokeDelegate(labelConnectInvokeOff));
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
                pictureBox1.Image = depthImage;
            }
            else
            {
                //Change output to none.
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

            pictureBox1.Image = null;
            labelCameraInfo.Text = "Distance: ---";
        }

        /// <summary>
        /// INVOKE ELEMENTS FOR DEPTH STREAMING
        /// </summary>

        private void labelConnectInvokeOn()
        {
            labelConnect.Text = "Connected!";
        }

        private void labelConnectInvokeOff()
        {
            labelConnect.Text = "Not Connected!";
        }

        private void buttonStreamInvokeOff()
        {
            buttonStream.Text = "Start Cam";
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

        private void buttonRecord_Click(object sender, EventArgs e)
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
