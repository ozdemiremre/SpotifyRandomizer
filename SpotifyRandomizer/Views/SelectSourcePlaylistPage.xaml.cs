using SpotifyRandomizer.ViewModels;
namespace SpotifyRandomizer;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SelectSourcePlaylistPage : ContentPage
{
    private SelectSourcePlaylistViewModel _dataSource;

    public SelectSourcePlaylistPage()
    {
        InitializeComponent();
        _dataSource = new SelectSourcePlaylistViewModel();
        this.BindingContext = _dataSource;
        PlaylistListView.ItemTapped += OnPlaylistListViewItemTapped;
    }

    private void OnPlaylistListViewItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is PlaylistViewModel playlist)
        {
            _dataSource.OnSelectionToggledForPlaylist(playlist);
        }
    }

    private void OnContinueButtonClicked(object sender, EventArgs e)
    {
        _dataSource.ExecuteContinue();
    }
}