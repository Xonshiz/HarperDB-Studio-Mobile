﻿using HarperDBStudioMobile.Services;
using HarperDBStudioMobile.Views;
using System;
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
