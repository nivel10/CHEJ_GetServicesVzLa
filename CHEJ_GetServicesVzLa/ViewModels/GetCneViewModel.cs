namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System;
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Helpers;
	using CHEJ_GetServicesVzLa.Models;
    using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class GetCneViewModel : BaseViewModel
    {
		#region Attributes
              
		#region Services

		private ApiService apiService;
		private DialogService dialogService;
		private NavigationService navigationService;

		#endregion Services

		private bool isValid;
		private string identificationCard;
		private string fullName;
		private string state;
		private string municipality;
		private string parish;
		private string center;
		private string address;
		private string messageLabel;
		private string messageLabelTextColor;
		private bool isEnabled;
		private bool isRunning;      
		private CneItemViewModel cneItemViewModel;

		#endregion Attributes
              
		#region Properties

		public string IdentificationCard
		{
			get { return this.identificationCard; }
			set { SetValue(ref this.identificationCard, value); }
		}

		public string FullName
		{
			get { return this.fullName; }
			set { SetValue(ref this.fullName, value); }
		}

		public string State
		{
			get { return this.state; }
			set { SetValue(ref this.state, value); }
		}

		public string Municipality
		{
			get { return this.municipality; }
			set { SetValue(ref this.municipality, value); }
		}

		public string Parish
		{
			get { return this.parish; }
			set { SetValue(ref this.parish, value); }
		}

		public string Center
		{
			get { return this.center; }
			set { SetValue(ref this.center, value); }
		}

		public string Address
		{
			get { return this.address; }
			set { SetValue(ref this.address, value); }
		}

		public string MessageLabel
		{
			get { return messageLabel; }
			set { SetValue(ref this.messageLabel, value); }
		}

		public string MessageLabelTextColor
		{
			get { return messageLabelTextColor; }
			set { SetValue(ref this.messageLabelTextColor, value); }
		}

		public bool IsEnabled
		{
			get { return isEnabled; }
			set { SetValue(ref this.isEnabled, value); }
		}

		public bool IsRunning
		{
			get { return isRunning; }
			set { SetValue(ref this.isRunning, value); }
		}

		public ICommand GoBackCommand
		{
			get { return new RelayCommand(GoBack); }         
		}

		#endregion Properties
              
		#region Constructor

		public GetCneViewModel(CneItemViewModel _cneItemViewModel)
		{
			//  Load value of the CneItemViewModel
			this.cneItemViewModel = _cneItemViewModel;

			//  Gets an instance of the services class
			this.apiService = new ApiService();
			this.dialogService = new DialogService();
			this.navigationService = new NavigationService();

			//  Sets status of controls
			this.SetStatusControl(true, false, "Green", 0);

			//  Initialize the values of the controls
			this.isValid = true;
			this.LoadValuesControls(0, null);

			//  Get data of the CNE
			FindDataCne();
		}

		#endregion Constructor

		private async void FindDataCne()
		{
			//  Sets status of controls
			this.SetStatusControl(false, true, "Green", 1);

            //  Validate the connections of internet
			var response = await this.apiService.CheckConnection();
			if(!response.IsSuccess)
			{
				//  Sets status of controls
				this.SetStatusControl(false, false, "Green", 0);
				await this.dialogService.ShowMessage(
					"Error", 
					response.Message, 
					"Accept");
				return;
			}

            //  Define the url parameter
			var ulrParameter = string.Format(
				"/{0}/{1}", 
				this.cneItemViewModel.NationalityDatas[0].Abbreviation,
				this.cneItemViewModel.IdentificationCard);
			                  
			//  Get data of the cne api
			response = await apiService.Get<Cne>(
				MethodsHelper.GetUrlCne(), 
				"/infove/cne", 
				"/elector", 
				ulrParameter);

			if(!response.IsSuccess)
			{
				//  Sets status of controls
				this.SetStatusControl(false, false, "Green", 0);

				await this.dialogService.ShowMessage(
					"Error", 
					response.Message, 
					"Accept");
				return;
			}

            //  Load values in the control
			this.LoadValuesControls(1, (Cne)response.Result);

			//  Sets status of controls
			if(isValid)
			{
				this.SetStatusControl(true, false, "Green", 0);
			}         
		}

		private void LoadValuesControls(int _option, Cne _cneData)
        {
            if (_option == 0)
            {
				this.IdentificationCard = string.Empty;
                this.FullName = string.Empty;
                this.State = string.Empty;
                this.Municipality = string.Empty;
                this.Parish = string.Empty;
                this.Center = string.Empty;
                this.Address = string.Empty;
            }
            else
            {
				if(!_cneData.Descripcion.Contains("El usuario no se encuentra"))
				{
					this.IdentificationCard = _cneData.Cedula;
                    this.FullName = _cneData.Nombre.ToLower().Trim();
                    this.State = _cneData.Estado.ToLower().Trim();
                    this.Municipality = _cneData.Municipio.ToLower().Trim();
                    this.Parish = _cneData.Parroquia.ToLower().Trim();
                    this.Center = _cneData.Centro.ToLower().Trim();
                    this.Address = _cneData.Direccion.ToLower().Trim();
				}
				else
				{
					this.MessageLabel = _cneData.Descripcion;
					//  Sets status of controls
					this.isValid = false;
					this.SetStatusControl(true, false, "Red", -1);               
				}            
            }
        }

		private async void GoBack()
        {
            this.LoadValuesControls(0, null);
            await navigationService.GoBackOnMaster();
        }

        private void SetStatusControl(
            bool _isEnabled,
            bool _isRunning,
			string _textColor,
            int _messageLabe)
        {
            this.IsEnabled = _isEnabled;
            this.IsRunning = _isRunning;
			this.MessageLabelTextColor = _textColor;
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
	}
}