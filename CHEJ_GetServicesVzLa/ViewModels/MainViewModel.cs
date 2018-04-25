namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System.Collections.ObjectModel;
	using CHEJ_GetServicesVzLa.Models;

	public class MainViewModel
    {
		#region Attrbutes

		private static MainViewModel instance;

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

		#endregion Properties

		#region Constructor

		public MainViewModel()
		{
			//  Get instance of MainViewModel
			instance = this;
            
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

		#endregion Methods
    }
}