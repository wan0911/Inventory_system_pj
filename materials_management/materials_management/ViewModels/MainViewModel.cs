﻿using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

using materials_management.Models;
using System.Windows;
using System.Windows.Input;
using materials_management.ViewModels.Commands;
using System.Windows.Controls;
using System.Windows.Media;
using materials_management.Views;

using System.Text.RegularExpressions;
using static materials_management.ViewModels.MainViewModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics.Metrics;

namespace materials_management.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private DatabaseModel dbConnector;

        public MainViewModel()
        {
            /* Db 연결 */
            dbConnector = DatabaseModel.Getins();
            dbConnector.Connect();

            /* DataContext */
            MaterialInfoList = dbConnector.GetMaterialInfoFromDatabase();
            CodeNameCombo = dbConnector.GetCodeNames();

            /* Command */
            SearchCommand = new SearchCommand(SearchBtn_Click);
            CalculateRowNumbers();

            SelectionChangedCommand = new RelayCommand<object>(OnSelectionChanged);
            DeleteCommand = new DeleteCommand(ExecuteDeleteCommand);
        }






        /* ------- dataContext 정의 ------- */
        // 1. datagridcolumnbox 설정


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


        private ObservableCollection<string> _codeNameCombo;
        public ObservableCollection<string> CodeNameCombo
        {
            get { return _codeNameCombo; }
            set { SetProperty(ref _codeNameCombo, value); }
        }



        /* ------- Command 함수 ------- */
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



        /* datagrid 행 선택 커멘드 --> + 삭제 커맨드, 수정 커맨드, 추가 커맨드 */
        private MaterialInfoModel _selectedMaterial;
        public MaterialInfoModel SelectedMaterial
        {
            get { return _selectedMaterial; }
            set
            {
                if (_selectedMaterial != value)
                {
                    _selectedMaterial = value;
                    OnPropertyChanged(nameof(SelectedMaterial));
                }
            }
        }

        public RelayCommand<object> SelectionChangedCommand { get; set; }  // 버튼이 아닌 command binding
        private void OnSelectionChanged(object para)
        {
            var args = para as SelectionChangedEventArgs;
            if (args == null)
            {
                return;
            }

            if (args.AddedItems.Count == 0)
            {
                MessageBox.Show("row 데이터를 가져오지 못했습니다.");
            }
            else
            {
                var selectedMaterial = args.AddedItems[0] as MaterialInfoModel;
                if (selectedMaterial != null)
                {
                    SelectedMaterial = selectedMaterial;
                    MessageBox.Show($"{selectedMaterial.MaterialName}");
                    return;
                }
            }
        }


        // 삭제 커맨드
        // 1. 삭제 버튼 클릭 command
        public ICommand DeleteCommand { get; private set; }
        private void ExecuteDeleteCommand()
        {
            var confirmationWindow = new DeleteWindow();
            confirmationWindow.Owner = Application.Current.MainWindow;

            if (SelectedMaterial != null)
            {
                string materialCode = SelectedMaterial.MaterialCode;

                confirmationWindow.ShowDialog();  // 삭제폼 실행

                if (confirmationWindow.IsConfirmed) // Yes
                {
                    SelectedMaterial.Status = "Delete";  
                    OnPropertyChanged(nameof(SelectedMaterial.Status));  // 데이터 바인딩 업데이트

                    MessageBox.Show("상태:Delete로 업데이트 되었습니다.");
                }
                else
                {
                    MessageBox.Show("삭제가 취소되었습니다.");
                }
            }
            else
            {
                MessageBox.Show("행에 대한 데이터가 없습니다.");
                return;
            }
        }

      

        //// 삭제 후 자재 목록을 업데이트 -> 저장 버튼 클릭 시로 변경
        //var deletedMaterial = MaterialInfoList.FirstOrDefault(m => m.MaterialCode == materialCode);
        //deletedMaterial.Status = "Delete";
        //if (deletedMaterial != null)
        //{
        //    MaterialInfoList.Remove(deletedMaterial);
        //}




        // 새로운 행 추가 기능 
        public void AddNewRow()
        {

        }

    }
}

