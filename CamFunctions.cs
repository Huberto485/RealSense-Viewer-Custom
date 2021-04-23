using Intel.RealSense;

namespace RealSense_Viewer_Custom
{
	public class CamFunctions
	{

		bool connectedBool = false;
		bool recordBool = false;

		public string fileName()
		{
			return "file name";
		}

		/// <summary>
		/// Code snippet from C# wrapper library of RealSense.
		/// USE: Create a new instance of D435 camera and start it.
		/// RETURN: Sends distance in meters from camera.
		/// </summary>

		public float tryCam()
        {

			var pipe = new Pipeline();
			pipe.Start();

			using (var frames = pipe.WaitForFrames())
			using (var depth = frames.DepthFrame)
			{
				return depth.GetDistance(depth.Width / 2, depth.Height / 2);
			}
		}

		public void recordVideo()
        {
			while (recordBool == true)
            {

            }


        }

		public void takePicture()
        {

        }

	}
}