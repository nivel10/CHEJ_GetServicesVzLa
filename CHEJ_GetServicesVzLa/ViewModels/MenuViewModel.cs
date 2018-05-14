namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System.Windows.Input;
    using CHEJ_GetServicesVzLa.Models;
    using CHEJ_GetServicesVzLa.Services;
    using GalaSoft.MvvmLight.Command;
      
    public class MenuViewModel : Menu
    {

        #region Attrbutes

        #region Services

		private NavigationService navigationService;

		#endregion Services

		private MainViewModel mainViewModel;

		#endregion Attributes

		#region Properties

		#region Commands

		public ICommand NavigateCommand => new RelayCommand(Navigate);
              
        #endregion Commands

        #endregion Properties

        #region Constructor

        public MenuViewModel()
        {
			//  Gets an instance of the ViewModels
			this.mainViewModel = MainViewModel.GetInstance();
            //  Intance the class of services
            navigationService = new NavigationService();
        }

        #endregion Constructor

        #region Methods

        private async void Navigate()
        {
            switch (PageName)
            {
                case "LoginPage":
					this.mainViewModel.Login = new LoginViewModel();
                    this.navigationService.SetMainPage(PageName);
                    break;

                case "MyProfilePage":
					this.mainViewModel.MyProfile = new MyProfileViewModel();
                    await this.navigationService.NavigateOnMaster(PageName);
                    break;
            }
        }

        #endregion Methods
              
    }
}