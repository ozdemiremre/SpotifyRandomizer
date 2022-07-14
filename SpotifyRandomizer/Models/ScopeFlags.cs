using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyRandomizer.Models
{
    public static class ScopeFlags
    {
        [Flags]
        public enum Scopes
        {
            /// <summary>
            /// Do not use.
            /// </summary>
            None = 0,
            /// <summary>
            /// Write access to user-provided images.
            /// </summary>
            ImageUpload = 1 << 0,
            /// <summary>
            /// Write access to a user’s playback state.
            /// </summary>
            ModifyPlaybackState = 1 << 1,
            /// <summary>
            /// Read access to a user’s player state.
            /// </summary>
            ReadPlaybackState = 1 << 2,
            /// <summary>
            /// Read access to a user’s currently playing content.
            /// </summary>
            ReadCurrentlyPlaying = 1 << 3,
            /// <summary>
            /// Write/delete access to the list of artists and other users that the user follows.
            /// </summary>
            FollowModify = 1 << 4,
            /// <summary>
            /// Read access to the list of artists and other users that the user follows.
            /// </summary>
            FollowRead = 1 << 5,
            /// <summary>
            /// Read access to a user’s recently played tracks.
            /// </summary>
            ReadRecentlyPlayed = 1 << 6,
            /// <summary>
            /// Read access to a user’s playback position in a content.
            /// </summary>
            ReadPlaybackPosition = 1 << 7,
            /// <summary>
            /// Read access to a user's top artists and tracks.
            /// </summary>
            ReadTopArtistsAndTracks = 1 << 8,
            /// <summary>
            /// Include collaborative playlists when requesting a user's playlists.
            /// </summary>
            PlaylistReadCollaborative = 1 << 9,
            /// <summary>
            /// Write access to a user's public playlists.
            /// </summary>
            PlaylistModifyPublic = 1 << 10,
            /// <summary>
            /// Read access to user's private playlists.
            /// </summary>
            PlaylistReadPrivate = 1 << 11,
            /// <summary>
            /// Write access to a user's private playlists.
            /// </summary>
            PlaylistModifyPrivate = 1 << 12,
            /// <summary>
            /// Remote control playback of Spotify. This scope is currently available to Spotify iOS and Android SDKs.
            /// </summary>
            AppRemoteControl = 1 << 13,
            /// <summary>
            /// Control playback of a Spotify track. This scope is currently available to the Web Playback SDK. The user must have a Spotify Premium account.
            /// </summary>
            Streaming = 1 << 14,
            /// <summary>
            /// Read access to user’s email address.
            /// </summary>
            ReadEmail = 1 << 15,
            /// <summary>
            /// Read access to user’s subscription details (type of user account).
            /// </summary>
            ReadPrivate = 1 << 16,
            /// <summary>
            /// Write/delete access to a user's "Your Music" library.
            /// </summary>
            LibraryModify = 1 << 17,
            /// <summary>
            /// Read access to a user's library.
            /// </summary>
            LibraryRead = 1 << 18,
        }

        public static string GetScopeRequestString(Scopes requestedScopes)
        {
            List<string> requests = new List<string>();

            foreach (Scopes value in Enum.GetValues(typeof(Scopes)))
            {
                if ((requestedScopes & value) == value)
                {
                    switch (value)
                    {
                        case Scopes.ImageUpload:
                            requests.Add("ugc-image-upload");
                            break;
                        case Scopes.ModifyPlaybackState:
                            requests.Add("user-modify-playback-state");
                            break;
                        case Scopes.ReadPlaybackState:
                            requests.Add("user-read-playback-state");
                            break;
                        case Scopes.ReadCurrentlyPlaying:
                            requests.Add("user-read-currently-playing");
                            break;
                        case Scopes.FollowModify:
                            requests.Add("user-follow-modify");
                            break;
                        case Scopes.FollowRead:
                            requests.Add("user-follow-read");
                            break;
                        case Scopes.ReadRecentlyPlayed:
                            requests.Add("user-read-recently-played");
                            break;
                        case Scopes.ReadPlaybackPosition:
                            requests.Add("user-read-playback-position");
                            break;
                        case Scopes.ReadTopArtistsAndTracks:
                            requests.Add("user-top-read");
                            break;
                        case Scopes.PlaylistReadCollaborative:
                            requests.Add("playlist-read-collaborative");
                            break;
                        case Scopes.PlaylistModifyPublic:
                            requests.Add("playlist-modify-public");
                            break;
                        case Scopes.PlaylistReadPrivate:
                            requests.Add("playlist-read-private");
                            break;
                        case Scopes.PlaylistModifyPrivate:
                            requests.Add("playlist-modify-private");
                            break;
                        case Scopes.AppRemoteControl:
                            requests.Add("app-remote-control");
                            break;
                        case Scopes.Streaming:
                            requests.Add("streaming");
                            break;
                        case Scopes.ReadEmail:
                            requests.Add("user-read-email");
                            break;
                        case Scopes.ReadPrivate:
                            requests.Add("user-read-private");
                            break;
                        case Scopes.LibraryModify:
                            requests.Add("user-library-modify");
                            break;
                        case Scopes.LibraryRead:
                            requests.Add("user-library-read");
                            break;
                        default:
                            break;
                    }
                }
            }

            return string.Join(" ", requests);
        }
    }
}
