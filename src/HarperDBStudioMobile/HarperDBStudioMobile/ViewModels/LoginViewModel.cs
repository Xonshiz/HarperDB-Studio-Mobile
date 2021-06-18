using HarperDBStudioMobile.Interfaces;
using HarperDBStudioMobile.Views;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HarperDBStudioMobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        IRestClient restClient;
        //ObservableCollection<CredentialsModel>();
        //public event PropertyChangedEventHandler PropertyChanged;
        private string username = null;
        private string password = null;
        private string loginMessage = null;
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
            }
        }
        //public string LoginMessage
        //{
        //    get { return loginMessage; }
        //    set
        //    {
        //        loginMessage = value; RaisePropertyChanged();
        //    }
        //}
        //public Command LoginCommand { get; }

        public ICommand LoginCommand
        {
            get;
            private set;
        }

        //protected void RaisePropertyChanged([CallerMemberName] string caller = "")
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(caller));
        //    }
        //}

        public LoginViewModel()
        {
            //LoginCommand = new Command(OnLoginClicked);
            LoginCommand = new Command((e) =>
            {
                var x = e;
                //var item = (e as CredentialsModel);
                ////TODO: LOGIN TO YOUR SYSTEM
                //loginMessage = string.Concat("Login successful for user: ",
                //item.Username);
            });
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
