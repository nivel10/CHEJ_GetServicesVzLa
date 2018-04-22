namespace CHEJ_GetServicesVzLa.Services
{
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
    }
}
