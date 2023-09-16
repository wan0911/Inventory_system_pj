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
using static materials_management.ViewModels.Commands.SearchCommand;
using System.Windows;
using materials_management.ViewModels.Commands;
using System.Windows.Input;

namespace materials_management.ViewModels
{

    public class MainViewModel : ObservableObject
    {

        private DatabaseModel dbConnector;
        public ICommand DisplaySearchCommand { get; set; }

        public MainViewModel()
        {
            
            dbConnector = DatabaseModel.Getins(); 
            dbConnector.Connect();

            /* dataContext로 넘겨줄 데이터 */
            MaterialInfoList = dbConnector.GetMaterialInfoFromDatabase();
            CodeNameCombo = dbConnector.GetCodeNames();


            /* 이벤트 */
            DisplaySearchCommand = new SearchCommand(SearchBtn_Click);
        }


        
        /* 데이터 형식 정의 */
        private ObservableCollection<MaterialInfoModel> materialInfoList;

        public ObservableCollection<MaterialInfoModel> MaterialInfoList
        {
            get { return materialInfoList; }
            set { SetProperty(ref materialInfoList, value); }
        }


        private List<string> _codeNameCombo;

        public List<string> CodeNameCombo
        {
            get { return _codeNameCombo; }
            set { SetProperty(ref _codeNameCombo, value); }
        }



        /*  클릭 이벤트 함수  */
        private void SearchBtn_Click()
        {
            MessageBox.Show("search 버튼 클릭");
        }
    }
}
