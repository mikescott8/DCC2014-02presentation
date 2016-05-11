using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms.Maps;

namespace DCC
{
    class Api
    {
        public static async Task<string[]> SearchByBeer(string searchText)
        {
			try {
				var beerUrl = BuildUrl("search");

				beerUrl += ("q=" + searchText);
				beerUrl += ("&type=beer");
				beerUrl += ("&withBreweries=Y");


				var json = await ExecuteCall(AppendKey(beerUrl));

				var data = JsonConvert.DeserializeObject<BrewApiResults>(json);
				var resultData = data.Data;

				var results = new List<string>();
				if (resultData!=null) {
					foreach (var res in resultData)
					{
						results.Add((string)res.SelectToken("name") + " (" + (string)res.SelectToken("breweries[0].name") + ")");
					}
				}
				else {
					results.Add("No results found.");
				}

				return results.ToArray();
			} catch (Exception ex) {
				var results = new List<string> ();

				results.Add ("Unknown Error Occurred");
				results.Add (ex.Message);
				//results.Add (ex.StackTrace);

				return results.ToArray ();
			}
        }  

        public static async Task<Pin[]> SearchByLocation(string searchLat, string serachLng)
        {
            var beerUrl = BuildUrl("search/geo/point");

            beerUrl += ("lat=" + searchLat);
            beerUrl += ("&lng=" + serachLng);


            var json = await ExecuteCall(AppendKey(beerUrl));

            var data = JsonConvert.DeserializeObject<BrewApiResults>(json);
            var resultData = data.Data;

            var results = new List<Pin>();

            foreach (var res in resultData)
            {
                double lat;
                double lng;
            
                if (double.TryParse((string) (res.SelectToken("latitude")), out lat) &&
                    double.TryParse((string) (res.SelectToken("longitude")), out lng))
                {
                    results.Add(new Pin
                    {
                        Label = (string)res.SelectToken("brewery.name") + " (" + (string)res.SelectToken("name") + ")",
                        Address = (string)res.SelectToken("streetAddress"),
                        Position = new Position(lat, lng),
                        Type = PinType.SearchResult,
                    });
                }
            }

            return results.ToArray();
        }  



        private const string baseEndpoint = "http://api.brewerydb.com/v2/";

        private static string AppendKey(string url)
        {
            return url + "&key=" + ApiKeys.BreweryDbDccKey; // you will have to get your own key and insert here
        }

        private static string BuildUrl(string endpoint)
        {
            return baseEndpoint + endpoint + "/?";
        }

        private static async Task<string> ExecuteCall(string url)
        {
            var client = new HttpClient();
            return await client.GetStringAsync(url);
        }

    }
    public class BrewApiResults
    {
        public string Status { get; set; }
        public int? NumberOfPages { get; set; }
        public int? CurrentPage { get; set; }
        public JObject[] Data { get; set; }
    }
}
