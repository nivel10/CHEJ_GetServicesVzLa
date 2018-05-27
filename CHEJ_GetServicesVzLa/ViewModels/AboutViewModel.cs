namespace CHEJ_GetServicesVzLa.ViewModels
{
    using System.Windows.Input;
    using CHEJ_GetServicesVzLa.Helpers;
    using CHEJ_GetServicesVzLa.Services;
    using GalaSoft.MvvmLight.Command;

    public class AboutViewModel : BaseViewModel
    {
        #region Attributes

        #region Services

        private DialogService dialogService;
        private NavigationService navigationService;

        #endregion Services

        private string appName;
        private string appVersion;
        private string appLicense;
        private string appWebPage;
        private string appDevCompany;
        private string appEmailCompany;
        private bool isEnabled;
        private bool isRunning;
        private string messageLabel;

        #endregion Attributes

        #region Properties

        public string AppName
        {
            get { return this.appName; }
            set { SetValue(ref this.appName, value); }
        }

        public string AppVersion
        {
            get { return this.appVersion; }
            set { SetValue(ref this.appVersion, value); }
        }

        public string AppLicense
        {
            get { return this.appLicense; }
            set { SetValue(ref this.appLicense, value); }
        }

        public string AppDevCompany
        {
            get { return this.appDevCompany; }
            set { SetValue(ref this.appDevCompany, value); }
        }

        public string AppWebPage
        {
            get { return this.appWebPage; }
            set { SetValue(ref this.appWebPage, value); }
        }

        public string AppEmailCompany
        {
            get { return this.appEmailCompany; }
            set { SetValue(ref this.appEmailCompany, value); }
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

        public ICommand GoBackCommand => new RelayCommand(GoBack);

        #endregion Properties

        #region Constructor

        public AboutViewModel()
        {
            //  Gets an intance of the services class
            this.dialogService = new DialogService();
            this.navigationService = new NavigationService();

            //  Invoke the method of load values CNE
            this.LoadData();
        }

        #endregion Constructor

        #region Methods

        private void LoadData()
        {
            //  Set status of controls
            this.SetStatusControl(true, false, 0);

            //  Load data in the fields
            this.AppName = "BCS-VzLaApp";
            this.AppVersion = "Pre Beta Ver. 1.0.0.0";
            this.AppLicense = "Test, with registration limit";
            this.AppDevCompany = MethodsHelper.GetCompanyName();
            this.AppWebPage = MethodsHelper.GetCompanyUrl();
            this.AppEmailCompany = MethodsHelper.GetCompanyEmail();

        }

        private async void GoBack()
        {
            //  Navigate on back
            await this.navigationService.GoBackOnLogin();
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