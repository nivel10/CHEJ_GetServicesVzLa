

using System.Windows.Input;
using CHEJ_GetServicesVzLa.Models;
using CHEJ_GetServicesVzLa.Services;
using GalaSoft.MvvmLight.Command;

namespace CHEJ_GetServicesVzLa.ViewModels
{

    public class MenuViewModel : Menu
    {
		//#region Constructor

		//public MenuViewModel()
		//{
		//}

		//#endregion Constructor

        #region Attrbutes

        private NavigationService navigationService;

        #endregion Attributes

        #region Properties

        #region Commands

        public ICommand NavigateCommand
        {
            get { return new RelayCommand(Navigate); }
        }

        #endregion Commands

        #endregion Properties

        #region Constructor

        public MenuViewModel()
        {
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
                    MainViewModel.GetInstance().Login = new LoginViewModel();
                    navigationService.SetMainPage(PageName);
                    break;

                case "MyProfilePage":
                    MainViewModel.GetInstance().MyProfile =
                        new MyProfileViewModel();
                    await navigationService.NavigateOnMaster(PageName);
                    break;
            }
        }

        #endregion Methods
              
    }
}
