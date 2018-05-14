namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Helpers;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class EditEmailViewModel : BaseViewModel
    {

        #region Attributes

        #region Services

        private ApiService apiService;
        private NavigationService navigationService;
        private DialogService dialogService;

        #endregion Services

        private MainViewModel mainViewModel;
		private string currentEmail;
		private string newEmail;
		private string confirmEmail;
        private bool isEnabled;
        private bool isRunning;
        private string messageLabel;
		private string messageLabelTextColor;

        #endregion Attributes

        #region Properties
        
		public string CurrentEmail
        {
            get { return this.currentEmail; }
            set { SetValue(ref this.currentEmail, value); }
        }

		public string NewEmail
        {
            get { return this.newEmail; }
            set { SetValue(ref this.newEmail, value); }
        }

		public string ConfirmEmail      
        {
			get { return this.confirmEmail; }
			set { SetValue(ref this.confirmEmail, value); }
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

        public string MessageLabelTextColor
        {
            get { return this.messageLabelTextColor; }
            set { SetValue(ref this.messageLabelTextColor, value); }
        }

        #region Commands
        
        public ICommand SaveCommand => new RelayCommand(Save);
        public ICommand GoBackCommand => new RelayCommand(GoBack);

        #endregion Commands

        #endregion Properties

        #region Constructor

		public EditEmailViewModel()
        {
            //  Gets an new instance the service class
            this.apiService = new ApiService();
            this.dialogService = new DialogService();
            this.navigationService = new NavigationService();

            //  Gets an instance of the MainViewModel
            this.mainViewModel = MainViewModel.GetInstance();

			//  Load values in the controls
            this.LoadValuesControls(0);

            //  Define the status of the controls
            this.SetStatusControl(true, false, "Green", 0);
        }

        #endregion Constructor

        #region Methods

		private async void Save()
        {
            //  Validate the field of form

            #region Current Email

			var response = MethodsHelper.IsValidField(
				"S",
				0,
				0,
				"email",
			this.CurrentEmail,
				false,
				false,
				string.Empty);
			if (!response.IsSuccess)
			{
				await dialogService.ShowMessage(
					"Error",
					response.Message,
					"Accept");
				return;
			}

			var isValidEmail = MethodsHelper.IsValidEmail(this.CurrentEmail);
			if (!isValidEmail)
			{
				await dialogService.ShowMessage(
					"Error",
					"You must enter an valid current email",
					"Accept");
				return;
			}

			#endregion Current Email

			#region New Email

			response = MethodsHelper.IsValidField(
				"S",
				0,
				0,
				"new email",
				this.NewEmail,
				false,
				false,
				string.Empty);
			if (!response.IsSuccess)
			{
				await dialogService.ShowMessage(
							 "Error",
							 response.Message,
							 "Accept");
				return;
			}

			isValidEmail = MethodsHelper.IsValidEmail(this.NewEmail);
			if (!isValidEmail)
			{
				await dialogService.ShowMessage(
					"Error",
					"You must enter an valid new email",
					"Accept");
				return;
			}

			#endregion New Email

			#region Confirm Email

			response = MethodsHelper.IsValidField(
				"S",
				0,
				0,
				"confirm email",
				this.ConfirmEmail,
						 false,
						 false,
						 string.Empty);
			if (!response.IsSuccess)
			{
				await dialogService.ShowMessage(
					"Error",
					response.Message,
					"Accept");
				return;
			}

			isValidEmail = MethodsHelper.IsValidEmail(this.ConfirmEmail);
			if (!isValidEmail)
			{
				await dialogService.ShowMessage(
					"Error",
					"You must enter an valid confirm email",
					"Accept");
				return;
			}

			#endregion Confirm Email

			#region Email Confirm and new Email

			response = MethodsHelper.IsValidField(
				"S",
				0,
				0,
				"email confirm",
				this.ConfirmEmail,
				false,
				true,
				this.NewEmail);
			if (!response.IsSuccess)
			{
				await dialogService.ShowMessage(
					"Error",
					response.Message,
					"Accept");
				return;
			}

			#endregion Email Confirm and new Email

			//  Set status controls
			SetStatusControl(false, true, "Green", 1);

			response = await apiService.CheckConnection();
            if (!response.IsSuccess)
            {
                //  Set status controls
                SetStatusControl(true, true, "Green", 0);

                await dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            ////  Use the user registration API         
            //var user = new User
            //{
            //    AppName = MethodsHelper.GetAppName(),
            //    Email = this.Email,
            //    FirstName = this.FirtsName,
            //    LastName = this.LastName,
            //    Password = this.Password,
            //    UserTypeId = Convert.ToInt32("5"),
            //};

            //response = await apiService.Post(
            //    MethodsHelper.GetUrlAPI(),
            //    "/api",
            //    "/Users",
            //    user);
            //if (!response.IsSuccess)
            //{
            //    //  Set status controls
            //    SetStatusControl(true, true, false, 0);

            //    await dialogService.ShowMessage(
            //        "Error",
            //        response.Message,
            //        "Accept");
            //    return;
            //}

            //  Set status controls
            SetStatusControl(true, true, "Grenn", 0);

            //  Set Initialize the fields
			LoadValuesControls(0);

            //  Go back login
            await dialogService.ShowMessage(
                "Information",
				"Successfully modified email, you must log in again...!!! ",
                "Accept");
            
            //  Navigate to LoginPage
			navigationService.SetMainPage("LoginPage");
        }
        
		private async void GoBack()
        {
            this.LoadValuesControls(0);
            await navigationService.GoBackOnMaster();
        }

		private void LoadValuesControls(int _option)
        {
            if (_option == 0)
            {
				this.CurrentEmail = string.Empty;
				this.NewEmail = string.Empty;
				this.ConfirmEmail = string.Empty;
                this.MessageLabel = string.Empty;
				MessageLabelTextColor = "Green";
            }
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