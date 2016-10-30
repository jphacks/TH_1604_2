using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using TsuraiClient.Extentions;

namespace TsuraiClient.Models
{
	public class TsuraiAPIConnector
	{
		private static TsuraiAPIConnector instance;

		public delegate void RecievedEventHandler(ResponseEventArgs e);
		public event RecievedEventHandler OnResponseRecieved;

		private static string ENDPOINT = GlobalSettings.TsuraiAPI_BASE_URL;

		private TsuraiAPIConnector() { }

		public static TsuraiAPIConnector Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new TsuraiAPIConnector();
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
			var username = "yataro";
			var password = "tsurai";

			var byteArray = Portable.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password));
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

			return client;
		}

		private async void _JudgeUserEmotionPhoto(byte[] image)
		{
			var client = getEmotionAPIClient();

			var content = new MultipartFormDataContent();
			var fileContent = new StreamContent(new MemoryStream(image));
			fileContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("image/jpeg");
			content.Add(fileContent, "file", "image.jpeg");

			var response = await client.PostAsync("status/update", content);

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				//var jsonDes = Utils.JsonConverterWrapper.Deserialize<List<Models.Entities.OfficialEmotionAPIJsonModel>>(response.Content.ReadAsStringAsync().Result, () => null);
				//System.Diagnostics.Debug.WriteLine(jsonDes);
				System.Diagnostics.Debug.WriteLine("Recieved");
			}
			var resutl = response.Content.ReadAsStringAsync().Result;
			OnResponseRecieved(new ResponseEventArgs(response.StatusCode == System.Net.HttpStatusCode.OK, resutl));
		}

		public async void JudgeUserEmotionPhoto(byte[] image) => _JudgeUserEmotionPhoto(image);

		public async void JudgeUserEmotionPhoto(System.IO.Stream fileStream)
		{
			if (!fileStream.CanRead)
			{
				var args = new ResponseEventArgs(false);
				OnResponseRecieved(args);
				return;
			}

			_JudgeUserEmotionPhoto(fileStream.ReadFully());
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
