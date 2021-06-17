using System;
using System.Globalization;

namespace HarperDBStudioMobile.ViewModels
{
    public class TableDataViewModel : BaseViewModel
    {
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        public TableDataViewModel()
        {
            Title = "Current Table Data";
        }

        public void SetTableTitle(string tableTitle)
        {
            this.Title = textInfo.ToTitleCase(tableTitle);
        }
    }
}
