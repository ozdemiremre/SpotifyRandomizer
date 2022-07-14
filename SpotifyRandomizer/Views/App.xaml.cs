
using SpotifyRandomizer.Models;

namespace SpotifyRandomizer;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		MainPage = new NavigationPage(new LoginPage());
        Shell.SetNavBarIsVisible(new NavBarChangeClass(), false);
    }
}

public class NavBarChangeClass: BindableObject
{

}
