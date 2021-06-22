using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace HarperDBStudioMobile.Views
{
    public class CustomTableRowCell: ContentView
    {
        public event EventHandler<RowTappedEventArgs> RowTapped;

        protected virtual void OnRowTapped(RowTappedEventArgs e)
        {
            EventHandler<RowTappedEventArgs> handler = RowTapped;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public CustomTableRowCell(string key, string value)
        {
            var tapGestureRecognizer = new TapGestureRecognizer();

            tapGestureRecognizer.Tapped += (sender, e) =>
            {
                // cast to an image
                RowTappedEventArgs rowTappedEventArgs = new RowTappedEventArgs();
                rowTappedEventArgs.hashValue = value;
                OnRowTapped(rowTappedEventArgs);

                // now you have a reference to the image
            };

            StackLayout layout = new StackLayout();
            layout.GestureRecognizers.Add(tapGestureRecognizer);

            layout.Children.Add(new Label { Text = key });
            layout.Children.Add(new Label { Text = value });

            this.BackgroundColor = Color.WhiteSmoke;
            Content = layout;
        }
    }

    public class RowTappedEventArgs : EventArgs
    {
        public string hashValue { get; set; }
    }
}