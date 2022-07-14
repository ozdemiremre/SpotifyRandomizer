using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Diagnostics;

namespace SpotifyRandomizer.Models
{
    public class AccessTokenResult
    {
        /// <summary>
        /// An Access Token that can be provided in subsequent calls, for example to Spotify Web API services.
        /// </summary>
        [JsonPropertyName("access_token")]
        public string Token { get; set; }

        /// <summary>
        /// How the Access Token may be used: always “Bearer”.
        /// </summary>
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// A space-separated list of scopes which have been granted for this access_token.
        /// </summary>
        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// The time period (in seconds) for which the Access Token is valid.
        /// </summary>
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// A token that can be sent to the Spotify Accounts service in place of an authorization code.
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }

    public class BaseResponse
    {
        public static string URL => "";
    }

    public class Playlist
    {
        [JsonPropertyName("collaborative")]
        public bool IsCollaborative { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Known external URLs for this user.
        /// </summary>
        [JsonPropertyName("external_urls")]
        public ExternalUrlType ExternalUrls { get; set; }

        /// <summary>
        /// A link to the Web API endpoint for this user.
        /// </summary>
        [JsonPropertyName("href")]
        public string Href { get; set; }

        /// <summary>
        /// The Spotify ID for the playlist.
        /// </summary>
        [JsonPropertyName("id")]
        public string ID { get; set; }

        /// <summary>
        /// The playlist's images.
        /// </summary>
        [JsonPropertyName("images")]
        public List<ImageType> Images { get; set; }

        /// <summary>
        /// The name for the playlist.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// The owner of the playlist.
        /// </summary>
        [JsonPropertyName("owner")]
        public BasicUser Owner { get; set; }

        /// <summary>
        /// Primary color of the playlist.
        /// </summary>
        [JsonPropertyName("primary_color")]
        public string PrimaryColor { get; set; }

        /// <summary>
        /// Represents if the playlist is public or not.
        /// </summary>
        [JsonPropertyName("public")]
        public bool IsPublic { get; set; }

        /// <summary>
        /// Snapshot Id of the playlist.
        /// </summary>
        [JsonPropertyName("snapshot_id")]
        public string SnapshotId { get; set; }

        /// <summary>
        /// Snapshot Id of the playlist.
        /// </summary>
        [JsonPropertyName("tracks")]
        public TracksCollection Tracks { get; set; }

        /// <summary>
        /// Type. "playlist"
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// spotify:playlist:{ID}
        /// </summary>
        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }

    public class TracksCollection
    {
        /// <summary>
        /// A link to the Web API endpoint for playlist tracks.
        /// </summary>
        [JsonPropertyName("href")]
        public string Href { get; set; }

        /// <summary>
        /// The total number of tracks.
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class BasicUser
    {
        /// <summary>
        /// The display name of the user.
        /// </summary>
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Known external URLs for this user.
        /// </summary>
        [JsonPropertyName("external_urls")]
        public ExternalUrlType ExternalUrls { get; set; }

        /// <summary>
        /// A link to the Web API endpoint for this user.
        /// </summary>
        [JsonPropertyName("href")]
        public string Href { get; set; }

        /// <summary>
        /// The Spotify ID for the user.
        /// </summary>
        [JsonPropertyName("id")]
        public string ID { get; set; }

        /// <summary>
        /// The Spotify type of the user.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }


        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }

    public class GetUserPlaylists
    {
        /// <summary>
        /// Get a list of the playlists owned or followed by a Spotify user.
        /// </summary>
        /// <param name="userID">The user's Spotify user ID.</param>
        /// <param name="limit">The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50.</param>
        /// <param name="offset">The index of the first playlist to return. Default: 0 (the first object). Maximum offset: 100.000. Use with limit to get the next set of playlists.</param>
        /// <returns></returns>
        public static string GetURL(string userID, int limit = 20, int offset = 0)
        {
            return $"https://api.spotify.com/v1/users/{userID}/playlists?offset={offset}&limit={limit}";
        }

        /// <summary>
        /// A link to the Web API endpoint returning the full result of the request
        /// </summary>
        [JsonPropertyName("href")]
        public string Href { get; set; }

        /// <summary>
        /// The requested content. Playlists
        /// </summary>
        [JsonPropertyName("items")]
        public List<Playlist> Items { get; set; }

        /// <summary>
        /// The maximum number of items in the response (as set in the query or by default).
        /// </summary>
        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// URL to the next page of items. (null if none)
        /// </summary>
        [JsonPropertyName("next")]
        public string Next { get; set; }

