﻿using Intel.RealSense;

namespace RealSense_Viewer_Custom
{
	public class CamFunctions
	{

		public string fileName()
		{
			return "file name";
		}

		public bool cameraConnected()
        {
			Context ctx = new Context();
			var devicesList = ctx.QueryDevices();

			if (devicesList.Count > 0)
            {
				return true;
            }
			else
            {
				return false;
            }

        }

		public float getDepth()
        {
			//Start streaming with default settings.
			using var pipe = new Pipeline();
			pipe.Start();

			using (var frames = pipe.WaitForFrames())
			using (var depth = frames.DepthFrame)
			{
				return depth.GetDistance(depth.Width / 2, depth.Height / 2);
			}
		}

		public void recordVideo()
        {

        }

		public int takePicture()
        {
			var pipe = new Pipeline();
			var cfg = new Config();

			//Enable camera infrared sensors for depth.
			cfg.EnableStream(Stream.Infrared, 1);
			cfg.EnableStream(Stream.Infrared, 2);

			pipe.Start(cfg);

			return 1;
		}

	}
}