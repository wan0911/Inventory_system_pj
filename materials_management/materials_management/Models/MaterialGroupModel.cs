using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace materials_management.Models
{
    public class MaterialGroup
    {
        private string _materialGroupCode;            // 자재그룹코드
        public string MaterialGroupCode
        {
            get { return _materialGroupCode; }
            set { _materialGroupCode = value; }
        }


        private string _materialGroupName;           // 자재그룹명
        public string MaterialGroupName
        {
            set { _materialGroupName = value; }
            get { return _materialGroupName; }
        }

        private string _materialGroupSelection;           // 사용여부
        public string MaterialGroupSelection
        {
            set { _materialGroupSelection = value; }
            get { return _materialGroupSelection; }
        }

        private DateTime _materiaGroupCreateDate;           // 생성일
        public DateTime MaterialGroupCreateDate
        {
            set { _materiaGroupCreateDate = value; }
            get { return _materiaGroupCreateDate; }
        }

        private DateTime _materialGroupUpdateDate;           // 수정일
        public DateTime MaterialGroupUpdateDate
        {
            set { _materialGroupUpdateDate = value; }
            get { return _materialGroupUpdateDate; }
        }
    }
}
