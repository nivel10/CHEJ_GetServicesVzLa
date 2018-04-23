namespace CHEJ_GetServicesVzLa
{
	using CHEJ_GetServicesVzLa.Views;
	using Xamarin.Forms;

	public partial class App : Application
    {
        #region Properties

		public static NavigationPage Navigator
		{
			get;
			internal set;
		}

		public static MasterPage Master 
		{ 
			get; 
			internal set; 
		}

		#endregion Properties

		#region Constructor

		public App()
		{
			InitializeComponent();

			//  MainPage = new MainPage();
			//  MainPage = new LoginPage();
			this.MainPage = new NavigationPage(new LoginPage());
			//  this.MainPage = new MasterPage();
		}

		#endregion Constructor

		#region Methods

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

        #endregion Methods
    }
}
