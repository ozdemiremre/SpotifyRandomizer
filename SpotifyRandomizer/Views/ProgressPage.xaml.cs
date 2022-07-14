using SpotifyRandomizer.Models;
using SpotifyRandomizer.ViewModels;

namespace SpotifyRandomizer.Views;

public partial class ProgressPage : ContentPage
{
    private readonly ProgressViewModel _dataSource;

    public ProgressPage(List<Playlist> sourcePlaylists, string targetNewPlaylistName, bool isLikedSongsSelected)
	{
        _dataSource = new ProgressViewModel(sourcePlaylists, targetNewPlaylistName, isLikedSongsSelected);
        this.BindingContext = _dataSource;
        InitializeComponent();
    }

    public ProgressPage(List<Playlist> sourcePlaylists, Playlist targetPlaylist, bool isLikedSongsSelected)
    {
        _dataSource = new ProgressViewModel(sourcePlaylists, targetPlaylist, isLikedSongsSelected);
        this.BindingContext = _dataSource;
        InitializeComponent();
    }

    private void OnExitClicked(object sender, EventArgs e)
    {
        Application.Current.Quit();
    }

    private void OnReturnToSourceSelectionClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage.Navigation.PopToRootAsync();
    }

    protected override bool OnBackButtonPressed()
    {
        return !_dataSource.IsFinished;
    }
}