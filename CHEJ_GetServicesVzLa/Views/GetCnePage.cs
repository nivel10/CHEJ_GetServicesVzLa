using System;

using Xamarin.Forms;

namespace CHEJ_GetServicesVzLa.Views
{
    public class GetCnePage : ContentPage
    {
        public GetCnePage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

