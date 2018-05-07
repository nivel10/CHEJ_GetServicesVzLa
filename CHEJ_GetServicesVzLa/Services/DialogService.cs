namespace CHEJ_GetServicesVzLa.Services
{
	using System;
    using System.Threading.Tasks;
	using Xamarin.Forms;

	public class DialogService
    {
        public async Task ShowMessage(
			string _title, 
			string _message, 
			string _button)
		{
			await Application.Current.MainPage.DisplayAlert(
				_title, 
				_message, 
				_button);
		}

		public async Task<bool> ShowMessageConfirm(
			string _title, 
			string _message, 
			string _button01, 
			string _button02)
        {
			return await Application.Current.MainPage.DisplayAlert(
				_title, 
				_message, 
				_button01, 
				_button02);
        }
    }
}