using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HarperDBStudioMobile.Interfaces;
using HarperDBStudioMobile.Models;
using HarperDBStudioMobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HarperDBStudioMobile.Views
{
    public partial class Organizations : ContentPage
    {
        ObservableCollection<OrgModel> orgModels = new ObservableCollection<OrgModel>() { };
        public ICommand CarouselItemTapped{ get; set; }

        public Organizations()
        {
            InitializeComponent();
            this.UpdateOrgModelList();
            MainCarousel.ItemsSource = orgModels;

            CarouselItemTapped= new Xamarin.Forms.Command(async (selectItem)=>{
                //Peform action here
                if (selectItem == null)
                    return;

                var selectedRequest = selectItem as OrgModel;
                LoggedInUserCurrentSelections.current_organization_customer_id = selectedRequest.customer_id;
                await Shell.Current.GoToAsync($"{nameof(Instances)}");

            });
        }

        private void UpdateOrgModelList()
        {
            foreach (var item in LoggedInUser.Orgs)
            {
                orgModels.Add(new OrgModel { active_coupons = item.active_coupons, customer_id = item.customer_id, customer_name = item.customer_name, free_cloud_instance_count = item.free_cloud_instance_count, owner_user_id = item.owner_user_id, status = item.status, total_instance_count = item.total_instance_count });
            }
            if (orgModels.Count == 0)
            {
                MainCarousel.IsVisible = false;
                noDataFrame.IsVisible = true;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        void resourcesToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            Utils.Utils.OpenResourcesWebPage();
        }

        void lougoutToolBarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            Utils.Utils.LogoutUser();
        }
    }
}
