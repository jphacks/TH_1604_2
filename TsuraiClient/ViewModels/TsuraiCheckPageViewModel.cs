using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace TsuraiClient.ViewModels
{
	public class TsuraiCheckPageViewModel : INotifyPropertyChanged
	{
		private string result;

		public TsuraiCheckPageViewModel()
		{
			result = "";

			Models.EmotionAPIConnector.Instance.OnResponseRecieved += (e) => {
				System.Diagnostics.Debug.WriteLine(e.Succeed);
				System.Diagnostics.Debug.WriteLine(e.Response ?? "");
				Result = e.Succeed ? e.Response : "サーバーとの通信エラー";
			};

			Models.TsuraiAPIConnector.Instance.OnResponseRecieved += (e) =>
			{
				System.Diagnostics.Debug.WriteLine(e.Succeed);
				System.Diagnostics.Debug.WriteLine(e.Response ?? "");
				Result = e.Succeed ? e.Response : "サーバーとの通信エラー";
			};

			Models.PhotoEmotionWrapper.Instance.OnCameraError += (e) =>
			{
				System.Diagnostics.Debug.WriteLine(e.ErrorCode);
				switch (e.ErrorCode) {
					case Models.PhotoEmotionWrapper.CameraErrorCode.NoCamera:
						Result = "カメラが起動できないため，あなたのつらさを推し量れません";
						break;
					case Models.PhotoEmotionWrapper.CameraErrorCode.SizeError:
						Result = "写真のサイズが大きすぎるようです，受け止めきれません";
						break;
				}
			};

			this.IsTsuraiCommand = new Command(() =>
			{
				Models.PhotoEmotionWrapper.Instance.GetUserEmotion(Models.PhotoEmotionWrapper.MediaType.Photo, isOfficial);
			}, () => true);
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public string Result
		{
			get
			{
				return result;
			}
			private set
			{
				if (result != value)
				{
					result = value;
					PropertyChanged(this, new PropertyChangedEventArgs(nameof(Result)));
					//IsTsuraiCommand.ChangeCanExecute();
				}
			}
		}

		private bool isOfficial;
		public bool IsOfficial
		{
			get {
				return isOfficial;
			}
			set {
				if (isOfficial != value)
				{
					isOfficial = value;
					PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsOfficial)));
				}
			}
		}

		public Command IsTsuraiCommand { get; private set;}
	}
}
