using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyRandomizer.Models;

namespace SpotifyRandomizer.ViewModels
{
    internal class ProgressViewModel : BindableObject
    {
        private readonly List<Playlist> _sourcePlaylists;
        private readonly Playlist _targetExistingTargetPlaylist;
        private readonly string _targetNewPlaylistName;
        private readonly bool _isLikedSongsSelected;
        private const string _targetNewPlaylistDescription = "All your tracks, randomized. By SpotifyRandomizer.";
        private const int _shuffleCount = 3;

        private bool _isUsingExistingPlaylist => _targetExistingTargetPlaylist != null;

        // Create new playlist
        public ProgressViewModel(List<Playlist> sourcePlaylists, string targetNewPlaylistName, bool isLikedSongsSelected)
        {
            _sourcePlaylists = sourcePlaylists;
            _isLikedSongsSelected = isLikedSongsSelected;
            _targetNewPlaylistName = targetNewPlaylistName;
            StartMainRandomizerTask();
        }

        // Clear existing playlist and use that
        public ProgressViewModel(List<Playlist> sourcePlaylists, Playlist targetPlaylist, bool isLikedSongsSelected)
        {
            _sourcePlaylists = sourcePlaylists;
            _targetExistingTargetPlaylist = targetPlaylist;
            _isLikedSongsSelected = isLikedSongsSelected;
            StartMainRandomizerTask();
        }

        private async void StartMainRandomizerTask()
        {
            IsOngoing = true;
            IsFinished = false;

            MainActionProgress = 0f;
            CurrentActionProgress = 0f;

            List<Track> allTracks = new List<Track>();

            // Gather all tracks from source playlists
            int totalTrackCount = _sourcePlaylists.Sum(p => p.Tracks.Total); // For progress
            int totalPlaylistCount = _sourcePlaylists.Count + (_isLikedSongsSelected ? 1 : 0);
            int processedPlaylistCount = 0;

            if (_isLikedSongsSelected)
            {
                MainAction = "Gathering liked songs";
                List<Track> likedSongs = await SpotifySession.ActiveSession.GetAllLikedSongs(f => { CurrentActionProgress = f; CurrentAction = $"Liked songs %{(int)(f * 100)}"; });

                totalTrackCount += likedSongs.Count;
                allTracks.AddRange(likedSongs);
                CurrentAction = "";
                processedPlaylistCount++;
                MainActionProgress = (processedPlaylistCount / (float)totalPlaylistCount) * 0.33f;
            }

            for (int i = 0; i < _sourcePlaylists.Count; i++)
            {
                var playlist = _sourcePlaylists[i];
                MainAction = $"Gathering songs in playlist {playlist.Name}";
                var tracksInPlaylist = await SpotifySession.ActiveSession.GetAllTracksOfPlaylist(playlist, f => { CurrentActionProgress = f; CurrentAction = $"{playlist.Name} %{(int)(f * 100)}"; });
                allTracks.AddRange(tracksInPlaylist);

                CurrentActionProgress += allTracks.Count / (float)totalTrackCount;
                CurrentAction = "";
                processedPlaylistCount++;
                MainActionProgress = (processedPlaylistCount / (float)totalPlaylistCount) * 0.33f;
            }

            MainActionProgress = 0.33f;

            MainAction = "Distilling songs";
            CurrentAction = "";

            // Distill tracks by their ids
            var uniqueTracks = allTracks.DistinctBy(t => t.Id).ToList();
            await Task.Delay(1000); // Simulate process :D 
            MainActionProgress = 0.40f;
            CurrentActionProgress = 0f;

            MainAction = "Shuffling songs";

            // Randomize gathered tracks
            for (int i = 0; i < _shuffleCount; i++)
            {
                CurrentAction = $"Shuffle {i + 1}/{_shuffleCount}";
                uniqueTracks.Shuffle();
                await Task.Delay(500); // Simulate process
                float progress = ((i + 1) / (float)_shuffleCount);
                CurrentActionProgress = progress;
                MainActionProgress = 0.4f + (0.1f * progress);
            }

            CurrentAction = "";
            MainActionProgress = 0.50f;

            Playlist playlistToAddTracksTo;

            if (_isUsingExistingPlaylist)
            {
                MainAction = $"Cleaning the existing playlist {_targetExistingTargetPlaylist.Name}";

                // Clean existing playlist
                bool clearResult = await SpotifySession.ActiveSession.ClearAllTracksInPlaylist(_targetExistingTargetPlaylist, f => { CurrentActionProgress = f; CurrentAction = $"Clearing tracks in ${_targetExistingTargetPlaylist.Name} %{(int)(f * 100)}"; });

                playlistToAddTracksTo = _targetExistingTargetPlaylist;
            }
            else
            {
                MainAction = $"Creating a new playlist: {_targetNewPlaylistName}";

                // Create new playlist with name
                playlistToAddTracksTo = await SpotifySession.ActiveSession.CreateNewPlaylist(_targetNewPlaylistName, _targetNewPlaylistDescription, false, false);
            }

            MainActionProgress = 0.80f;

            MainAction = $"Adding shuffled tracks to {playlistToAddTracksTo.Name}";
            bool addResult = await SpotifySession.ActiveSession.AddTracksToPlaylist(playlistToAddTracksTo, uniqueTracks, f => { CurrentActionProgress = f; CurrentAction = $"Added tracks %{(int)(f * 100)}"; });

            MainActionProgress = 1f;
            CurrentActionProgress = 1f;

            CurrentAction = "";
            MainAction = "Finished!";
            IsFinished = true;
            IsOngoing = false;
        }

        #region Properties
        private float _currentActionProgress;
        private string _currentAction;
        private float _mainActionProgress;
        private string _mainAction;
        private bool _isFinished;
        private bool _isOngoing;

        public float MainActionProgress
        {
            get { return _mainActionProgress; }
            set
            {
                if (_mainActionProgress != value)
                {
                    _mainActionProgress = value;
                    OnPropertyChanged();
                }
            }
        }

        public float CurrentActionProgress
        {
            get { return _currentActionProgress; }
            set
            {
                if (_currentActionProgress != value)
                {
                    _currentActionProgress = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CurrentAction
        {
            get { return _currentAction; }
            set
            {
                if (_currentAction != value)
                {
                    _currentAction = value;
                    OnPropertyChanged();
                }
            }
        }

        public string MainAction
        {
            get { return _mainAction; }
            set
            {
                if (_mainAction != value)
                {
                    _mainAction = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsFinished
        {
            get { return _isFinished; }
            set
            {
                if (_isFinished != value)
                {
                    _isFinished = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsOngoing
        {
            get { return _isOngoing; }
            set
            {
                if (_isOngoing != value)
                {
                    _isOngoing = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
    }
}
