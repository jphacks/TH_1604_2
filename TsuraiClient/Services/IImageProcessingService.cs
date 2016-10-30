using System;
namespace TsuraiClient.Services
{
	public interface IImageProcessingService
	{
		byte[] ShrinkImage(System.IO.Stream fileStream, float minSize);
	}
}
