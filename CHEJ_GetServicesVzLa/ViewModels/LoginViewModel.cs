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
			SetStatusControl(true, true, false);

			//  Instancia las clase de servicios
			dialogService = new DialogService();
			apiService = new ApiService();
			navigationService = new NavigationService();
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
			SetStatusControl(false, true, true);

			//  Validate if there is an internet connection
			var resposne = await apiService.CheckConnection();
			if(!resposne.IsSuccess)
			{
				//  Establece el estatus de los controles
                SetStatusControl(true, true, false);

				await dialogService.ShowMessage(
					"Error", 
					resposne.Message, 
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
                    SetStatusControl(true, true, false);

					await dialogService.ShowMessage(
						"Error", 
						token.ErrorDescription, 
						"Accept");
					return;
				}
			}
			else            
			{
				//  EStablishes the status of controls
                SetStatusControl(true, true, false);

				await dialogService.ShowMessage(
					"Error", 
					"An error has occurred, try later...!!!", 
					"Accept");
				return;
			}

			//  Get new instance of ViewModel (Token)
			MainViewModel.GetInstance().Token = token;

			//  Get new instance of MenuViewModel
			MainViewModel.GetInstance().Menu = new MenuViewModel();

			//  EStablishes the status of controls
			SetInitialize();
            SetStatusControl(true, true, false);

			//  Navigate to the page
			//  await navigationService.NavigateOnMaster("MenuPage");

			//  Define the MainPage
			navigationService.SetMainPage("MasterPage");
        }
        
		private async void Register()
        {
			//  EStablishes the status of controls
            SetInitialize();
            SetStatusControl(true, true, false);

            // Instance the class NewUserView Model
			MainViewModel.GetInstance().NewUser = new NewUserViewModel();
            
            //  Navigate to the page register
			await navigationService.NavigateOnLogin("NewUserPage");
        }

		private async void About()
        {
			//  EStablishes the status of controls
            SetInitialize();
            SetStatusControl(true, true, false);

			//  Instance the class AboutViewModel
			MainViewModel.GetInstance().About = new AboutViewModel();

			//  Navigate to the page AboutPage
			await navigationService.NavigateOnLogin("AboutPage");
        }

		private async void ForgotPassword()
        {
			//  EStablishes the status of controls
            SetInitialize();
            SetStatusControl(true, true, false);

			//  Instance the class RecoveryViewModel
			MainViewModel.GetInstance().Recovery = new RecoveryViewModel();

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
			bool _isRunning)
		{
			this.IsEnabled = _isEnabled;
			this.IsRemembered = _isRemembered;
			this.IsRunning = _isRunning;
		}

		#endregion Methods
	}
}