using System;
using System.Collections.Generic;
using HarperDBStudioMobile.ViewModels;
using Xamarin.Forms;

namespace HarperDBStudioMobile.Views
{
    public partial class TableData : ContentPage
    {
        public TableData()
        {
            InitializeComponent();
            this.BindingContext = new TableDataViewModel();
        }
    }
}
