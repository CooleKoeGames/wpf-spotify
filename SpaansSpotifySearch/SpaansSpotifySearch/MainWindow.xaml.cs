using SpaansSpotifySearch.Helpers;
using SpaansSpotifySearch.Models;
using System.Windows;
using System.Windows.Input;

namespace SpaansSpotifySearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Task.Run(async () => await SearchHelper.GetTokenAsync());
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            var result = SearchHelper.SearchArtistOrSong(txtSearch.Text);

            if (result == null)
            {
                return;
            }

            var listArtist = new List<SpotifyArtist>();

            foreach (var item in result.artists.items)
            {
                listArtist.Add(new SpotifyArtist()
                {
                    ID = item.id,
                    Image = item.images.Any() ? item.images[0].url : "",
                    Name = item.name,
                    Popularity = $"{item.popularity}% popularity",
                    Followers = $"{item.followers.total.ToString("N")} followers"
                });
            }

            ListArtist.ItemsSource = listArtist;
        }
    }
}