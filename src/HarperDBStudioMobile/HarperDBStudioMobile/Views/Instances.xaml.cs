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
        IRestClient restClient;
        public ICommand CarouselItemTapped { get; set; }

        ObservableCollection<InstanceModel> instanceModels = new ObservableCollection<InstanceModel>() { };

        public Instances()
        {
            InitializeComponent();
            
            restClient = RestService.For<IRestClient>(Utils.Utils.BASE_API_URL);
            InstanceCarousel.ItemsSource = instanceModels;
            this.GetInstanceDetails();
            CarouselItemTapped = new Xamarin.Forms.Command(async (selectItem) => {
                //Peform action here
                if (selectItem == null)
                    return;

                var selectedRequest = selectItem as InstanceModel;
                //LoggedInUserCurrentSelections.current_organization_customer_id = selectedRequest.customer_id;
                //await Shell.Current.GoToAsync($"{nameof(Instances)}");

            });
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
    }
}
