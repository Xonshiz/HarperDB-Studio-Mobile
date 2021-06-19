using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HarperDBStudioMobile.Interfaces;
using HarperDBStudioMobile.Models;
using HarperDBStudioMobile.ViewModels;
using Xamarin.Forms;

namespace HarperDBStudioMobile.Views
{
    public partial class Organizations : ContentPage
    {
        IRestClient restClient;
        ObservableCollection<OrgModel> orgModels = new ObservableCollection<OrgModel>() { };

        public Organizations()
        {
            InitializeComponent();
            this.UpdateOrgModelList();
            MainCarousel.ItemsSource = orgModels;
        }

        private void UpdateOrgModelList()
        {
            //orgModels = LoggedInUser.Orgs;
            foreach (var item in LoggedInUser.Orgs)
            {
                orgModels.Add(new OrgModel { active_coupons = item.active_coupons, customer_id = item.customer_id, customer_name = item.customer_name, free_cloud_instance_count = item.free_cloud_instance_count, owner_user_id = item.owner_user_id, status = item.status, total_instance_count = item.total_instance_count });
            }
            //orgModels.AddRange(LoggedInUser.Orgs);
            var count = orgModels.Count;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void GetOrganizations()
        {

        }
    }
}
