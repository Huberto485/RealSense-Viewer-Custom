using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using Intel.RealSense;

namespace RealSense_Viewer_Custom
{
    public partial class Form1 : Form
    {
        //Boolean values for logic inside functions.
        private bool depthVideo = false;
        private bool depthRecording = false;
        private bool depthStreaming = false;

        //Workers that work on different threads.
        private BackgroundWorker depthWorker = new BackgroundWorker();
        private BackgroundWorker recordWorker = new BackgroundWorker();
        private BackgroundWorker depthCheckWorker = new BackgroundWorker();

        //Global mouse coordinates for mouse over depth stream.
        private int mouseX = 0;
        private int mouseY = 0;

        private Pipeline pipeline = new Pipeline();

        //Frame filters which get applied.
        //Add a depth threshold filter.
        private ThresholdFilter thresholdFilter = new ThresholdFilter();
        private DisparityTransform disparityTransform = new DisparityTransform();
        private TemporalFilter temporalFilter = new TemporalFilter();
        private Colorizer colorizer = new Colorizer();

        //Information about camera settings.
        private float distance = 0;
        private float distancePixel = 0;

        //Bitmaps for holding depth and color frames.
        public Bitmap depthImage;
        //public Bitmap colorImage;

        //Create new configuration settings set for camera.
        private Config cfg = new Config();

        //New camera information variable class.
        private Context ctx = new Context();

        private delegate void InvokeDelegate();

        public Form1()
        {
            InitializeComponent();
            InitializeDepthWorker();
            InitializeDepthCheckWorker();
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
            //Initialize work-report-finish parts for the worker thread.
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

                        //Change configuration settings for the camera.
                        cfg.EnableStream(Stream.Depth, 640, 480);
                        //cfg.EnableStream(Stream.Color, 640, 480);

                        //Enable camera and start streaming with pre-set settings.
                        pipeline.Start(cfg);

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
                                    using (var frameSet = pipeline.WaitForFrames())
                                    using (var frame = frameSet.DepthFrame)
                                    //using (var color = frameSet.ColorFrame)
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

                        pipeline.Stop();
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
                labelDistance.Text = string.Format("Centre-Point Distance: {0:N3} meters", distance);
                pictureBox1.Image = depthImage;
            }
            else
            {
                //Change output to none.
                labelDistance.Text = "Centre-Point Distance: ---";
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
            labelDistance.Text = "Centre-Point Distance: ---";
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
        /// PIXEL DEPTH CHECK FUNCTIONALITY
        /// </summary>

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            if (depthCheckWorker.IsBusy != true && depthStreaming != false)
            {
                depthCheckWorker.RunWorkerAsync();
            }
        }

        private void pixtureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            mouseX = e.Location.X + 1;
            mouseY = e.Location.Y + 1;
        }

        private void InitializeDepthCheckWorker()
        {
            //Initialize work-report-finish parts for the worker thread.
            depthCheckWorker.DoWork += new DoWorkEventHandler(depthCheckWorker_DoWork);
            depthCheckWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(depthCheckWorker_RunWorkerCompleted);
            depthCheckWorker.ProgressChanged += new ProgressChangedEventHandler(depthCheckWorker_ProgressChanged);

            //Worker actions: can be cancelled; report progress./
            depthCheckWorker.WorkerSupportsCancellation = true;
            depthCheckWorker.WorkerReportsProgress = true;
        }

        private void depthCheckWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            using BackgroundWorker worker = sender as BackgroundWorker;
            {
                try
                {

                    while (depthStreaming == true)
                    {
                        //Set the latency to 25 milliseconds.
                        Thread.Sleep(25);

                        //Get frameSet from the pipeline and then get an individual frame.
                        using (var frameSet = pipeline.WaitForFrames())
                        using (var frame = frameSet.DepthFrame)
                        {
                            using DepthFrame depthFrame = frameSet.DepthFrame;

                            //Create a depth values array which is the size of 640 * 480 pixels.
                            //This is a 1D long array of ushort type.
                            var depthArray = new ushort[frame.Width * frame.Height];
                            depthFrame.CopyTo(depthArray);

                            //Get the pixel depth from the array.
                            //Get mouseY value, multiply it by 640 and add mouseX value to get depth index.
                            distancePixel = depthArray[(mouseY - 1) * 640 + (mouseX - 1)];
                            distancePixel /= 1000;
                        }

                        worker.ReportProgress(1);
                    }
                }
                catch (Exception error)
                {
                    worker.CancelAsync();
                }
                
            }
        }

        private void depthCheckWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Check progress flags.
            if (e.ProgressPercentage == 1)
            {
                if (mouseX >= 2 && mouseX <= 639 && mouseY >= 2 && mouseY <= 479)
                {
                    labelDistancePixel.Text = string.Format("Distance: {0:N3} meters", distancePixel);
                    labelPixel.Text = string.Format("Pixel: {0},{1}", mouseX, mouseY);
                }
                else
                {
                    //Change output to none.
                    labelDistancePixel.Text = "Distance: ---";
                    labelPixel.Text = "Pixel: ---,---";
                }
            }
            else
            {
                //Change output to none.
                labelDistancePixel.Text = "Distance: ---";
                labelPixel.Text = "Pixel: ---,---";
            }
        }

        private void depthCheckWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            labelDistancePixel.Text = "Distance: ---";
            labelPixel.Text = "Pixel: ---,---";
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
