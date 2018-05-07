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

	public class EditCneViewModel : BaseViewModel
    {
		#region Attributes

		#region Services

		private ApiService apiService;
		private DialogService dialogService;
		private NavigationService navigationService;

		#endregion Services

		private List<NationalityData> listNationalities;
		private int nationalityId;
		private string identificationCard;
		private bool isEnabled;
		private bool isRunning;
		private string messageLabel;
		private MainViewModel mainViewModel;
		private CneItemViewModel cneItemViewModel;
		private CantvViewModel cantvViewModel;
		private ObservableCollection<NationalityData> nationalities;

		#endregion Attributes

		#region Properties

		public int NationalityId
		{
			get { return this.nationalityId; }
			set { SetValue(ref this.nationalityId, value); }
		}

		public string IdentificationCard
		{
			get { return this.identificationCard; }
			set { SetValue(ref this.identificationCard, value); }
		}

		public ObservableCollection<NationalityData> Nationalities
		{
			get { return this.nationalities; }
			set { SetValue(ref this.nationalities, value); }
		}

		public bool IsEnabled
		{
			get { return this.isEnabled; }
			set { SetValue(ref this.isEnabled, value); }
		}

		public bool IsRunning
		{
			get { return this.isRunning; }
			set { SetValue(ref this.isRunning, value); }
		}

		public string MessageLabel
		{
			get { return this.messageLabel; }
			set { SetValue(ref this.messageLabel, value); }
		}
        
		public ICommand GoBackCommand
		{
			get { return new RelayCommand(GoBack); }         
		}

		public ICommand SaveCommand
		{
			get { return new RelayCommand(Save); }         
		}
        
		#endregion Properties

		#region Constructor

		public EditCneViewModel(CneItemViewModel _cneItemViewModel)
		{
			//  Gets an intance of the services class
			this.apiService = new ApiService();
			this.dialogService = new DialogService();
			this.navigationService = new NavigationService();

			//  Capture the instance of the CneItemViewModel
			this.cneItemViewModel = _cneItemViewModel;
			this.mainViewModel = MainViewModel.GetInstance();
			this.cantvViewModel = CantvViewModel.GetInstance();

			//  Invoke the method of load values CNE
			this.LoadData();
		}

		#endregion Constructor

		#region Methods

		private async void LoadData()
		{
			//  Set status of controls
			this.SetStatusControl(false, true, 1);

			//  Validate the connections of internet
			var response = await this.apiService.CheckConnection();
			if (!response.IsSuccess)
			{
				//  Set status of controls
				this.SetStatusControl(true, false, 0);
				await this.dialogService.ShowMessage(
					"Error",
					response.Message,
					"Accept");
				return;
			}
            
			//  Find the values of the Nationality
			response = await this.apiService.GetList<NationalityData>(
				MethodsHelper.GetUrlAPI(),
				"/api",
				"/Nationalities",
				"",
				this.mainViewModel.Token.TokenType,
				this.mainViewModel.Token.AccessToken);
			if (!response.IsSuccess)
			{
				//  Set status of controls
				this.SetStatusControl(true, false, 0);
				await this.dialogService.ShowMessage(
					"Error",
					response.Message,
					"Accept");
				return;
			}

			//  
			this.listNationalities = 
				new List<NationalityData>((List<NationalityData>)response.Result);

			//  Load to values the ObservableCollection
			this.Nationalities =
				new ObservableCollection<NationalityData>(
					    listNationalities.OrderBy(n => n.NationalityId));

			//  Load the values in the fields
			this.NationalityId = this.cneItemViewModel.NationalityId;
			this.IdentificationCard = this.cneItemViewModel.IdentificationCard;

			//  Set status of controls
			this.SetStatusControl(true, false, 0);
		}

		private async void GoBack()
		{
			//  Navigate on back
			await this.navigationService.GoBackOnMaster();
		}

		private async void Save()
        {
            //  Validate the fields of the form
            if (this.NationalityId == 0)
            {
                await this.dialogService.ShowMessage(
                    "Error",
                    "You must select an nationality...!!!",
                    "Accept");
                return;
            }

            var response = MethodsHelper.IsValidField(
                "I",
                8,
                8,
                "identification card",
                this.IdentificationCard,
                true,
                false,
                string.Empty);
            if (!response.IsSuccess)
            {
                await this.dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            //  Define the status of the controls
            this.SetStatusControl(false, true, 1);

            //  Check the connection internet
            response = await this.apiService.CheckConnection();
            if (!response.IsSuccess)
            {
                //  Define the status of the controls
                this.SetStatusControl(true, false, 0);
                await this.dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            // Create the object 
            var cneIvssData = new CneIvssDataItem
			{
                //  BirthDate = this.BirthDate,
				BirthDate = this.cneItemViewModel.BirthDate,
				CneIvssDataId = this.cneItemViewModel.CneIvssDataId,
				IdentificationCard = this.IdentificationCard,
				IsCne = this.cneItemViewModel.IsCne,
				IsIvss = this.cneItemViewModel.IsIvss,
				NationalityId = this.NationalityId,
				NationalityDatas = this.GetNationalityDatas(
					this.NationalityId),
                UserId = this.mainViewModel.UserData.UserId,
			};

			response = await this.apiService.Put<CneIvssDataItem>(
                MethodsHelper.GetUrlAPI(),
                "/api",
                "/CneIvssDatas",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                cneIvssData);
            if (!response.IsSuccess)
            {
                //  Define the status of the controls
                this.SetStatusControl(true, false, 0);
                await dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

			//  Edit record to CneData
			this.cantvViewModel.UpdateCneData(
				0,
				this.ToCneItemViewModel(cneIvssData));

            //  Navigate to back
            await this.navigationService.GoBackOnMaster();

            //  Define the status of the controls
            this.SetStatusControl(true, false, 0);
        }

		//public CneIvssData ToCneItemViewModel(
			//CneIvssDataItem _cneIvssData)
        //{
        //    return new CneIvssData
        //    {
        //        BirthDate = _cneIvssData.BirthDate,
        //        CneIvssDataId = _cneIvssData.CneIvssDataId,
        //        IdentificationCard = _cneIvssData.IdentificationCard,
        //        IsCne = _cneIvssData.IsCne,
        //        IsIvss = _cneIvssData.IsIvss,
        //        NationalityDatas = this.GetNationalityDatas(
        //            _cneIvssData.NationalityId),
        //        NationalityId = _cneIvssData.NationalityId,
        //    };
        //}

		public CneItemViewModel ToCneItemViewModel(
            CneIvssDataItem _cneIvssData)
        {
            return new CneItemViewModel
            {
                BirthDate = _cneIvssData.BirthDate,
                CneIvssDataId = _cneIvssData.CneIvssDataId,
                IdentificationCard = _cneIvssData.IdentificationCard,
                IsCne = _cneIvssData.IsCne,
                IsIvss = _cneIvssData.IsIvss,
                NationalityDatas = this.GetNationalityDatas(
                    _cneIvssData.NationalityId),
                NationalityId = _cneIvssData.NationalityId,
            };
        }

        private List<NationalityData> GetNationalityDatas(int _nationalityId)
        {
			var listGetNationalities = new List<NationalityData>();
            var list = this.listNationalities
                          .Where(lnd => lnd.NationalityId == _nationalityId)
                          .FirstOrDefault();

            listGetNationalities.Add(list);

			return listGetNationalities;
        }
              
		private void SetStatusControl(
			bool _isEnabled,
			bool _isRunning,
			int _messageLabe)
		{
			this.IsEnabled = _isEnabled;
			this.IsRunning = _isRunning;
			switch (_messageLabe)
			{
				case 0:
					this.MessageLabel = string.Empty;
					break;
				case 1:
					this.MessageLabel = string.Format(
						"{0}",
						"Wait a moment, we are processing your request...!!! ");
					break;
				case 2:
					this.MessageLabel = string.Format(
						"{0}",
						"Wait a moment, we are getting your data...!!! ");
					break;
			}
		}

		#endregion Methods
	}
}