using System;
using System.Collections.Generic;
using HarperDBStudioMobile.ViewModels;
using Xamarin.Forms;

namespace HarperDBStudioMobile.Views
{
    public partial class Instances : ContentPage
    {
        public Instances()
        {
            InitializeComponent();
            this.BindingContext = new InstancesViewModel();
        }
    }
}
