namespace CHEJ_GetServicesVzLa.Helpers
{
	using System.Text.RegularExpressions;
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

        #endregion Methods
    }

}
