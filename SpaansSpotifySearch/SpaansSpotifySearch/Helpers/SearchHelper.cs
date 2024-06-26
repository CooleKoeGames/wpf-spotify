﻿using Newtonsoft.Json;
using RestSharp;
using SpaansSpotifySearch.Models;
using System.Net.Http;
using System.Text;
using static SpaansSpotifySearch.Models.SpotifySearch;

namespace SpaansSpotifySearch.Helpers
{
    public static class SearchHelper
    {
        public static Token token { get; set; }

        public static async Task GetTokenAsync()
        {
            #region SecretVault
            string clientID = "fff578959e094135bd4cb00817de0779";
            string clientSecret = "84adab790c5c4d7da24068788b1bd5a5";
            #endregion

            string auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(clientID + ":" + clientSecret));

            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            };

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {auth}");
            HttpContent content = new FormUrlEncodedContent(args);

            HttpResponseMessage resp = await client.PostAsync("https://accounts.spotify.com/api/token", content);
            string msg = await resp.Content.ReadAsStringAsync();

            token = JsonConvert.DeserializeObject<Token>(msg);
        }

        public static SpotifyResult SearchArtistOrSong(string searchWord)
        {
            var client = new RestClient("https://api.spotify.com/v1/search");
            client.AddDefaultHeader("Authorization", $"Bearer {token.access_token}");
            var request = new RestRequest($"?q={searchWord}&type=artist", Method.Get);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<SpotifyResult>(response.Content);
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}