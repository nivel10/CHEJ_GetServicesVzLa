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
        
		public async Task<Response> Get<T>(
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

		public async Task<Response> Get<T>(
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

                var get = JsonConvert.DeserializeObject<T>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = get,
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

        public async Task<Response> Post<T>(
			string _urlPI,
            string _urlPrefix,
            string _urlController,
            string _tokenType,
            string _accessToken,
            T model)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(_urlPI);
                client.DefaultRequestHeaders.Authorization =
                          new AuthenticationHeaderValue(
						      _tokenType, 
						      _accessToken);
                var urlAPI = 
					string.Format("{0}{1}", _urlPrefix, _urlController);

                var request = JsonConvert.SerializeObject(model);

                var content = new StringContent(
					request, 
					Encoding.UTF8, 
					"application/json");

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
                    else if(result.Contains("There is already a record with the same name...!!!"))
                    {
                        return new Response
                        {
                            IsSuccess = false,
                            Message = string.Format(
                                "{0}{1}",
                                "here is already a record with the ",
                                "same name...!!!"),
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

		public async Task<Response> Put<T>(
			string _urlPI,
            string _urlPrefix,
            string _urlController,
            string _tokenType,
            string _accessToken,
            T model)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(_urlPI);
                client.DefaultRequestHeaders.Authorization =
                          new AuthenticationHeaderValue(
						      _tokenType, 
						      _accessToken);
				
                var urlAPI = string.Format(
					"{0}{1}/{2}", 
					_urlPrefix, 
					_urlController,
					model.GetHashCode());

                var request = JsonConvert.SerializeObject(model);

                var content = new StringContent(
					request, 
					Encoding.UTF8, 
					"application/json");

                var response = await client.PutAsync(urlAPI, content);

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
                    else if (result.Contains("There is already a record with the same name...!!!"))
                    {
                        return new Response
                        {
                            IsSuccess = false,
                            Message = string.Format(
                                "{0}{1}",
                                "here is already a record with the ",
                                "same name...!!!"),
                        };
                    }

                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                var editRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Edit record add is ok...!!!",
					Result = editRecord,
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
    
		public async Task<Response> Put<T>(
            string _urlPI,
            string _urlPrefix,
            string _urlController,
			string _urlParameter,
            string _tokenType,
            string _accessToken,
            T model)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(_urlPI);
                client.DefaultRequestHeaders.Authorization =
                          new AuthenticationHeaderValue(
                              _tokenType,
                              _accessToken);

                var urlAPI = string.Format(
					"{0}{1}/?{2}={3}",
                    _urlPrefix,
                    _urlController,
					_urlParameter,
                    model.GetHashCode());

                var request = JsonConvert.SerializeObject(model);

                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");

                var response = await client.PutAsync(urlAPI, content);

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
                    else if (result.Contains("There is already a record with the same name...!!!"))
                    {
                        return new Response
                        {
                            IsSuccess = false,
                            Message = string.Format(
                                "{0}{1}",
                                "here is already a record with the ",
                                "same name...!!!"),
                        };
                    }

                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                //var editRecord = JsonConvert.DeserializeObject<T>(result);

                //return new Response
                //{
                //    IsSuccess = true,
                //    Message = "Edit record add is ok...!!!",
                //    Result = editRecord,
                //};

				return new Response
                {
                    IsSuccess = true,
                    Message = "Edit record add is ok...!!!",
                    Result = null,
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
		
        public async Task<Response> Delete<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            T model)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokenType, accessToken);
                var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controller,
                    model.GetHashCode());
                var response = await client.DeleteAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record removed successfully...!!!",
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