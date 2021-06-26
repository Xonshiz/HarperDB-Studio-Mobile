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
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HarperDBStudioMobile.Views
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        IRestClient restClient;
        RequestGetUserModel requestGetUserModel;
        private bool isCachedLogin = false;

        public ICommand RememberMeTapped { get; set; }

        public LoginPage()
        {
            InitializeComponent();
            var rememberMeLabelTap = new TapGestureRecognizer();
            rememberMeLabelTap.Tapped += (s, e) =>
            {
                this.remember_me_checkbox.IsChecked = !this.remember_me_checkbox.IsChecked;
            };
            remember_me_label.GestureRecognizers.Add(rememberMeLabelTap);

            restClient = RestService.For<IRestClient>(Utils.Utils.BASE_API_URL);
            Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
            this.requestGetUserModel = new RequestGetUserModel();
            user_email.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeNone);
            user_password.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeNone);

            //If we have cached credentials, try to log the user in.
            if (!String.IsNullOrWhiteSpace(LoggedInUser.LoginEmail) && !String.IsNullOrWhiteSpace(LoggedInUser.LoginPassword))
            {
                user_email.Text = LoggedInUser.LoginEmail;
                user_password.Text = LoggedInUser.LoginPassword;
                this.isCachedLogin = true;
                this.LoginUser();
            } else
            {
                this.isCachedLogin = false;
            }
        }

        private void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(user_email.Text) || String.IsNullOrWhiteSpace(user_password.Text))
            {
                return;
            }
            this.LoginUser();
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            
        }

        private async void LoginUser()
        {
            this.loadingFrame.IsVisible = true;
            this.loginFrame.IsVisible = false;

            requestGetUserModel.password = user_password.Text;
            requestGetUserModel.email = user_email.Text;
            requestGetUserModel.loggingIn = true;

            try
            {
                var loginInfo = await restClient.GetUser(requestGetUserModel);

                this.loadingFrame.IsVisible = false;
                this.loginFrame.IsVisible = true;


                if (loginInfo != null && loginInfo.IsSuccessStatusCode && loginInfo.Content.Body != null && loginInfo.Content.Body.user_id != null)
                {
                    //Success
                    LoggedInUser.BaseAuth = Utils.Utils.GetBasicAuthString(user_email.Text, user_password.Text);
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

                    if (remember_me_checkbox.IsChecked)
                    {
                        try
                        {
                            Preferences.Set(Utils.Utils.STORAGE_KEYS.BASE_EMAIL.ToString(), user_email.Text);
                            Preferences.Set(Utils.Utils.STORAGE_KEYS.BASE_PASSWORD.ToString(), user_password.Text);
                            Preferences.Set(Utils.Utils.STORAGE_KEYS.LOGGED_IN_USER_ORGS.ToString(), JsonConvert.SerializeObject(LoggedInUser.Orgs));
                        }
                        catch (Exception ex)
                        {
                            // Possible that device doesn't support secure storage on device.
                            await DisplayAlert("Can't Remember You", "Seems like we can't store info to remember you", "OK");
                        }
                    }

                    if (!this.isCachedLogin)
                    {
                        await DisplayAlert("Success", "You've Logged in " + loginInfo.Content.Body.firstname, "OK");
                    }
                    await Shell.Current.GoToAsync($"{nameof(Organizations)}");
                }
                else
                {
                    //Failure
                    await DisplayAlert("Failure", loginInfo.StatusCode.ToString(), "OK");
                    user_email.Text = String.Empty;
                    user_password.Text = String.Empty;
                    remember_me_checkbox.IsChecked = false;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Failure", ex.Message, "OK");
                return;
            }
        }
    }
}