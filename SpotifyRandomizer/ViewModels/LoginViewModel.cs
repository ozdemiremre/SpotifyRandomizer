using SpotifyRandomizer.Models;
using SpotifyRandomizer.Views;

namespace SpotifyRandomizer.ViewModels
{
    public class LoginViewModel
    {
        private bool _requestedToken = false;

        public LoginViewModel()
        {
            SpotifySession.ActiveSession.OnLoginConcluded += OnLoginConcluded;
        }

        public void ExecuteLogin()
        {
#if DEBUG_WTEST
            Helpers.ExecuteOnUIThread(() => Application.Current.MainPage.Navigation.PushAsync(new SelectSourcePlaylistPage()));
#else   
            if (!_requestedToken)
            {
                // Start token request
                var requestUri = SpotifySession.ActiveSession.GetAuthorizeTokenRequestUri();

                // Open request page
                Application.Current.MainPage.Navigation.PushModalAsync(new WebViewPage(requestUri));
                _requestedToken = true;
            }
            else
            {
                Helpers.ExecuteOnUIThread(() => Application.Current.MainPage.Navigation.PushAsync(new SelectSourcePlaylistPage()));
            }
#endif
        }

        private void OnLoginConcluded(bool success)
        {
            if (success)
            {
                if (_requestedToken)
                {
                    Helpers.ExecuteOnUIThread(() => Application.Current.MainPage.Navigation.PopModalAsync());
                }

                Helpers.ExecuteOnUIThread(() => Application.Current.MainPage.Navigation.PushAsync(new SelectSourcePlaylistPage()));
            }
            else
            {
                _requestedToken = false;
            }
        }
    }
}
