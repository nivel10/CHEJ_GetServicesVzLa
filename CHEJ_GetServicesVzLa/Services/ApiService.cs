namespace CHEJ_GetServicesVzLa.Services
{
	using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
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
                if (!CrossConnectivity.Current.IsConnected)
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
                if (!isRemoteReachabe)
                {
                    return new Response
                    {
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
            catch (Exception ex)
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
            try
            {

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
            catch (Exception ex)
            {
                return new TokenResponse
                {
                    ErrorDescription = ex.Message,
                };
            }
        }
        
		public async Task<Response> GetData<T>(
			string _urlApi, 
			string _urlPrefix, 
			string _urlController,
		    string _urlParameter)
        {
			try{

				var client = new HttpClient();
				client.BaseAddress = new Uri(_urlApi);
				var ulrBase = string.Format(
					"{0}{1}{2}",
					_urlPrefix,
					_urlController,
				    _urlParameter);

				var response = await client.GetAsync(ulrBase);
                
				var result = await response.Content.ReadAsStringAsync();

				if(!response.IsSuccessStatusCode)
				{
					return new Response
					{
						IsSuccess = false,
						Message = result,
					};
				}

				var data = JsonConvert.DeserializeObject<T>(result);

				return new Response
				{
					IsSuccess = true,
					Message = "Restful get is ok...!!!",
					Result = data,
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

        public async Task<Response> Post<T>(
            string _urlPI,
            string _urlPrefix,
            string _urlController,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);

                var content =
                    new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.BaseAddress = new Uri(_urlPI);

                var urlAPI = string.Format("{0}{1}", _urlPrefix, _urlController);

                var response = await client.PostAsync(urlAPI, content);

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    if (result.Contains("No se encuentra el recurso"))
                    {
                        return new Response
                        {
                            IsSuccess = false,
                            Message = string.Format(
                                "{0}{1}",
                                "Sorry, the system is currently ",
                                "down. Try later...!!!"),
                        };
                    }
                    else if (result.Contains("The email you are using"))
                    {
                        return new Response
                        {
                            IsSuccess = false,
                            Message = string.Format(
                                "{0}{1}",
                                "The email you are using is already ",
                                "registered...!!!"),
                        };
                    }

                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "New record add is ok...!!!",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> GetList<T>(
            string _urlBase,
            string _servicePrefix,
            string _controller,
            string _parameter,
            string _tokenType,
            string _accessToken)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(_tokenType, _accessToken);
                client.BaseAddress = new Uri(_urlBase);

                var url = string.Format(
                    "{0}{1}{2}",
                    _servicePrefix,
                    _controller,
                    _parameter);
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}