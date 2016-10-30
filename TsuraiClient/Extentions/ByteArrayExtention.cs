using System;
using System.IO;

namespace TsuraiClient.Extentions
{
	public static class ByteArrayExtention
	{
		// http://stackoverflow.com/questions/221925/creating-a-byte-array-from-a-stream
		public static byte[] ReadFully(this Stream input)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				input.CopyTo(ms);
				return ms.ToArray();
			}
		}
	}
}
