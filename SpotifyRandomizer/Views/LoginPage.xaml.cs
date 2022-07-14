using SpotifyRandomizer.Models;
using SpotifyRandomizer.ViewModels;

namespace SpotifyRandomizer;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class LoginPage : ContentPage
{
	private readonly LoginViewModel _viewModel;
	public LoginPage()
	{
		InitializeComponent();
		_viewModel = new LoginViewModel();
		this.BindingContext = _viewModel;
    }

	private void OnLoginButtonClicked(object sender, EventArgs e)
	{
		IsBusy = true;
		_viewModel.ExecuteLogin();
    }
}