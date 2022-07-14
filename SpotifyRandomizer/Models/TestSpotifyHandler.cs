using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyRandomizer.Models
{
    public class TestSpotifyHandler : ISpotifyHandler
    {
        public event ISpotifyHandler.OnRequestResultDelegate OnLoginConcluded;
        private const int _defaultWaitAmount = 1000;

        public async Task<bool> AddTracksToPlaylist(Playlist playlistToAddTracksTo, List<Track> uniqueTracks, Action<float> reportProgress)
        {
            for (int i = 0; i < uniqueTracks.Count; i++)
            {
                reportProgress?.Invoke(i / (float)uniqueTracks.Count);
                await Task.Delay(_defaultWaitAmount / 100);
            }

            return true;
        }

        public async Task<bool> ClearAllTracksInPlaylist(Playlist targetExistingTargetPlaylist, Action<float> reportProgress)
        {
            reportProgress?.Invoke(0f);

            for (int i = 0; i < targetExistingTargetPlaylist.Tracks.Total; i++)
            {
                await Task.Delay(_defaultWaitAmount / 100);
                reportProgress?.Invoke(i / (float)targetExistingTargetPlaylist.Tracks.Total);
            }

            reportProgress?.Invoke(1f);
            return true;
        }

        public async Task<Playlist> CreateNewPlaylist(string newPlaylistName, string newPlaylistDescription, bool isCollaborative, bool isPublic)
        {
            await Task.Delay(_defaultWaitAmount * 2);

            return GetDummyPlaylist(newPlaylistName);
        }

        public async Task<List<Track>> GetAllLikedSongs(Action<float> reportProgress)
        {
            List<Track> tracks = new List<Track>();
            int trackAmountToAdd = 1000;
            reportProgress?.Invoke(0);

            for (int i = 0; i < trackAmountToAdd; i++)
            {
                var trackStr = $"track_{i}";
                tracks.Add(new Track() { Name = trackStr, Uri = trackStr, Href = trackStr, Id = trackStr });
                reportProgress?.Invoke(i / (float)trackAmountToAdd);
                await Task.Delay(_defaultWaitAmount / 100);
            }

            reportProgress?.Invoke(1f);
            return tracks;
        }

        public async Task<List<Playlist>> GetAllPlaylistsOfActiveUser()
        {
            List<Playlist> playlists = new List<Playlist>();
            int playlistAmountToAdd = 20;

            for (int i = 0; i < playlistAmountToAdd; i++)
            {
                playlists.Add(GetDummyPlaylist(i.ToString()));
                await Task.Delay(_defaultWaitAmount / 100);
            }

            return playlists;
        }

        public async Task<List<Track>> GetAllTracksOfPlaylist(Playlist playlist, Action<float> reportProgress)
        {
            List<Track> tracks = new List<Track>();
            int trackAmountToAdd = 1000;

            for (int i = 0; i < trackAmountToAdd; i++)
            {
                var trackStr = $"track_{i}";
                tracks.Add(new Track() { Name = trackStr, Uri = trackStr, Href = trackStr, Id = trackStr });
                reportProgress?.Invoke(i / (float)trackAmountToAdd);
                await Task.Delay(_defaultWaitAmount / 100);
            }

            return tracks;
        }

        public string GetAuthorizeTokenRequestUri()
        {
            return "test.com";
        }

        private static Playlist GetDummyPlaylist(string diff)
        {
            return new Playlist()
            {
                IsCollaborative = false,
                IsPublic = false,
                ID = $"playlist_{diff}",
                Href = $"playlist_href_{diff}",
                Name = $"test_playlist_{diff}",
                Images = new List<ImageType>() { new ImageType() { Height = 0, Href = "", Width = 0 } },
                Description = $"playlist_desc_{diff}",
                Owner = new BasicUser() { DisplayName = $"owner_{diff}", ExternalUrls = new ExternalUrlType(), Href = $"owner_{diff}", ID = $"owner_{diff}", Type = "user", Uri = $"owner_{diff}" },
                PrimaryColor = "123",
                Type = "playlist",
                Tracks = new TracksCollection(),
                ExternalUrls = new ExternalUrlType(),
                SnapshotId = $"snap_id_{diff}",
                Uri = $"uri_{diff}"
            };
        }
    }
}
