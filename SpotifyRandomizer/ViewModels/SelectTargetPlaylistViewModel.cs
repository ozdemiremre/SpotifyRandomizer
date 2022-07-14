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
    public class SelectTargetPlaylistViewModel : BindableObject
    {
        public ObservableCollection<PlaylistViewModel> Playlists { get; private set; }
        private Playlist _selectedExistingPlaylist;
        private bool _isLikedSongsSelected;
        private List<Playlist> _selectedSourcePlaylists;

        public SelectTargetPlaylistViewModel(List<Playlist> selectedSourcePlaylists, bool isLikedSongsSelected)
        {
            _selectedSourcePlaylists = selectedSourcePlaylists;
            _isLikedSongsSelected = isLikedSongsSelected;
            Playlists = new ObservableCollection<PlaylistViewModel>();
            IsSelectedExisting = true;
            GetPlaylistDataOfUserAsync();
        }

        private async void GetPlaylistDataOfUserAsync()
        {
            var result = await SpotifySession.ActiveSession.GetAllPlaylistsOfActiveUser();

            for (int i = 0; i < result.Count; i++)
            {
                Playlists.Add(new PlaylistViewModel(result[i]));
            }
        }

        internal void OnExistingPlaylistSelected(Playlist selectedItem)
        {
            _selectedExistingPlaylist = selectedItem;
            UpdateIsContinueEnabled();
        }

        internal void ExecuteContinue()
        {
            if (IsSelectedExisting)
            {
                Helpers.ExecuteOnUIThread(() => Application.Current.MainPage.Navigation.PushAsync(new ProgressPage(_selectedSourcePlaylists, _selectedExistingPlaylist, _isLikedSongsSelected)));
            }
            else
            {
                
                Helpers.ExecuteOnUIThread(() => Application.Current.MainPage.Navigation.PushAsync(new ProgressPage(_selectedSourcePlaylists, NewPlaylistName, _isLikedSongsSelected)));
            }
        }

        private void UpdateIsContinueEnabled()
        {
            if (IsSelectedExisting)
            {
                IsContinueEnabled = _selectedExistingPlaylist != null;
            }
            else
            {
                //TODO: Improve rules for new name
                IsContinueEnabled = NewPlaylistName?.Length > 3 && !string.IsNullOrWhiteSpace(NewPlaylistName) && NewPlaylistName.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
            }
        }

        #region Properties
        private bool _isSelectedExisting;
        private bool _isSelectedNew;
        private bool _isContinueEnabled;
        private string _newPlaylistName;

        public bool IsSelectedExisting
        {
            get { return _isSelectedExisting; }
            set
            {
                if (value != _isSelectedExisting)
                {
                    _isSelectedExisting = value;
                    OnPropertyChanged();
                    UpdateIsContinueEnabled();
                }
            }
        }

        public bool IsSelectedNew
        {
            get { return _isSelectedNew; }
            set
            {
                if (value != _isSelectedNew)
                {
                    _isSelectedNew = value;
                    OnPropertyChanged();
                    UpdateIsContinueEnabled();
                }
            }
        }

        public string NewPlaylistName
        {
            get { return _newPlaylistName; }
            set
            {
                if (value != _newPlaylistName)
                {
                    _newPlaylistName = value;
                    OnPropertyChanged();
                    UpdateIsContinueEnabled();
                }
            }
        }

        public bool IsContinueEnabled
        {
            get { return _isContinueEnabled; }
            set
            {
                if (value != _isContinueEnabled)
                {
                    _isContinueEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
    }
}
