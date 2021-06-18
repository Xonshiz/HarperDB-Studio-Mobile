using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HarperDBStudioMobile.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
            //return base.OnBackButtonPressed();
        }
        
    }
}