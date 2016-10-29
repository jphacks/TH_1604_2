using System;
using System.Net.Http;

namespace TsuraiClient.Models
{
	public class EmotionAPIConnector
	{
		private static EmotionAPIConnector instance;

		public delegate void RecievedEventHandler(ResponseEventArgs e);
		public event RecievedEventHandler OnResponseRecieved;

		private static string API_KEY = GlobalSettings.EmotionAPI_KEY;
		private static string ENDPOINT = GlobalSettings.EmotionAPI_BASE_URL;

		private EmotionAPIConnector(){}

		public static EmotionAPIConnector Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new EmotionAPIConnector();
				}
				return instance;
			}
		}

		private HttpClient getEmotionAPIClient() 
		{
			var client = new HttpClient()
			{
				BaseAddress = new Uri(ENDPOINT)
			};
			client.DefaultRequestHeaders.TryAddWithoutValidation("Ocp-Apim-Subscription-Key", API_KEY);

			return client;
		}

		public async void JudgeUserEmotionPhoto(System.IO.Stream fileStream)
		{
			if (!fileStream.CanRead) 
			{
				var args = new ResponseEventArgs(false);
				OnResponseRecieved(args);
				return;
			}

			var client = getEmotionAPIClient();

			var fileContent = new StreamContent(fileStream);
			fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

			var response = await client.PostAsync("recognize", fileContent);

			OnResponseRecieved(new ResponseEventArgs(response.StatusCode == System.Net.HttpStatusCode.OK, response.Content.ReadAsStringAsync().Result));
		}

		public class ResponseEventArgs : EventArgs
		{
			public bool Succeed { get; }
			public string Response { get; }

			public ResponseEventArgs(bool succeed, string response = null)
			{
				Succeed = succeed;
				Response = response;
			}
		}
	}
}
