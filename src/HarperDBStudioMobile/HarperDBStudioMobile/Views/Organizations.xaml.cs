using System;
using System.Collections.Generic;
using HarperDBStudioMobile.ViewModels;
using Xamarin.Forms;

namespace HarperDBStudioMobile.Views
{
    public partial class Organizations : ContentPage
    {
        public Organizations()
        {
            InitializeComponent();
            this.BindingContext = new OrganizationsViewModel();
        }
    }
}
