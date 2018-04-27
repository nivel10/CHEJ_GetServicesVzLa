namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System;
    using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class NewPhoneViewModel : BaseViewModel
    {
		#region Attributes

		private bool isEnabled;
		private string codePhone;
		private string nuberPhone;
		private bool isRunning;
		private string messageLabel;
		private int userId;

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

		public NewPhoneViewModel(int _userId)
		{
			//  Define control format
			SetInitialize();
			SetStatusControl(true, false, 0);

			//  Gets an instances of the services class
			apiservices = new ApiService();
			dialogService = new DialogService();
			navigationService = new NavigationService();

			//  Load data UserId
			this.userId = _userId;
		}

		#endregion Constructor
        
		#region Methods
        
		private async void GoBack()
		{
			await navigationService.GoBackOnMaster();
		}

		private void Save()
		{
			var a = this.userId;
			throw new NotImplementedException();
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