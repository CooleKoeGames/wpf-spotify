using Newtonsoft.Json;
using NotSpotify.MVVM.Model;
using RestSharp;
using RestSharp.Authenticators.OAuth2;
using System.Collections.ObjectModel;

namespace NotSpotify.MVVM.ViewModel
{
    public class MainViewModel
    {
        public ObservableCollection<Item> Songs { get; set; }

        public MainViewModel()
        {
            Songs = new ObservableCollection<Item>();
            PopulateCollection();
        }

        private void PopulateCollection()
        {
            var options = new RestClientOptions();
            options.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator("BQD2nqZUKqwInky__cLAmnFNSd9jn3UMWSGAnhvCJO1K7KVHSGOZxanQ72q-ds35sLnJp7FrOeGpFzBLDwF_Pg2uSm-wHHKc0ndUrpNe18u6kqYrg7XX9wuT93RxmK7uXsctUmZBLUruEZm1WbRHz4P5bYLMqdNjcUKAfponIdJiazF-3i5M9UHs2FANh24_SzD8UH_XZ1YJYDoQDhEPcCr4z_imnQJxWL-cuyRpgrz4zAwBgsBQHWTl9w", "Bearer");
            using var client = new RestClient(options);

            var request = new RestRequest("https://api.spotify.com/v1/browse/new-releases", Method.Get);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");

            var response = client.GetAsync(request).GetAwaiter().GetResult();
            var data = JsonConvert.DeserializeObject<TrackModel>(response.Content);

            for (int i = 0; i < data.Albums.Limit; i++)
            {
                var track = data.Albums.Items[i];
                track.Duration = "2:32";
                Songs.Add(track);
            }
        }
    }
}