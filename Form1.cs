using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Linq;
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
        private bool waitingToStop = true;
        private bool paused = false;

        //Workers that work on different threads.
        private BackgroundWorker depthWorker = new BackgroundWorker();
        private BackgroundWorker depthCheckWorker = new BackgroundWorker();
        private BackgroundWorker playbackWorker = new BackgroundWorker();

        //Global mouse coordinates for mouse over depth stream.
        private int mouseX = 0;
        private int mouseY = 0;

        //Variable to hold the file location to load.
        private string file;

        //Pipeline variable for global camera information - required as depth workers and checker can communicate same pipeline info.
        private Pipeline pipeline = new Pipeline();

        //Frame filters which are applied to the depth frame.
        private ThresholdFilter thresholdFilter = new ThresholdFilter(); //Get min and max values that can be used to compare other values
        private DisparityTransform disparityTransform = new DisparityTransform(); //Make the depth smoother
        private TemporalFilter temporalFilter = new TemporalFilter(); //Reduce the amount of black spots (places of unknown depth)
        private Colorizer colorizer = new Colorizer(); //Colourize the frame

        //Information about camera settings.
        private float distance = 0;
        private float distancePixel = 0;
        private float distancePixelMax = 0;
        private float distancePixelMin = 0;

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
            InitializePlaybackWorker();
            loadFileNames();
            pictureDepthBar.Image = Image.FromFile("D:/Program Files/VS Community Workspace/RealSense-Viewer-Custom/Graphics/depthBar.jpg");
            buttonPicture.Enabled = false;
            buttonRecord.Enabled = false;
            buttonPlay.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void loadFileNames()
        {
            //Get list of files from the media folder.
            string[] listOfFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "/Media/");
            labelListOfFiles.Text = "Your recordings:" + "\n";

            //If there are no files, update the label.
            if (listOfFiles.Length == 0)
            {
                labelListOfFiles.Text = "There are no previous recordings!";
            }
            else if (listOfFiles.Length > 0 && listOfFiles.Length < 3)
            {
                //If there are less than 3 files, update the label using max value.
                for (int i = 0; i < listOfFiles.Length; i++)
                {
                    labelListOfFiles.Text += listOfFiles[i].Substring(listOfFiles[i].Length - 15) + "\n";
                }
            }
            else
            {
                //If there are more than 3 files, just show the first 3 files.
                for (int i = 0; i < 3; i++)
                {
                    labelListOfFiles.Text += listOfFiles[i].Substring(listOfFiles[i].Length - 15) + "\n";
                }
            }
        }

        /// <summary>
        /// #########################
        /// ## DEPTH FUNCTIONALITY ##
        /// #########################
        /// </summary>

        //Loading depth file functionality.
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            //Check whether a valid option was chosen.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //Get file path.
                    var filePath = openFileDialog1.FileName;

                    //Check if this was an action taken during depth streaming.
                    if (depthWorker.IsBusy == true)
                    {
                        depthRecording = false;
                        depthStreaming = false;
                        depthWorker.CancelAsync();
                    }

                    //Set variables.
                    buttonStream.Enabled = false;
                    buttonLoad.Enabled = false;
                    buttonPlay.Enabled = true;
                    depthPlayback = true;
                    paused = true;

                    //Pass the file path to a global variable.
                    file = filePath;
                    labelFileName.Text = file.Substring(file.Length - 15);

                    //Enable the playback thread.
                    playbackWorker.RunWorkerAsync();
                }
                catch (Exception ex)
                {
                    //If an exception is thrown during file searching - throw an error.
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        //Depth streaming button for this thread.
        private void buttonStream_Click(object sender, EventArgs e)
        {
            //If this button was clicked while viewing media - cancel that thread.
            if (playbackWorker.IsBusy == true)
            {
                buttonPlay.Enabled = false;
                buttonLoad.Enabled = true;
                labelFileName.Text = "No File Selected!";

                playbackWorker.CancelAsync();
                pipeline.Stop();
                System.Diagnostics.Debug.WriteLine("playback stopped.");
            }

            //Check if the button is clicked while streaming or not.
            if (depthWorker.IsBusy != true)
            {
                //Enable buttons.
                buttonRecord.Enabled = true;
                buttonPicture.Enabled = true;

                //Set variable.
                depthStreaming = true;

                //Start the thread.
                depthWorker.RunWorkerAsync();

                labelCameraBox.Text = null;
                buttonStream.Text = "Stop Cam";
            }
            else
            {
                depthStreaming = false;
                depthRecording = false;
                depthWorker.CancelAsync();
                buttonStream.Text = "Start Cam";
            }
        }

        //Depth recording button for this thread.
        private void buttonRecord_Click(object sender, EventArgs e)
        {
            //Check if the button was previously clicked.
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

        //Take Picture button functionality.
        private void buttonPicture_Click(object sender, EventArgs e)
        {
            if (depthPicture != true)
            {
                depthPicture = true;
                restarted = false;
            }
        }

        //Play button functionality.
        private void buttonPlay_Click(object sender, EventArgs e)
        {
            //Check if the button was previously clicked.
            if (paused != true)
            {
                paused = true;
                waitingToStop = true;
                buttonPlay.Text = "Play";
            }
            else
            {
                waitingToStop = false;
                paused = false;
                buttonPlay.Text = "Pause";
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

                        //Generate a random number.
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
                                    //Finish video and restart with default settings.
                                    pipeline.Stop();
                                    pipeline.Start(cfgDefault);

                                    labelListOfFiles.BeginInvoke(new InvokeDelegate(updateListOfFiles));
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
                                    //Finish image and start with default settings.
                                    pipeline.Stop();
                                    pipeline.Start(cfgDefault);

                                    restarted = true;
                                    labelListOfFiles.BeginInvoke(new InvokeDelegate(updateListOfFiles));
                                    random = new Random().Next(10000, 99999);
                                }
                                else if (restarted == true)
                                {

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
                worker.CancelAsync();
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

            if (depthCheckWorker.IsBusy == true)
            {
                depthCheckWorker.CancelAsync();
            }

            depthCheckWorker.Dispose();

            buttonRecord.Enabled = false;
            buttonPicture.Enabled = false;
            labelCameraBox.Text = "Camera is not streaming!";
            pictureBox1.Image = null;
            labelDistance.Text = "Centre-Point Distance: ---";
        }

        /// <summary>
        /// #########################################
        /// ## INVOKE ELEMENTS FOR DEPTH STREAMING ##
        /// #########################################
        /// </summary>

        private void updateListOfFiles()
        {
            loadFileNames();
        }

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
        /// #####################################
        /// ## PIXEL DEPTH CHECK FUNCTIONALITY ##
        /// #####################################
        /// </summary>

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            if (depthCheckWorker.IsBusy != true && depthStreaming != false)
            {
                depthCheckWorker.RunWorkerAsync();
            }
            else if (depthCheckWorker.IsBusy != true && depthPlayback != false)
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
                        //Set the latency to 25 milliseconds - REQUIRED!
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

                            //Update max value
                            distancePixelMax = depthArray.Where(x => x < 4000).Max();
                            distancePixelMax /= 1000;

                            //Update min value.
                            distancePixelMin = depthArray.Where(x => x != 0).DefaultIfEmpty().Min();
                            distancePixelMin /= 1000;

                            worker.ReportProgress(1);

                            //While a video is being played and its paused, repeat these settings without reporting data.
                            //Update data by using delegates instead.
                            while (paused == true && depthPlayback == true)
                            {
                                //Set the latency to 25 milliseconds - REQUIRED!
                                Thread.Sleep(25);

                                distancePixel = depthArray[(mouseY - 1) * 640 + (mouseX - 1)];
                                distancePixel /= 1000;

                                distancePixelMax = depthArray.Where(x => x < 4000).Max();
                                distancePixelMax /= 1000;

                                distancePixelMin = depthArray.Where(x => x != 0).DefaultIfEmpty().Min();
                                distancePixelMin /= 1000;

                                labelMaxValue.BeginInvoke(new InvokeDelegate(updateMaxValue));
                                labelMinValue.BeginInvoke(new InvokeDelegate(updateMinValue));

                                labelDistancePixel.BeginInvoke(new InvokeDelegate(updatePixelDistance));
                                labelPixel.BeginInvoke(new InvokeDelegate(updatePixelValue));
                            }
                        }
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

                    //Update scale values.
                    labelMaxValue.Text = string.Format("Max: {0} meters", distancePixelMax);
                    labelMinValue.Text = string.Format("Min: {0} meters", distancePixelMin);
                }
                else
                {
                    //Change output to none.
                    labelDistancePixel.Text = "Distance: ---";
                    labelPixel.Text = "Pixel: ---,---";
                    labelMaxValue.Text = "Max: ---";
                    labelMinValue.Text = "Min: ---";
                }
            }
            else
            {
                //Change output to none.
                labelDistancePixel.Text = "Distance: ---";
                labelPixel.Text = "Pixel: ---,---";
                labelMaxValue.Text = "Max: ---";
                labelMinValue.Text = "Min: ---";
            }
        }

        private void depthCheckWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Reset labels.
            labelDistancePixel.Text = "Distance: ---";
            labelPixel.Text = "Pixel: ---,---";
            labelMaxValue.Text = "Max: ---";
            labelMinValue.Text = "Min: ---";
        }

        /// <summary>
        /// ##################################
        /// ## DEPTH CHECK WORKER DELEGATES ##
        /// ##################################
        /// </summary>

        private void updatePixelDistance()
        {
            labelDistancePixel.Text = string.Format("Distance: {0:N3} meters", distancePixel);
        }

        private void updatePixelValue()
        {
            labelPixel.Text = string.Format("Pixel: {0},{1}", mouseX, mouseY);
        }

        private void updateMaxValue()
        {
            labelMaxValue.Text = string.Format("Max: {0} meters", distancePixelMax);
        }

        private void updateMinValue()
        {
            labelMinValue.Text = string.Format("Min: {0} meters", distancePixelMin);
        }

        /// <summary>
        /// ############################
        /// ## PLAYBACK FUNCTIONALITY ##
        /// ############################
        /// </summary>

        private void InitializePlaybackWorker()
        {
            //Initialize work-report-finish parts for the worker thread.
            playbackWorker.DoWork += new DoWorkEventHandler(playbackWorker_DoWork);
            playbackWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(playbackWorker_RunWorkerCompleted);
            playbackWorker.ProgressChanged += new ProgressChangedEventHandler(playbackWorker_ProgressChanged);

            //Worker actions: can be cancelled; report progress./
            playbackWorker.WorkerSupportsCancellation = true;
            playbackWorker.WorkerReportsProgress = true;
        }

        private void playbackWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //If the thread was enabled while recording or streaming data already - disable it and reset variables.
            if (depthWorker.IsBusy == true)
            {
                depthRecording = false;
                depthStreaming = false;
                depthWorker.CancelAsync();
            }

            using BackgroundWorker worker = sender as BackgroundWorker;
            {
                //Count repeats.
                var i = 0;
                while (depthPlayback == true)
                {
                    //Check if video is being replayed automatically.
                    if ( i > 0)
                    {
                        pipeline.Stop();
                    }
                    //Create a playback device with new settings.
                    cfgPlayback.EnableDeviceFromFile(@file, repeat: false);
                    using (var device = pipeline.Start(cfgPlayback).Device)
                    using (var playback = PlaybackDevice.FromDevice(device))
                    {
                        try
                        {
                            while (depthPlayback == true)
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

                                //Look out for a stop flag.
                                if (waitingToStop == true)
                                {
                                    //Reset the flag.
                                    waitingToStop = false;

                                    //Pause the file reader.
                                    playback.Pause();

                                    //Deactivate thread until button is clicked again.
                                    while (paused == true)
                                    {
                                        Thread.Sleep(25);
                                    }

                                    //Resume the file reader.
                                    playback.Resume();
                                }
                            }
                        }
                        catch (Exception error)
                        {
                            //Exception thrown if there is no more data to stream - end of recording.
                            pipeline.Stop();
                            depthPlayback = false;
                            MessageBox.Show("End of recording!", "Recording finished!");
                        }
                    }
                    i++;
                }
                depthPlayback = false;
                worker.CancelAsync();
            }
        }

        private void playbackWorker_ProgressChanged (object sender, ProgressChangedEventArgs e)
        {
            if (labelCameraBox.Text != null)
            {
                labelCameraBox.Text = null;
            }

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

        private void playbackWorker_RunWorkerCompleted (object sender, RunWorkerCompletedEventArgs e)
        {
            //Set depth streaming back to false when method is not used anymore.
            if (depthPlayback != false)
            {
                depthPlayback = !depthPlayback;
            }

            if (depthCheckWorker.IsBusy == true)
            {
                depthCheckWorker.CancelAsync();
            }

            //Reset variables for loading of next file.
            waitingToStop = true;
            paused = true;

            //Reset camera info panel and buttons.
            buttonStream.Enabled = true;
            buttonLoad.Enabled = true;
            buttonPlay.Enabled = false;
            pictureBox1.Image = null;
            buttonPlay.Text = "Play";
            labelFileName.Text = "No File Selected!";
            labelCameraBox.Text = "Camera is not streaming!";
            labelDistance.Text = "Centre-Point Distance: ---";
        }
    }
}
