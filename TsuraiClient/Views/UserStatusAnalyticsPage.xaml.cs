using System;
using System.Collections.Generic;
using OxyPlot;
using Xamarin.Forms;

namespace TsuraiClient.Views
{
	public partial class UserStatusAnalyticsPage : ContentPage
	{
		public UserStatusAnalyticsPage()
		{
			InitializeComponent();
			this.Content = new OxyPlot.Xamarin.Forms.PlotView
			{
				Model = new PlotModel { Title = "Hello, Forms!" },
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.Fill,
			};
		}
	}
}
