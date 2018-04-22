namespace CHEJ_GetServicesVzLa.ViewModels
{
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

		#endregion Properties

		#region Constructor

		public MainViewModel()
		{
			//  Get instance of MainViewModel
			instance = this;

			//  Se instancia la clase del LoginViewModel
			Login = new LoginViewModel();
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

		#endregion Methods
    }
}