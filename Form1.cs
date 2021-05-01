using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.IO;
using Intel.RealSense;

namespace RealSense_Viewer_Custom
{
    public partial class Form1 : Form
    {
        //Boolean values for logic inside functions.
        private bool depthPicture = false;
        private bool depthPlayback = false;
        private bool depthRecording = false;
        private bool depthStreaming = false;
        private bool restarted = false;

        //Workers that work on different threads.
        private BackgroundWorker depthWorker = new BackgroundWorker();
        private BackgroundWorker depthCheckWorker = new BackgroundWorker();

        //Global mouse coordinates for mouse over depth stream.
        private int mouseX = 0;
        private int mouseY = 0;

        //Variable to hold the file location to load.
        private string file;

        //Pipeline 
        private Pipeline pipeline = new Pipeline();

        //Frame filters which are applied to the depth frame.
        private ThresholdFilter thresholdFilter = new ThresholdFilter(); //Get min and max values that can be used to compare other values
        private DisparityTransform disparityTransform = new DisparityTransform(); //Make the depth smoother
        private TemporalFilter temporalFilter = new TemporalFilter(); //Reduce the amount of black spots (places of unknown depth)
        private Colorizer colorizer = new Colorizer(); //Colourize the frame

        //Information about camera settings.
        private float distance = 0;
        private float distancePixel = 0;

        //Bitmaps for holding depth and color frames.
        public Bitmap depthImage;

        //Create new configuration settings set for camera.
        private Config cfgDefault = new Config();
        private Config cfgCapture = new Config();
        private Config cfgPlayback = new Config();

        //New camera information variable class.
        private Context ctx = new Context();

        //Create a delegate variable which can be used for DoWork stage task reporting.
        private delegate void InvokeDelegate();

