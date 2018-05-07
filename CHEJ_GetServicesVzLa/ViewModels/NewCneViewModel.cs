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

	public class NewCneViewModel : BaseViewModel
    {
		#region Attributes
              
        private MainViewModel mainViewModel;
		private CantvViewModel cantvViewModel;

		#region Services

		private ApiService apiService;
		private DialogService dialogService;
		private NavigationService navigationService;

		#endregion Services

		private List<NationalityData> listNationalities;
		private string identificationCard;      
		private string messageLabel;
		private bool isEnabled;
		private bool isRunning;
		private ObservableCollection<NationalityData> nationalities;      
		private int nationalityId;

        #endregion Attributes

        #region Properties
             
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

		public string MessageLabel
		{
			get { return this.messageLabel; }
			set { SetValue(ref this.messageLabel, value); }
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
              
		public int NationalityId
		{
			get { return this.nationalityId; }
			set { SetValue(ref this.nationalityId, value); }
		}

		#region Command

		public ICommand SaveCommand
		{
			get { return new RelayCommand(Save); }
		}

		public ICommand GoBackCommand
		{
			get { return new RelayCommand(GoBack); }
		}

		#endregion Command

		#endregion Properties

		#region Constructor

		public NewCneViewModel()
        {
			//  Gets an instance of the service class         
			this.apiService = new ApiService();
			this.dialogService = new DialogService();
            this.navigationService = new NavigationService();         

            //  Gets an instances of the ViewModels
			this.mainViewModel = MainViewModel.GetInstance();
			this.cantvViewModel = CantvViewModel.GetInstance();

			//  Load the value in the form
			this.LoadValues();
        }

		private async void LoadValues()
		{
			//  this.Nationality = "";
			this.IdentificationCard = "";
			//  this.BirthDate = DateTime.Now.Date;

			//  Define the status of the controls
			this.SetStatusControl(false, true, 1);

			var response = await this.apiService.CheckConnection();
			if(!response.IsSuccess)
			{
				//  Define the status of the controls
				this.SetStatusControl(true, false, 0);

				await this.dialogService.ShowMessage(
					"Error", 
					response.Message, 
					"Accept");
				return;
			}
                     
            //  Get an List<> of the Nationalities
			response = await this.apiService.GetList<NationalityData>(
				MethodsHelper.GetUrlAPI(),
				"/api", 
				"/Nationalities", 
				"", 
				mainViewModel.Token.TokenType, 
				mainViewModel.Token.AccessToken);

			if(!response.IsSuccess)
			{
				//  Define the status of the controls
                this.SetStatusControl(true, false, 0);

				await this.dialogService.ShowMessage(
					"Error", 
					response.Message, 
					"Accept");
				return;
			}

			//  Method that load values
			//this.mainViewModel.ListNationalityDatas = 
				//new List<NationalityData>((List<NationalityData>)response.Result);

			//  Method that load values
            //  this.LoadNationalities(this.mainViewModel.ListNationalityDatas);

			this.listNationalities =
                new List<NationalityData>((List<NationalityData>)response.Result);
			
			//  Method that load values
			// this.LoadNationalities(this.listNationalityDatas);

			this.Nationalities =
                new ObservableCollection<NationalityData>(
					    listNationalities.OrderBy(n => n.NationalityId));
			
			//  Method that load values
			//  this.LoadNationalities(this.mainViewModel.ListNationalityDatas);

			//  Define the status of the controls
            this.SetStatusControl(true, false, 0);
		}

		//private void LoadNationalities(List<NationalityData> _nationalites)
		//{
		//	//  Add an new value in the List<>
		//	_nationalites.Add(new NationalityData 
		//	{
		//		Abbreviation = "N",
		//		Name = "[Select a nationality...]",
		//		NationalityId = 0,
		//	});
                     
		//	this.Nationalities = 
		//		new ObservableCollection<NationalityData>(
		//			_nationalites.OrderBy(n => n.NationalityId));
		//}

		#endregion Constructor

		#region Methods
        
		private async void Save()
        {
			//  Validate the fields of the form
			if(this.NationalityId == 0)
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
			if(!response.IsSuccess)
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
			if(!response.IsSuccess)
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
				BirthDate = DateTime.Today.Date,
				IdentificationCard = this.IdentificationCard,
				IsCne = true,
				IsIvss = false,
				NationalityId = this.NationalityId,
				UserId = this.mainViewModel.UserData.UserId,
			};

			response = await this.apiService.Post<CneIvssDataItem>(
				MethodsHelper.GetUrlAPI(), 
				"/api", 
				"/CneIvssDatas", 
				mainViewModel.Token.TokenType, 
				mainViewModel.Token.AccessToken, 
				cneIvssData);         
			if(!response.IsSuccess)
			{
				//  Define the status of the controls
				this.SetStatusControl(true, false, 0);
				await dialogService.ShowMessage(
					"Error", 
					response.Message, 
					"Accept");
				return;
			}

			//  Add record to CneData
			this.cantvViewModel.UpdateCneData(
				1, 
				this.ToCneItemViewModel((CneIvssDataItem)response.Result));
			
			//  Navigate to back
			await this.navigationService.GoBackOnMaster();
            
			//  Define the status of the controls
			this.SetStatusControl(true, false, 0);
		}

		//public CneIvssData ToCneItemViewModel(
			//CneIvssDataItem _cneIvssData)
   //     {
			//return new CneIvssData
    //        {
				//BirthDate = _cneIvssData.BirthDate,
				//CneIvssDataId = _cneIvssData.CneIvssDataId,
				//IdentificationCard = _cneIvssData.IdentificationCard,
				//IsCne = _cneIvssData.IsCne,
				//IsIvss = _cneIvssData.IsIvss,
    //            NationalityDatas = this.GetNationalityDatas(
				//	_cneIvssData.NationalityId),
				//NationalityId = _cneIvssData.NationalityId,
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
			var listNationalitiData = new List<NationalityData>();
			var list = this.listNationalities
                          .Where(lnd => lnd.NationalityId == _nationalityId)
                          .FirstOrDefault();
			
			listNationalitiData.Add(list);

			return listNationalitiData;
		}

		private async void GoBack()
        {
            //  Navigate on back
            await this.navigationService.GoBackOnMaster();
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