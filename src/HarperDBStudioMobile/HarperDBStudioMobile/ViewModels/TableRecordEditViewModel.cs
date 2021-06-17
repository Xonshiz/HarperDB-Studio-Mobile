using System;
using static HarperDBStudioMobile.Utils.Utils;

namespace HarperDBStudioMobile.ViewModels
{
    public class TableRecordEditViewModel: BaseViewModel
    {
        public TableRecordEditViewModel()
        {
            Title = "Add Record";
        }

        public void SetRecordWindowTitle(TABLE_RECORD_MODES title = TABLE_RECORD_MODES.Add)
        {
            this.Title = title.ToString();
        }
    }
}
