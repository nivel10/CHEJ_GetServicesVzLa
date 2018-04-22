namespace CHEJ_GetServicesVzLa.Services
{
	using System.Threading.Tasks;
	using CHEJ_GetServicesVzLa.Views;
	using Xamarin.Forms;

	public class NavigationService
    {
        public async Task Navigate(string _namePage)
		{
			switch(_namePage)
			{
				case "MenuPage":
					await Application.Current.MainPage.Navigation.PushAsync(
						new MenuPage());
					break;
			}
		}
    }
}