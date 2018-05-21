namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Helpers;
	using CHEJ_GetServicesVzLa.Models;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;
	using Plugin.Media;
	using Plugin.Media.Abstractions;
	using Xamarin.Forms;

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
		private bool isEnabled;
        private bool isRunning;
		private string messageLabel;
		private ImageSource imageSource;
		private MediaFile file;

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

		public ImageSource ImageSource
		{
			get { return this.imageSource; }
			set { SetValue(ref this.imageSource, value); }
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
        
        private async void Save()
        {
			//  Validate the field of form
            var response = MethodsHelper.IsValidField(
                "S",
                3,
                20,
                "firts name",
                this.FirstName,
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

            //  Get image
			byte[] imageArray = null;
			if(file != null)
			{
				imageArray = FilesHelper.ReadFully(file.GetStream());
				//  file.Dispose();
			}

			//  Set status controls
            SetStatusControl(false, true, 1);

            response = await apiService.CheckConnection();
            if (!response.IsSuccess)
            {
                //  Set status controls
				SetStatusControl(true, false, 0);

                await dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }
                     
			//  Use the user registration API         
            var userEdit = new UserEdit
            {
                AppName = MethodsHelper.GetAppName(),
                Email = this.Email,
                FirstName = this.FirstName,
				ImageArray = imageArray,
				ImagePath = this.mainViewModel.UserData.ImagePath,
                LastName = this.LastName,            
				Password = this.mainViewModel.UserData.Password,            
				Telephone = this.Telephone,
				UserId = this.mainViewModel.UserData.UserId,
                UserTypeId = 5,
            };

			response = await apiService.Put<UserEdit>(
                MethodsHelper.GetUrlAPI(),
                "/api",
                "/Users",
				this.mainViewModel.Token.TokenType,
				this.mainViewModel.Token.AccessToken,
				userEdit);
            if (!response.IsSuccess)
            {
                //  Set status controls
				this.SetStatusControl(true, false, 0);
                
                await dialogService.ShowMessage(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

			//  Update data Image
			this.mainViewModel.UserData.FirstName = this.FirstName;
			this.mainViewModel.UserData.LastName = this.LastName;
			this.mainViewModel.UserData.Telephone = this.Telephone;
			this.mainViewModel.UserData.ImagePath = userEdit.ImagePath;

            //  Set status controls
            this.SetStatusControl(true, false, 0);

            //  Go back login
            await dialogService.ShowMessage(
				"Information",
				"Infromation updated successfully...!!!",
                "Accept");         
        }

        private async void EditPassword()
        {
			//  Gets an instance of the EditPasswordViewModel
			this.mainViewModel.EditPassword = new EditPasswordViewModel();

            //  Vanigate to the page EditPassword
            await this.navigationService.NavigateOnMaster("EditPasswordPage");
        }

        private async void EditEmail()
        {
			//  Gets an instance of the EditMailViewModel
			this.mainViewModel.EditEmail = new EditEmailViewModel();

			//  Vanigate to the page EditEmailPage
			await this.navigationService.NavigateOnMaster("EditEmailPage");
        }

        private void LoadValue()
        {
            this.FirstName = this.mainViewModel.UserData.FirstName;
            this.LastName = this.mainViewModel.UserData.LastName;
            this.Email = this.mainViewModel.UserData.Email;
            this.Telephone = this.mainViewModel.UserData.Telephone;
			this.ImageSource =
                string.IsNullOrEmpty(this.mainViewModel.UserData.ImagePath) ?
				    "NoImageLogo.png" :
                        this.mainViewModel.UserData.ImageFullPath;         
        }

		private async void EditImage()
        {
            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsCameraAvailable &&
                CrossMedia.Current.IsTakePhotoSupported)
            {
                var source = await dialogService.ShowImageOptions();

                if (source == "Cancel")
                {
                    file = null;
                    return;
                }

                if (source == "From Camera")
                {
					file = await CrossMedia.Current.TakePhotoAsync(
					    new StoreCameraMediaOptions
					    { 
					        Directory = "Sample",
					        Name = "test.jpg",
					        PhotoSize = PhotoSize.Small,
					    }
					);
                }
                else
                {
                    file = await CrossMedia.Current.PickPhotoAsync();
                }
            }
            else
            {
                file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }


		private async void GoBack()
        {
			//  Dispose the object file
			if(file != null) 
			{
				file.Dispose();
			}

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