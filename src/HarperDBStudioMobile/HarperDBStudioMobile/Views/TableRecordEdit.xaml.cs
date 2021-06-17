using System;
using System.Collections.Generic;
using HarperDBStudioMobile.ViewModels;
using Xamarin.Forms;

namespace HarperDBStudioMobile.Views
{
    public partial class TableRecordEdit : ContentPage
    {
        public TableRecordEdit()
        {
            InitializeComponent();
            this.BindingContext = new TableRecordEditViewModel();
        }
    }
}
