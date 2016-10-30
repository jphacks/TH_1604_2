using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TsuraiClient.Views
{
	public partial class AndroidRootMasterDetailPage : MasterDetailPage
	{
		private Page currentPage;

		public AndroidRootMasterDetailPage()
		{
			InitializeComponent();
			var page = new TsuraiCheckPage();
			Detail = new NavigationPage(page);
			currentPage = page;

			masterPage.PageListView.ItemSelected += OnItemSelected;

			masterPage.PageListView.SelectedItem = ((List<MasterPageItem>)masterPage.PageListView.ItemsSource)[0];

			//masterPage.PageListView.BackgroundColor = Color.Purple;
		}

		async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as MasterPageItem;
			if (item == null) {
				return;
			}

			IsPresented = false;

			if (currentPage.GetType() == item.TargetType) {
				return;
			}

			var page = (Page)Activator.CreateInstance(item.TargetType);

			currentPage = page;
			Detail = new NavigationPage(page);
		}

		protected override bool OnBackButtonPressed()
		{
			if (Navigation.NavigationStack.Count == 0) {
				DependencyService.Get<Services.IAndroidActivityControlService>().MoveTaskToBack(false);
				return true;
			}
			return base.OnBackButtonPressed();
		}
	}
}
