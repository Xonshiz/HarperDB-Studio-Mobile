using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HarperDBStudioMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingBox : ContentView
    {
        public string _text;
        public LoadingBox()
        {
            InitializeComponent();

        }

        public string Text
        {
            get { return _text; }
            set { textLabel.Text = value; _text = value; OnPropertyChanged(nameof(Text)); }
        }
    }
}
