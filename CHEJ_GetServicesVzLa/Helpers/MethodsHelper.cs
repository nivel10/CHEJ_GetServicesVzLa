namespace CHEJ_GetServicesVzLa.Helpers
{
	using System;
    using System.Text.RegularExpressions;
	using CHEJ_GetServicesVzLa.Models;
	using Xamarin.Forms;

    public class MethodsHelper
    {
        #region Methods

        public static bool IsValidEmail(string _email)
        {
            //  return _email != null && 
            //      Regex.IsMatch(_email, "^(([\\w-]+\\.)+[\\w-]+|([a-zA-Z]{1}|
            //  [\\w-]{2,}))@(([a-zA-Z]+[\\w-]+\\.){1,2}[a-zA-Z]{2,4})$");

            var lcVarAux001 = string.Empty;
            lcVarAux001 = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9";
            lcVarAux001 += @"!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-";
            lcVarAux001 += @"9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0";
            lcVarAux001 += @"-9])?)\Z";

            return Regex.IsMatch(
                _email,
                lcVarAux001,
                RegexOptions.IgnoreCase);
        }

        public static string GetUrlAPI()
        {
            return Application.Current.Resources["UrlAPI"].ToString().Trim();
        }
        
        public static Response IsValidField(
			string _typeField, 
			int _longInitial, 
			int _longFinal,
            string _nameField,
			string _valueFiled,
		    bool _validLong,
			bool _isPasswordConfirm,
		    string _password)
        {
    		//  The field is string value
    		if(_typeField == "S")
    		{
    			if(string.IsNullOrEmpty(_valueFiled))
    			{
    				return new Response
    				{
    					IsSuccess = false,
    					Message = string.Format(
    						"You must enter a {0}...!!!",
    						_nameField),
    				};
    			}

				if(_validLong)
				{
					if (_valueFiled.Length < _longInitial ||
                   _valueFiled.Length > _longFinal)
                    {
                        return new Response
                        {
                            IsSuccess = false,
                            Message = string.Format(
                                "The {0} only can contains between {1} and {2} characters...!!!",
                                _nameField, _longInitial, _longFinal),
                        };
                    }
				}

				if(_isPasswordConfirm)
				{
					if(!_valueFiled.Equals(_password))
					{
						return new Response
						{
							IsSuccess = false,
							Message = 
								"The password and confirm are not the same...!!!",
						};
					}
				}
			}

			return new Response
            {
                IsSuccess = true,
                Message = "The field is Ok....!!!",
            };
        }
        
        public static string GetAppName()
        {
			return Application.Current.Resources["AppName"].ToString().Trim();
        }

        #endregion Methods
    }

}
