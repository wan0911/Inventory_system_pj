using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace materials_management.Models
{
    public class MainModel : INotifyPropertyChanged
    {

        private string materialGroupCode;            // 자재그룹코드
        public string MaterialGroupCode { 
            set { materialGroupCode = value; }
            get {  return materialGroupCode; }
        }


        private string materialGroupName;           // 자재그룹명
        public string MaterialGroupName {
            set { materialGroupName = value; }
            get { return materialGroupName; }
        }

        private string materialGroupSelection;           // 사용여부
        public string MaterialGroupSelection {
            set { MaterialGroupSelection = value; }
            get { return MaterialGroupSelection; }
        }



        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
