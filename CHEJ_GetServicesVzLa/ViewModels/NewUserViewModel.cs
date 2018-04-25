namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System;
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Helpers;
	using CHEJ_GetServicesVzLa.Models;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class NewUserViewModel : BaseViewModel
	{
		#region Attributes

		private bool isEnabled;
		private string firtsName;
		private string lastName;
		private string email;
		private string password;
		private string confirm;
		private bool isRunning;
		private bool isVisible;
		private string message;

		#region Services

		private ApiService apiService;
		private DialogService dialogService;
		private NavigationService navigationService;

		#endregion Services

		#endregion Attributes

		#region Properties

		public bool IsEnabled
		{
			get { return this.isEnabled; }
			set { SetValue(ref this.isEnabled, value); }
		}

		public string FirtsName
		{
			get { return this.firtsName; }
			set { SetValue(ref this.firtsName, value); }
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

		public string Password
		{
			get { return this.password; }
			set { SetValue(ref this.password, value); }
		}

		public string Confirm
		{
			get { return this.confirm; }
			set { SetValue(ref this.confirm, value); }
		}

		public bool IsRunning
		{
			get { return this.isRunning; }
			set { SetValue(ref this.isRunning, value); }
		}

		public bool IsVisible
		{
			get { return this.isVisible; }
			set { SetValue(ref this.isVisible, value); }
		}

		public string Message
		{
			get { return this.message; }
			set { SetValue(ref this.message, value); }
		}

		#region Commands

		public ICommand RegisterCommand
		{
			get
			{
				return new RelayCommand(Register);
			}
		}

		public ICommand BackCommand
		{
			get
			{
				return new RelayCommand(Back);
			}
		}

		#endregion Commands

		#endregion Properties

		#region Constructor

		public NewUserViewModel()
		{
			//  Initialize the fields
			SetInitializaFields();

			//  Set status controls
			SetStatusControl(true, true, false, 0);

			//  Generate a instance of the services class
			apiService = new ApiService();
			dialogService = new DialogService();
			navigationService = new NavigationService();
		}

		#endregion Constructor

		#region Methods

		private void SetInitializaFields()
		{
			this.FirtsName = "";
			this.LastName = "";
			this.Email = "";
			this.Password = "";
			this.Confirm = "";
			this.Message = "";
		}

		private async void Register()
		{
			//  Validate the field of form
			var response = MethodsHelper.IsValidField(
				"S",
				3,
				20,
				"firts name",
				this.FirtsName,
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

			response = MethodsHelper.IsValidField(
				"S",
				3,
				20,
				"last name",
				this.LastName,
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

			response = MethodsHelper.IsValidField(
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
				6,
				10,
				"password",
				this.Password,
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

			response = MethodsHelper.IsValidField(
				"S",
				6,
				10,
				"password confirm",
				this.Confirm,
				true,
				true,
				this.Password);
			if (!response.IsSuccess)
			{
				await dialogService.ShowMessage(
					"Error",
					response.Message,
					"Accept");
				return;
			}

			//  Set status controls
			SetStatusControl(false, true, true, 1);
			
			response = await apiService.CheckConnection();
			if (!response.IsSuccess)
			{
				//  Set status controls
                SetStatusControl(true, true, false, 0);

				await dialogService.ShowMessage(
					"Error",
					response.Message,
					"Accept");
				return;
			}

			//  Use the user registration API         
			var user = new User
			{
				AppName = MethodsHelper.GetAppName(),
				Email = this.Email,
				FirstName = this.FirtsName,
				LastName = this.LastName,
				Password = this.Password,
				UserTypeId = Convert.ToInt32("5"),
			};

			response = await apiService.Post(
				MethodsHelper.GetUrlAPI(),
			    "/api",
			    "/Users",
			    user);
			if(!response.IsSuccess)
			{
				//  Set status controls
                SetStatusControl(true, true, false, 0);
				            
				await dialogService.ShowMessage(
					"Error", 
					response.Message, 
					"Accept");
				return;
			}

			//  Set status controls
            SetStatusControl(true, true, false, 0);

			//  Set Initialize the fields
			SetInitializaFields();

			//  Go back login
			await dialogService.ShowMessage(
				"Information", 
				string.Format(
					"{0}{1}", 
					"User registered successfully, you can now log in to the ",
					"application whit this username and password...!!!"),
				"Accept");

			await navigationService.GoBackOnLogin();
		}

		private async void Back()
		{
			//  Initialize the fields
			SetInitializaFields();

			//  Navigate to back page
			await navigationService.GoBackOnLogin();
		}

		private void SetStatusControl(
			bool _isEnabled,
			bool _isVisible,
			bool _isRunning,
			int _message)
		{
			this.IsEnabled = _isEnabled;
			this.IsVisible = _isVisible;
			this.IsRunning = _isRunning;
			if (this.isVisible)
			{
				switch(_message)
				{
					case 0:
						//this.Message = string.Format(
							//"{0}{1}",
							//"Remember to enter a valid email, as you must ",
							//"confirm the email in your account...!!!");
						this.Message = string.Empty;
						break;
					case 1:
                        this.Message = string.Format(
                            "{0}",
							"Wait a moment, we are processing your request...!!! ");
                        break;
				}
			}
		}

		#endregion Methods
	}
}