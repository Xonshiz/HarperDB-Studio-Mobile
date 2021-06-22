using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace HarperDBStudioMobile.Views
{
    public partial class ScrollingGridView : ContentView
    {
        public ScrollingGridView(Dictionary<string, string> currentTableDataList)
        {
            InitializeComponent();

            AddItemsToGrid(currentTableDataList);

        }

        void AddItemsToGrid(Dictionary<string, string> currentTableDataList)
        {
            gridView.Children.Clear();
            for (int x = 1; x < gridView.ColumnDefinitions.Count; x++)
            {
                gridView.ColumnDefinitions.RemoveAt(x);
            }
            int i = 0;
            foreach (var item in currentTableDataList)
            {
                gridView.ColumnDefinitions.Add(new ColumnDefinition());
                var view = new ContentView
                {
                    Content = new CustomTableRowCell(item.Key, item.Value)
                };

                gridView.Children.Add(view, i, 0);
                i++;
            }
        }
    }
}
