using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using SpotifyRandomizer.ViewModels;

namespace SpotifyRandomizer.Models
{
    public class DefaultSpotifyHandler : ISpotifyHandler
    {
        private const int _requestCountPerResponse = 50;
        public const int _maxTrackAddRemoveCount = 80;

        public bool IsInitialized => !string.IsNullOrEmpty(_accessToken);

        private string _authorizationCode;
        private string _accessToken;
        private const string _localResponseAddress = "http://localhost:4002";
        private const string _clientID = "3495a08d93d3404092aff3cfca3b38b3";
        private const string _requestBase = "https://accounts.spotify.com/authorize?";
        private const string _apiBase = "https://api.spotify.com/v1";
        private readonly string _state;
        private readonly string _codeChallange;
        private readonly string _verifier;
        private AccessTokenResult _tokenResult;
        private UserProfileResult _activeUser;
        private readonly string _clientSecret;

        public event ISpotifyHandler.OnRequestResultDelegate OnLoginConcluded;

        public DefaultSpotifyHandler()
        {
            _state = Helpers.GenerateRandomString(16);
            _verifier = Helpers.GenerateRandomString(50);
            _codeChallange = Helpers.ComputeSHA256(_verifier);
            _clientSecret = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_SECRET");
            var listenerTask = Task.Run(() => StartListenerAsync());
        }

        public string GetAuthorizeTokenRequestUri()
        {
            return GetUserAuthorizationRequest().GetUri();
        }

        /// <summary>
        /// Start waiting for redirection to _localResponseAddress with result
        /// If successful, get authorized token and user information
        /// </summary>
        private async Task StartListenerAsync()
        {
            try
            {
                var listener = new System.Net.HttpListener();
                listener.Prefixes.Add(_localResponseAddress + "/");
                listener.Start();

                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                string responseString = "<HTML><BODY> Response Captured! Exiting..</BODY></HTML>";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);

                listener.Stop();

                var result = context.Request.Url.Query;
                result = result.Remove(0, 1);
                var codeAndState = result.Split('&');

                // Check state
                var state = codeAndState[1].Split("=");
                if (state[0] != "state" || state[1] != _state)
                {
                    throw new Exception("Wrong state ID!");
                }

                // Get code
                var code = codeAndState[0].Split("=");
                if (code[0] == "code")
                {
                    _authorizationCode = code[1];
                }
                else
                {
                    throw new Exception("Couldn't parse code");
                }

                _tokenResult = await GetAccessTokenAsync();

                _activeUser = await GetResponseAsync<UserProfileResult>();

                await Task.Delay(50);

                OnLoginConcluded?.Invoke(true);
            }
            catch (Exception)
            {
                OnLoginConcluded?.Invoke(false);
            }
        }

        public UserAuthorizationRequest GetUserAuthorizationRequest()
        {
            return new UserAuthorizationRequest()
            {
                ClientId = _clientID,
                ResponseType = "code",
                RedirectUri = _localResponseAddress,
                State = _state,
                Scope = ScopeFlags.GetScopeRequestString(ScopeFlags.Scopes.LibraryRead
                    | ScopeFlags.Scopes.PlaylistModifyPrivate
                    | ScopeFlags.Scopes.PlaylistModifyPublic
                    | ScopeFlags.Scopes.PlaylistReadPrivate),
                ShowDialog = "false",
                CodeChallengeMethod = "S256",
                CodeChallenge = _codeChallange
            };
        }

