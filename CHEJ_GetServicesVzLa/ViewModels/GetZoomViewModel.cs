namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System.Collections.ObjectModel;
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Helpers;
	using CHEJ_GetServicesVzLa.Models;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class GetZoomViewModel : BaseViewModel
    {
		#region Attributes

		#region Services

		private ApiService apiService;
		private DialogService dialogService;
		private NavigationService navigationService;

		#endregion Services

		private string trackId;
		private string reference;
		private string status;
		private string shippingType;
		private string date;
		private string origin;
		private string destination;
		private bool isValid;
		private string messageLabel;
		private string messageLabelTextColor;
		private bool isEnabled;
		private bool isRunning;
		private MainViewModel mainViewModel;
		private ZoomItemViewModel zoomItemViewModel;
		public Zoom zoom;
		//  private ObservableCollection<Seguimiento> tracking;

		#endregion Attributes

		#region Properties

		public string TrackId
        {
			get { return this.trackId; }
			set { SetValue(ref this.trackId, value); }
        }

		public string Reference
		{
			get { return this.reference; }
			set { SetValue(ref this.reference, value); }
		}

		public string Status
		{
			get { return this.status; }
			set { SetValue(ref this.status, value); }
		}

		public string ShippinigType
		{
			get { return this.shippingType; }
			set { SetValue(ref this.shippingType, value); }
		}

		public string Date
		{
			get { return this.date; }
			set { SetValue(ref this.date, value); }
		}

		public string Origin
		{
			get { return this.origin; }
			set { SetValue(ref this.origin, value); }
		}

		public string Destination
		{
			get { return this.destination; }
			set { SetValue(ref this.destination, value); }
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

		public string MessageLabelTextColor
		{
			get { return this.messageLabelTextColor; }
			set { SetValue(ref this.messageLabelTextColor, value); }
		}

		//public ObservableCollection<Seguimiento> Tracking
		//{
		//	get { return this.tracking; }
		//	set { SetValue(ref this.tracking, value); }
		//}

		#region Commands

		public ICommand GoBackCommand => new RelayCommand(GoBack);
		public ICommand GetZoomDetailsCommand => new RelayCommand(GetZoomDetails);
              
		#endregion Commands 

		#endregion Properties

		#region Constructor

		public GetZoomViewModel(ZoomItemViewModel _zoomItemViewModel)
		{
			//  Gets an instance of the ViewModels
			this.zoomItemViewModel = _zoomItemViewModel;
			this.mainViewModel = MainViewModel.GetInstance();
				
			//  Gets an instance of the services class
			this.apiService = new ApiService();
			this.dialogService = new DialogService();
			this.navigationService = new NavigationService();

			//  Load values in the control
            this.LoadValuesControls(0, null);

			//  Find data of the Zoom
			FindData();
		}

		#endregion Constructor

		#region Methods

        private async void FindData()
        {
            //  Sets status of controls
            this.SetStatusControl(false, true, "Green", 1);

            //  Validate the connections of internet
            var response = await this.apiService.CheckConnection();
            if (!response.IsSuccess)
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
				"/{0}", 
				this.zoomItemViewModel.Tracking);

            //  Get data of the cne api
            response = await apiService.Get<Zoom>(
                MethodsHelper.GetUrlIvss(),
                "/infove/zoom",
                "/seguimiento",
                ulrParameter);

            if (!response.IsSuccess)
            {
                //  Sets status of controls
                this.SetStatusControl(false, false, "Green", 0);

                await this.dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

			//  Convert result response
			this.zoom = (Zoom)response.Result;

			//  Load values in the control
			//  this.LoadValuesControls(1, (Zoom)response.Result);
			this.LoadValuesControls(1, this.zoom);

            //  Sets status of controls
            if (isValid)
            {
                this.SetStatusControl(true, false, "Green", 0);
            }
        }

		private void LoadValuesControls(int _option, Zoom _zoom)
        {
            if (_option == 0)
            {
				this.TrackId = string.Empty;
				this.Reference = string.Empty;
				this.Status = string.Empty;
				this.ShippinigType = string.Empty;
				this.Date = string.Empty;
				this.Origin = string.Empty;
				this.Destination = string.Empty;
				this.MessageLabel = string.Empty;
            }
            else
            {
				if(_zoom != null)
				{
					if (!_zoom.Descripcion.Contains(
                    "la Cedula no esta registrada como asegurado"))
                    {
                        this.TrackId = this.zoomItemViewModel.Tracking;
                        this.Reference = _zoom.Referencia;
                        this.Status =
                            MethodsHelper.TitleText(_zoom.Estatus.Trim());
                        this.ShippinigType =
                            MethodsHelper.TitleText(_zoom.TipoEnvio.Trim());
                        this.Date = _zoom.Fecha.Trim();
                        this.Origin =
                            MethodsHelper.TitleText(_zoom.Origen.Trim());
                        this.Destination = _zoom.Destino.Trim();

                        //if (_zoom.Seguimiento.Count > 0)
                        //{
                        //    Tracking =
                        //        new ObservableCollection<Seguimiento>(_zoom.Seguimiento);
                        //}
                        //else
                        //{
                        //    Tracking = new ObservableCollection<Seguimiento>();
                        //}
                        this.isValid = true;
                    }
                    else
                    {
                        this.MessageLabel = _zoom.Descripcion;
                        //  Sets status of controls
                        this.isValid = false;
                        this.SetStatusControl(true, false, "Red", -1);
                    }	
				}
				else
				{
					this.MessageLabel = 
						string.Format("{0}{1}{2}{3}", 
						              string.Format(
							              "El número: {0} de rastreo no está ", 
							              this.zoomItemViewModel.Tracking),
						              "registrado en el sistema o ha ingresado ",
						              "un numero de rastreo erroneo, ",
						              "favor verifique...!!!");
                    //  Sets status of controls
                    this.isValid = false;
					this.SetStatusControl(true, false, "Red", -1);
				}
            }
        }

		private async void GetZoomDetails()
        {
			if(this.isValid)
			{
				if (this.zoom.Seguimiento.Count > 0)
                {
                    //  Gets an instance of the GetZoomDetails
                    this.mainViewModel.GetZoomDetails =
                        new GetZoomDetailsViewModel(this);

                    //  Navegate to page GetZoomDetailsPage
                    await this.navigationService.NavigateOnMaster("GetZoomDetailsPage");
                }
                else
                {
                    await dialogService.ShowMessage(
                        "Information",
                        "No records have been added yet...!!!",
                        "Accept");
                    return;
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

        #endregion Methods
        
	}
}