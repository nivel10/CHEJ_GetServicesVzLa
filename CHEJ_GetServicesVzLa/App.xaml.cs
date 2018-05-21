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

        //  Quede aqui
        //  Mejorar este metodo
		//  Cuando el usuario se autentica con Facebook no puede cambiar:
        //  - La imagen
        //  - La contraseña
        //  - Nombre
		public static async Task NavigateToProfile(FacebookResponse profile)
		{
			if (profile == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPage());
				return;
			}

			var apiService = new ApiService();
			var dialogServices = new DialogService();
			//  var dataService = new DataService();

			//  var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
			var token = await apiService.LoginFacebook(
				MethodsHelper.GetUrlAPI(),
				"/api",
				"/Users/LoginFacebook",
				profile);

			if (token == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPage());
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
            }

			var mainViewModel = MainViewModel.GetInstance();

			LoadOfValueUserData(
				(UserDataResponse)response.Result, 
				mainViewModel);
			
			//var user = await apiService.GetUserByEmail(
			//	apiSecurity,
			//	"/api",
			//	"/Users/GetUserByEmail",
			//	token.TokenType,
			//	token.AccessToken,
			//	token.UserName);

			//UserLocal userLocal = null;
			//if (user != null)
			//{
			//	userLocal = Converter.ToUserLocal(user);
			//	dataService.DeleteAllAndInsert(userLocal);
			//}
                     
			mainViewModel.Token = token;
			//  mainViewModel.User = userLocal;
			//  mainViewModel.UserData = 
			//  mainViewModel.Lands = new LandsViewModel();
			Application.Current.MainPage = new MasterPage();
			//  Settings.IsRemembered = "true";

			//  mainViewModel.Lands = new LandsViewModel();
			Application.Current.MainPage = new MasterPage();
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