        /// <summary>
        /// The offset of the items returned (as set in the query or by default)
        /// </summary>
        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        /// <summary>
        /// URL to the previous page of items. (null if none)
        /// </summary>
        [JsonPropertyName("previous")]
        public string Previous { get; set; }

        /// <summary>
        /// The total number of items available to return.
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class GetTracksOfPlaylist
    {
        /// <summary>
        /// Get a list of the playlists owned or followed by a Spotify user.
        /// </summary>
        /// <param name="limit">The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50.</param>
        /// <param name="offset">The index of the first playlist to return. Default: 0 (the first object). Maximum offset: 100.000. Use with limit to get the next set of playlists.</param>
        /// <returns></returns>
        public static string GetURL(string playlistId, int limit = 20, int offset = 0)
        {
            return $"https://api.spotify.com/v1/playlists/{playlistId}/tracks?offset={offset}&limit={limit}&fields=href,items(track(name,href,id,uri)),limit,next,offset,previous,total";
        }

        /// <summary>
        /// A link to the Web API endpoint returning the full result of the request
        /// </summary>
        [JsonPropertyName("href")]
        public string Href { get; set; }

        /// <summary>
        /// The requested content. Tracks
        /// </summary>
        [JsonPropertyName("items")]
        public List<TrackWrapper> Items { get; set; }

        /// <summary>
        /// The maximum number of items in the response (as set in the query or by default).
        /// </summary>
        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// URL to the next page of items. (null if none)
        /// </summary>
        [JsonPropertyName("next")]
        public string Next { get; set; }

        /// <summary>
        /// The offset of the items returned (as set in the query or by default)
        /// </summary>
        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        /// <summary>
        /// URL to the previous page of items. (null if none)
        /// </summary>
        [JsonPropertyName("previous")]
        public string Previous { get; set; }

        /// <summary>
        /// The total number of items available to return.
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class GetSavedTracksOfActiveUser
    {
        /// <summary>
        /// Get a list of the playlists owned or followed by a Spotify user.
        /// </summary>
        /// <param name="userID">The user's Spotify user ID.</param>
        /// <param name="limit">The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50.</param>
        /// <param name="offset">The index of the first playlist to return. Default: 0 (the first object). Maximum offset: 100.000. Use with limit to get the next set of playlists.</param>
        /// <returns></returns>
        public static string GetURL(int limit = 20, int offset = 0)
        {
            return $"https://api.spotify.com/v1/me/tracks/?offset={offset}&limit={limit}";
        }

        /// <summary>
        /// A link to the Web API endpoint returning the full result of the request
        /// </summary>
        [JsonPropertyName("href")]
        public string Href { get; set; }

        /// <summary>
        /// The requested content. Tracks
        /// </summary>
        [JsonPropertyName("items")]
        public List<TrackWrapper> Items { get; set; }

        /// <summary>
        /// The maximum number of items in the response (as set in the query or by default).
        /// </summary>
        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// URL to the next page of items. (null if none)
        /// </summary>
        [JsonPropertyName("next")]
        public string Next { get; set; }

        /// <summary>
        /// The offset of the items returned (as set in the query or by default)
        /// </summary>
        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        /// <summary>
        /// URL to the previous page of items. (null if none)
        /// </summary>
        [JsonPropertyName("previous")]
        public string Previous { get; set; }

        /// <summary>
        /// The total number of items available to return.
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Track
    {
        /// <summary>
        /// A link to the Web API endpoint for the track.
        /// </summary>
        [JsonPropertyName("href")]
        public string Href { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }

    public class TrackWrapper
    {
        [JsonPropertyName("track")]
        public Track Track { get; set; }
    }

    public class UserProfileResult : BaseResponse
    {
        new public static string URL => "https://api.spotify.com/v1/me";

        /// <summary>
        /// The country of the user, as set in the user's account profile. An ISO 3166-1 alpha-2 country code. This field is only available when the current user has granted access to the user-read-private scope.
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; }

        /// <summary>
        /// The name displayed on the user's profile. null if not available.
        /// </summary>
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// The user's email address, as entered by the user when creating their account. Important! This email address is unverified; there is no proof that it actually belongs to the user. This field is only available when the current user has granted access to the user-read-email scope.
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        /// The user's explicit content settings. This field is only available when the current user has granted access to the user-read-private scope.
        /// </summary>
        [JsonPropertyName("explicit_content")]
        public ExplicitContentType ExplicitContent { get; set; }

        /// <summary>
        /// Known external URLs for this user.
        /// </summary>
        [JsonPropertyName("external_urls")]
        public ExternalUrlType ExternalUrls { get; set; }

        /// <summary>
        /// Information about the followers of the user.
        /// </summary>
        [JsonPropertyName("followers")]
        public FollowersType Followers { get; set; }

        /// <summary>
        /// A link to the Web API endpoint for this user.
        /// </summary>
        [JsonPropertyName("href")]
        public string Href { get; set; }

        /// <summary>
        /// The Spotify user ID for the user.
        /// </summary>
        [JsonPropertyName("id")]
        public string ID { get; set; }

        /// <summary>
        /// The user's profile image.
        /// </summary>
        [JsonPropertyName("images")]
        public List<ImageType> Images { get; set; }

        /// <summary>
        /// The user's Spotify subscription level: "premium", "free", etc. (The subscription level "open" can be considered the same as "free".) This field is only available when the current user has granted access to the user-read-private scope.
        /// </summary>
        [JsonPropertyName("product")]
        public string Product { get; set; }

        /// <summary>
        /// The object type: "user"
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// The Spotify URI for the user.
        /// </summary>
        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }

    public class CreatePlaylistBody
    {
        public static string GetURL(string userId)
        {
            return $"https://api.spotify.com/v1/users/{userId}/playlists";
        }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("public")]
        public bool IsPublic { get; set; }

        [JsonPropertyName("collaborative")]
        public bool IsCollaborative { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class AddTracksToPlaylistBody
    {
        public static string GetURL(string playlistId, int limit = 20, int offset = 0)
        {
            return $"https://api.spotify.com/v1/playlists/{playlistId}/tracks";
        }

        [JsonPropertyName("uris")]
        public List<string> TrackUris { get; set; }
    }

    public class RemoveTracksFromPlaylistBody
    {
        public static string GetURL(string playlistId, int limit = 20, int offset = 0)
        {
            return $"https://api.spotify.com/v1/playlists/{playlistId}/tracks";
        }

        [JsonPropertyName("uris")]
        public List<string> TrackUris { get; set; }
    }

    public class GetCurrentUserPlaylistsResult : BaseResponse
    {
        new public static string URL => "https://api.spotify.com/v1/me/playlists";
    }


    /// <summary>
    /// The user's explicit content settings. This field is only available when the current user has granted access to the user-read-private scope.
    /// </summary>
    public class ExplicitContentType
    {
        /// <summary>
        /// When true, indicates that explicit content should not be played.
        /// </summary>
        [JsonPropertyName("filter_enabled")]
        public bool FilterEnabled { get; set; }

        /// <summary>
        /// When true, indicates that the explicit content setting is locked and can't be changed by the user.
        /// </summary>
        [JsonPropertyName("filter_locked")]
        public bool FilterLocked { get; set; }
    }

    public class ExternalUrlType
    {
        /// <summary>
        /// The Spotify URL for the object.
        /// </summary>
        public string Url { get; set; }
    }


    public class FollowersType
    {
        /// <summary>
        /// This will always be set to null, as the Web API does not support it at the moment.
        /// </summary>
        [JsonPropertyName("href")]
        public string Href { get; set; }

        /// <summary>
        /// The total number of followers.
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class ImageType
    {
        /// <summary>
        /// The source URL of the image.
        /// </summary>
        [JsonPropertyName("url")]
        public string Href { get; set; }

        /// <summary>
        /// The image height in pixels. Can be null.
        /// </summary>
        [JsonPropertyName("height")]
        public int? Height { get; set; }

        /// <summary>
        /// The image width in pixels. Can be null.
        /// </summary>
        [JsonPropertyName("width")]
        public int? Width { get; set; }
    }

    public class UserAuthorizationRequest
    {
        public string ClientId { get; set; }
        public string ResponseType { get; set; }
        public string RedirectUri { get; set; }
        public string State { get; set; }
        public string Scope { get; set; }
        public string ShowDialog { get; set; }
        public string CodeChallengeMethod { get; set; }
        public string CodeChallenge { get; set; }

        public string GetUri()
        {
            StringBuilder builder = new StringBuilder("https://accounts.spotify.com/authorize/?");
            builder.Append("client_id=" + ClientId);
            builder.Append("&response_type=" + ResponseType);
            builder.Append("&redirect_uri=" + RedirectUri);
            builder.Append("&state=" + State);
            builder.Append("&scope=" + Scope);
            builder.Append("&show_dialog=" + ShowDialog);
            builder.Append("&code_challenge_method=" + CodeChallengeMethod);
            builder.Append("&code_challenge=" + CodeChallenge);
            return builder.ToString();
        }
    }
}
