using SpotifyRandomizer.Models;
using SpotifyRandomizer.ViewModels;

namespace SpotifyRandomizer.Views;

public partial class SelectTargetPlaylistPage : ContentPage
{
    private SelectTargetPlaylistViewModel _dataSource;

    public SelectTargetPlaylistPage(List<Playlist> selectedSourcePlaylists, bool isLikedSongsSelected)
	{
        _dataSource = new SelectTargetPlaylistViewModel(selectedSourcePlaylists, isLikedSongsSelected);
        this.BindingContext = _dataSource;
        InitializeComponent();
        ExistingPlaylistListView.ItemSelected += OnExistingPlaylistListViewItemSelected;
	}

    private void OnExistingPlaylistListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        _dataSource.OnExistingPlaylistSelected((e.SelectedItem as PlaylistViewModel)?.Playlist);
    }

    private void OnContinueButtonClicked(object sender, EventArgs e)
    {
        _dataSource.ExecuteContinue();
    }
}