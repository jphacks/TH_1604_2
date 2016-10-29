using System;
namespace TsuraiClient.Services
{
	public interface IAndroidActivityControlService
	{
		void MoveTaskToBack(bool nonRoot);
		void finish();
	}
}
