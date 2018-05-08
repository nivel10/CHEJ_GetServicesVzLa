using System.Windows.Input;
using CHEJ_GetServicesVzLa.Helpers;
using CHEJ_GetServicesVzLa.Models;
using CHEJ_GetServicesVzLa.Services;
using GalaSoft.MvvmLight.Command;

namespace CHEJ_GetServicesVzLa.ViewModels
{
	public class GetIvssViewModel : BaseViewModel
    {
		#region Attributes

        #region Services

        private ApiService apiService;
        private DialogService dialogService;
        private NavigationService navigationService;

        #endregion Services

        private bool isValid;
        private string identificationCard;
        private string fullName;
		private string gender;
		private string birthDate;
		private string company;
		private string employerNumber;
		private string entry;
		private string status;
		private string membership;
		private string contingency;
		private string weeks;
		private string salaries;
        private string messageLabel;
        private string messageLabelTextColor;
        private bool isEnabled;
        private bool isRunning;
        private IvssItemViewModel ivssItemViewModel;

        #endregion Attributes

        #region Properties

        public string IdentificationCard
        {
            get { return this.identificationCard; }
            set { SetValue(ref this.identificationCard, value); }
        }

        public string FullName
        {
            get { return this.fullName; }
            set { SetValue(ref this.fullName, value); }
        }

		public string Gender
        {
            get { return this.gender; }
			set { SetValue(ref this.gender, value); }
        }

		public string BirthDate
		{
			get { return this.birthDate; }
			set { SetValue(ref this.birthDate, value); }
		}

		public string Company
        {
            get { return this.company; }
            set { SetValue(ref this.company, value); }
        }

		public string EmployerNumber
        {
			get { return this.employerNumber; }
			set { SetValue(ref this.employerNumber, value); }
        }

		public string Entry
        {
            get { return this.entry; }
            set { SetValue(ref this.entry, value); }
        }

		public string Status
        {
            get { return this.status; }
            set { SetValue(ref this.status, value); }
        }

		public string Membership
        {
            get { return this.membership; }
            set { SetValue(ref this.membership, value); }
        }

		public string Contingency
		{
			get { return this.contingency; }
			set { SetValue(ref this.contingency, value); }
		}

		public string Weeks
		{
			get { return this.weeks; }
			set { SetValue(ref this.weeks, value); }
		}

		public string Salaries
		{
			get { return this.salaries; }
			set { SetValue(ref this.salaries, value); }
		}

		public string MessageLabel
        {
            get { return this.messageLabel; }
            set { SetValue(ref this.messageLabel, value); }
        }

        public string MessageLabelTextColor
        {
            get { return this.messageLabelTextColor; }
            set { SetValue(ref this.messageLabelTextColor, value); }
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

		#region Commands

		public ICommand GoBackCommand => new RelayCommand(GoBack);

		#endregion Commands
              
		#endregion Properties

		#region Constructor

		public GetIvssViewModel(IvssItemViewModel _ivssItemViewModel)
        {
			//  Load value of the IvssItemViewModel
			this.ivssItemViewModel = _ivssItemViewModel;

            //  Gets an instance of the services class
            this.apiService = new ApiService();
            this.dialogService = new DialogService();
            this.navigationService = new NavigationService();

            //  Sets status of controls
            this.SetStatusControl(true, false, "Green", 0);

            //  Initialize the values of the controls
            this.isValid = true;
            this.LoadValuesControls(0, null);

            //  Get data of the IVSS
			FindData();
        }

        #endregion Constructor

        #region Methods

        private async void FindData()
        {
            //  Sets status of controls
            this.SetStatusControl(false, true, "Green", 1);

            //  Validate the connections of internet
            var response = await this.apiService.CheckConnection();
            if (!response.IsSuccess)
            {
                //  Sets status of controls
                this.SetStatusControl(false, false, "Green", 0);
                await this.dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            //  Define the url parameter
            var ulrParameter = string.Format(
				"/{0}/{1}/{2}/{3}/{4}",
				this.ivssItemViewModel.NationalityDatas[0].Abbreviation,
				this.ivssItemViewModel.IdentificationCard,
				this.ivssItemViewModel.BirthDate.Day,
				this.ivssItemViewModel.BirthDate.Month,
				this.ivssItemViewModel.BirthDate.Year);

            //  Get data of the cne api
            response = await apiService.Get<Ivss>(
				MethodsHelper.GetUrlIvss(),
                "/infove/ivss",
                "/cuenta",
                ulrParameter);

            if (!response.IsSuccess)
            {
                //  Sets status of controls
                this.SetStatusControl(false, false, "Green", 0);

                await this.dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            //  Load values in the control
            this.LoadValuesControls(1, (Ivss)response.Result);

            //  Sets status of controls
            if (isValid)
            {
                this.SetStatusControl(true, false, "Green", 0);
            }
        }

		private void LoadValuesControls(int _option, Ivss _ivssData)
        {
            if (_option == 0)
            {
                this.IdentificationCard = string.Empty;
                this.FullName = string.Empty;
				this.BirthDate = string.Empty;
                this.EmployerNumber = string.Empty;
                this.Company = string.Empty;
                this.Entry = string.Empty;
                this.Status = string.Empty;
				this.Membership = string.Empty;
				this.Contingency = string.Empty;
				this.Weeks = string.Empty;
				this.Salaries = string.Empty;
            }
            else
            {
				if (!_ivssData.Descripcion.Contains(
					"la Cedula no esta registrada como asegurado"))
                {
                    this.IdentificationCard = _ivssData.Cedula;
					this.FullName = 
						MethodsHelper.TitleText(_ivssData.Nombre.Trim());
					this.Gender = 
						MethodsHelper.TitleText(_ivssData.Sexo.Trim());
					this.BirthDate = _ivssData.Nacimiento.Trim();
					this.EmployerNumber = 
						MethodsHelper.TitleText(_ivssData.NumeroPatronal.Trim());
					this.Company = _ivssData.Empresa.Trim();
					this.Entry = _ivssData.Ingreso.ToUpper().Trim();
					this.Status = 
						MethodsHelper.TitleText(_ivssData.Estatus.Trim());
					this.Membership = _ivssData.Afiliacion.Trim();
					this.Contingency = _ivssData.Contingencia.Trim();
					this.Weeks = _ivssData.Semanas.Trim();
					this.Salaries = _ivssData.Salarios.Trim();
                }
                else
                {
                    this.MessageLabel = _ivssData.Descripcion;
                    //  Sets status of controls
                    this.isValid = false;
                    this.SetStatusControl(true, false, "Red", -1);
                }
            }
        }

        private async void GoBack()
        {
            this.LoadValuesControls(0, null);
            await navigationService.GoBackOnMaster();
        }

        private void SetStatusControl(
            bool _isEnabled,
            bool _isRunning,
            string _textColor,
            int _messageLabe)
        {
            this.IsEnabled = _isEnabled;
            this.IsRunning = _isRunning;
            this.MessageLabelTextColor = _textColor;
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