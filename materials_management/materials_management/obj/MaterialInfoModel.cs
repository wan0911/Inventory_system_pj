using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;


namespace materials_management.Models
{
    public class MaterialInfoModel : ObservableObject
    {
        private string _materialCode;            // 자재코드
        public string MaterialCode
        {
            get { return _materialCode; }
            set { _materialCode = value; }
        }

        //[Required(ErrorMessage = "자재코드는 필수 입력 항목입니다.")]
        //[MaxLength(10, ErrorMessage = "자재코드는 최대 10자 이내로 입력하세요.")]
        //public string MaterialCode
        //{
        //    get { return _materialCode; }
        //    set { SetProperty(ref _materialCode, value); }
        //}


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

        private string _materiaCreateDate;           // 생성일
        public string MaterialCreateDate
        {
            set { _materiaCreateDate = value; }
            get { return _materiaCreateDate; }
        }

        private string _materiaUpdateDate;           // 수정일
        public string MaterialUpdateDate
        {
            set { _materiaUpdateDate = value; }
            get { return _materiaUpdateDate; }
        }


        private int _rowNumber;
        public int RowNumber
        {
            get { return _rowNumber; }
            set
            {
                _rowNumber = value;
                OnPropertyChanged("RowNumber");
            }
        }


        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        //public event PropertyChangedEventHandler? PropertyChanged;
    }
}
