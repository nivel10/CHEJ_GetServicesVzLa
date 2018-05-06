namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Helpers;
	using CHEJ_GetServicesVzLa.Models;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class GetCantvViewModel : BaseViewModel
	{
		#region Attributes

		private CantvDataItemViewModel cantvDataItemView;
		private string phoneNumber;
		private string currentBalance;
		private string lastBilling;
		private string cuteDate;
		private string expirateDate;
		private string expirateBalance;
		private string lastPayment;      
        private string messageLabel;
		private bool isEnabled;
		private bool isRunning;

		#region Services

		private ApiService apiService;
		private DialogService dialogService;
		private NavigationService navigationService;

		#endregion Services 

		#endregion Attributes

		#region Properties

		public string PhoneNumber
		{
			get { return this.phoneNumber; }
			set { this.SetValue(ref this.phoneNumber, value); }
		}

		public string CurrentBalance
		{
			get { return this.currentBalance; }
			set { this.SetValue(ref this.currentBalance, value); }
		}

		public string LastBilling
		{
			get { return this.lastBilling; }
			set { this.SetValue(ref this.lastBilling, value); }
		}

		public string CutDate
		{
			get { return this.cuteDate; }
			set { this.SetValue(ref this.cuteDate, value); }
		}

		public string ExpirateDate
		{
			get { return this.expirateDate; }
			set { this.SetValue(ref this.expirateDate, value); }
		}

		public string ExpirateBalance
		{
			get { return this.expirateBalance; }
			set { this.SetValue(ref this.expirateBalance, value); }
		}

		public string LastPayment
		{
			get { return this.lastPayment; }
			set { this.SetValue(ref this.lastPayment, value); }
		}

		public bool IsRunning
		{
			get { return this.isRunning; }
			set { this.SetValue(ref this.isRunning, value); }
		}

		public string MessageLabel
		{
			get { return this.messageLabel; }
			set { this.SetValue(ref this.messageLabel, value); }
		}

		public bool IsEnabled
		{
			get { return this.isEnabled; }
			set { this.SetValue(ref this.isEnabled, value); }
		}

		#region Commands

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

		public GetCantvViewModel(
			CantvDataItemViewModel _cantvDataItemViewModel)
		{
			//  Generate an instance of the services class
			apiService = new ApiService();
			dialogService = new DialogService();
			navigationService = new NavigationService();

			//  Load values data of CantvDataItemViewModel
			this.cantvDataItemView = _cantvDataItemViewModel;

			//  Search data Cantv
			FindDataCantv();
		}

		#endregion Constructor

		#region Methods

		private async void FindDataCantv()
		{
			//  Asing status to the controls
			this.SetStatusControl(false, true, 1);

			//  Validat if the connection es successfully
			var response = await apiService.CheckConnection();
			if (!response.IsSuccess)
			{
				//  Asing status to the controls
				SetStatusControl(false, false, 0);

				await dialogService.ShowMessage(
					"Error",
					response.Message,
					"Accept");

                //  Navigate on back
				GoBack();
				return;
			}

			//  Gets the data of Cantv
			response = await apiService.Get<Cantv>(
				MethodsHelper.GetUrlCantv(),
				"/infove/cantv",
				"/deuda",
				string.Format(
					"/{0}/{1}", 
					this.cantvDataItemView.CodePhone, 
					this.cantvDataItemView.NumberPhone));
			if (!response.IsSuccess)
			{
				//  Asing status to the controls
                this.SetStatusControl(false, false, 0);

				if(response.Message.Contains(
					"No se ha indicado el número de teléfono"))
				{
					await dialogService.ShowMessage(
                    "Error",
                    "No se ha indicado el número de télefono",
                    "Accept");
				}
				else
				{
					await dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
				}
                            
				//  Navigate on Back
				this.GoBack();
				return;
			}
            
			//  Load values in the controsl
			this.LoadValuesControls((Cantv)response.Result, 1);

			//  Asing status to the controls
			this.SetStatusControl(false, false, 0);
		}
        
		private void LoadValuesControls(Cantv _cantvData, int _option)
		{
			if(_option == 0)
			{
				this.PhoneNumber = string.Empty;
				this.CurrentBalance = string.Empty;
				this.LastBilling = string.Empty;
				this.CutDate = string.Empty;
				this.ExpirateDate = string.Empty;
				this.ExpirateBalance = string.Empty;
				this.LastPayment = string.Empty;
			}
			else
			{
				this.PhoneNumber = this.cantvDataItemView.GetFullCantvData;
                this.CurrentBalance = _cantvData.SaldoActual;
                this.LastBilling = _cantvData.UltimaFacturacion;
				this.CutDate = _cantvData.FechaCorte;
				this.ExpirateDate = _cantvData.FechaVencimiento;
				this.ExpirateBalance = _cantvData.SaldoVencido;
				this.LastPayment = _cantvData.UltimoPago;
			}         
		}

		private async void GoBack()
        {
			this.LoadValuesControls(null, 0);
			await navigationService.GoBackOnMaster();
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