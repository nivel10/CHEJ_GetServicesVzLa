namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Helpers;
	using CHEJ_GetServicesVzLa.Models;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class CantvViewModel : BaseViewModel
	{
		#region Attributes
        
		private bool isRefreshing;
		private List<CantvDataItemViewModel> listCantvData;
		private ObservableCollection<CantvDataItemViewModel> cantvs;
		private static MainViewModel mainViewModel;
		private static CantvViewModel instance;      

		#region Services

		ApiService apiService;
		DialogService dialogService;

		#endregion Services
        
		#endregion Attributes
        
		#region Properties

		public bool IsRefreshing
		{
			get { return this.isRefreshing; }
			set { SetValue(ref this.isRefreshing, value); }
		}
        
		public ObservableCollection<CantvDataItemViewModel> Cantvs
		{
			get { return this.cantvs; }
			set { SetValue(ref this.cantvs, value); }
		}

		#region Commands

		public ICommand RefreshCommand
		{
			get
			{
				return new RelayCommand(LoadUserData);
			}
		}

		#endregion Commands

		#endregion Properties

		#region Constructor

		public CantvViewModel()
		{
			//  Gets an instance of the class
            instance = this;

			//  Gets an instance of the MainViewModel
            mainViewModel = MainViewModel.GetInstance();

			//  Optain an instance of service
			this.apiService = new ApiService();
			this.dialogService = new DialogService();
                     
			//  Load values in the object UserData
			this.LoadUserData();
		}

		#endregion Constructor

		#region Methods

		private async void LoadUserData()
		{         
			//  Establishes the status of controls
			this.SetStatusControls(true);

			//  Validate if there is an internet connection
			var response = await this.apiService.CheckConnection();
			if (!response.IsSuccess)
			{
				//  Establece el estatus de los controles
				this.SetStatusControls(false);

				await this.dialogService.ShowMessage(
					"Error",
					response.Message,
					"Accept");
				return;
			}

			//  Get data of the user
            response = await this.apiService.Get<UserDataResponse>(
				MethodsHelper.GetUrlAPI(),
                "/api/Users",
                "/GetServicesVzLaUSerByEmail",
                string.Format(
					"/?email={0}",
                    mainViewModel.Token.UserName),
                    mainViewModel.Token.TokenType,
                    mainViewModel.Token.AccessToken);
            if (!response.IsSuccess)
            {
                //  Establishes the status of controls
                this.SetStatusControls(false);

                await this.dialogService.ShowMessage(
					"Error",
                    response.Message,
                    "Accept");
                return;
            }
            
			this.LoadOfValueUserData((UserDataResponse)response.Result);
			this.listCantvData = 
				this.ToListCantvDataItemViewModel(
					mainViewModel.UserData.CantvDatas);

			this.Cantvs = new ObservableCollection<CantvDataItemViewModel>(
				this.listCantvData);
			
			//  Establishes the status of controls
			this.SetStatusControls(false);         
		}

		private void LoadOfValueUserData(UserDataResponse _userDataResponse)
		{
			//  Get new instance of UserDataResponse         
            mainViewModel.UserData = new UserDataResponse();
			mainViewModel.UserData.AppName = _userDataResponse.AppName;
			mainViewModel.UserData.CantvDatas = _userDataResponse.CantvDatas;
			mainViewModel.UserData.CneIvssDatas = _userDataResponse.CneIvssDatas;
			mainViewModel.UserData.Email = _userDataResponse.Email;
			mainViewModel.UserData.FirstName = _userDataResponse.FirstName;
			mainViewModel.UserData.ImageArray = _userDataResponse.ImageArray;
			mainViewModel.UserData.ImagePath = _userDataResponse.ImagePath;
			mainViewModel.UserData.LastName = _userDataResponse.LastName;
			mainViewModel.UserData.Password = _userDataResponse.Password;
			mainViewModel.UserData.Telephone = _userDataResponse.Telephone;
			mainViewModel.UserData.UserId = _userDataResponse.UserId;
			mainViewModel.UserData.UserTypeId = _userDataResponse.UserTypeId;
			mainViewModel.UserData.ZoomDatas = _userDataResponse.ZoomDatas;         
		}

		private List<CantvDataItemViewModel> ToListCantvDataItemViewModel(
			List<CantvData> _cantvDatas)
        {
			var cantvData = new List<CantvDataItemViewModel>();
			foreach (var _cantvData in _cantvDatas
			         .OrderBy(cd => cd.CodePhone)
			         .ThenBy(cd => cd.NumberPhone))
			{
				cantvData.Add(new CantvDataItemViewModel
				{  CantvDataId = _cantvData.CantvDataId,
					CodePhone = _cantvData.CodePhone,
					NumberPhone = _cantvData.NumberPhone,
				});
			}
            
			return cantvData;
		}
        
		private CantvDataItemViewModel ToCantvDataItemViewModel(
			CantvData _cantvData)
        {
            var cantvDataItemViewModel = new CantvDataItemViewModel();
                     
			cantvDataItemViewModel.CantvDataId = _cantvData.CantvDataId;
			cantvDataItemViewModel.CodePhone = _cantvData.CodePhone;
			cantvDataItemViewModel.NumberPhone = _cantvData.NumberPhone;
                     
			return cantvDataItemViewModel;
        }

		public void UpdateCantvData(int _option, CantvData _cantvData)
        {
            this.SetStatusControls(true);
            
			var cantvDataItemViewModel = ToCantvDataItemViewModel(_cantvData);
            if(_option == -1)
            {
				listCantvData.Remove(cantvDataItemViewModel);
            }
            else if(_option == 0)
            {
				var oldCantvData = listCantvData
					.Where(cd => cd.CantvDataId == _cantvData.CantvDataId)
					.FirstOrDefault();
				
				oldCantvData = cantvDataItemViewModel;
            }
            else if(_option == 1)
            {
				listCantvData.Add(ToCantvDataItemViewModel(cantvDataItemViewModel));
            }

			this.Cantvs.Clear();

			this.Cantvs = new ObservableCollection<CantvDataItemViewModel>(
				listCantvData
				.OrderBy(lcd => lcd.CodePhone)
				.ThenBy(lcd => lcd.NumberPhone)); 
			
            this.SetStatusControls(false);
        }

		public static CantvViewModel GetInstance()
		{
            if(instance == null)
            {
				return new CantvViewModel();
            }
            return instance;
		}

		private void SetStatusControls(bool _isRefreshing)
		{
			this.IsRefreshing = _isRefreshing;
		}
        
		#endregion Methods
	}
}