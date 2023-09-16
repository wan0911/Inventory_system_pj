using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

using materials_management.Models;
using static materials_management.MainWindow;
using static materials_management.Models.DatabaseModel;


namespace materials_management.ViewModels
{

    public class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            dbConnector = new DbConnector(); // 싱글톤 패턴 적용 필요
            dbConnector.Connect();


            /* dataContext로 넘겨줄 데이터 */
            // 자재 정보 데이터
            MaterialInfoList = dbConnector.GetMaterialInfoFromDatabase();  
        }


        private DbConnector dbConnector;

        private ObservableCollection<MaterialInfoModel> materialInfoList;

        public ObservableCollection<MaterialInfoModel> MaterialInfoList
        {
            get { return materialInfoList; }
            set { SetProperty(ref materialInfoList, value); }
        }
    }
}
