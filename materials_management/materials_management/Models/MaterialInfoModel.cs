using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace materials_management.Models
{
    public class MaterialInfoModel : INotifyPropertyChanged
    {

        private string _materialCode;            // 자재코드
        public string MaterialCode
        {
            get { return _materialCode; }
            set { _materialCode = value; }
        }

        private string _materialName;           // 자재명
        public string MaterialName
        {
            set { _materialName = value; }
            get { return _materialName; }
        }

        private string _materialGroupName;           // 자재그룹
        public string MaterialGroupName
        {
            set { _materialGroupName = value; }
            get { return _materialGroupName; }
        }

        private string _materiaUseSelection;           // 사용여부
        public string MaterialUseSelection
        {
            set { _materiaUseSelection = value; }
            get { return _materiaUseSelection; }
        }

        private DateTime _materiaCreateDate;           // 생성일
        public DateTime MaterialCreateDate
        {
            set { _materiaCreateDate = value; }
            get { return _materiaCreateDate; }
        }

        private DateTime _materiaUpdateDate;           // 수정일
        public DateTime MaterialUpdateDate
        {
            set { _materiaUpdateDate = value; }
            get { return _materiaUpdateDate; }
        }



        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
