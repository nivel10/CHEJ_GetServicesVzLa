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
		private NavigationService navigationSerive;      

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

		//public List<NationalityData> ListNationalityDatas
		//{
		//	get;
		//	set;
		//}

		#region Commands

		public ICommand GoNewCantvCommand
		{
			get { return new RelayCommand(GoNewCantv); }
		}

		public ICommand GoNewCneCommand
		{
			get { return new RelayCommand(GoNewCne); }
		}

		public GetCneViewModel GetCne { get; internal set; }

        #endregion Command#endregion Commands

		#endregion Properties

		#region Constructor

		public MainViewModel()
		{
			//  Get instance of MainViewModel
			instance = this;

			//  Gets an instance og the services class
			this.navigationSerive = new NavigationService();

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
			await navigationSerive.NavigateOnMaster("NewCantvPage");
        }

		private async void GoNewCne()
        {
			//  Get an instance of the NewCneViewModel
			this.NewCne = new NewCneViewModel();

			//  Navigate to page NewCnePage
			await this.navigationSerive.NavigateOnMaster("NewCnePage");
        }

		#endregion Methods
	}
}