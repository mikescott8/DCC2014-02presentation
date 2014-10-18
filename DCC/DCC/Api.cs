using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DCC
{
    class Api
    {
        public static async Task<string[]> SearchByBeer(string searchText)
        {
            var beerUrl = BuildUrl("search");

            beerUrl += ("q=" + searchText);
            beerUrl += ("&type=beer");
            beerUrl += ("&withBreweries=Y");


            var json = await ExecuteCall(AppendKey(beerUrl));

            var data = JsonConvert.DeserializeObject<BrewApiResults>(json);
            var resultData = data.Data;

            var results = new List<string>();
            foreach (var res in resultData)
            {
                results.Add((string)res.SelectToken("name") + " (" + (string)res.SelectToken("breweries[0].name") + ")");
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
