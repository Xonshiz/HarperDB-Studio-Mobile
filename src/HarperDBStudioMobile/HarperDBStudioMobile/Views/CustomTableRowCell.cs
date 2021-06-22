using System.Collections.Generic;
using Xamarin.Forms;

namespace HarperDBStudioMobile.Views
{
    public class CustomTableRowCell: ContentView
    {
        public CustomTableRowCell(string key, string value)
        {
            StackLayout layout = new StackLayout();

            layout.Children.Add(new Label { Text = key });
            layout.Children.Add(new Label { Text = value });

            this.BackgroundColor = Color.WhiteSmoke;
            Content = layout;
        }
    }
}