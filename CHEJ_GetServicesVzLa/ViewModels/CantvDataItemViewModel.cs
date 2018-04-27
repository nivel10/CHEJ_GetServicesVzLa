namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Models;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class CantvDataItemViewModel : CantvData
    {

		#region Attributes

		private ApiService apiService;
		private DialogService dialogService;
		private NavigationService navigationService;

		#endregion Attributes

		#region Properties

		public ICommand GetCantvCommand
		{
			get { return new RelayCommand(GetCantv); }
		}

		#endregion Properties
        
        public CantvDataItemViewModel()
		{
			//  Generate an instance of the services class
			apiService = new ApiService();
			dialogService = new DialogService();
			navigationService = new NavigationService();
		}

		private async void GetCantv()
		{
			//  Generate an intsntance of the QueryCantvViewModel
			MainViewModel.GetInstance().QueryCantv = new QueryCantvViewModel(this);

			//  Navigate to de page QueryCantvPage
			await navigationService.NavigateOnMaster("QueryCantvPage");
		}
	}
}