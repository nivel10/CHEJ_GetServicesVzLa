namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System;
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Models;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class ZoomItemViewModel : ZoomData
    {
		private NavigationService navigationService;
		private MainViewModel mainViewModel;

		#region Properties

		#region Commands
              
		public ICommand EditCommand => new RelayCommand(Edit);
		public ICommand DeleteCommand => new RelayCommand(Delete);
		public ICommand GetZoomCommand => new RelayCommand(GetZoom);

		#endregion Commands

		#endregion Properties
        
		public ZoomItemViewModel()
        {
			//  Gets an intance of the service class
			navigationService = new NavigationService();

			//  Gets an instance of the sigleton
			this.mainViewModel = MainViewModel.GetInstance();
        }

		private void GetZoom()
        {
            throw new NotImplementedException();
        }

        private void Delete()
        {
            throw new NotImplementedException();
        }

        private void Edit()
        {
            throw new NotImplementedException();
        }
    }
}