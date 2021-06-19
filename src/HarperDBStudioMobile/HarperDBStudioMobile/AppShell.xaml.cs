using HarperDBStudioMobile.ViewModels;
using HarperDBStudioMobile.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Refit;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace HarperDBStudioMobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
            Routing.RegisterRoute(nameof(Organizations), typeof(Organizations));
            Routing.RegisterRoute(nameof(Instances), typeof(Instances));
            Routing.RegisterRoute(nameof(InstanceDetailPage), typeof(InstanceDetailPage));
            
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
