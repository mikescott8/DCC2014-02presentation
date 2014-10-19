using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace DCC
{
    public class App
    {
        private static NavigationPage _mainPage;
        public static Page GetMainPage()
        {
            return _mainPage = new NavigationPage(new ContentPage
            {
                Content = new StackLayout
                {
                    // left and right padding: 5; top padding: 20 (only on iOS)
                    Padding = new Thickness(5, Device.OnPlatform(20, 0, 0), 5, 0),
                    Children = 
                    {
                        new Button
                        {
                            Text="By Beer Name",
                            Command = GotoPage,
                            CommandParameter="Beer",
                        },
                        new Button
                        {
                            Text="By Brewery Name",
                            Command = GotoPage,
                            CommandParameter="Brewery",
                        },
                        new Button
                        {
                            Text="By Location",
                            Command = GotoPage,
                            CommandParameter="Location",
                        },
                    }
                },
            });
       }
        static private Command GotoPage = new Command<string>(prm =>
        {
            Page newPage;

            switch (prm)
            {
                case "Beer":
                    newPage = ByBeerPage.GetPage(); //TODO: replace with real page
                    break;
                case "Brewery":
                    newPage = new ContentPage(); //TODO: replace with real page
                    break;
                case "Location":
                    newPage = ByLocationPage.GetPage(); //TODO: replace with real page
                    break;
                default:
                    newPage = new ContentPage
                    {
                        Content = new Label
                        {
                            Text = "An error occurred. Could not find requested screen.",
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                        }
                    };
                    break;
            }
            _mainPage.PushAsync(newPage);
        });
    }
}
