namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Helpers;
	using CHEJ_GetServicesVzLa.Models;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class CantvDataItemViewModel : CantvData
    {

		#region Attributes

		private ApiService apiService;
		private DialogService dialogService;
		private NavigationService navigationService;
		private MainViewModel mainViewModel;
		private CantvViewModel cantvViewModel;

		#endregion Attributes

		#region Properties

		public ICommand GetCantvCommand
		{
			get { return new RelayCommand(GetCantv); }
		}

        public ICommand EditCommand
        {
            get { return new RelayCommand(Edit); }
        }

        public ICommand DeleteCommand
        {
            get { return new RelayCommand(Delete); }
        }

        #endregion Properties

		#region Constructor
        
		public CantvDataItemViewModel()
		{
			this.cantvViewModel = CantvViewModel.GetInstance();
			this.mainViewModel = MainViewModel.GetInstance();

			//  Generate an instance of the services class
			this.apiService = new ApiService();
			this.dialogService = new DialogService();
			this.navigationService = new NavigationService();
		}

		#endregion Constructor

		#region Methods

		private async void GetCantv()
		{
			//  Generate an intsntance of the GetCantvViewModel         
			this.mainViewModel.GetCantv = new GetCantvViewModel(this);

			//  Navigate to de page QueryCantvPage
			await this.navigationService.NavigateOnMaster("GetCantvPage");
		}

		private async void Delete()
		{
			//  Validate connection to internet
			var response = await this.apiService.CheckConnection();
			if (!response.IsSuccess)
			{
				await this.dialogService.ShowMessage(
					"Error",
					response.Message,
					"Accept");
				return;
			}

			//  Generate an object CantvDatas
			var cantvData = new CantvDataItem
			{
				CantvDataId = this.CantvDataId,
				CodePhone = this.CodePhone,
				NumberPhone = this.NumberPhone,            
				UserId = this.mainViewModel.UserData.UserId,
			};
                     
			response = await this.apiService.Delete<CantvDataItem>(
				MethodsHelper.GetUrlAPI(),
				"/api",
				"/CantvDatas",
				this.mainViewModel.Token.TokenType,
				this.mainViewModel.Token.AccessToken,
				cantvData);
			if (!response.IsSuccess)
			{
				await this.dialogService.ShowMessage(
					"Error",
					response.Message,
					"Accept");
				return;
			}

			//  Invokate the method that removes a CantvDatas
			cantvData.UserId = 0;         
			this.cantvViewModel.UpdateCantvData(
				-1, 
				this.ToCantvDataItemViewModel(cantvData));

			await this.dialogService.ShowMessage(
				"Information",
				string.Format(
					"Record: {0} remove successfully...!!!",
					cantvData.GetFullCantvData),
				"Accept");
		}

		private async void Edit()
		{
			//  Gets a new instance of EditViewModel
			this.mainViewModel.EditCantv = 
				new EditCantvViewModel(this);

			//  Navigate to page EditViewModel
			await this.navigationService.NavigateOnMaster("EditCantvPage");
		}

		private CantvDataItemViewModel ToCantvDataItemViewModel(
			CantvData _cantvData)
		{
			return new CantvDataItemViewModel
			{
				CantvDataId = _cantvData.CantvDataId,
                CodePhone = _cantvData.CodePhone,
                NumberPhone = _cantvData.NumberPhone,
			};
        }

		#endregion Methods
	}
}