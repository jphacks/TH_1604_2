using Xamarin.Forms;

namespace TsuraiClient
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			if (Device.OS.ToString().Contains("Android")) {
				MainPage = new Views.AndroidRootMasterDetailPage();
			} else {
				MainPage = new Views.iOSRootTabbedPage();
			}
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
