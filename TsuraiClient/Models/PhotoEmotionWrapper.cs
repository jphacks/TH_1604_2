using System;
using System.IO;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace TsuraiClient.Models
{
	public class PhotoEmotionWrapper
	{
		public delegate void CameraEventHandler(CameraEventArgs e);
		public event CameraEventHandler OnCameraError;

		private static PhotoEmotionWrapper instance;

		private PhotoEmotionWrapper(){}

		public static PhotoEmotionWrapper Instance 
		{
			get {
				if (instance == null) {
					instance = new PhotoEmotionWrapper();
				}
				return instance;
			}
		}

		public enum MediaType {
			Photo,
			Video,
			Picker
		}

		public enum CameraErrorCode {
			SizeError,
			NoCamera,
			NotSupport
		}

		private static readonly StoreCameraMediaOptions DefaultCameraMediaOptions = new StoreCameraMediaOptions
		{
			DefaultCamera = CameraDevice.Front,
			SaveToAlbum = false
		};

		public void GetUserEmotion(MediaType type, bool isOfficial = false) 
		{
			switch (type) {
				case MediaType.Photo:
					GetEmotionWithPhoto(isOfficial);
					break;
				case MediaType.Picker:
				case MediaType.Video:
					System.Diagnostics.Debug.WriteLine("Not supported");
					break;
			}
		}

		private async void GetEmotionWithPhoto(bool isOfficial)
		{
			//
			await CrossMedia.Current.Initialize();

			//
			if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
			{
				var file = await CrossMedia.Current.TakePhotoAsync(DefaultCameraMediaOptions);
				if (file == null) return;

				// up to 3Mb
				if (GetFileSize(file) > 1024 * 1024 * 4)
				{
					OnCameraError(new CameraEventArgs(CameraErrorCode.SizeError));
					return;
				}

				// crop min height or width to 512
				var image = DependencyService.Get<Services.IImageProcessingService>().ShrinkImage(file.GetStream(), 512f);

				if (image != null) {
					if (isOfficial)
						EmotionAPIConnector.Instance.JudgeUserEmotionPhoto(image);	
					else
						TsuraiAPIConnector.Instance.JudgeUserEmotionPhoto(image);	
				}
				else {
					if (isOfficial)
						EmotionAPIConnector.Instance.JudgeUserEmotionPhoto(file.GetStream());
					else
						TsuraiAPIConnector.Instance.JudgeUserEmotionPhoto(file.GetStream());
				}
			}
			else
			{
				OnCameraError(new CameraEventArgs(CameraErrorCode.NoCamera));
			}
		}

		private static long GetFileSize(MediaFile file)
		{
			using (var stream = file.GetStream())
			{
				var size = 0L;

				var buffer = new byte[1024 * 1024 * 10];
				var bytesRead = 0L;
				while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
				{
					size += bytesRead;
				}

				return size;
			}
		}

		public class CameraEventArgs: EventArgs
		{
			public CameraErrorCode ErrorCode { get;}

			public CameraEventArgs(CameraErrorCode code) 
			{
				this.ErrorCode = code;
			}
		}
	}
}
