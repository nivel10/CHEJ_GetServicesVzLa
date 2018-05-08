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

	public class EditIvssViewModel : BaseViewModel
    {
		private IvssItemViewModel ivssItemViewModel;

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

        public EditIvssViewModel(IvssItemViewModel _ivssItemViewModel)
        {
			//  Gets an instance of the class
			this.ivssItemViewModel = _ivssItemViewModel;
			this.mainViewModel = MainViewModel.GetInstance();
			this.cantvViewModel = CantvViewModel.GetInstance();
				
			//  Gets an new instance the service class
            this.apiService = new ApiService();
            this.dialogService = new DialogService();
            this.navigationService = new NavigationService();

            //  Invoke the method Preload
            this.LoadValues();
        }
        
		#region Methods

        private async void LoadValues()
        {
			this.NationalityId = this.ivssItemViewModel.NationalityId;
			this.IdentificationCard = this.ivssItemViewModel.IdentificationCard;
			this.BirthDate = this.ivssItemViewModel.BirthDate;

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
				CneIvssDataId = this.ivssItemViewModel.CneIvssDataId,
                IdentificationCard = this.IdentificationCard,
				IsCne = this.ivssItemViewModel.IsCne,
				IsIvss = this.ivssItemViewModel.IsIvss,
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
			this.cantvViewModel.LoadUserData();

            //  Navigate to back
            await this.navigationService.GoBackOnMaster();

            //  Define the status of the controls
            this.SetStatusControl(true, false, 0);
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