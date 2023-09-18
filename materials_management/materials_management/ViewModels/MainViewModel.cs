﻿using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

using materials_management.Models;
using System.Windows;
using System.Windows.Input;
using materials_management.ViewModels.Commands;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Numerics;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Collections;
using System.Windows.Media;
using materials_management.Views;
using System.Drawing;
using System.Runtime.Intrinsics.X86;
using System.Globalization;
using System.Text.RegularExpressions;
using static materials_management.ViewModels.MainViewModel;
using System.ComponentModel.DataAnnotations;

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

        }

     


        /* ---- dataContext 정의 ---- */
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



        /* ---- Command 함수 ---- */
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
            }

            

            MessageBox.Show($"SearchText1: {searchText1}, SearchText2: {searchText2}, SelectedItem: {selectedSearchGroup} {selectedUseItem}");

            ObservableCollection<MaterialInfoModel> result = dbConnector.SearchMaterialInfo(searchText1, searchText2, selectedSearchGroup, selectedUseItem);
            MaterialInfoList = result;
        }


        // 삭제 커맨드
        private bool IsExecuteDelete = false;
        public void selectRow(object sender, MouseEventArgs e)
        {
            // 삭제 버튼을 눌렀을 때만 행 데이터 가져오게 로직 수정 필요
            // 순서를 바꾸는게 좋을 듯

            DependencyObject dep = (DependencyObject)e.OriginalSource;

            // 이벤트 소스로부터 부모 DataGridRow을 찾을 때까지 반복
            while ((dep != null) && !(dep is DataGridRow))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep is DataGridRow dataGridRow)
            {
                if (dataGridRow.Item is MaterialInfoModel selectedMaterial)
                {
                    var materialCode = selectedMaterial.MaterialCode;
                    var materialUse = selectedMaterial.MaterialUseSelection;

                    //MessageBox.Show($"선택한 행의 MaterialCode: {materialCode}, MaterialUseSelection: {materialUse}");

                    // 여기서 db에서 자재그룹 사용여부 데이터 가져와서 사용중, 갯수에 따라 밑의 코드 실행 시키기

                    IsExecuteDelete = true;     // 행을 선택하고 삭제버튼을 클릭했을 때만, 삭제 폼 뜨기 위해 지정

                    if (clickedDeleteBtn == true)   // 삭제 폼 -> 예를 클릭한 경우
                    {
                        selectedMaterial.Status = "Delete";  // 상태 변경 로직 다시 생각해보기
                        OnPropertyChanged(nameof(selectedMaterial.Status));  // 데이터 바인딩 업데이트

                        DeleteRow(materialCode);
                    }
                }
            }
        }
      
        public bool clickedDeleteBtn = false;
        public void ExecuteDeleteRow()
        {
            var confirmationWindow = new DeleteWindow();
            confirmationWindow.Owner = Application.Current.MainWindow;

            if (!IsExecuteDelete)
            {
                MessageBox.Show("그리드뷰에서 삭제할 행을 선택해주세요.");
                return;
            }
            else
            {
                confirmationWindow.ShowDialog();

                if (confirmationWindow.IsConfirmed) // Yes
                {
                    MessageBox.Show("삭제가 완료되었습니다.");
                    clickedDeleteBtn = true;
                }
                else
                {
                    MessageBox.Show("삭제가 취소되었습니다.");
                }
            }
        }



        public void DeleteRow(string materialCode)
        {
            try
            {
                MessageBox.Show("deletrow 실행");
                //dbConnector.DeleteMaterialInfo(materialCode);
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}");
            }

            //// 삭제 후에 자재 목록을 업데이트하려면 아래와 같이 처리할 수 있습니다.
            //var deletedMaterial = MaterialInfoList.FirstOrDefault(m => m.MaterialCode == materialCode);
            //deletedMaterial.Status = "Delete";
            //if (deletedMaterial != null)
            //{
            //    MaterialInfoList.Remove(deletedMaterial);
            //}
        }

        
    }
}

