using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyRandomizer.Models;
using SpotifyRandomizer.Views;

namespace SpotifyRandomizer.ViewModels
{
    public class SelectSourcePlaylistViewModel : BindableObject
    {
        private const string _likedSongsImageLink = "https://i1.sndcdn.com/artworks-y6qitUuZoS6y8LQo-5s2pPA-t500x500.jpg";

        public ObservableCollection<PlaylistViewModel> Playlists { get; private set; }

        public SelectSourcePlaylistViewModel()
        {
            Playlists = new ObservableCollection<PlaylistViewModel>();
            GetPlaylistDataOfUserAsync();
        }

        private async void GetPlaylistDataOfUserAsync()
        {
            var result = await SpotifySession.ActiveSession.GetAllPlaylistsOfActiveUser();

            Playlists.Add(new PlaylistViewModel("Liked Songs", _likedSongsImageLink, "Your liked songs.", true));

            for (int i = 0; i < result.Count; i++)
            {
                Playlists.Add(new PlaylistViewModel(result[i]));
            }
        }

        public void ExecuteContinue()
        {
            var selectedPlaylists = Playlists.Where(p => p.IsSelected && p.Playlist != null).Select(p => p.Playlist);
            bool isLikedSongsSelected = Playlists.SingleOrDefault(p => p.IsLikedSongsList)?.IsSelected == true;

            if (selectedPlaylists.Any() || isLikedSongsSelected)
            {
                Helpers.ExecuteOnUIThread(() => Application.Current.MainPage.Navigation.PushAsync(new SelectTargetPlaylistPage(selectedPlaylists.ToList(), isLikedSongsSelected)));
            }
        }

        internal void OnSelectionToggledForPlaylist(PlaylistViewModel playlist)
        {
            playlist.IsSelected = !playlist.IsSelected;
            IsContinueEnabled = Playlists.Any(p => p.IsSelected);
        }

        #region Properties
        private bool _isContinueEnabled;

        public bool IsContinueEnabled
        {
            get { return _isContinueEnabled; }
            set
            {
                if (_isContinueEnabled != value)
                {
                    _isContinueEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
    }

    public class PlaylistViewModel : BindableObject
    {
        public Playlist Playlist { get; private set; }
        public bool IsLikedSongsList { get; private set; }

        public PlaylistViewModel(Playlist playlist)
        {
            Playlist = playlist;
            Name = Playlist.Name;
            Description = Playlist.Description;
            ImageLink = Playlist.Images?.ElementAtOrDefault(0)?.Href ?? string.Empty;
        }

        public PlaylistViewModel(string name, string imageLink, string description, bool isLikedSongsList)
        {
            Name = name;
            ImageLink = imageLink;
            Description = description;
            IsLikedSongsList = isLikedSongsList;
        }

        #region Properties
        private bool _isSelected;
        private string _imageLink;
        private string _name;
        private string _description;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ImageLink
        {
            get { return _imageLink; }
            set
            {
                if (_imageLink != value)
                {
                    _imageLink = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
    }
}
