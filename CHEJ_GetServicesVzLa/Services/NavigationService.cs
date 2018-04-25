namespace CHEJ_GetServicesVzLa.Services
{
	using System.Threading.Tasks;
	using CHEJ_GetServicesVzLa.Views;
	using Xamarin.Forms;

	public class NavigationService
    {
        public async Task NavigateOnMaster(string _namePage)
		{
			//  This property hide automaty the menu
			App.Master.IsPresented = false;

			switch(_namePage)
			{
				case "MenuPage":
					//await Application.Current.MainPage.Navigation.PushAsync(
						//new MenuPage());
					await App.Navigator.PushAsync(new MenuPage());
					break;

				case "MyProfilePage":
					await App.Navigator.PushAsync(new MyProfilePage());
					break;
			}
		}

		public async Task NavigateOnLogin(string _namePage)
        {
            switch (_namePage)
            {
				case "NewUserPage":
                    await Application.Current.MainPage.Navigation.PushAsync(
						new NewUserPage());               
                    break;

				case "AboutPage":
					await Application.Current.MainPage.Navigation.PushAsync(
						new AboutPage());
					break;

				case "RecoveryPage":
                    await Application.Current.MainPage.Navigation.PushAsync(
                        new RecoveryPage());
                    break;
            }
        }

		public void SetMainPage(string _pageName)
		{
			switch(_pageName)
			{
				case "LoginPage":
					Application.Current.MainPage = 
						new NavigationPage(new LoginPage());
					break;

				case "MasterPage":
					Application.Current.MainPage = new MasterPage();
					break;
			}
		}

		public async Task GoBackOnMaster()
		{
			await App.Navigator.PopAsync();
		}

		public async Task GoBackOnLogin()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}