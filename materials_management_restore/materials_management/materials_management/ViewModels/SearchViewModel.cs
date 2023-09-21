using CommunityToolkit.Mvvm.ComponentModel;
using materials_management.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace materials_management.ViewModels
{
    internal class SearchViewModel : ObservableObject
    {
        public SearchViewModel()
        {
        }

        private DatabaseModel dbConnector;

        private ObservableCollection<MaterialInfoModel> _materialInfoList;
        public ObservableCollection<MaterialInfoModel> MaterialInfoList
        {
            get { return _materialInfoList; }
            set
            {
                SetProperty(ref _materialInfoList, value);
                OnPropertyChanged("MaterialInfoList");
            }
        }


        // 조회 커멘드
        public ICommand SearchCommand { get; set; }

        private string _searchText1;
        public string SearchText1
        {
            get { return _searchText1; }
            set
            {
                if (_searchText1 != value)
                {
                    _searchText1 = value;
                    OnPropertyChanged("SearchText1");
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

        private void CalculateRowNumbers()
        {
            int rowNumber = 1;
            foreach (var item in MaterialInfoList)
            {
                item.RowNumber = rowNumber++;
            }
        }

        private bool IsValidSearchText(string text)
        {
            return Regex.IsMatch(text, "^[a-zA-Z0-9]*$") && !string.IsNullOrEmpty(text) && text.Length <= 10;
        }

        private bool IsValidSearchText2(string text)
        {
            return Regex.IsMatch(text, "^[a-zA-Z0-9_]*$") && !string.IsNullOrEmpty(text) && text.Length <= 10;
        }


        private void SearchBtn_Click()
        {
            string searchText1 = SearchText1;
            string searchText2 = SearchText2;
            string selectedSearchGroup = SelectedSearchGroup == "ALL" ? "" : SelectedSearchGroup;
            string selectedUseItem = SelectedSearchUseItem?.Content?.ToString() == "ALL" ? "" : SelectedSearchUseItem?.Content?.ToString();

            if (!IsValidSearchText(SearchText1))
            {
                MessageBox.Show("자재코드는 영문과 숫자 조합, 공백과 특수문자를 제외하고 최대 10자 이내로 입력하세요.");
                return;
            }

            if (!IsValidSearchText2(searchText2))
            {
                MessageBox.Show("자재명은 영문, 숫자, _ 조합과 공백 제외 최대 10자 이내로 입력하세요.");
                return;
            } // 오버라이딩? 조건 수정...


            ObservableCollection<MaterialInfoModel> result = dbConnector.SearchMaterialInfo(searchText1, searchText2, selectedSearchGroup, selectedUseItem);
            MaterialInfoList = result;
        }

    }
}
