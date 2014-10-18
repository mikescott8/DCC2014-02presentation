using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DCC
{
    public class ByBeerPage
    {
        private static Entry _nameEntry;
        private static Button _searchButton;
        private static ListView _resultsListView;

        public static Page GetPage()
        {
            var nameLabel = new Label();
            nameLabel.Text = "Name of beer looking for:";

            _nameEntry = new Entry();

            _searchButton = new Button
            {
                Text="Search",
                Command = new Command(x => { OnSearch(x); })
            };

            _resultsListView = new ListView();

            var stackLayout = new StackLayout
            {
                Children =
                {
                    nameLabel,
                    _nameEntry,
                    _searchButton,
                    _resultsListView
                }
            };

            return new ContentPage
            {
                Content = stackLayout
            };
        }

        public static async Task OnSearch(object prm)
        {
            var results = await Api.SearchByBeer(_nameEntry.Text);

            _resultsListView.ItemsSource = results ;
        }
    }
}
