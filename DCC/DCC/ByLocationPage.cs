using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace DCC
{
    public class ByLocationPage
    {
        private static Map _map;

        public static Page GetPage()
        {
            _map = new Map(
                MapSpan.FromCenterAndRadius(
                    new Position(33.29188, -111.794362),    // this lat-lng position is Location of DCC2014.2
                    Distance.FromMiles(5)))
            {
                IsShowingUser = true,
                HasZoomEnabled = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
        
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(_map);
            stack.Children.Add(new Button
            {
                Text="Search",
                Command = new Command(async x => { await OnSearch(x); }),
            });
            return new ContentPage
            {
                Content = stack
            };
        }

        public static async Task OnSearch(object prm)
        {
            var results = await Api.SearchByLocation(_map.VisibleRegion.Center.Latitude.ToString(),
                _map.VisibleRegion.Center.Longitude.ToString());

            if (results.Any())
            {
                foreach (var result in results)
                {
                    _map.Pins.Add(result);
                }
            }
            else
            {
                _map.Pins.Clear();
            }
        }
    }
}
