using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HarperDBStudioMobile.Interfaces;
using HarperDBStudioMobile.Models;
using HarperDBStudioMobile.ViewModels;
using Newtonsoft.Json;
using Refit;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HarperDBStudioMobile.Views
{
    public partial class Instances : ContentPage
    {
        private string currentInstanceId = String.Empty;
        IRestClient restClient, instanceRestClient;

        public ICommand CarouselItemTapped { get; set; }

        RequestOperationsModel requestOperationsModel = new RequestOperationsModel();

        ObservableCollection<InstanceModel> instanceModels = new ObservableCollection<InstanceModel>() { };
        //List<Dictionary<string, string>> cachedInstanceDetails = new List<Dictionary<string, string>>() { };
        Dictionary<string, string> cachedInstanceDetails = new Dictionary<string, string>() { };

        public Instances()
        {
            InitializeComponent();
            this.ReadInstanceAuthData();

            restClient = RestService.For<IRestClient>(Utils.Utils.BASE_API_URL);
            instance_username.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeNone);
            instance_password.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeNone);

            InstanceCarousel.ItemsSource = instanceModels;
            InstanceCarousel.TabIndex = 0;

            //Do this when no Internet.
            //ReadInstancesFromCache();

            this.GetInstanceDetails();
            CarouselItemTapped = new Xamarin.Forms.Command(async (selectItem) => {
                if (selectItem == null)
                    return;

                var selectedRequest = selectItem as InstanceModel;
                this.currentInstanceId = selectedRequest.instance_id;
                LoggedInUserCurrentSelections.INSTANCE_BASE_URL = selectedRequest.url;

                string instanceAuth = String.Empty;
                var cachedCredentials = cachedInstanceDetails.TryGetValue(this.currentInstanceId, out instanceAuth);
                if (String.IsNullOrWhiteSpace(instanceAuth))
                {
                    InstanceLoginStackLayout.IsVisible = true;
                    InstanceCarousel.IsVisible = false;
                } else
                {
                    LoggedInUserCurrentSelections.current_instance_auth = instanceAuth;
                    this.PushToInstanceDetails();
                }
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
                    try
                    {
                        Preferences.Set(Utils.Utils.STORAGE_KEYS.INSTANCE_MODELS.ToString(), JsonConvert.SerializeObject(instanceModels));
                    }
                    catch (Exception ex)
                    {
                        //No need to do anything.
                        Console.WriteLine("Couldn't set Instance Models in Cache: " + ex.Message);
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

        private void ReadInstancesFromCache()
        {
            try
            {
                var instanceData = Preferences.Get(Utils.Utils.STORAGE_KEYS.INSTANCE_MODELS.ToString(), null);
                if (instanceData != null)
                {
                    this.instanceModels = JsonConvert.DeserializeObject<ObservableCollection<InstanceModel>>(instanceData);
                }
            }
            catch (Exception ex)
            {
                //Do Nothing
                Console.WriteLine("Couldn't read Instance Models in Cache: " + ex.Message);
            }
        }

        private async void PushToInstanceDetails()
        {
            await Shell.Current.GoToAsync($"{nameof(InstanceDetailPage)}");
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
                    //Can add the auth to DICTIONARY here.
                    this.CacheInstanceAuthDetails(this.currentInstanceId, basicAuth);
                    await DisplayAlert("Success!", instanceAuthVerification.Content.username, "OK");
                    this.PushToInstanceDetails();
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

        private void ReadInstanceAuthData()
        {
            try
            {
                var cachedValues = Preferences.Get(Utils.Utils.STORAGE_KEYS.INSTANCE_CREDENTIALS.ToString(), null);
                if (cachedValues != null)
                {
                    this.cachedInstanceDetails = JsonConvert.DeserializeObject<Dictionary<string, string>>(cachedValues);
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error while reading cached instance auths." + ex.Message);
            }
        }

        private void CacheInstanceAuthDetails(string instanceId, string instanceBasicAuth)
        {
            try
            {
                cachedInstanceDetails.Add(instanceId, instanceBasicAuth);
                Preferences.Set(Utils.Utils.STORAGE_KEYS.INSTANCE_CREDENTIALS.ToString(), JsonConvert.SerializeObject(cachedInstanceDetails));
            }
            catch (Exception ex)
            {
                Console.Write("Error while caching Instance logins: " + ex.Message);
            }
        }
    }
}
