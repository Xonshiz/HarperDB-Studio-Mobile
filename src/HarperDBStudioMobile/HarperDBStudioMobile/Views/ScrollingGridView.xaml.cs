using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace HarperDBStudioMobile.Views
{
    public partial class ScrollingGridView : ContentView
    {
        public string hashAttributeForRow;
        public event EventHandler<RowTappedEventArgs> GridRowTapped;

        public ScrollingGridView(Dictionary<string, string> currentTableDataList, string hashColumn)
        {
            InitializeComponent();

            AddItemsToGrid(currentTableDataList, hashColumn);

        }

        void AddItemsToGrid(Dictionary<string, string> currentTableDataList, string hashColumn)
        {
            gridView.Children.Clear();
            for (int x = 1; x < gridView.ColumnDefinitions.Count; x++)
            {
                gridView.ColumnDefinitions.RemoveAt(x);
            }
            int i = 0;
            foreach (var item in currentTableDataList)
            {
                if (item.Key.ToLower() == hashColumn.ToLower())
                {
                    this.hashAttributeForRow = item.Value;
                }
                gridView.ColumnDefinitions.Add(new ColumnDefinition());
                CustomTableRowCell customTableRowCell = new CustomTableRowCell(item.Key, item.Value);
                customTableRowCell.Margin = 0;
                customTableRowCell.Padding = 0;
                customTableRowCell.FlowDirection = FlowDirection.LeftToRight;

                var view = new ContentView
                {
                    Content = customTableRowCell
                };
                customTableRowCell.RowTapped += CustomTableRowCell_RowTapped;

                gridView.Children.Add(view, i, 0);
                i++;
            }
        }

        protected virtual void OnRowTapped(RowTappedEventArgs e)
        {
            EventHandler<RowTappedEventArgs> handler = GridRowTapped;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private async void CustomTableRowCell_RowTapped(object sender, RowTappedEventArgs e)
        {
            RowTappedEventArgs rowTappedEventArgs = new RowTappedEventArgs();
            if (!String.IsNullOrWhiteSpace(this.hashAttributeForRow))
            {
                rowTappedEventArgs.hashValue = this.hashAttributeForRow;
                OnRowTapped(rowTappedEventArgs);
            }
        }
    }
}
