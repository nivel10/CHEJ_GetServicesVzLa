namespace CHEJ_GetServicesVzLa.Services
{
	using System;
	using System.Net.Http;
	using System.Text;
	using System.Threading.Tasks;
    using CHEJ_GetServicesVzLa.Models;
	using Newtonsoft.Json;
	using Plugin.Connectivity;

    public class ApiService
    {
		public async Task<Response> CheckConnection()
		{
			try
			{
				if(!CrossConnectivity.Current.IsConnected)
				{
					return new Response
					{
						IsSuccess = false,
						Message = "Plase turn on your internet settings...!!!",
						Result = null,
					};
				}

				var isRemoteReachabe = 
					await CrossConnectivity.Current.IsRemoteReachable("google.com");
				if(!isRemoteReachabe)
				{
					return new Response {
						IsSuccess = false,
						Message = "Plase check your internet connection...!!!",
						Result = null,
					};
				}

				return new Response 
				{
					IsSuccess = true,
					Message = "Connection to internet is Ok....!!!",
					Result = null,               
				};
			}
			catch(Exception ex)
			{
				return new Response
				{
					IsSuccess = false,
					Message = ex.Message,
					Result = null,
				};
			}
		}

		public async Task<TokenResponse> GetToken(
			string _userName, 
			string _userPasswor, 
			string _urlAPI)
		{
			try{
				
				var client = new HttpClient();
				client.BaseAddress = new Uri(_urlAPI);

				var response = await client.PostAsync(
					"Token",
					new StringContent(
						string.Format(
							"grant_type=password&username={0}&password={1}",
							_userName,
							_userPasswor),
						Encoding.UTF8,
						"application/x-www-form-urlencoded"));
				
				var resultJason = await response.Content.ReadAsStringAsync();
				var result = JsonConvert.DeserializeObject<TokenResponse>(resultJason);

				return result;
			}         
			catch(Exception ex)
			{
				return new TokenResponse
				{
					ErrorDescription = ex.Message,
				};
			}
		}
	}
}
