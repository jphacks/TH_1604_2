using System;
using System.IO;
using System.Net.Http;
using System.Linq;
using TsuraiClient.Extentions;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TsuraiClient.Models
{
	public class EmotionAPIConnector
	{
		private static EmotionAPIConnector instance;

		public delegate void RecievedEventHandler(ResponseEventArgs e);
		public event RecievedEventHandler OnResultRecieved;

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

		private async void _JudgeUserEmotionPhoto(byte[] image) 
		{
			var client = getEmotionAPIClient();

			var fileContent = new StreamContent(new MemoryStream(image));
			fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

			var response = await client.PostAsync("recognize", fileContent);

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				var jsonDes = Utils.JsonConverterWrapper.Deserialize<List<Models.Entities.OfficialEmotionAPIJsonModel>>(response.Content.ReadAsStringAsync().Result, () => null).FirstOrDefault();
				OnResultRecieved(new ResponseEventArgs(response.StatusCode == System.Net.HttpStatusCode.OK, jsonDes));
			} else {
				OnResultRecieved(new ResponseEventArgs(response.StatusCode == System.Net.HttpStatusCode.OK, null));	
			}
		}

		public async void JudgeUserEmotionPhoto(byte[] image) => _JudgeUserEmotionPhoto(image);

		public async void JudgeUserEmotionPhoto(System.IO.Stream fileStream)
		{
			if (!fileStream.CanRead) 
			{
				var args = new ResponseEventArgs(false);
				OnResultRecieved(args);
				return;
			}

			_JudgeUserEmotionPhoto(fileStream.ReadFully());
		}

		public class ResponseEventArgs : EventArgs
		{
			public bool Succeed { get; }
			public Entities.OfficialEmotionAPIJsonModel Model { get; }

			public ResponseEventArgs(bool succeed, Entities.OfficialEmotionAPIJsonModel model = null)
			{
				Succeed = succeed;
				Model = model;
			}
		}
	}
}
