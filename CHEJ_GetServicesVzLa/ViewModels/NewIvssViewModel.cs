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

	public class NewIvssViewModel : BaseViewModel
    {
		#region Attributes

		#region Services

		private ApiService apiService;
		private NavigationService navigationService;
		private DialogService dialogService;

		#endregion Services

		private CantvViewModel cantvViewModel;
		private MainViewModel mainViewModel;
		private List<NationalityData> listNationalities;
		private ObservableCollection<NationalityData> nationalities;
		private int nationalityId;
		private string identificationCard;
		private DateTime birthDate;
		private string messageLabel;
		private bool isEnabled;
		private bool isRunning;

		#endregion Attributes

		#region Properties

		public ObservableCollection<NationalityData> Nationalities
		{
			get { return this.nationalities; }
			set { SetValue(ref this.nationalities, value); }
		}

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

		public DateTime BirthDate
		{
			get { return this.birthDate; }
			set { SetValue(ref this.birthDate, value); }
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

		public NewIvssViewModel()
		{
			//  Gets an instance of the class
			this.mainViewModel = MainViewModel.GetInstance();
			this.cantvViewModel = CantvViewModel.GetInstance();

			//  Gets an new instance the service class
			this.apiService = new ApiService();
			this.dialogService = new DialogService();
			this.navigationService = new NavigationService();

			//  Invoke the method Preload
			this.LoadValues();
		}

		#endregion Constructor

		#region Methods

		private async void LoadValues()
		{
			//  this.Nationality = "";
			this.IdentificationCard = "";
			this.BirthDate = DateTime.Now.Date;

			//  Define the status of the controls
			this.SetStatusControl(false, true, 1);

			var response = await this.apiService.CheckConnection();
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

			//  Get an List<> of the Nationalities
			response = await this.apiService.GetList<NationalityData>(
				MethodsHelper.GetUrlAPI(),
				"/api",
				"/Nationalities",
				"",
				this.mainViewModel.Token.TokenType,
				this.mainViewModel.Token.AccessToken);

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

			this.listNationalities =
				new List<NationalityData>((List<NationalityData>)response.Result);

			this.Nationalities =
				new ObservableCollection<NationalityData>(
						listNationalities.OrderBy(n => n.NationalityId));

			//  Define the status of the controls
			this.SetStatusControl(true, false, 0);
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
                BirthDate = this.BirthDate,
                IdentificationCard = this.IdentificationCard,
				IsCne = false,
				IsIvss = true,
                NationalityId = this.NationalityId,
                UserId = this.mainViewModel.UserData.UserId,
            };

            response = await this.apiService.Post<CneIvssDataItem>(
                MethodsHelper.GetUrlAPI(),
                "/api",
                "/CneIvssDatas/PostCneIvssDataInsertByOption/?_option=ivss",
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

			//  Add record to CneData
			this.cantvViewModel.LoadUserData();
			    
            //  Navigate to back
            await this.navigationService.GoBackOnMaster();

            //  Define the status of the controls
            this.SetStatusControl(true, false, 0);
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