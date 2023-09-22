using System;
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

using CommunityToolkit.Mvvm.Input;
using materials_management.DataBase;


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

            SelectRowCommand = new RelayCommand<object>(OnSelectionChanged);
            DeleteCommand = new RelayCommand(OnDelete, () => SelectedMaterial != null);    // SelectedMaterial에 행 데이터가 들어오면 활성화
            //AddRowCommand = new RelayCommand(AddRow, IsAdding);
            AddRowCommand = new RelayCommand(AddRow, () => IsAdding != false);


            PropertyChanged += MainViewModel_PropertyChanged;

        }




        private bool _isEditing;
        public bool IsEditing
        {
            get { return _isEditing; }
            set { SetProperty(ref _isEditing, value); }
        }

        /* 뷰모델에서 발생하는 프로퍼티 체인지 감지
           뷰 -> 사용자 인터렉션 -> 뷰모델에서 설정된 함수에 따라 프로퍼티 변경 */
        private void MainViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectedMaterial):
                    DeleteCommand.NotifyCanExecuteChanged();  // 행 선택 -> 선택된 행 = material 프로퍼티 변화 -> deletecommand에서 감지 
                    break;
            }
        }


        /* ------- dataContext 정의 ------- */
        // 1. datagridcolumnbox 설정
        private ObservableCollection<MaterialInfoModel> _materialInfoList;
        public ObservableCollection<MaterialInfoModel> MaterialInfoList
        {
            get { return _materialInfoList; }
            set
            {
                _materialInfoList = value;
                //SetProperty(ref _materialInfoList, value);
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





        /* 행 선택 커멘드
           + 삭제 커맨드, 수정 커맨드, 추가 커맨드 */
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


        public RelayCommand<object> SelectRowCommand { get; set; }  // 버튼이 아닌 command binding
        private void OnSelectionChanged(object para)
        {
            var args = para as SelectionChangedEventArgs;
            if (args == null)
            {
                return;
            }

            if (args.AddedItems.Count == 0)
            {
                SelectedMaterial = null;
                MessageBox.Show("row 데이터를 가져오지 못했습니다.");
            }
            else
            {
                var row = args.AddedItems[0] as MaterialInfoModel;
                if (row != null)
                {
                    SelectedMaterial = row;     // 프로퍼티로 접근해서 값 
                    MessageBox.Show($"선택된 행: {SelectedMaterial.MaterialName}");

                    return;
                }

            }
        }


        // 행 삭제 커맨드
        public RelayCommand DeleteCommand { get; private set; }
        private void OnDelete()
        {
            var result = MessageBox.Show("선택된 아이템을 삭제하시겠습니까?", "삭제 확인", MessageBoxButton.YesNo);
            // No 클릭
            if (result == MessageBoxResult.No)
            {
                return;
            }
            // Yes 클릭
            else
            {
                var removeRowData = SelectedMaterial;
                if (removeRowData != null)
                {
                    SelectedMaterial.Status = "Delete";
                    MessageBox.Show("상태:Delete로 업데이트 되었습니다.");
                    SelectedMaterial = null;
                }
            }
        }





        // 행 추가 커멘드 
        private bool _isAdding = true; // 초기에 비활성화 상태
        public bool IsAdding
        {
            get { return _isAdding; }
            set { SetProperty(ref _isAdding, value); }
        }

        //public bool IsAdding()
        //{
        //    if (SelectedMaterial != null || AddedMaterial != null)
        //    {
        //        return true;
        //    } else
        //    {
        //        return false;
        //    }
        //}

        private MaterialInfoModel _addedMaterial;
        public MaterialInfoModel AddedMaterial
        {
            get { return _addedMaterial; }
            set
            {
                if (_addedMaterial != value)
                {
                    _addedMaterial = value;
                    OnPropertyChanged(nameof(AddedMaterial));
                }
            }
        }


        public ICommand AddRowCommand { get; }
        private void AddRow()
        {
            if (AddedMaterial == null)
            {
                int selectedIndex = MaterialInfoList.IndexOf(SelectedMaterial);

                if (selectedIndex >= 0)
                {
                    // 새로운 행 지정
                    MaterialInfoList.Insert(selectedIndex + 1, new MaterialInfoModel());
                    AddedMaterial = MaterialInfoList[selectedIndex + 1];
                    AddedMaterial.Status = "New";

                    //
                    IsAdding = false;
                }
            } 
            else
            {
                MessageBox.Show("fddfd");
                if (!string.IsNullOrEmpty(AddedMaterial.MaterialName) && !string.IsNullOrEmpty(AddedMaterial.MaterialCode))
                {
                    IsAdding = true;
                }
                else
                {
                    MessageBox.Show("데이터를 제대로 입력해주세요");
                }
            }
          
        }






        //public bool CheckNewRowData(MaterialInfoModel newRow)
        //{
        //    if (!string.IsNullOrEmpty(newRow.MaterialCode) && !string.IsNullOrEmpty(newRow.MaterialName))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        MessageBox.Show("데이터 입력해주세요");
        //        return false;
        //    }
        //}
















        // 저장 커맨드 

        //// 삭제 후 자재 목록을 업데이트 -> 저장 버튼 클릭 시로 변경
        //var deletedMaterial = MaterialInfoList.FirstOrDefault(m => m.MaterialCode == materialCode);
        //deletedMaterial.Status = "Delete";
        //if (deletedMaterial != null)
        //{
        //    MaterialInfoList.Remove(deletedMaterial);
        //}


    }
}

