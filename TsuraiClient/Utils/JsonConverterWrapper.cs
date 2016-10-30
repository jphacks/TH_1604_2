using System;
using Newtonsoft.Json;

namespace TsuraiClient.Utils
{
	public class JsonConverterWrapper
	{
		public static T Deserialize<T>(string source, Func<T> defaultValueGenerator)
		{
			try
			{
				var obj = JsonConvert.DeserializeObject<T>(source);
				if (obj != null)
				{
					return obj;
				}
				return defaultValueGenerator();
			}
			catch (ArgumentException)
			{
				return defaultValueGenerator();
			}
			catch (JsonReaderException ex)
			{
				return defaultValueGenerator();
			}
		}
	}
}
