using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CHEJ_GetServicesVzLa.Models;

namespace CHEJ_GetServicesVzLa.ViewModels
{
	public class CneViewModel : BaseViewModel
    {
		private List<CneIvssData> listCnes;      
		private ObservableCollection<CneIvssData> cnes;
		private MainViewModel mainViewModel;

		public ObservableCollection<CneIvssData> Cnes
		{
			get { return this.cnes; }
			set { SetValue(ref this.cnes, value); }
		}

		public CneViewModel()
        {
			//  Load values 
			this.LoadValues();
        }

		private void LoadValues()
		{
			// Gets an instance of the MainViewModel
			this.mainViewModel = MainViewModel.GetInstance();

			this.listCnes = mainViewModel.UserData.CneIvssDatas
				.Where(cniv => cniv.IsCne == true)
				.OrderBy(cniv => cniv.NationalityId)
				.ThenBy(cniv => cniv.IdentificationCard)
				.ToList();

			//  Load valuen in the ObservableCollection
			this.Cnes = new ObservableCollection<CneIvssData>(
				this.listCnes);
		}
	}
}
