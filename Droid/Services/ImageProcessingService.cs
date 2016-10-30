using System;
using System.IO;
using Android.Graphics;

[assembly: Xamarin.Forms.Dependency(typeof(TsuraiClient.Droid.ImageProcessingService))]
namespace TsuraiClient.Droid
{
	public class ImageProcessingService : TsuraiClient.Services.IImageProcessingService
	{
		// http://www.united-bears.co.jp/blog/archives/909
		public byte[] ShrinkImage(Stream fileStream, float minSize)
		{
			BitmapFactory.Options options = new BitmapFactory.Options();
			options.InJustDecodeBounds = true;
			BitmapFactory.DecodeStream(fileStream, null, options);
			fileStream.Position = 0;

			float desireWidth, desireHeight;
			if (options.OutWidth > options.OutHeight) {
				desireHeight = minSize;
				desireWidth = options.OutWidth * desireHeight / options.OutHeight;
			} else {
				desireWidth = minSize;
				desireHeight = options.OutHeight * desireWidth / options.OutWidth;
			}

			float scaleX = options.OutWidth / desireWidth;
			float scaleY = options.OutHeight / desireHeight;
			options.InSampleSize = (int)Math.Floor(Math.Max(scaleX, scaleY));
			options.InJustDecodeBounds = false;

			byte[] byteArray;

			using (Bitmap bitmap = BitmapFactory.DecodeStream(fileStream, null, options))
			using (MemoryStream ms = new MemoryStream())
			{
				bitmap.Compress(Bitmap.CompressFormat.Png, 80, ms);
				byteArray = ms.ToArray();
			}

			return byteArray;
		}
	}
}
