namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System;
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class MyProfileViewModel : BaseViewModel
    {
		#region Attributes

		#region Services

        private ApiService apiService;
        private NavigationService navigationService;
        private DialogService dialogService;

        #endregion Services
        
		private MainViewModel mainViewModel;      
		private string firstName;
		private string lastName;
		private string email;
		private string telephone;
		private string imagePath;
		private bool isEnabled;
        private bool isRunning;
		private string messageLabel;

		#endregion Attributes

		#region Properties

		public string FirstName
		{
			get { return this.firstName; }
			set { SetValue(ref this.firstName, value); }
		}

		public string LastName
		{
			get { return this.lastName; }
			set { SetValue(ref this.lastName, value); }
		}

		public string Email
		{
			get { return this.email; }
			set { SetValue(ref this.email, value); }
		}

		public string Telephone
		{
			get { return this.telephone; }
			set { SetValue(ref this.telephone, value); }
		}

		public string ImagePath
		{
			get { return this.imagePath; }
			set { SetValue(ref this.imagePath, value); }
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

		#region Commands

		public ICommand EditEmailCommand => new RelayCommand(EditEmail);
		public ICommand EditPasswordCommand => new RelayCommand(EditPassword);
		public ICommand EditImageCommand => new RelayCommand(EditImage);
		public ICommand SaveCommand => new RelayCommand(Save);
		public ICommand GoBackCommand => new RelayCommand(GoBack);

		#endregion Commands

		#endregion Properties
        
		#region Constructor

		public MyProfileViewModel()
		{
			//  Gets an new instance the service class
            this.apiService = new ApiService();
            this.dialogService = new DialogService();
            this.navigationService = new NavigationService();

			//  Gets an instance of the MainViewModel
			this.mainViewModel = MainViewModel.GetInstance();

			//  Define the status of the controls
            this.SetStatusControl(true, false, 0);

			//  Load values in the controls
			this.LoadValue();
		}

		#endregion Constructor

		#region Methods
        
        private void Save()
        {
            throw new NotImplementedException();
        }

        private void EditImage()
        {
            throw new NotImplementedException();
        }

        private void EditPassword()
        {
            throw new NotImplementedException();
        }

        private void EditEmail()
        {
            throw new NotImplementedException();
        }

        private void LoadValue()
        {
            this.FirstName = this.mainViewModel.UserData.FirstName;
            this.LastName = this.mainViewModel.UserData.LastName;
            this.Email = this.mainViewModel.UserData.Email;
            this.Telephone = this.mainViewModel.UserData.Telephone;
            this.ImagePath =
                string.IsNullOrEmpty(this.mainViewModel.UserData.ImagePath) ?
                    "NoImageLogo" :
                        this.mainViewModel.UserData.ImagePath;

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