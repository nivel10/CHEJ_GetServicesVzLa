namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Models;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class MainViewModel
    {
		#region Attrbutes

		private static MainViewModel instance;
		private NavigationService navigationService;      

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
		//  public ObservableCollection<MenuItemViewModel> MyMenu
        public ObservableCollection<MenuViewModel> MyMenu
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

		public GetCantvViewModel GetCantv
		{
			get;
			set;
		}

		public NewCantvViewModel NewCantv
		{
			get;
			set;
		}

		public EditCantvViewModel EditCantv
		{
			get;
			set;
		}

        public GetCneViewModel GetCne
		{
			get;
			set;
		}

		public NewCneViewModel NewCne
		{
			get;
			set;
		}

		public EditCneViewModel EditCne
		{
			get;
			set;
		}

		public NewIvssViewModel NewIvss
		{
			get;
			set;
		}

		public EditIvssViewModel EditIvss
		{
			get;
			set;
		}

		public GetIvssViewModel GetIvss
		{
			get;
			set;
		}

		public NewZoomViewModel NewZoom
		{
			get;
			set;
		}

		//public List<NationalityData> ListNationalityDatas
		//{
		//	get;
		//	set;
		//}

		#region Commands

		public ICommand GoNewCantvCommand => new RelayCommand(GoNewCantv);
		public ICommand GoNewCneCommand => new RelayCommand(GoNewCne);      
		public ICommand GoNewIvssCommand => new RelayCommand(GoNewIvss);
		public ICommand GoNewZoomCommand => new RelayCommand(GoNewZoom);

		#endregion Commands

		#endregion Properties

		#region Constructor

		public MainViewModel()
		{
			//  Get instance of MainViewModel
			instance = this;

			//  Gets an instance og the services class
			this.navigationService = new NavigationService();

			//  Se instancia la clase del LoginViewModel
			this.Login = new LoginViewModel();

			//  Load elements of menu
			this.LoadMenu();
		}      

		#endregion Constructor

		#region Methods

		//  Sigleton
		public static MainViewModel GetInstance()
		{
			if (instance == null)
			{
				return new MainViewModel();
			}
			return instance;
		}

		private void LoadMenu()
        {
			//  MyMenu = new ObservableCollection<Menu>();
			//  MyMenu = new ObservableCollection<MenuItemViewModel>();
			this.MyMenu = new ObservableCollection<MenuViewModel>();

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
                     
			//MyMenu.Add(new MenuItemViewModel
   //         {
   //             Icon = "ic_settings.png",
   //             PageName = "MyProfilePage",
   //             Title = "My Profile",
   //         });

			//MyMenu.Add(new MenuItemViewModel
            //{
            //    Icon = "ic_exit_to_app.png",
            //    PageName = "LoginPage",
            //    Title = "Close Sesion",
            //});

            MyMenu.Add(new MenuViewModel
            {
                Icon = "ic_settings.png",
                PageName = "MyProfilePage",
                Title = "My Profile",
            });

            MyMenu.Add(new MenuViewModel
            {
                Icon = "ic_exit_to_app.png",
                PageName = "LoginPage",
                Title = "Close Sesion",
            });
        }
              
		private async void GoNewCantv()
        {
			//  Gets an instance of the NewPhoneViewModel
			this.NewCantv = new NewCantvViewModel();

			//  Navigate to teh pag NewPhonePage
			await navigationService.NavigateOnMaster("NewCantvPage");
        }

		private async void GoNewCne()
        {
			//  Get an instance of the NewCneViewModel
			this.NewCne = new NewCneViewModel();

			//  Navigate to page NewCnePage
			await this.navigationService.NavigateOnMaster("NewCnePage");
        }

		private async void GoNewIvss()
        {
            //  Gets an instance of the NewIvssViewModel
            this.NewIvss = new NewIvssViewModel();

            //  Navigate to NewIvssPage
			await this.navigationService.NavigateOnMaster("NewIvssPage");
        }

		private async void GoNewZoom()
        {
            //  Gets an instance of tne NewZommViewModel
            this.NewZoom = new NewZoomViewModel();

            //  Navigate to the NewZoomPage
            await this.navigationService.NavigateOnMaster("NewZoomPage");         
        }

		#endregion Methods
	}
}