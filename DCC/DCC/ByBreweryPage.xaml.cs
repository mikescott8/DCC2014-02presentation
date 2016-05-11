using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DCC
{
	public partial class ByBreweryPage : ContentPage
	{
		public ByBreweryPage ()
		{
			InitializeComponent ();
			var searchButton = this.FindByName<Button> ("_searchButton");

			if (searchButton !=null) {
				searchButton.Command = new Command (async x => {
					await OnSearch (x);
				});
			}
		}

		public async Task OnSearch(object prm)
		{
			var results = await Api.SearchByBeer (this.FindByName<Entry> ("_nameEntry").Text);
			this.FindByName<ListView> ("_resultsListView").ItemsSource = results;
		}
	}
}

