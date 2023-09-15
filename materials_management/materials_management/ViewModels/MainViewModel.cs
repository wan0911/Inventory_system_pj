using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using materials_management.Models;

namespace materials_management.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Models.MainModel model = null;

        //public Models.MainModel Model
        //{

        //}
        
        public MainViewModel ()
        {
            model = new Models.MainModel ();    
        }



        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
