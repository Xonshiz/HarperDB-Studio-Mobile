using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HarperDBStudioMobile.Models
{
    public class DynamicModel : INotifyPropertyChanged
    {
        public Dictionary<string, string> data;

        public event PropertyChangedEventHandler PropertyChanged;

        public Dictionary<string, string> Values
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Values"));
            }

        }
        public DynamicModel()
        {
            this.data = new Dictionary<string, string>();

        }
    }
}
