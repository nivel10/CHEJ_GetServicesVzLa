namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System;
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Helpers;
	using CHEJ_GetServicesVzLa.Models;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class RecoveryViewModel : BaseViewModel
    {
		
		#region Attributes

        #region Services

        private ApiService apiService;
        private NavigationService navigationService;
        private DialogService dialogService;

        #endregion Services

        private MainViewModel mainViewModel;
		private string email;
		private string labelMessage001;
        private string labelMessage002;
        private bool isEnabled;
        private bool isRunning;
        private string messageLabel;
        private string messageLabelTextColor;

        #endregion Attributes

		#region Properties

		public string Email
		{
			get { return this.email; }
			set { SetValue(ref this.email, value); }
		}

		public string LabelMessage001
		{
			get { return this.labelMessage001; }
			set { SetValue(ref this.labelMessage001, value); }
		}

		public string LabelMessage002
		{
			get { return this.labelMessage002; }
			set { SetValue(ref this.labelMessage002, value); }
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

		public string MessageLabelTextColor
		{
			get { return this.messageLabelTextColor; }
			set { SetValue(ref this.messageLabelTextColor, value); }
		}
        
		public ICommand GoBackCommand => new RelayCommand(GoBack);
		public ICommand RecoveryCommand => new RelayCommand(Recovery);

		#endregion Properties

		#region Constructor

		public RecoveryViewModel()
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

		private async void Recovery()
        {
			var response = MethodsHelper.IsValidField(
				"S",
                0,
                0,
                "email",
				this.Email,
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

			var isValidEmail = MethodsHelper.IsValidEmail(this.Email);
            if (!isValidEmail)
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You must enter an valid email...!!!",
                    "Accept");
                return;
            }

			//  Set status controls
            SetStatusControl(false, true, "Green", 1);

			response = await apiService.PasswordRecovery(
                MethodsHelper.GetUrlAPI(),
                "/api",
                "/Users/PasswordRecovery",
				this.Email);
            if (!response.IsSuccess)
            {
                //  Set status controls
                SetStatusControl(true, false, "Green", 0);

                await dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            //  Set status controls
            SetStatusControl(true, false, "Green", 0);

            //  Set Initialize the fields
            LoadValuesControls(0);

            //  Go back login
            await dialogService.ShowMessage(
                "Information",
                "You new password has been successfully sent to email...!!!",
                "Accept");

            //  Navigate to LoginPage
			await navigationService.GoBackOnLogin();

        }

		private async void GoBack()
		{
			this.LoadValuesControls(0);
			await navigationService.GoBackOnLogin();
		}

		private void LoadValuesControls(int _option)
		{
			if (_option == 0)
			{
				this.Email = string.Empty;
				this.LabelMessage001 = string.Format(
					"{0}{1}",
						  "Enter your email address with which you registered in our ",
						  "system...!!!");

				this.LabelMessage002 = string.Format(
					"{0}{1}",
					"We will send you an email with you new password, remember to ",
					"change the password for a new password more secure...!!!");
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