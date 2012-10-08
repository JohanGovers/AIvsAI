using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Jato.UI
{
    public class JatoPlayer : INotifyPropertyChanged
    {
        public int Money { get; set; }

        private int _keepHealth;
        public int KeepHealth 
        { 
            get
            {
                return _keepHealth;
            }
            set
            {
                _keepHealth = value;
                RaisePropertyChanged("KeepHealth");
            } 
        }

        public int Score { get; set; }

        public void Update()
        {
            KeepHealth = KeepHealth - 1;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string prop)
        {
            if( PropertyChanged != null )
            {
                PropertyChanged(this, new  PropertyChangedEventArgs(prop));
            }
        }
    }
}
