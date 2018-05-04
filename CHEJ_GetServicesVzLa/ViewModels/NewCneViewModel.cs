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
		private ApiService apiService;
		private DialogService dialogService;
        private NavigationService navigationService;      
		private string identificationCard;
		private DateTime birthDay;
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

		public DateTime BirthDate
		{
			get { return this.birthDay; }
			set { SetValue(ref this.birthDay, value); }
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
			this.mainViewModel = MainViewModel.GetInstance();
			this.apiService = new ApiService();
			this.dialogService = new DialogService();
            this.navigationService = new NavigationService();         

			//  Load the value in the form
			this.LoadValues();
        }

		private async void LoadValues()
		{
			//  this.Nationality = "";
			this.IdentificationCard = "";
			this.BirthDate = DateTime.Now.Date;

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
			response = await apiService.GetList<NationalityData>(
				MethodsHelper.GetUrlAPI(),
				"/api", 
				"/Nationalities", 
				"", 
				mainViewModel.Token.TokenType, 
				mainViewModel.Token.AccessToken);

			if(!response.IsSuccess)
			{
				//  Define the status of the controls
                SetStatusControl(true, false, 0);

				await dialogService.ShowMessage(
					"Error", 
					response.Message, 
					"Accept");
				return;
			}
                     
            //  Method that load values
			LoadNationalities((List<NationalityData>)response.Result);

			//  Define the status of the controls
            SetStatusControl(true, false, 0);
		}

		private void LoadNationalities(List<NationalityData> _nationalites)
		{
			//  Add an new value in the List<>
			_nationalites.Add(new NationalityData 
			{
				Abbreviation = "N",
				Name = "[Select a nationality...]",
				NationalityId = 0,
			});
                     
			this.Nationalities = 
				new ObservableCollection<NationalityData>(
					_nationalites.OrderBy(n => n.NationalityId));
		}

		#endregion Constructor

		#region Methods
        
		private async void Save()
        {
			//  Validate the fields of the form
			if(this.NationalityId == 0)
			{
				await dialogService.ShowMessage("Error", "You must select an nationality...!!!", "Accept");
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
				await dialogService.ShowMessage(
					"Error", 
					response.Message, 
					"Accept");
				return;
			}

			//  Define the status of the controls
			SetStatusControl(false, true, 1);

			//  Check the connection internet
			response = await apiService.CheckConnection();
			if(!response.IsSuccess)
			{
				//  Define the status of the controls
				SetStatusControl(true, false, 0);
				await dialogService.ShowMessage(
					"Error", 
					response.Message, 
					"Accept");
				return;
			}

			// Create the object 
			var cneIvssData = new CneIvssData
			{ 
				BirthDate = this.BirthDate,
				IdentificationCard = this.IdentificationCard,
				IsCne = true,
				IsIvss = false,
				NationalityId = this.NationalityId,
			};

			response = await apiService.Post<CneIvssData>(
				MethodsHelper.GetUrlAPI(), 
				"/api", 
				"/CneIvssDatas", 
				mainViewModel.Token.TokenType, 
				mainViewModel.Token.AccessToken, 
				cneIvssData);         
			if(!response.IsSuccess)
			{
				//  Define the status of the controls
				SetStatusControl(true, false, 0);
				await dialogService.ShowMessage(
					"Error", 
					response.Message, 
					"Accept");
				return;
			}

			//  Define the status of the controls
			SetStatusControl(true, false, 0);

        }

        private async void GoBack()
        {
            //  Navigate on back
            await navigationService.GoBackOnMaster();
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