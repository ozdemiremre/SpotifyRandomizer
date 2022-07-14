using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyRandomizer.Models
{
    public interface ISpotifyHandler
    {
        /// <summary>
        /// Start waiting for redirection to _localResponseAddress with result
        /// If successful, get authorized token and user information.
        /// Should be called on session first.
        /// </summary>
        //protected Task StartListenerAsync();

        public delegate void OnRequestResultDelegate(bool success);

        /// <summary>
        /// Gets called when the login stage has been concluded
        /// </summary>
        public event OnRequestResultDelegate OnLoginConcluded;

        public string GetAuthorizeTokenRequestUri();

        /// <summary>
        /// Gets all playlists(not including liked songs) of the active user.
        /// </summary>
        public Task<List<Playlist>> GetAllPlaylistsOfActiveUser();

        /// <summary>
        /// Gets all tracks in the given playlist.
        /// </summary>
        public Task<List<Track>> GetAllTracksOfPlaylist(Playlist playlist, Action<float> reportProgress);

        /// <summary>
        /// Create a new playlist for the active user with given parameters.
        /// </summary>
        public Task<Playlist> CreateNewPlaylist(string newPlaylistName, string newPlaylistDescription, bool isCollaborative, bool isPublic);

        /// <summary>
        /// Clear all tracks in the playlist.
        /// </summary>
        public Task<bool> ClearAllTracksInPlaylist(Playlist targetExistingTargetPlaylist, Action<float> reportProgress);

        /// <summary>
        /// Add tracks to the given playlist.
        /// </summary>
        public Task<bool> AddTracksToPlaylist(Playlist playlistToAddTracksTo, List<Track> uniqueTracks, Action<float> reportProgress);

        /// <summary>
        /// Get all liked songs of the active user.
        /// </summary>
        public Task<List<Track>> GetAllLikedSongs(Action<float> reportProgress);
    }
}
