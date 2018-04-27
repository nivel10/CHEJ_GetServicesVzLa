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
		//  private ObservableCollection<CantvData> cantvDatas;
		private ObservableCollection<CantvDataItemViewModel> cantvDatas;

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

		//  public ObservableCollection<CantvData> CantvDatas
		public ObservableCollection<CantvDataItemViewModel> CantvDatas
		{
			get { return this.cantvDatas; }
			set { SetValue(ref this.cantvDatas, value); }
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
			SetStatusControl(true);

			//  Validate if there is an internet connection
			var response = await this.apiService.CheckConnection();
			if (!response.IsSuccess)
			{
				//  Establece el estatus de los controles
				SetStatusControl(false);

				await this.dialogService.ShowMessage(
					"Error",
					response.Message,
					"Accept");
				return;
			}

			//  Get data of the user
			response = await this.apiService.GetList<UserDataResponse>(
				MethodsHelper.GetUrlAPI(),
				"/api/Users",
				"/GetServicesVzLaUSerByEmail",
				string.Format(
					"/?email={0}",
					MainViewModel.GetInstance().Token.UserName),
				MainViewModel.GetInstance().Token.TokenType,
				MainViewModel.GetInstance().Token.AccessToken);
			if (!response.IsSuccess)
			{
				//  Establishes the status of controls
				SetStatusControl(false);

				await this.dialogService.ShowMessage(
					"Error",
					response.Message,
					"Accept");
				return;
			}

            //  Get new instance of UserDataResponse
			MainViewModel.GetInstance().UserData = new UserDataResponse();

			//  Assing the data to the UserData
			SetValueUserData(
				(List<UserDataResponse>)response.Result,
				MainViewModel.GetInstance().UserData);
   
			//this.CantvDatas = new ObservableCollection<CantvData>(
				//MainViewModel.GetInstance().UserData.CantvDatas);
			this.CantvDatas = new ObservableCollection<CantvDataItemViewModel>(
				ToCantvDataItemViewModel(MainViewModel.GetInstance().UserData.CantvDatas));
			         
			//  Establishes the status of controls
			SetStatusControl(false);         
		}

		private IEnumerable<CantvDataItemViewModel> ToCantvDataItemViewModel(
			List<CantvData> _cantvDatas)
		{
			return _cantvDatas.Select(lst => new CantvDataItemViewModel
			{
				CantvDataId = lst.CantvDataId,
				CodePhone = lst.CodePhone,
				NumberPhone = lst.NumberPhone,
			});
		}

		private void SetValueUserData(
			List<UserDataResponse>
			_result, UserDataResponse _userData)
		{
			foreach (var item in _result)
			{
				_userData.AppName = item.AppName;
				_userData.CantvDatas = new List<CantvData>(item.CantvDatas);
				_userData.CneIvssDatas = new List<CneIvssData>(item.CneIvssDatas);
				_userData.Email = item.Email;
				_userData.FirstName = item.FirstName;
				_userData.FullName = item.FullName;
				_userData.ImageArray = item.ImageArray;
				_userData.ImagePath = item.ImagePath;
				_userData.ImageFullPath = item.ImageFullPath;
				_userData.LastName = item.LastName;
				_userData.Password = item.Password;
				_userData.Telephone = item.Telephone;
				_userData.UserId = item.UserId;
				_userData.UserTypeId = item.UserTypeId;
				_userData.ZoomDatas = new List<ZoomData>(item.ZoomDatas);
			}
		}      

		private void SetStatusControl(bool _isRefreshing)
		{
			this.IsRefreshing = _isRefreshing;
		}

		#endregion Methods
	}
}