using System;
using System.Threading.Tasks;
using Android.App;
using CHEJ_GetServicesVzLa.Models;
using CHEJ_GetServicesVzLa.Services;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(
	typeof(CHEJ_GetServicesVzLa.Views.LoginFacebookPage),
	typeof(CHEJ_GetServicesVzLa.Droid.Implementations.LoginInstagramPageRenderer))]

namespace CHEJ_GetServicesVzLa.Droid.Implementations
{
	public class LoginInstagramPageRenderer : PageRenderer
    {
		public LoginPageRenderer()
        {
            var activity = this.Context as Activity;

			var auth = new OAuth2Authenticator(
                clientId: "", // your OAuth2 client id
                scope: "basic", // The scopes for the particular API you're accessing. The format for this will vary by API.
                authorizeUrl: new Uri("https://api.instagram.com/oauth/authorize/"), // the auth URL for the service
                redirectUrl: new Uri("http://www.chejconsultor.com.ve/")); // the redirect URL for the service

            auth.Completed += async (sender, eventArgs) =>
            {
                if (eventArgs.IsAuthenticated)
                {
                    var accessToken = eventArgs.Account.Properties["access_token"].ToString();
                    var profile = await GetInstagramProfileAsync(accessToken);
                    await App.NavigateToProfile(profile);
                }
                else
                {
                    App.HideLoginView();
                }
            };

            activity.StartActivity(auth.GetUI(activity));
        }
        
		private async Task<InstagramResponse> GetInstagramProfileAsync(string accessToken)
        {
            var requestUrl = string.Empty;
            requestUrl = "https://graph.facebook.com/v2.8/me/?fields=name,";
            requestUrl += "picture.width(999),cover,age_range,devices,email,";
            requestUrl += "gender,is_verified,birthday,languages,work,website,";
            requestUrl += "religion,location,locale,link,first_name,last_name,";
            requestUrl += "hometown&access_token=" + accessToken;

            var apiService = new ApiService();
            return await apiService.GetInstagram(accessToken);
        }
	}
}