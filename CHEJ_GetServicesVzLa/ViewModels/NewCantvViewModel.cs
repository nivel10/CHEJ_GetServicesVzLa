namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Helpers;
	using CHEJ_GetServicesVzLa.Models;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class NewCantvViewModel : BaseViewModel
    {
		#region Attributes

		private bool isEnabled;
		private string codePhone;
		private string nuberPhone;
		private bool isRunning;
		private string messageLabel;      
		private static MainViewModel mainViewModel;
		private static CantvViewModel cantvViewModel;

		#region Services

		private ApiService apiservices;
		private NavigationService navigationService;
		private DialogService dialogService;

		#endregion Services

		#endregion Attributes

		#region Properties

		public string CodePhone
		{
			get { return this.codePhone; }
			set { SetValue(ref this.codePhone, value); }
		}

		public string NuberPhone
		{
			get { return this.nuberPhone; }
			set { SetValue(ref this.nuberPhone, value); }
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

		public ICommand SaveCommand
		{
			get
			{
				return new RelayCommand(Save);
			}
		}

		public ICommand GoBackCommand
		{
			get
			{
				return new RelayCommand(GoBack);
			}
		}

		#endregion Commands

		#endregion Properties

		#region Constructor
        
		public NewCantvViewModel()
		{
			//  Define control format
			this.SetInitialize();
			this.SetStatusControl(true, false, 0);

			//  Gets an instances of the services class
			this.apiservices = new ApiService();
			this.dialogService = new DialogService();
			this.navigationService = new NavigationService();

			//  Gets an instance of the MainViewModel
			mainViewModel = MainViewModel.GetInstance();

			//  Gets an instance of the CantvViewModel
			cantvViewModel = CantvViewModel.GetInstance();
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
				3,
				3,
				"code phone",
				this.CodePhone,
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

            response = MethodsHelper.IsValidField(
				"I",
                7,
                7,
                "number phone",
				this.NuberPhone,
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

            //  Define control format
			this.SetStatusControl(false, true, 1);

            response = await apiservices.CheckConnection();
            if(!response.IsSuccess)
            {
                //  Define control format
				this.SetStatusControl(true, false, 0);

				await this.dialogService.ShowMessage(
                    "Error", 
                    response.Message, 
                    "Accept");
                return;
            }

            var cantvData = new CantvDataItem
            {   
                CodePhone = this.CodePhone,
                NumberPhone = this.NuberPhone,
				UserId = mainViewModel.UserData.UserId,
            };

            //  Save the data CatvData         
			response = await apiservices.Post<CantvDataItem>(
				MethodsHelper.GetUrlAPI(),
                "/api",
                "/CantvDatas",
				mainViewModel.Token.TokenType,
				mainViewModel.Token.AccessToken,
                cantvData);
            if(!response.IsSuccess)
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
            cantvData.CantvDataId = ((CantvData)response.Result).CantvDataId;
			cantvViewModel.UpdateCantvData(
				1, 
				this.ToCantvItemViewModel(cantvData));

            //  Define control format
			this.SetStatusControl(true, false, 0);

            //  Navigate to back
			await this.navigationService.GoBackOnMaster();
		}

		private CantvItemViewModel ToCantvItemViewModel(
			CantvDataItem _cantvDataItem)
        {
            return new CantvItemViewModel
            {
                CantvDataId = _cantvDataItem.CantvDataId,
                CodePhone = _cantvDataItem.CodePhone,
                NumberPhone = _cantvDataItem.NumberPhone,
            };
        }

		private void SetInitialize()
        {
            this.CodePhone = "";
            this.NuberPhone = "";
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