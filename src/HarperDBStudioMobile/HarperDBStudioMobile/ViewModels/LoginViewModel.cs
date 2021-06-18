using HarperDBStudioMobile.Interfaces;
using HarperDBStudioMobile.Views;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HarperDBStudioMobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        IRestClient restClient;
        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            restClient = RestService.For<IRestClient>(Utils.Utils.BASE_API_URL);
            //var loginInfo = await restClient.GetUser("");
            //await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }
    }
}
