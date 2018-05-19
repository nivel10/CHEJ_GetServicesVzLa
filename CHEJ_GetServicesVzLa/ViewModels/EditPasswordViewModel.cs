namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Helpers;
	using CHEJ_GetServicesVzLa.Models;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class EditPasswordViewModel : BaseViewModel
    {
		#region Attributes

        #region Services

        private ApiService apiService;
        private NavigationService navigationService;
        private DialogService dialogService;

        #endregion Services

        private MainViewModel mainViewModel;
		private string currentPassword;
		private string newPassword;
		private string confirmPassword;
        private bool isEnabled;
        private bool isRunning;
        private string messageLabel;
        private string messageLabelTextColor;

        #endregion Attributes

        #region Properties

		public string CurrentPassword
        {
            get { return this.currentPassword; }
            set { SetValue(ref this.currentPassword, value); }
        }

		public string NewPassword
        {
            get { return this.newPassword; }
            set { SetValue(ref this.newPassword, value); }
        }

		public string ConfirmPassword
        {
            get { return this.confirmPassword; }
            set { SetValue(ref this.confirmPassword, value); }
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

        public EditPasswordViewModel()
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
            #region Current Password

            var response = MethodsHelper.IsValidField(
                "S",
                0,
                0,
                "current password",
            this.CurrentPassword,
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
            
			#endregion Current Password

			#region New Password

			response = MethodsHelper.IsValidField(
                "S",
                6,
                10,
                "new password",
                this.NewPassword,
                true,
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

            #endregion New Password

            #region Confirm Password

			response = MethodsHelper.IsValidField(
                "S",
                6,
                10,
                "password confirm",
                this.ConfirmPassword,
                true,
                true,
                this.NewPassword);
            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            #endregion Confirm Password
           
            //  Set status controls
            SetStatusControl(false, true, "Green", 1);

            response = await apiService.CheckConnection();
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

            //  Use the user registration API         
            var userEdit = new UserEdit
            {
                AppName = MethodsHelper.GetAppName(),
                Email = this.mainViewModel.UserData.Email,
                FirstName = this.mainViewModel.UserData.FirstName,
                LastName = this.mainViewModel.UserData.LastName,
				NewEmail = this.mainViewModel.UserData.Email,
                Password = this.CurrentPassword,
				NewPassword = this.NewPassword,
                UserId = this.mainViewModel.UserData.UserId,
                UserTypeId = 5,
            };
            
			response = await apiService.EditPassword(
                MethodsHelper.GetUrlAPI(),
                "/api",
				"/Users/EditPassword",
                this.mainViewModel.Token.TokenType,
                this.mainViewModel.Token.AccessToken,
                userEdit);
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
                "Successfully modified password, you must log in again...!!! ",
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
                this.CurrentPassword = string.Empty;
                this.NewPassword = string.Empty;
                this.ConfirmPassword = string.Empty;
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