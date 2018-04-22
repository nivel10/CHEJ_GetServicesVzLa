﻿namespace CHEJ_GetServicesVzLa
{
	using CHEJ_GetServicesVzLa.Views;
	using Xamarin.Forms;

	public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

			//  MainPage = new MainPage();
			//  MainPage = new LoginPage();
			this.MainPage = new NavigationPage(new LoginPage());
        }

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
    }
}
