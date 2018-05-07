namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System;
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Models;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class CneItemViewModel : CneIvssData
    {
		#region Attributes

		private MainViewModel mainViewModel;

		#region Services

		private ApiService apiService;
		private DialogService dialogService;
		private NavigationService navigationService;

		#endregion

		#endregion Attributes

		#region Properties

		public ICommand GoNewCneCommand
		{
			get { return new RelayCommand(GoNewCne); }
		}

		public ICommand EditCommand
		{
			get { return new RelayCommand(Edit); }
		}

		public ICommand DeleteCommand
		{
			get { return new RelayCommand(Delete); }
		}

		public ICommand GetCneCommand
		{
			get { return new RelayCommand(GetCne); }
		}

		#endregion Properties

		#region Constructor

		public CneItemViewModel()
		{
			//  Gets an instance of the MainViewModel
			this.mainViewModel = MainViewModel.GetInstance();

			//  Gets an instance of the service class
			apiService = new ApiService();
			dialogService = new DialogService();
			navigationService = new NavigationService();
		}

		#endregion Constructor

		#region Methods

		private async void Delete()
		{
			if(await this.dialogService.ShowMessageConfirm(
				"Infomation", 
				"Are you sure delete this record...?", 
				"Yes", 
				"No"))
			{
				
			}
		}

		private async void GoNewCne()
		{
			//  Gets an instance of the NewCne
			this.mainViewModel.NewCne = new NewCneViewModel();

			//  Navigate to the page NewCnePage
			await this.navigationService.NavigateOnMaster("NewCnePage");
		}

		private async void Edit()
		{
			//  Gets an instance of the EditCneViewModel
			this.mainViewModel.EditCne = new EditCneViewModel(this);

			//  Navigate to the page EditCneViewModel
			await this.navigationService.NavigateOnMaster("EditCnePage");
		}
        
		private async void GetCne()
		{
			//  Gets an instance of the GetCnePage
			this.mainViewModel.GetCne = new GetCneViewModel(this);

			//  Navigate to page GetCnePage
			await navigationService.NavigateOnMaster("GetCnePage");         
        }

		#endregion Methods
    }
}