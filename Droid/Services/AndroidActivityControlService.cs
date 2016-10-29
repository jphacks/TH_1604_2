using System;

[assembly: Xamarin.Forms.Dependency(typeof(TsuraiClient.Droid.AndroidActivityControlService))]
namespace TsuraiClient.Droid
{
	public class AndroidActivityControlService : TsuraiClient.Services.IAndroidActivityControlService
	{
		public void finish()
		{
			MainActivity.Instance.Finish();
		}

		public void MoveTaskToBack(bool nonRoot)
		{
			MainActivity.Instance.MoveTaskToBack(nonRoot);
		}
	}
}
