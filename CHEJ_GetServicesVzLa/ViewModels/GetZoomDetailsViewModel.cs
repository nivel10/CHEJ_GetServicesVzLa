namespace CHEJ_GetServicesVzLa.ViewModels
{
	using System.Collections.ObjectModel;
	using CHEJ_GetServicesVzLa.Models;

	public class GetZoomDetailsViewModel : BaseViewModel
    {
		private ObservableCollection<Seguimiento> tracking;
		private GetZoomViewModel getZoomViewModel;

		public ObservableCollection<Seguimiento> Tracking
		{
			get { return this.tracking; }
			set { SetValue(ref this.tracking, value); }
		}

		public GetZoomDetailsViewModel(GetZoomViewModel _getZoomViewModel)
		{
			//  Get an instance of the GetZoomViewModel
			this.getZoomViewModel = _getZoomViewModel;

			//  Load data in the form
			LoadData();
		}

		private void LoadData()
		{
			this.Tracking = 
				new ObservableCollection<Seguimiento>(
					this.getZoomViewModel.zoom.Seguimiento);
		}
	}
}