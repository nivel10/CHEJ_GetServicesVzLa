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

        public ObservableCollection<Menu> MyMenu
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
			MyMenu = new ObservableCollection<Menu>();

			MyMenu.Add(new Menu
			{
				Icon = "ic_settings.png",
				PageName = "MyProfilePage",
                Title = "My Profile",
			});

			MyMenu.Add(new Menu
			{
				Icon = "ic_exit_to_app.png",
				PageName = "Loginage",
				Title = "Close Sesion",
			});         
        }

		#endregion Methods
    }
}