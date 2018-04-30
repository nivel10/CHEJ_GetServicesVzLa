namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System;
    using System.Windows.Input;
    using CHEJ_GetServicesVzLa.Services;
    using GalaSoft.MvvmLight.Command;

	public class NewCneViewModel : BaseViewModel
    {
		#region Attributes

        //  private MainViewModel mainViewModel;
        private NavigationService navigationService;
		private string nationality;
		private string identificationCard;
		private DateTime birthDay;

        #endregion Attributes

        #region Properties
        
		public string Nationality
        {
			get { return this.nationality; }
			set { SetValue(ref this.nationality, value); }
        }

		public string IdentificationCard
        {
			get { return this.identificationCard; }
			set { SetValue(ref this.identificationCard, value); }
        }

		public DateTime BirthDate
		{
			get { return this.birthDay; }
			set { SetValue(ref this.birthDay, value); }
		}

		#region Command

		public ICommand SaveCommand
		{
			get { return new RelayCommand(Save); }
		}

		public ICommand GoBackCommand
		{
			get { return new RelayCommand(GoBack); }
		}

		#endregion Command

		#endregion Properties

		#region Constructor

		public NewCneViewModel()
        {
            //  Gets an instance of the service class
            navigationService = new NavigationService();

			//  Load the value in the form
			LoadValues();
        }

		private void LoadValues()
		{
			this.Nationality = "";
			this.IdentificationCard = "";
			this.BirthDate = DateTime.Now.Date;
		}

		#endregion Constructor

		#region Methods

		private void Save()
        {
            throw new NotImplementedException();
        }

        private async void GoBack()
        {
            //  Navigate on back
            await navigationService.GoBackOnMaster();
        }

        #endregion Methods
    }
}