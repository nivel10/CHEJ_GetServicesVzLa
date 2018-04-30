namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Helpers;
	using CHEJ_GetServicesVzLa.Models;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class EditCantvViewModel : BaseViewModel
	{
		#region Attributes

		private bool isEnabled;
		private string codePhone;
		private string nuberPhone;
		private bool isRunning;
		private string messageLabel;
		private CantvDataItemViewModel editCantv;
		private static CantvViewModel cantvViewModel;
		private static MainViewModel mainViewModel;

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

		public EditCantvViewModel(
			CantvDataItemViewModel _editCantvData)
		{         
			//  Load value object
			this.editCantv = _editCantvData;

			//  Define control format
			SetInitialize(1, _editCantvData);
			SetStatusControl(true, false, 0);

			//  Gets an instances of the services class
			apiservices = new ApiService();
			dialogService = new DialogService();
			navigationService = new NavigationService();

			//  Gets an instance of the CantvViewModel
			cantvViewModel = CantvViewModel.GetInstance();

			//  Gets an instance of the MainViewModel
			mainViewModel = MainViewModel.GetInstance();
		}

		#endregion Constructor

		#region Methods

		private async void GoBack()
		{
			await navigationService.GoBackOnMaster();
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
			if (!response.IsSuccess)
			{
				await dialogService.ShowMessage(
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
			if (!response.IsSuccess)
			{
				await dialogService.ShowMessage(
					"Error",
					response.Message,
					"Accept");
				return;
			}

			//  Define control format
			SetStatusControl(false, true, 1);

			response = await apiservices.CheckConnection();
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

			this.editCantv.CodePhone = this.CodePhone;
			this.editCantv.NumberPhone = this.NuberPhone;

			//  Save the data CatvData         
			response = await apiservices.Put<CantvDataItem>(
                MethodsHelper.GetUrlAPI(),
                "/api",
                "/CantvDatas",
				mainViewModel.Token.TokenType,
				mainViewModel.Token.AccessToken,
                this.ToCantvDataItem(this.editCantv));
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
                     
			//  Update record         
			cantvViewModel.UpdateCantvData(0, this.editCantv);

			//  Define control format
			SetStatusControl(true, false, 0);

			//  Navigate to back
			await navigationService.GoBackOnMaster();
		}

		private CantvDataItem ToCantvDataItem(CantvDataItemViewModel _editCantv)
		{
			return new CantvDataItem 
			{ 
				CantvDataId = _editCantv.CantvDataId,
				CodePhone = _editCantv.CodePhone,
				NumberPhone = _editCantv.NumberPhone,
				UserId = mainViewModel.UserData.UserId,
			};
		}

		private void SetInitialize(
			int _option, 
            CantvDataItemViewModel _editCantvData)
        {
			if(_option == 0)
			{
				this.CodePhone = "";
                this.NuberPhone = "";
                this.MessageLabel = "";
			}
			else if(_option == 1)
			{
				this.CodePhone = _editCantvData.CodePhone;
				this.NuberPhone = _editCantvData.NumberPhone;
			}         
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