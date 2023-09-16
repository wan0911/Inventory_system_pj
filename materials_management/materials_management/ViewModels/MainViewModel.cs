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
using System.Windows;
using System.Windows.Input;
using System.Security.Cryptography.X509Certificates;
using CommunityToolkit.Mvvm.Input;
using System.Globalization;
using System.Windows.Data;
using materials_management.ViewModels.Commands;
using System.Windows.Controls;

namespace materials_management.ViewModels
{

    public class MainViewModel : ObservableObject
    {
        private DatabaseModel dbConnector;
        

        public MainViewModel()
        {
            
            dbConnector = DatabaseModel.Getins(); 
            dbConnector.Connect();

            /* dataContext 데이터 */
            MaterialInfoList = dbConnector.GetMaterialInfoFromDatabase();
            CodeNameCombo = dbConnector.GetCodeNames();


            /* 커멘드 이벤트 */
            SearchCommand = new SearchCommand(SearchBtn_Click);
        }


        
        /* 데이터 형식 정의 */
        private ObservableCollection<MaterialInfoModel> materialInfoList;

        public ObservableCollection<MaterialInfoModel> MaterialInfoList
        {
            get { return materialInfoList; }
            set { SetProperty(ref materialInfoList, value); }
        }


        private ObservableCollection<string> _codeNameCombo;

        public ObservableCollection<string> CodeNameCombo
        {
            get { return _codeNameCombo; }
            set { SetProperty(ref _codeNameCombo, value); }
        }




        /*  클릭 이벤트 함수  */
        public ICommand SearchCommand { get; set; }

        private Tuple<string, string> _searchParameters;
        public Tuple<string, string> SearchParameters
        {
            get { return _searchParameters; }
            set { SetProperty(ref _searchParameters, value); }
        }

        private string _searchText1;

        public string SearchText1
        {
            get { return _searchText1; }
            set
            {
                if (_searchText1 != value)
                {
                    _searchText1 = value;
                    OnPropertyChanged("SearchText");
                }
            }
        }

        private string _searchText2;

        public string SearchText2
        {
            get { return _searchText2; }
            set
            {
                if (_searchText2 != value)
                {
                    _searchText2 = value;
                    OnPropertyChanged("SearchText2");
                }
            }
        }

        private string _selectedSearchGroup;

        public string SelectedSearchGroup
        {
            get { return _selectedSearchGroup; }
            set
            {
                if (_selectedSearchGroup != value)
                {
                    _selectedSearchGroup = value;
                    OnPropertyChanged("SelectedComboItem");
                }
            }
        }

        private ComboBoxItem _selectedSearchUseItem;
        public ComboBoxItem SelectedSearchUseItem
        {
            get { return _selectedSearchUseItem; }
            set
            {
                if (_selectedSearchUseItem != value)
                {
                    _selectedSearchUseItem = value;
                    OnPropertyChanged("SelectedSearchUseItem");
                }
            }
        }



        private void SearchBtn_Click()
        {
            string searchText1 = SearchText1;
            string searchText2 = SearchText2;
            string selectedSearchGroup = SelectedSearchGroup;
            //string selectedSearchUse = _selectedSearchUse;
            string selectedUseItem = SelectedSearchUseItem?.Content?.ToString();

            MessageBox.Show($"SearchText1: {searchText1}, SearchText2: {searchText2}, SelectedItem: {selectedSearchGroup} {selectedUseItem}");

            ObservableCollection<MaterialInfoModel> result = dbConnector.SearchMaterialInfo(searchText1, searchText2, selectedSearchGroup, selectedUseItem);

            MaterialInfoList = result;
        }

    }
}
