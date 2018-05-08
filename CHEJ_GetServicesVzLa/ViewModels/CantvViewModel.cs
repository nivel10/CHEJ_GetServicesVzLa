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
		private List<CantvItemViewModel> listCantvs;
		private ObservableCollection<CantvItemViewModel> cantvs;
		private List<CneItemViewModel> listCnes;
		private ObservableCollection<CneItemViewModel> cnes;
		private List<IvssItemViewModel> lisIvsses;
		private ObservableCollection<IvssItemViewModel> ivsses;
		private List<ZoomItemViewModel> listZooms;
		private ObservableCollection<ZoomItemViewModel> zooms;
		private MainViewModel mainViewModel;
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
        
		public ObservableCollection<CantvItemViewModel> Cantvs
		{
			get { return this.cantvs; }
			set { SetValue(ref this.cantvs, value); }
		}

		public ObservableCollection<CneItemViewModel> Cnes
		{
			get { return this.cnes; }
			set { SetValue(ref this.cnes, value); }
		}
        
		public ObservableCollection<IvssItemViewModel> Ivsses
		{
			get { return this.ivsses; }
			set { SetValue(ref this.ivsses, value); }
		}

		public ObservableCollection<ZoomItemViewModel> Zooms
		{
			get { return this.zooms; }
			set { SetValue(ref this.zooms, value); }
		}

		#region Commands

		public ICommand RefreshCommand
		{
			get { return new RelayCommand(LoadUserData); }
		}

		#endregion Commands

		#endregion Properties

		#region Constructor

		public CantvViewModel()
		{
			//  Gets an instance of the class
            instance = this;

			//  Gets an instance of the MainViewModel
            this.mainViewModel = MainViewModel.GetInstance();

			//  Optain an instance of service
			this.apiService = new ApiService();
			this.dialogService = new DialogService();
                     
			//  Load values in the object UserData
			this.LoadUserData();
		}

		#endregion Constructor

		#region Methods

		public async void LoadUserData()
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

			this.listCantvs = 
				new List<CantvItemViewModel>(this.ToListCantvItemViewModel(
					this.mainViewModel.UserData.CantvDatas));
			
			//  Load valuen in the ObservableCollection
			this.Cantvs = new ObservableCollection<CantvItemViewModel>(
				this.listCantvs);

			//  Select only data of cne
			this.listCnes = 
				new List<CneItemViewModel>(this.ToListCneItemViewModel(
					mainViewModel.UserData.CneIvssDatas));

            //  Load valuen in the ObservableCollection
            this.Cnes = new ObservableCollection<CneItemViewModel>(
                this.listCnes);

			//  Select only data on ivss
			this.lisIvsses = new List<IvssItemViewModel>(
				ToListIvssItemViewModel(
					this.mainViewModel.UserData.CneIvssDatas));

			//  Load values in the ObservableCollection
			this.Ivsses = new ObservableCollection<IvssItemViewModel>(
				this.lisIvsses);
                         
			//  Select data of teh zoom
			this.listZooms = new List<ZoomItemViewModel>(
				ToZoomItemViewModel(this.mainViewModel.UserData.ZoomDatas));

			//  Load value in the ObservabeCollection
			this.Zooms = new ObservableCollection<ZoomItemViewModel>(
				this.listZooms.OrderBy(lz => lz.Tracking));
			
			//  Establishes the status of controls
			this.SetStatusControls(false);         
		}

		private List<ZoomItemViewModel> 
		ToZoomItemViewModel(List<ZoomData> _zoomDatas)
		{
			var listZoomItemViewModel = new List<ZoomItemViewModel>();

			foreach (var _zoomData in _zoomDatas)
			{
				listZoomItemViewModel.Add(new ZoomItemViewModel 
				{ 
					Tracking = _zoomData.Tracking,
					ZoomDataId = _zoomData.ZoomDataId,
				});
			}

			return listZoomItemViewModel;
		}

		private List<IvssItemViewModel> ToListIvssItemViewModel(
			List<CneIvssData> _cneIvssDatas)
		{
			var listIvss = new List<IvssItemViewModel>();

			foreach (var _cneIvssData in _cneIvssDatas
			         .Where(cid => cid.IsIvss == true)
			         .OrderBy(cid => cid.NationalityDatas.First().Abbreviation)
			         .ThenBy(cid => cid.IdentificationCard))
			{
				listIvss.Add(new IvssItemViewModel
				{
					BirthDate = _cneIvssData.BirthDate,
					CneIvssDataId = _cneIvssData.CneIvssDataId,
					IdentificationCard = _cneIvssData.IdentificationCard,
					IsCne = _cneIvssData.IsCne,
					IsIvss = _cneIvssData.IsIvss,
					NationalityDatas = _cneIvssData.NationalityDatas,
					NationalityId = _cneIvssData.NationalityId,
				});
			}

			return listIvss;
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
              
		private List<CneItemViewModel> ToListCneItemViewModel(
			List<CneIvssData> _listCnes)
		{
			var listCneIvssItem = new List<CneItemViewModel>();
			foreach (var _listCneIvssData in _listCnes
			         .Where(cne => cne.IsCne == true)
			         .OrderBy(cne => cne.NationalityDatas.First().Abbreviation)
			         .ThenBy(cne => cne.IdentificationCard))
			{
				listCneIvssItem.Add(new CneItemViewModel
				{  
					BirthDate = _listCneIvssData.BirthDate,
					CneIvssDataId = _listCneIvssData.CneIvssDataId,
					IdentificationCard = _listCneIvssData.IdentificationCard,
					IsCne = _listCneIvssData.IsCne,
					IsIvss = _listCneIvssData.IsIvss,
					NationalityDatas = _listCneIvssData.NationalityDatas,
					NationalityId = _listCneIvssData.NationalityId,
				});
			}
            
			return listCneIvssItem;
		}

		private List<CantvItemViewModel> ToListCantvItemViewModel(
			List<CantvData> _cantvDatas)
        {
			var listCantvItem = new List<CantvItemViewModel>();
			foreach (var _cantvData in _cantvDatas
			         .OrderBy(cd => cd.CodePhone)
			         .ThenBy(cd => cd.NumberPhone))
			{
				listCantvItem.Add(new CantvItemViewModel
				{  CantvDataId = _cantvData.CantvDataId,
					CodePhone = _cantvData.CodePhone,
					NumberPhone = _cantvData.NumberPhone,
				});
			}
            
			return listCantvItem;
		}

		public void UpdateCneData(
			int _option, 
			CneItemViewModel _cneItemViewModel)
        {
			this.SetStatusControls(true);

			//  Opti an list of the CneIvssData
			var oldListCne = this.listCnes
								 .Where(cid => cid.CneIvssDataId == _cneItemViewModel.CneIvssDataId)
								 .FirstOrDefault();
            switch (_option)
            {
                case -1:
					this.listCnes.Remove(_cneItemViewModel);
                    break;
                case 0:
					oldListCne = _cneItemViewModel;
                    break;
                case 1:
					this.listCnes.Add(_cneItemViewModel);
                    break;               
            }

			//this.Cnes.Clear();

			//this.Cnes = new ObservableCollection<CneItemViewModel>(
			//this.listCnes
			//.OrderBy(cne => cne.NationalityDatas.First().Abbreviation)
			//.ThenBy(cne => cne.IdentificationCard));

			LoadUserData();

			this.SetStatusControls(false);
        }
     
		public void UpdateCantvData(
			int _option, 
			CantvItemViewModel _cantvItemViewModel)
        {
            this.SetStatusControls(true);         

			var oldListCantv = this.listCantvs
                   .Where(
                       cd => cd.CantvDataId == _cantvItemViewModel.CantvDataId)
                   .FirstOrDefault();

			switch(_option)
			{
				case -1:
					this.listCantvs.Remove(oldListCantv);
					break;
				case 0:
					oldListCantv = _cantvItemViewModel;
					break;
				case 1:
					this.listCantvs.Add(_cantvItemViewModel);
					break;
			}
            
            this.Cantvs = new ObservableCollection<CantvItemViewModel>(
                this.listCantvs
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