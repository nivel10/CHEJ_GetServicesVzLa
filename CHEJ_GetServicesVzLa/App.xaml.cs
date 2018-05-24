namespace CHEJ_GetServicesVzLa
{
	using System;
	using System.Threading.Tasks;
	using CHEJ_GetServicesVzLa.Helpers;
	using CHEJ_GetServicesVzLa.Models;
	using CHEJ_GetServicesVzLa.Services;
	using CHEJ_GetServicesVzLa.ViewModels;
	using CHEJ_GetServicesVzLa.Views;
	using Xamarin.Forms;

	public partial class App : Application
    {
		private static ApiService apiService;
		private static DialogService dialogService;
		private static NavigationService navigationService;
		private static MainViewModel mainViewModel;

        #region Properties

		public static NavigationPage Navigator
		{
			get;
			internal set;
		}

		public static MasterPage Master 
		{ 
			get; 
			internal set; 
		}

		#endregion Properties

		#region Constructor

		public App()
		{
			InitializeComponent();

			//  Gets an instance of the services
			apiService = new ApiService();
			dialogService = new DialogService();
			navigationService = new NavigationService();
			mainViewModel = MainViewModel.GetInstance();

			//  MainPage = new MainPage();
			//  MainPage = new LoginPage();
			this.MainPage = new NavigationPage(new LoginPage());
			//  this.MainPage = new MasterPage();
		}

		#endregion Constructor

		#region Methods

		public static Action HideLoginView
		{
			get
			{
				return new Action(() => Application.Current.MainPage =
								  new NavigationPage(new LoginPage()));
			}
		}
        
		public static async Task NavigateToProfile(FacebookResponse profile)
		{
			if (profile == null)
			{
				mainViewModel.Login.SetStatusControl(true, true, false, 0);
				await dialogService.ShowMessage(
					"Error", 
					"the facebook response is not available, try later...!!!", 
					"Accept");
				navigationService.SetMainPage("LoginPage");
				//  Application.Current.MainPage = new NavigationPage(new LoginPage());            
				return;
			}
            
			var token = await apiService.LoginFacebook(
				MethodsHelper.GetUrlAPI(),
				"/api",
				"/Users/LoginFacebook",
				profile);

			if (token == null)
			{
				mainViewModel.Login.SetStatusControl(true, true, false, 0);
				await dialogService.ShowMessage(
					"Error", 
					"Login with facebook is not available, try later...!!!", 
					"Accept");
				//  Application.Current.MainPage = new NavigationPage(new LoginPage());
				navigationService.SetMainPage("LoginPage");
				return;
			}

			//  Get data of the user
            var response = await apiService.Get<UserDataResponse>(
                MethodsHelper.GetUrlAPI(),
                "/api/Users",
                "/GetServicesVzLaUSerByEmail",
                string.Format(
					"/?email={0}",
                    token.UserName),
                    token.TokenType,
                    token.AccessToken);
            if (!response.IsSuccess)
            {
				mainViewModel.Login.SetStatusControl(true, true, false, 0);
				await dialogService.ShowMessage(
					"Error", 
					response.Message, 
					"Accept");
				navigationService.SetMainPage("LoginPage");
				return;
            }
            
            //  Load values of the UserData
			LoadOfValueUserData(
				(UserDataResponse)response.Result, 
				mainViewModel);
			
			//  Load value to token   
			mainViewModel.Token = token;

            //  Gets an instance of the CantvViewModel
			mainViewModel.Cantv = new CantvViewModel();

			//  Set the MainPage to MasterPage
			navigationService.SetMainPage("MasterPage");
		}

		private static void LoadOfValueUserData(
			UserDataResponse _userDataResponse, MainViewModel _mainViewModel)
        {
            //  Get new instance of UserDataResponse         
			_mainViewModel.UserData = new UserDataResponse();
			_mainViewModel.UserData.AppName = _userDataResponse.AppName;
			_mainViewModel.UserData.CantvDatas = _userDataResponse.CantvDatas;
			_mainViewModel.UserData.CneIvssDatas = _userDataResponse.CneIvssDatas;
			_mainViewModel.UserData.Email = _userDataResponse.Email;
			_mainViewModel.UserData.FirstName = _userDataResponse.FirstName;
			_mainViewModel.UserData.ImageArray = _userDataResponse.ImageArray;
			_mainViewModel.UserData.ImagePath = _userDataResponse.ImagePath;
			_mainViewModel.UserData.LastName = _userDataResponse.LastName;
			_mainViewModel.UserData.Password = _userDataResponse.Password;
			_mainViewModel.UserData.Telephone = _userDataResponse.Telephone;
			_mainViewModel.UserData.UserId = _userDataResponse.UserId;
			_mainViewModel.UserData.UserTypeId = _userDataResponse.UserTypeId;
			_mainViewModel.UserData.ZoomDatas = _userDataResponse.ZoomDatas;
        }

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

        #endregion Methods
    }
}
