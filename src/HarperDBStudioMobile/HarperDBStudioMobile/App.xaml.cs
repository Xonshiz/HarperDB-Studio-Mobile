using HarperDBStudioMobile.Models;
using HarperDBStudioMobile.Services;
using HarperDBStudioMobile.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HarperDBStudioMobile
{
    public partial class App : Application
    {

        public App()
        {
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();

            this.ReadAppSettings();
        }

        private async void ReadAppSettings()
        {
            try
            {
                LoggedInUser.LoginEmail = Preferences.Get(Utils.Utils.STORAGE_KEYS.BASE_EMAIL.ToString(), null);
                LoggedInUser.LoginPassword = Preferences.Get(Utils.Utils.STORAGE_KEYS.BASE_PASSWORD.ToString(), null);
                var orgsData = Preferences.Get(Utils.Utils.STORAGE_KEYS.LOGGED_IN_USER_ORGS.ToString(), null);
                if (orgsData != null)
                {
                    LoggedInUser.Orgs = JsonConvert.DeserializeObject<List<OrgModel>>(orgsData);
                }
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                Console.WriteLine("Exception While Reading Cache: " + ex.Message);
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