        public Form1()
        {
            InitializeComponent();
            InitializeDepthWorker();
            InitializeDepthCheckWorker();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// DEPTH FUNCTIONALITY
        /// </summary>

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var filePath = openFileDialog1.FileName;
                    file = filePath;
                    labelFileName.Text = file.Substring(file.Length - 15);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        //Depth streaming button for this thread.
        private void buttonStream_Click(object sender, EventArgs e)
        {
            if (depthWorker.IsBusy != true)
            {
                depthStreaming = true;
                depthWorker.RunWorkerAsync();
                buttonStream.Text = "Stop Cam";
            }
            else
            {
                depthStreaming = false;
                depthWorker.CancelAsync();
                buttonStream.Text = "Start Cam";
            }
        }

        //Depth recording button for this thread.
        private void buttonRecord_Click(object sender, EventArgs e)
        {
            if (depthRecording != true)
            {
                restarted = false;
                depthRecording = true;
                buttonRecord.Text = "Stop";
            }
            else
            {
                restarted = true;
                depthRecording = false;
                buttonRecord.Text = "Record";
            }
        }

        private void buttonPicture_Click(object sender, EventArgs e)
        {
            if (depthPicture != true)
            {
                depthPicture = true;
                restarted = false;
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

                //Change configuration settings for the camera.
                cfgDefault.EnableStream(Intel.RealSense.Stream.Depth, 640, 480);
                cfgCapture.EnableStream(Intel.RealSense.Stream.Depth, 640, 480);

                //Check if camera is already connected and streaming.
                if (list.Count != 0)
                {
                    if (depthStreaming == true)
                    {
                        //Update camera connection label.
                        labelConnect.BeginInvoke(new InvokeDelegate(labelConnectInvokeOn));

                        var random = new Random().Next(10000,99999);

                        //Enable camera and start streaming with pre-set settings.
                        pipeline.Start(cfgDefault);

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
                                else if (depthPlayback == true)
                                {

                                }
                                else if (depthRecording == true)
                                {
                                    pipeline.Stop();
                                    cfgCapture.EnableRecordToFile(Directory.GetCurrentDirectory() + "/Media/" 
                                        + new Random().Next(10000, 99999) + "_video.bag");
                                    pipeline.Start(cfgCapture);

                                    while (depthRecording == true && depthStreaming == true)
                                    {
                                        //Get camera depth from current frame.
                                        using (var frameSet = pipeline.WaitForFrames())
                                        using (var frame = frameSet.DepthFrame)
                                        {
                                            //Apply filters to the frame.
                                            var filteredFrame = thresholdFilter.Process(frame).DisposeWith(frameSet);
                                            filteredFrame = disparityTransform.Process(filteredFrame).DisposeWith(frameSet);
                                            filteredFrame = temporalFilter.Process(filteredFrame).DisposeWith(frameSet);
                                            filteredFrame = colorizer.Process(filteredFrame).DisposeWith(frameSet);

                                            //Put metadata from depth stream into its Bitmap.
                                            depthImage = new Bitmap(frame.Width, frame.Height,
                                                1920, System.Drawing.Imaging.PixelFormat.Format24bppRgb, filteredFrame.Data);

                                            distance = frame.GetDistance(frame.Width / 2, frame.Height / 2);
                                            //Update camera info panel.
                                            worker.ReportProgress(1);
                                        }
                                    }
                                    random = new Random().Next(10000,99999);
                                }
                                else if (depthPicture == true)
                                {
                                    pipeline.Stop();
                                    cfgCapture.EnableRecordToFile(Directory.GetCurrentDirectory() + "/Media/"
                                        + new Random().Next(10000, 99999) + "_image.bag");
                                    pipeline.Start(cfgCapture);
                                    Thread.Sleep(100);

                                    while (depthPicture == true && depthStreaming == true)
                                    {
                                        //Get camera depth from current frame.
                                        using (var frameSet = pipeline.WaitForFrames())
                                        using (var frame = frameSet.DepthFrame)
                                        {
                                            //Apply filters to the frame.
                                            var filteredFrame = thresholdFilter.Process(frame).DisposeWith(frameSet);
                                            filteredFrame = disparityTransform.Process(filteredFrame).DisposeWith(frameSet);
                                            filteredFrame = temporalFilter.Process(filteredFrame).DisposeWith(frameSet);
                                            filteredFrame = colorizer.Process(filteredFrame).DisposeWith(frameSet);

                                            //Put metadata from depth stream into its Bitmap.
                                            depthImage = new Bitmap(frame.Width, frame.Height,
                                                1920, System.Drawing.Imaging.PixelFormat.Format24bppRgb, filteredFrame.Data);

                                            distance = frame.GetDistance(frame.Width / 2, frame.Height / 2);
                                            //Update camera info panel.
                                            worker.ReportProgress(1);
                                            depthPicture = false;
                                        }
                                    }
                                    restarted = true;
                                    random = new Random().Next(10000, 99999);
                                }
                                else if (restarted == true)
                                {
                                    pipeline.Stop();
                                    pipeline.Start(cfgDefault);

                                    while (restarted == true && depthStreaming == true)
                                    {
                                        //Get camera depth from current frame.
                                        using (var frameSet = pipeline.WaitForFrames())
                                        using (var frame = frameSet.DepthFrame)
                                        {
                                            //Apply filters to the frame.
                                            var filteredFrame = thresholdFilter.Process(frame).DisposeWith(frameSet);
                                            filteredFrame = disparityTransform.Process(filteredFrame).DisposeWith(frameSet);
                                            filteredFrame = temporalFilter.Process(filteredFrame).DisposeWith(frameSet);
                                            filteredFrame = colorizer.Process(filteredFrame).DisposeWith(frameSet);

                                            //Put metadata from depth stream into its Bitmap.
                                            depthImage = new Bitmap(frame.Width, frame.Height,
                                                1920, System.Drawing.Imaging.PixelFormat.Format24bppRgb, filteredFrame.Data);

                                            distance = frame.GetDistance(frame.Width / 2, frame.Height / 2);
                                            //Update camera info panel.
                                            worker.ReportProgress(1);
                                        }
                                    }
                                }
                                else
                                {
                                    //Get camera depth from current frame.
                                    using (var frameSet = pipeline.WaitForFrames())
                                    using (var frame = frameSet.DepthFrame)
                                    {
                                        //Apply filters to the frame.
                                        var filteredFrame = thresholdFilter.Process(frame).DisposeWith(frameSet);
                                        filteredFrame = disparityTransform.Process(filteredFrame).DisposeWith(frameSet);
                                        filteredFrame = temporalFilter.Process(filteredFrame).DisposeWith(frameSet);
                                        filteredFrame = colorizer.Process(filteredFrame).DisposeWith(frameSet);

                                        //Put metadata from depth stream into its Bitmap.
                                        depthImage = new Bitmap(frame.Width, frame.Height,
                                            1920, System.Drawing.Imaging.PixelFormat.Format24bppRgb, filteredFrame.Data);

                                        distance = frame.GetDistance(frame.Width / 2, frame.Height / 2);
                                        //Update camera info panel.
                                        worker.ReportProgress(1);
                                    }
                                }
                            }

                            //Stop camera streaming.
                            pipeline.Stop();

                        }
                        catch (Exception error)
                        {
                            //Cancel thread action, reset button, and show an error.
                            depthWorker.CancelAsync();
                            buttonStream.BeginInvoke(new InvokeDelegate(buttonStreamInvokeOff));
                            MessageBox.Show("Error during runtime: " + error, "Error");
                        }
                    }
                    else
                    {
                        worker.ReportProgress(0);
                        worker.CancelAsync();
                    }
                }
                
                //If at any time this point is reached, cancel worker action.
                depthWorker.CancelAsync();
                buttonStream.BeginInvoke(new InvokeDelegate(buttonStreamInvokeOff));
                labelConnect.BeginInvoke(new InvokeDelegate(labelConnectInvokeOff));
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

                    while (depthStreaming == true || depthPlayback == true || depthPicture == true || depthRecording == true)
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
    }
}
