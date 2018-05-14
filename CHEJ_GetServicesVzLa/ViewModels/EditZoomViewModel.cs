namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System.Windows.Input;
    using CHEJ_GetServicesVzLa.Helpers;
    using CHEJ_GetServicesVzLa.Models;
    using CHEJ_GetServicesVzLa.Services;
    using GalaSoft.MvvmLight.Command;

	public class EditZoomViewModel : BaseViewModel
    {
		#region Attributes

        private bool isEnabled;
        private string tracking;
        private bool isRunning;
        private string messageLabel;
        private MainViewModel mainViewModel;
        private CantvViewModel cantvViewModel;
		private ZoomItemViewModel zoomItemViewModel;

        #region Services

        private ApiService apiservices;
        private NavigationService navigationService;
        private DialogService dialogService;

        #endregion Services

        #endregion Attributes

        #region Properties

        public string Tracking
        {
            get { return this.tracking; }
            set { SetValue(ref this.tracking, value); }
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

        #region Commands

        public ICommand SaveCommand => new RelayCommand(Save);
        public ICommand GoBackCommand => new RelayCommand(GoBack);

        #endregion Commands

        #endregion Properties

        #region Constructor

		public EditZoomViewModel(ZoomItemViewModel _zoomItemViewModel)
        {
			//  Get an instance of the ZoomItemViewModel
			this.zoomItemViewModel = _zoomItemViewModel;

            //  Define control format
            this.SetInitialize();
            this.SetStatusControl(true, false, 0);

            //  Gets an instances of the services class
            this.apiservices = new ApiService();
            this.dialogService = new DialogService();
            this.navigationService = new NavigationService();

            //  Gets an instance of the MainViewModel
            this.mainViewModel = MainViewModel.GetInstance();

            //  Gets an instance of the CantvViewModel
            this.cantvViewModel = CantvViewModel.GetInstance();

			//  Load value in the controls
			this.Tracking = this.zoomItemViewModel.Tracking;
        }

        #endregion Constructor

        #region Methods

        private async void GoBack()
        {
            await this.navigationService.GoBackOnMaster();
        }

        private async void Save()
        {
            var response = MethodsHelper.IsValidField(
                "I",
                8,
                20,
                "number tracking",
                this.Tracking,
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

            response = MethodsHelper.IsValidField(
                "I",
                8,
                20,
                "number tracking",
                this.Tracking,
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

            //  Define control format
            this.SetStatusControl(false, true, 1);

            response = await apiservices.CheckConnection();
            if (!response.IsSuccess)
            {
                //  Define control format
                this.SetStatusControl(true, false, 0);

                await this.dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            var zoomDataItem = new ZoomDataItem
            {
                Tracking = this.Tracking,
                UserId = mainViewModel.UserData.UserId,
				ZoomDataId = this.zoomItemViewModel.ZoomDataId,
            };

            //  Save the data CatvData         
            response = await apiservices.Put<ZoomDataItem>(
                MethodsHelper.GetUrlAPI(),
                "/api",
                "/ZoomDatas",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                zoomDataItem);
            if (!response.IsSuccess)
            {
                //  Define control format
                SetStatusControl(true, false, 0);

                await dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            //  Add new record         
            //  zoomDataItem.ZoomDataId = ((ZoomDataItem)response.Result).ZoomDataId;
            cantvViewModel.LoadUserData();

            //  Define control format
            this.SetStatusControl(true, false, 0);

            //  Navigate to back
            await this.navigationService.GoBackOnMaster();
        }

        private void SetInitialize()
        {
            this.Tracking = "";
            this.MessageLabel = "";
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