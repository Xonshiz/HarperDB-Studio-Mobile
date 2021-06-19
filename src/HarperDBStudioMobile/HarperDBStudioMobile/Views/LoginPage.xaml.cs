using HarperDBStudioMobile.Interfaces;
using HarperDBStudioMobile.Models;
using HarperDBStudioMobile.ViewModels;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HarperDBStudioMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        IRestClient restClient;
        RequestGetUserModel requestGetUserModel;

        public LoginPage()
        {
            InitializeComponent();
            Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
            this.requestGetUserModel = new RequestGetUserModel();
            user_email.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeNone);
            user_password.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeNone);

            user_email.Text = "";
            user_password.Text = "";
        }

        private async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(user_email.Text) || String.IsNullOrWhiteSpace(user_password.Text))
            {
                return;
            }
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            restClient = RestService.For<IRestClient>(Utils.Utils.BASE_API_URL);
            requestGetUserModel.password = user_password.Text;
            requestGetUserModel.email = user_email.Text;
            requestGetUserModel.loggingIn = true;
            
            try
            {
                var loginInfo = await restClient.GetUser(requestGetUserModel);
                if (loginInfo != null && loginInfo.IsSuccessStatusCode && loginInfo.Content.Body != null && loginInfo.Content.Body.user_id != null)
                {
                    //Success
                    LoggedInUser.Firstname = loginInfo.Content.Body.firstname;
                    LoggedInUser.Lastname = loginInfo.Content.Body.lastname;
                    LoggedInUser.Email = loginInfo.Content.Body.email;
                    LoggedInUser.UserId = loginInfo.Content.Body.user_id;
                    LoggedInUser.PrimaryCustomerId = loginInfo.Content.Body.primary_customer_id;
                    LoggedInUser.EmailBounced = loginInfo.Content.Body.email_bounced;
                    LoggedInUser.UpdatePassword = loginInfo.Content.Body.update_password;
                    LoggedInUser.GithubRepo = loginInfo.Content.Body.github_repo;
                    LoggedInUser.LastLogin = loginInfo.Content.Body.last_login;
                    LoggedInUser.Orgs = loginInfo.Content.Body.orgs;

                    await DisplayAlert("Success", "You've Logged in " + loginInfo.Content.Body.firstname, "OK");
                    await Shell.Current.GoToAsync($"{nameof(Organizations)}");
                }
                else
                {
                    //Failure
                    await DisplayAlert("Failure", loginInfo.StatusCode.ToString(), "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Failure", ex.Message, "OK");
                return;
            }
            
            //await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }
    }
}