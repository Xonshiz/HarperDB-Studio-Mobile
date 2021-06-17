using System;
using System.Collections.Generic;
using HarperDBStudioMobile.ViewModels;
using Xamarin.Forms;

namespace HarperDBStudioMobile.Views
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
            this.BindingContext = new SignUpViewModel();
        }
    }
}
