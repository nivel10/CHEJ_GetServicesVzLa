namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Helpers;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class LoginViewModel : BaseViewModel
    {      
		#region Attributes

		private string email;
		private string password;
		private bool isEnabled;
		private bool isRunning;
		private bool isRemembered;
		private string messageLabel;
		private MainViewModel mainViewModel;

		#region Services

		private DialogService dialogService;
		private ApiService apiService;
		private NavigationService navigationService;

		#endregion Services

		#endregion Attributes

		#region Properties

		public string Email
		{
			get { return this.email; }
			set { SetValue(ref this.email, value); }
		}

		public string Password
		{
			get { return this.password; }
			set { SetValue(ref this.password, value); }
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

		public bool IsRemembered
		{
			get { return this.isRemembered; }
			set { SetValue(ref this.isRemembered, value); }
		}

		public string MessageLabel
		{
			get { return this.messageLabel; }
			set { SetValue(ref this.messageLabel, value); }
		}

		#region Commands

		public ICommand LoginCommand => new RelayCommand(Login);      
		public ICommand RegisterCommand =>new RelayCommand(Register);
		public ICommand AboutCommand => new RelayCommand(About);      
		public ICommand ForgotPasswordCommand => new RelayCommand(ForgotPassword);
		public ICommand LoginFacebookCommand => new RelayCommand(LoginFacebook);
        
		#endregion Commands

		#endregion Properties

		#region Constructor

		public LoginViewModel()
		{
			//  Establece el estatus de los controles
			this.SetInitialize();
			this.SetStatusControl(true, true, false, 0);

			//  Instancia las clase de servicios
			this.dialogService = new DialogService();
			this.apiService = new ApiService();
			this.navigationService = new NavigationService();

            //  Delete this
			this.Email = "nikole.a.herrera.v@gmail.com";
			this.Password = "123456";
            
            //  Gets an instance of MainViewModel
			this.mainViewModel = MainViewModel.GetInstance();
		}
        
		#endregion Constructor
        
		#region Methods

        private async void LoginFacebook()
		{
			await this.navigationService.NavigateOnLogin("LoginFacebookPage");
			SetStatusControl(false, true, true, 1);
		}
              
		private async void Login()
        {
			//  Validate the fields of form
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
				await this.dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            var isValidEmail = MethodsHelper.IsValidEmail(this.Email);
            if (!isValidEmail)
            {
				await this.dialogService.ShowMessage(
                    "Error",
                    "You must enter an valid email",
                    "Accept");
                return;
            }

			response = MethodsHelper.IsValidField(
                "S",
                0,
                0,
                "password",
                this.Password,
                false,
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

			//  Establece el estatus de los controles
			this.SetStatusControl(false, true, true, 1);

			//  Validate if there is an internet connection
			response = await apiService.CheckConnection();
			if(!response.IsSuccess)
			{
				//  Establece el estatus de los controles
				this.SetStatusControl(true, true, false, 0);

				await dialogService.ShowMessage(
					"Error", 
					response.Message, 
					"Accept");
				return;
			}

			//  Get user Token
			var token = await this.apiService.GetToken(
				Email, 
				Password, 
				MethodsHelper.GetUrlAPI());

			if(token != null)
			{
				if(!string.IsNullOrEmpty(token.ErrorDescription))
				{
					//  EStablishes the status of controls
					this.SetStatusControl(true, true, false, 0);

					await this.dialogService.ShowMessage(
						"Error", 
						token.ErrorDescription, 
						"Accept");
					return;
				}
			}
			else            
			{
				//  Establishes the status of controls
				this.SetStatusControl(true, true, false, 0);

				await this.dialogService.ShowMessage(
					"Error", 
					"An error has occurred, try later...!!!", 
					"Accept");
				return;
			}

			//  Get new instance of ViewModel (Token)         
			this.mainViewModel.Token = token;
            
			//  Get new instance of MenuViewModel
			//  MainViewModel.GetInstance().Menu = new MenuViewModel();

			//  Establishes the status of controls
			this.SetInitialize();
			this.SetStatusControl(true, true, false, 0);

			//  Navigate to the page
			//  await navigationService.NavigateOnMaster("MenuPage");

			//  MainViewModel.GetInstance().Cantv = new CantvViewModel();   
			this.mainViewModel.Cantv = new CantvViewModel();

			//  Define the MainPage
			this.navigationService.SetMainPage("MasterPage");
        }

		private async void Register()
        {
			//  EStablishes the status of controls
			this.SetInitialize();
			this.SetStatusControl(true, true, false, 0);

            // Instance the class NewUserView Model
			//  MainViewModel.GetInstance().NewUser = new NewUserViewModel();
            
			// Instance the class NewUserView Model
            //  MainViewModel.GetInstance().NewUser = new NewUserViewModel();
			this.mainViewModel.NewUser = new NewUserViewModel();

            //  Navigate to the page register
			await this.navigationService.NavigateOnLogin("NewUserPage");
        }

		private async void About()
        {
			//  EStablishes the status of controls
			this.SetInitialize();
			this.SetStatusControl(true, true, false, 0);

			//  Navigate to the page AboutPage
			await this.navigationService.NavigateOnLogin("AboutPage");

			//  Instance the class AboutViewModel
			this.mainViewModel.About = new AboutViewModel();
        }

		private async void ForgotPassword()
        {
			//  EStablishes the status of controls
			this.SetInitialize();
			this.SetStatusControl(true, true, false, 0);

			//  Instance the class RecoveryViewModel         
			this.mainViewModel.Recovery = new RecoveryViewModel();

			//  Navigate to the page RecoveryPage
			await this.navigationService.NavigateOnLogin("RecoveryPage");
        }

		private void SetInitialize()
        {
			this.Email = "";
			this.Password = "";
        }

		public void SetStatusControl(
			bool _isEnabled,
			bool _isRemembered,
			bool _isRunning,
			int _messageLabe)
		{
			this.IsEnabled = _isEnabled;
			this.IsRemembered = _isRemembered;
			this.IsRunning = _isRunning;
			switch(_messageLabe)
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