using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TsuraiClient.Views
{
	public partial class AndroidRootMasterPage : ContentPage
	{
		public ListView PageListView;

		public AndroidRootMasterPage()
		{
			InitializeComponent();

			PageListView = pageListView;

			var pageItems = new List<MasterPageItem>();
			pageItems.Add(new MasterPageItem()
			{
				Title = "つらさ診断",
				TargetType = typeof(TsuraiCheckPage)
			});
			pageItems.Add(new MasterPageItem()
			{
				Title = "つらさ統計",
				TargetType = typeof(UserStatusAnalyticsPage)
			});

			pageListView.ItemsSource = pageItems;
		}
	}

	public class MasterPageItem
	{
		public string Title { get; set;}
		public Type TargetType { get; set;}
	}
}
