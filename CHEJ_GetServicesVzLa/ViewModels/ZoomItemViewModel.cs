namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System;
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Helpers;
	using CHEJ_GetServicesVzLa.Models;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class ZoomItemViewModel : ZoomData
    {
		#region Attributes

		#region Services

		private ApiService apiService;
		private DialogService dialogService;
		private NavigationService navigationService;

		#endregion Services

		private MainViewModel mainViewModel;
		private CantvViewModel cantvViewModel;

		#endregion Attributes

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
			apiService = new ApiService();
			dialogService = new DialogService();
			navigationService = new NavigationService();
            
			//  Gets an instance of the sigleton
			this.mainViewModel = MainViewModel.GetInstance();
			this.cantvViewModel = CantvViewModel.GetInstance();
        }

		private async void GetZoom()
        {
			//  Gets an instance of the GetZoom
			this.mainViewModel.GetZoom = new GetZoomViewModel(this);

			//  Navigate to the page GetZoomPage
			await navigationService.NavigateOnMaster("GetZoomPage");
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
				var zoomDataItem = new ZoomDataItem
                {
					Tracking = this.Tracking,
					UserId = this.mainViewModel.UserData.UserId,
					ZoomDataId = this.ZoomDataId,
                };

                //  Delete the record            
				response = await this.apiService.Delete<ZoomDataItem>(
                    MethodsHelper.GetUrlAPI(),
                    "/api",
                    "/ZoomDatas",
                    this.mainViewModel.Token.TokenType,
                    this.mainViewModel.Token.AccessToken,
                    zoomDataItem);
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
                        zoomDataItem.Tracking),
                    "Accept");
            }
        }

        private async void Edit()
        {
			//  Gets an instance of the EditZoomViewModel
			this.mainViewModel.EditZoom = new EditZoomViewModel(this);

			//  Navigate to the page EditZoomPage
			await navigationService.NavigateOnMaster("EditZoomPage");
        }
    }
}