namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System;
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Helpers;
	using CHEJ_GetServicesVzLa.Models;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class IvssItemViewModel : CneIvssData
    {
		#region Attributes
        
		private ApiService apiService;
		private DialogService dialogService;
		private NavigationService navigationService;

		private MainViewModel mainViewModel;
		private CantvViewModel cantvViewModel;

		#endregion Attributes

		#region Properties

		#region Commands      

		public ICommand EditCommand
		{
			get { return new RelayCommand(Edit); }
		}
        
		public ICommand DeleteCommand
        {
			get { return new RelayCommand(Delete); }
        }
        
		public ICommand GetIvssCommand
		{
			get { return new RelayCommand(GetIvss); }         
		}

		#endregion Commands

		#endregion Properties

		#region Constructor

		public IvssItemViewModel()
		{
			//  Gets an instance of the services class
			this.apiService = new ApiService();
			this.dialogService = new DialogService();
			this.navigationService = new NavigationService();

			//  Gets an instance of the MainViewModel
			this.mainViewModel = MainViewModel.GetInstance();
			this.cantvViewModel = CantvViewModel.GetInstance();
		}

		#endregion Constructor

		#region Methods

		private async void Edit()
        {
			//  Gets an instance of the EditViewModel
			this.mainViewModel.EditIvss = new EditIvssViewModel(this);

			//  Navigate to page EditIvssPage
			await this.navigationService.NavigateOnMaster("EditIvssPage");
        }

		private async void Delete()
        {
            if (await this.dialogService.ShowMessageConfirm(
                "Infomation",
                "Are you sure delete this record...?",
                "Yes",
                "No"))
            {
                //  Check the connections to internet
                var response = await this.apiService.CheckConnection();
                if (!response.IsSuccess)
                {
                    await this.dialogService.ShowMessage(
                        "Error",
                        response.Message,
                        "Accept");
                    return;
                }

                //  Generate an object
                var cneIvssDataItem = new CneIvssDataItem
                {
                    BirthDate = this.BirthDate,
                    CneIvssDataId = this.CneIvssDataId,
                    IdentificationCard = this.IdentificationCard,
                    IsCne = this.IsCne,
                    IsIvss = this.IsIvss,
                    NationalityDatas = this.NationalityDatas,
                    NationalityId = this.NationalityId,
                    UserId = this.mainViewModel.UserData.UserId,
                };

                //  Delete the record            
                response = await this.apiService.Post<CneIvssDataItem>(
                    MethodsHelper.GetUrlAPI(),
                    "/api",
                    "/CneIvssDatas/PostCneIvssDataByOption/?_option=ivss",
                    this.mainViewModel.Token.TokenType,
                    this.mainViewModel.Token.AccessToken,
                    cneIvssDataItem);
                if (!response.IsSuccess)
                {
                    await this.dialogService.ShowMessage(
                        "Error",
                        response.Message,
                        "Accept");
                    return;
                }

                //  Delete record
				this.cantvViewModel.LoadUserData();

                await dialogService.ShowMessage(
                    "Infromation",
                    string.Format(
                        "Record: {0} remove successfully...!!!",
                        cneIvssDataItem.GetCneIvssFull),
                    "Accept");
            }
        }
        
		private async void GetIvss()
        {
			//  Gets an instance of the GetIvssPage
            this.mainViewModel.GetIvss = new GetIvssViewModel(this);

            //  Navigate to page GetIvssPage
            await navigationService.NavigateOnMaster("GetIvssPage");
        }

		#endregion Methods
    }
}