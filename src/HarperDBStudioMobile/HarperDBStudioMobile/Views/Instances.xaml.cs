using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HarperDBStudioMobile.Interfaces;
using HarperDBStudioMobile.Models;
using HarperDBStudioMobile.ViewModels;
using Refit;
using Xamarin.Forms;

namespace HarperDBStudioMobile.Views
{
    public partial class Instances : ContentPage
    {
        IRestClient restClient, instanceRestClient;
        public ICommand CarouselItemTapped { get; set; }

        RequestOperationsModel requestOperationsModel = new RequestOperationsModel();
        ObservableCollection<InstanceModel> instanceModels = new ObservableCollection<InstanceModel>() { };

        public Instances()
        {
            InitializeComponent();
            instance_username.Text = Utils.Utils.instance_username;
            instance_password.Text = Utils.Utils.instance_password;

            restClient = RestService.For<IRestClient>(Utils.Utils.BASE_API_URL);
            instance_username.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeNone);
            instance_password.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeNone);

            InstanceCarousel.ItemsSource = instanceModels;
            InstanceCarousel.TabIndex = 0;
            this.GetInstanceDetails();
            CarouselItemTapped = new Xamarin.Forms.Command(async (selectItem) => {
                if (selectItem == null)
                    return;

                InstanceLoginStackLayout.IsVisible = true;
                InstanceCarousel.IsVisible = false;

                var selectedRequest = selectItem as InstanceModel;
                LoggedInUserCurrentSelections.INSTANCE_BASE_URL = selectedRequest.url;
                //Can add the auth to DICTIONARY here.

                //LoggedInUserCurrentSelections.current_organization_customer_id = selectedRequest.customer_id;
                //await Shell.Current.GoToAsync($"{nameof(Instances)}");

            });
        }

        private void InstanceCarousel_PositionChanged(object sender, PositionChangedEventArgs e)
        {
            Console.WriteLine("CurrentItem's CurrentPosition:" + e.CurrentPosition);
        }

        private async void GetInstanceDetails()
        {
            RequestGetInstancesModel requestGetInstancesModel = new RequestGetInstancesModel();
            requestGetInstancesModel.customer_id = LoggedInUserCurrentSelections.current_organization_customer_id;
            requestGetInstancesModel.user_id = LoggedInUser.UserId;

            try
            {
                var instanceList = await restClient.GetInstances(LoggedInUser.BaseAuth, requestGetInstancesModel);
                if (instanceList != null && instanceList.IsSuccessStatusCode && instanceList.Content.body != null)
                {
                    instanceModels.Clear();
                    foreach (InstanceModel instance in instanceList.Content.body)
                    {
                        instanceModels.Add(instance);
                    }
                }
                else
                {
                    //Failure
                    await DisplayAlert("Failure", instanceList.StatusCode.ToString(), "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Failure", ex.Message, "OK");
                return;
            }
        }

        private async void instance_submit_button_Clicked(System.Object sender, System.EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(instance_username.Text) || String.IsNullOrWhiteSpace(instance_password.Text))
            {
                return;
            }
            try
            {
                var basicAuth = Utils.Utils.GetBasicAuthString(instance_username.Text, instance_password.Text);
                LoggedInUserCurrentSelections.current_instance_auth = basicAuth;

                instanceRestClient = RestService.For<IRestClient>(LoggedInUserCurrentSelections.INSTANCE_BASE_URL);
                requestOperationsModel.operation = Utils.Utils.INSTANCE_OPERATIONS.user_info.ToString();
                var instanceAuthVerification = await instanceRestClient.VerifyInstanceLogin(LoggedInUserCurrentSelections.current_instance_auth, requestOperationsModel);
                if (instanceAuthVerification != null && instanceAuthVerification.IsSuccessStatusCode && instanceAuthVerification.Content.username != null)
                {
                    await DisplayAlert("Success!", instanceAuthVerification.Content.username, "OK");
                } else
                {
                    await DisplayAlert("Error!", "Wrong Login Info?", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", ex.Message, "OK");
            }
        }

        private void instance_cancel_button_Clicked(System.Object sender, System.EventArgs e)
        {
            InstanceLoginStackLayout.IsVisible = false;
            InstanceCarousel.IsVisible = true;
        }
    }
}
