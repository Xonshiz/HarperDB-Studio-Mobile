using HarperDBStudioMobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace HarperDBStudioMobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}