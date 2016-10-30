using System;
using System.Drawing;
using System.IO;
using Foundation;
using TsuraiClient.iOS;
using TsuraiClient.Extentions;
using UIKit;

namespace TsuraiClient.iOS
{
	// https://bortolu.wordpress.com/2014/03/21/xamarin-c-how-to-convert-byte-array-to-uiimage-with-an-extension-method/

	public static class ImageProcessingServiceExtention
	{
		public static UIImage ToImage(this byte[] data)
		{
			if (data == null)
			{
				return null;
			}
			UIImage image = null;
			try
			{

				image = new UIImage(NSData.FromArray(data));
				data = null;
			}
			catch (Exception)
			{
				return null;
			}
			return image;
		}

		public static byte[] ToNSData(this UIImage image)
		{
			if (image == null)
			{
				return null;
			}
			NSData data = null;

			try
			{
				data = image.AsPNG();
				return data.ToArray();
			}
			catch (Exception)
			{
				return null;
			}
			finally
			{
				if (image != null)
				{
					image.Dispose();
					image = null;
				}
				if (data != null)
				{
					data.Dispose();
					data = null;
				}
			}
		}
	}
		
	public class ImageProcessingService : TsuraiClient.Services.IImageProcessingService
	{
		public byte[] ShrinkImage(Stream fileStream, float minSize)
		{
			var byteImage = fileStream.ReadFully();
			if (byteImage == null) {
				return null;
			}

			var img = byteImage.ToImage();
			var width = img.Size.Width;
			var height = img.Size.Height;

			float newWidth, newHeight;
			if (width > height) {
				newHeight = minSize;
				newWidth = (float)(width * newHeight / height);
			} else {
				newWidth = minSize;
				newHeight = (float)(height * newWidth / width);
			}

			UIGraphics.BeginImageContext(new SizeF(newWidth, newHeight));
			img.Draw(new RectangleF(0, 0, newWidth, newHeight));
			img = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			return img.ToNSData();
		}
	}
}
