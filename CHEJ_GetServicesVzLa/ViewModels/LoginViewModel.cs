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

		public ICommand LoginCommand
		{
			get
			{
				return new RelayCommand(Login);
			}

		}

		public ICommand RegisterCommand
		{
			get
			{
				return new RelayCommand(Register);
			}

		}

		public ICommand AboutCommand
		{
			get
			{
				return new RelayCommand(About);
			}
		}

		public ICommand ForgotPasswordCommand
		{
			get
			{
				return new RelayCommand(ForgotPassword);
			}

		}
        
		#endregion Commands

		#endregion Properties

		#region Constructor

		public LoginViewModel()
		{
			//  Establece el estatus de los controles
			SetInitialize();
			SetStatusControl(true, true, false, 0);

			//  Instancia las clase de servicios
			dialogService = new DialogService();
			apiService = new ApiService();
			navigationService = new NavigationService();

            //  Delete this
			this.Email = "nikole.a.herrera.v@gmail.com";
			this.Password = "123456";

            //  Gets an instance of MainViewModel
			mainViewModel = MainViewModel.GetInstance();
		}
        
		#endregion Constructor

		#region Methods

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
                await dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

			//  Establece el estatus de los controles
			SetStatusControl(false, true, true, 1);

			//  Validate if there is an internet connection
			response = await apiService.CheckConnection();
			if(!response.IsSuccess)
			{
				//  Establece el estatus de los controles
                SetStatusControl(true, true, false, 0);

				await dialogService.ShowMessage(
					"Error", 
					response.Message, 
					"Accept");
				return;
			}

			//  Get user Token
			var token = await apiService.GetToken(
				Email, 
				Password, 
				MethodsHelper.GetUrlAPI());

			if(token != null)
			{
				if(!string.IsNullOrEmpty(token.ErrorDescription))
				{
					//  EStablishes the status of controls
                    SetStatusControl(true, true, false, 0);

					await dialogService.ShowMessage(
						"Error", 
						token.ErrorDescription, 
						"Accept");
					return;
				}
			}
			else            
			{
				//  Establishes the status of controls
                SetStatusControl(true, true, false, 0);

				await dialogService.ShowMessage(
					"Error", 
					"An error has occurred, try later...!!!", 
					"Accept");
				return;
			}

			//  Get new instance of ViewModel (Token)         
			mainViewModel.Token = token;
            
			//  Get new instance of MenuViewModel
			//  MainViewModel.GetInstance().Menu = new MenuViewModel();

			//  Establishes the status of controls
			SetInitialize();
			SetStatusControl(true, true, false, 0);

			//  Navigate to the page
			//  await navigationService.NavigateOnMaster("MenuPage");

			//  MainViewModel.GetInstance().Cantv = new CantvViewModel();   
			mainViewModel.Cantv = new CantvViewModel();         

			//  Define the MainPage
			navigationService.SetMainPage("MasterPage");
        }

		private async void Register()
        {
			//  EStablishes the status of controls
            SetInitialize();
            SetStatusControl(true, true, false, 0);

            // Instance the class NewUserView Model
			//  MainViewModel.GetInstance().NewUser = new NewUserViewModel();
            
			// Instance the class NewUserView Model
            //  MainViewModel.GetInstance().NewUser = new NewUserViewModel();
			mainViewModel.NewUser = new NewUserViewModel();

            //  Navigate to the page register
			await navigationService.NavigateOnLogin("NewUserPage");
        }

		private async void About()
        {
			//  EStablishes the status of controls
            SetInitialize();
            SetStatusControl(true, true, false, 0);

			//  Navigate to the page AboutPage
			await navigationService.NavigateOnLogin("AboutPage");

			//  Instance the class AboutViewModel
			mainViewModel.About = new AboutViewModel();
        }

		private async void ForgotPassword()
        {
			//  EStablishes the status of controls
            SetInitialize();
            SetStatusControl(true, true, false, 0);

			//  Instance the class RecoveryViewModel         
			mainViewModel.Recovery = new RecoveryViewModel();

			//  Navigate to the page RecoveryPage
			await navigationService.NavigateOnLogin("RecoveryPage");
        }

		private void SetInitialize()
        {
			this.Email = "";
			this.Password = "";
        }

		private void SetStatusControl(
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