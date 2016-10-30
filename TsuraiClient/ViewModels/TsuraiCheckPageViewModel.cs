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

			Models.EmotionAPIConnector.Instance.OnResultRecieved+= (e) => {
				if (e == null) {
					Result = "サーバーとの通信エラー";
				} else {
					if (e.Model == null) {
						Result = "サーバーとの通信エラー";
					} else {
						var sum = (e.Model.scores.fear + e.Model.scores.neutral + e.Model.scores.sadness) / 3f;
						if (sum > 0.8) {
							Result = "すごく つらい";
						} else if (sum > 0.6) {
							Result = "そこそこ つらい";
						} else if (sum > 0.4) {
							Result = "まぁ つらい";
						} else if (sum > 0.2) {
							Result = "まぁ なんとか";
						} else {
							Result = "まだまだ 余裕";
						}
					}
				}
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
				Result = "診断中";

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