        private async Task<AccessTokenResult> GetAccessTokenAsync()
        {
            AccessTokenResult tokenResult = null;

            using (var httpClient = new HttpClient())
            {
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("code", _authorizationCode),
                    new KeyValuePair<string, string>("redirect_uri", _localResponseAddress),
                    new KeyValuePair<string, string>("client_id", _clientID),
                    new KeyValuePair<string, string>("code_verifier", _verifier)
                };

                using (var content = new FormUrlEncodedContent(postData))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var authStr = "Basic " + Helpers.Base64Encode(_clientID) + ":" + Helpers.Base64Encode(_clientSecret);

                    content.Headers.TryAddWithoutValidation("Authorization", authStr);
                    HttpResponseMessage postResponse = await httpClient.PostAsync("https://accounts.spotify.com/api/token", content);

                    if (postResponse.IsSuccessStatusCode)
                    {
                        _accessToken = postResponse.Content.ToString();
                        var resultAsString = await postResponse.Content.ReadAsStringAsync();
                        tokenResult = (AccessTokenResult)System.Text.Json.JsonSerializer.Deserialize(resultAsString, typeof(AccessTokenResult));
                    }
                }
            }

            return tokenResult;
        }

        /// <summary>
        /// Request GET for a response with the current active user. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private async Task<T> GetResponseAsync<T>() where T : BaseResponse
        {
            T responseType = null;
            string url = typeof(T).GetProperty("URL", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).GetValue(null).ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBase);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenResult.Token);

                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var resultAsString = await response.Content.ReadAsStringAsync();
                        responseType = (T)System.Text.Json.JsonSerializer.Deserialize(resultAsString, typeof(T));
                    }
                }
            }

            return responseType;
        }

        private async Task<T> GetResponseAsType<T>(string URL)
        {
            T responseType = default;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBase);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenResult.Token);

                using (HttpResponseMessage response = await client.GetAsync(URL))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var resultAsString = await response.Content.ReadAsStringAsync();
                        responseType = (T)System.Text.Json.JsonSerializer.Deserialize(resultAsString, typeof(T));
                    }
                }
            }

            return responseType;
        }

        public async Task<List<Playlist>> GetAllPlaylistsOfActiveUser()
        {
            List<Playlist> playlists = new List<Playlist>();
            int requestOffset = 0;
            int foundPlaylistCount = 0;

            do
            {
                var initialRequestURL = GetUserPlaylists.GetURL(_activeUser.ID, _requestCountPerResponse, requestOffset);
                var result = await GetResponseAsType<GetUserPlaylists>(initialRequestURL);
                foundPlaylistCount = result.Total;
                requestOffset += _requestCountPerResponse;
                playlists.AddRange(result.Items);
            }
            while (foundPlaylistCount >= _requestCountPerResponse);

            return playlists;
        }

        public async Task<List<Track>> GetAllTracksOfPlaylist(Playlist playlist, Action<float> reportProgress)
        {
            List<Track> tracks = new List<Track>();
            int requestOffset = 0;
            int foundTrackCount = 0;
            reportProgress?.Invoke(0f);

            do
            {
                var initialRequestURL = GetTracksOfPlaylist.GetURL(playlist.ID, _requestCountPerResponse, requestOffset);
                var result = await GetResponseAsType<GetTracksOfPlaylist>(initialRequestURL);
                foundTrackCount = result.Total;
                requestOffset += _requestCountPerResponse;
                tracks.AddRange(result.Items.Select(r => r.Track));
                reportProgress?.Invoke(tracks.Count / (float)foundTrackCount);
            }
            while (foundTrackCount > tracks.Count);

            return tracks;
        }

        public async Task<Playlist> CreateNewPlaylist(string newPlaylistName, string newPlaylistDescription, bool isCollaborative, bool isPublic)
        {
            Playlist tokenResult = null;

            using (var httpClient = new HttpClient())
            {
                var creationMessage = new CreatePlaylistBody
                {
                    IsPublic = isPublic,
                    IsCollaborative = isCollaborative,
                    Description = newPlaylistDescription,
                    Name = newPlaylistName
                };

                var creationJsonString = System.Text.Json.JsonSerializer.Serialize(creationMessage, typeof(CreatePlaylistBody));

                using (var content = new StringContent(creationJsonString, Encoding.UTF8, "application/json"))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/json");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenResult.Token);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var url = CreatePlaylistBody.GetURL(_activeUser.ID);
                    HttpResponseMessage postResponse = await httpClient.PostAsync(url, content);

                    if (postResponse.IsSuccessStatusCode)
                    {
                        _accessToken = postResponse.Content.ToString();
                        var resultAsString = await postResponse.Content.ReadAsStringAsync();
                        tokenResult = (Playlist)System.Text.Json.JsonSerializer.Deserialize(resultAsString, typeof(Playlist));
                    }
                }
            }

            return tokenResult;
        }

        public async Task<bool> ClearAllTracksInPlaylist(Playlist targetExistingTargetPlaylist, Action<float> reportProgress)
        {
            bool success = false;

            var allTracks = await GetAllTracksOfPlaylist(targetExistingTargetPlaylist, f => reportProgress?.Invoke(f * 0.5f));

            using (var httpClient = new HttpClient())
            {
                var urisOfTracks = allTracks.ConvertAll(t => t.Uri);
                int removedTrackCount = 0;
                reportProgress?.Invoke(0.5f);

                do
                {
                    int countThatWillBeRemoved = Math.Min(_maxTrackAddRemoveCount, urisOfTracks.Count - removedTrackCount);
                    var selectUris = urisOfTracks.GetRange(removedTrackCount, countThatWillBeRemoved);
                    var removalMessage = new RemoveTracksFromPlaylistBody
                    {
                        TrackUris = selectUris
                    };

                    var deletionJsonString = System.Text.Json.JsonSerializer.Serialize(removalMessage, typeof(RemoveTracksFromPlaylistBody));

                    using (var content = new StringContent(deletionJsonString, Encoding.UTF8, "application/json"))
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenResult.Token);
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var url = RemoveTracksFromPlaylistBody.GetURL(targetExistingTargetPlaylist.ID);
                        var request = new HttpRequestMessage(HttpMethod.Delete, url);
                        request.Content = new StringContent(deletionJsonString, Encoding.UTF8, "application/json");

                        HttpResponseMessage postResponse = await httpClient.SendAsync(request);

                        if (postResponse.IsSuccessStatusCode)
                        {
                            success = true;
                        }
                    }

                    removedTrackCount += countThatWillBeRemoved;
                    reportProgress?.Invoke(0.5f + ((removedTrackCount / (float)urisOfTracks.Count) * 0.5f));
                }
                while (urisOfTracks.Count > removedTrackCount);

            }

            return success;
        }

        public async Task<bool> AddTracksToPlaylist(Playlist playlistToAddTracksTo, List<Track> uniqueTracks, Action<float> reportProgress)
        {
            bool success = false;

            using (var httpClient = new HttpClient())
            {
                var urisOfTracks = uniqueTracks.ConvertAll(t => t.Uri);
                int addedTrackCount = 0;
                reportProgress?.Invoke(0f);

                do
                {
                    int countThatWillBeAdded = Math.Min(_maxTrackAddRemoveCount, urisOfTracks.Count - addedTrackCount);
                    var selectUris = urisOfTracks.GetRange(addedTrackCount, countThatWillBeAdded);
                    var additionMessage = new AddTracksToPlaylistBody
                    {
                        TrackUris = selectUris
                    };

                    var creationJsonString = System.Text.Json.JsonSerializer.Serialize(additionMessage, typeof(AddTracksToPlaylistBody));

                    using (var content = new StringContent(creationJsonString, Encoding.UTF8, "application/json"))
                    {
                        content.Headers.Clear();
                        content.Headers.Add("Content-Type", "application/json");
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenResult.Token);
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var url = AddTracksToPlaylistBody.GetURL(playlistToAddTracksTo.ID);
                        HttpResponseMessage postResponse = await httpClient.PostAsync(url, content);

                        if (postResponse.IsSuccessStatusCode)
                        {
                            success = true;
                        }
                    }

                    addedTrackCount += countThatWillBeAdded;
                    reportProgress?.Invoke(addedTrackCount / (float)urisOfTracks.Count);
                }
                while (urisOfTracks.Count > addedTrackCount);

            }

            return success;
        }

        public async Task<List<Track>> GetAllLikedSongs(Action<float> reportProgress)
        {
            List<Track> tracks = new List<Track>();
            int requestOffset = 0;
            int foundTrackCount = 0;
            reportProgress?.Invoke(0f);

            do
            {
                var requestURL = GetSavedTracksOfActiveUser.GetURL(_requestCountPerResponse, requestOffset);
                var result = await GetResponseAsType<GetSavedTracksOfActiveUser>(requestURL);
                foundTrackCount = result.Total;
                requestOffset += _requestCountPerResponse;
                tracks.AddRange(result.Items.Select(r => r.Track));
                reportProgress?.Invoke(tracks.Count / (float)foundTrackCount);
            }
            while (foundTrackCount > tracks.Count);

            reportProgress?.Invoke(1f);

            return tracks;
        }
    }
}
