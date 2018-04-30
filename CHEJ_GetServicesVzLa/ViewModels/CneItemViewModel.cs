﻿namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System;
	using System.Windows.Input;
	using CHEJ_GetServicesVzLa.Models;
	using CHEJ_GetServicesVzLa.Services;
	using GalaSoft.MvvmLight.Command;

	public class CneItemViewModel : CneIvssData
    {
		#region Attributes

		private MainViewModel mainViewModel;
		private NavigationService navigationService;

		#endregion Attributes

		#region Properties

		public ICommand GoNewCneCommand
		{
			get { return new RelayCommand(GoNewCne); }
		}

		public ICommand EditCommand
		{
			get { return new RelayCommand(Edit); }
		}

		public ICommand DeleteCommand
		{
			get { return new RelayCommand(Delete); }
		}

		#endregion Properties

		#region Constructor

		public CneItemViewModel()
		{
			//  Gets an instance of the MainViewModel
			this.mainViewModel = MainViewModel.GetInstance();

			//  Gets an instance of the service class
			navigationService = new NavigationService();
		}

		#endregion Constructor

		#region Methods

		private void Delete()
		{
			throw new NotImplementedException();
		}

		private async void GoNewCne()
		{
			//  Gets an instance of the NewCne
			this.mainViewModel.NewCne = new NewCneViewModel();

			//  Navigate to the page NewCnePage
			await this.navigationService.NavigateOnMaster("NewCnePage");
		}

		private void Edit()
		{
			throw new NotImplementedException();
		}

		#endregion Methods
    }
}