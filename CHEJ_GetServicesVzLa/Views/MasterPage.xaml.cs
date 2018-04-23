namespace CHEJ_GetServicesVzLa.Views
{  
	using Xamarin.Forms;

	public partial class MasterPage : MasterDetailPage
    {
        public MasterPage()
        {
            InitializeComponent();
        }

        //  This method makes the navigator persistent
		protected override void OnAppearing()
		{
			base.OnAppearing();

            //  Create object to navigation
			App.Navigator = this.Navigator;

			//  Create propertie to hide menu automaty
			App.Master = this;
		}
	}
}
