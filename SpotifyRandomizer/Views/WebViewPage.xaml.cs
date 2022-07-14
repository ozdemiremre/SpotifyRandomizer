namespace SpotifyRandomizer.Views;

public partial class WebViewPage : ContentPage
{
	public WebViewPage(string requestedURL)
	{
		InitializeComponent();
		webView.Source = requestedURL;
    }
}