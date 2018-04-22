namespace CHEJ_GetServicesVzLa.ViewModels
{
    public class MainViewModel
    {
		#region Properties
              
		public LoginViewModel Login
		{
			get;
			set;
		}

		#endregion Properties

		#region Constructor

		public MainViewModel()
		{
			//  Se instancia la clase del LoginViewModel
			Login = new LoginViewModel();
			
		}

		#endregion Constructor
    }
}
