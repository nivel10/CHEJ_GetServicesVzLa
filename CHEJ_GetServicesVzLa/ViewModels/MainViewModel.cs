namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System;
	using System.Collections.ObjectModel;
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Models;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class MainViewModel
    {
		#region Attrbutes

		private static MainViewModel instance;
		private static NavigationService navigationSerive;

		#endregion Attributes

		#region Properties
              
		public LoginViewModel Login
		{
			get;
			set;
		}

        public TokenResponse Token
		{
			get;
			set;
		}

		public MenuViewModel Menu
		{
			get;
			set;
		}

        //  public ObservableCollection<Menu> MyMenu
		public ObservableCollection<MenuItemViewModel> MyMenu
		{
			get;
			set;
		}

        public NewUserViewModel NewUser
		{
			get;
			set;
		}

		public AboutViewModel About 
		{ 
			get;
			set; 
		}

        public RecoveryViewModel Recovery 
		{ 
			get; 
			set; 
		}
        
        public TabMasterViewModel TabMaster
		{
			get;
			set;
		}

		public MyProfileViewModel MyProfile
		{
			get;
			set;
		}
        
        public CantvViewModel Cantv
		{
			get;
			set;
		}

        public CneViewModel Cne
		{
			get;
			set;
		}

        public IvssViewModel Ivss
		{
			get;
			set;
		}

        public ZoomViewModel Zoom
		{
			get;
			set;
		}

		public UserDataResponse UserData
		{
			get;
			set;
		}

		public QueryCantvViewModel QueryCantv
		{
			get;
			set;
		}

		public NewPhoneViewModel NewPhone
		{
			get;
			set;
		}

		public ICommand NewPhoneCommand
		{
			get { return new RelayCommand(GoNewPhone); }
		}
        
		#endregion Properties

		#region Constructor

		public MainViewModel()
		{
			//  Get instance of MainViewModel
			instance = this;

			//  Gets an instance og the services class
			navigationSerive = new NavigationService();

			//  Se instancia la clase del LoginViewModel
			Login = new LoginViewModel();

			//  Load elements of menu
			LoadMenu();
		}      

		#endregion Constructor

		#region Methods

		//  Sigleton
		public static MainViewModel GetInstance()
		{
			if (instance == null)
			{
				instance = new MainViewModel();
			}
			return instance;
		}

		private void LoadMenu()
        {
			//  MyMenu = new ObservableCollection<Menu>();
			MyMenu = new ObservableCollection<MenuItemViewModel>();

			//MyMenu.Add(new Menu
			//{
			//	Icon = "ic_settings.png",
			//	PageName = "MyProfilePage",
            //  Title = "My Profile",
			//});

			//MyMenu.Add(new Menu
			//{
			//	Icon = "ic_exit_to_app.png",
			//	PageName = "LoginPage",
			//	Title = "Close Sesion",
			//});  
                     
			MyMenu.Add(new MenuItemViewModel
            {
                Icon = "ic_settings.png",
                PageName = "MyProfilePage",
                Title = "My Profile",
            });

			MyMenu.Add(new MenuItemViewModel
            {
                Icon = "ic_exit_to_app.png",
                PageName = "LoginPage",
                Title = "Close Sesion",
            });
        }
              
		private async void GoNewPhone()
        {
			//  Gets an instance of the NewPhoneViewModel
			NewPhone = new NewPhoneViewModel(UserData.UserId);

			//  Navigate to teh pag NewPhonePage
			await navigationSerive.NavigateOnMaster("NewPhonePage");
        }

		#endregion Methods
    }
}