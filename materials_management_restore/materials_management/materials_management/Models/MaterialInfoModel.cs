using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;


namespace materials_management.Models
{
    public partial class MaterialInfoModel : ObservableValidator
    {
        //private string _materialCode;            // 자재코드
        //public string MaterialCode
        //{
        //    get { return _materialCode; }
        //    set { _materialCode = value; }
        //}


        //private string _materialName;           // 자재명
        //public string MaterialName
        //{
        //    set { _materialName = value; }
        //    get { return _materialName; }
        //}

        //private string _materialGroupName;           // 자재그룹
        //public string MaterialGroupName
        //{
        //    set { _materialGroupName = value; }
        //    get { return _materialGroupName; }
        //}

        //private string _materiaUseSelection;           // 사용여부
        //public string MaterialUseSelection
        //{
        //    set { _materiaUseSelection = value; }
        //    get { return _materiaUseSelection; }
        //}

        //private string _materiaCreateDate;           // 생성일
        //public string MaterialCreateDate
        //{
        //    set { _materiaCreateDate = value; }
        //    get { return _materiaCreateDate; }
        //}

        //private string _materiaUpdateDate;           // 수정일
        //public string MaterialUpdateDate
        //{
        //    set { _materiaUpdateDate = value; }
        //    get { return _materiaUpdateDate; }
        //}


        //private int _rowNumber;
        //public int RowNumber
        //{
        //    get { return _rowNumber; }
        //    set
        //    {
        //        _rowNumber = value;
        //        OnPropertyChanged("RowNumber");
        //    }
        //}


        //private string _status;
        //public string Status
        //{
        //    get { return _status; }
        //    set
        //    {
        //        if (_status != value)
        //        {
        //            _status = value;
        //            OnPropertyChanged(nameof(Status));
        //        }
        //    }
        //}

 


        [ObservableProperty]
        private string _materialCode;            // 자재코드

        [ObservableProperty]
        private string _materialName;           // 자재명

        [ObservableProperty]
        private string _materialGroupName;           // 자재그룹


        [ObservableProperty]
        private string _materialUseSelection;           // 사용여부

        [ObservableProperty]
        private string _materialCreateDate;           // 생성일

        [ObservableProperty]
        private string _materialUpdateDate;           // 수정일

        [ObservableProperty]
        private string _status;

        [ObservableProperty]
        private int _rowNumber;
    }
}
