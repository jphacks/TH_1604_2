using System;
using System.Collections.Generic;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace TsuraiClient.Views
{
	public partial class TsuraiCheckPage : ContentPage
	{
		public TsuraiCheckPage()
		{
			InitializeComponent();
			this.BindingContext = new ViewModels.TsuraiCheckPageViewModel();
		}
	}
}
