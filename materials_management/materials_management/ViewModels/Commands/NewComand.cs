//using materials_management.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Input;

//namespace materials_management.ViewModels.Commands
//{
//    public class NewComand : ICommand
//    {
//        public event EventHandler? CanExecuteChanged
//        {
//            add { CommandManager.RequerySuggested += value; }
//            remove { CommandManager.RequerySuggested -= value;}
//        }


//        private Action _execute;
//        private Predicate<MaterialInfoModel> _canExecute;

//        public NewComand(Action execute, Predicate<MaterialInfoModel> canExecute) {
//            _execute = execute;
//            _canExecute = canExecute;
//        }

//        //public bool CanExecute(object? parameter)
//        //{
//        //    if (parameter is MaterialInfoModel selectedMaterial)
//        //    {
//        //        bool isMaterialCodeValid = !string.IsNullOrEmpty(selectedMaterial.MaterialCode) && selectedMaterial.MaterialCode.Length <= 10;
//        //        bool isMaterialNameValid = !string.IsNullOrEmpty(selectedMaterial.MaterialName); // MaterialName에 대한 유효성 검사 추가

//        //        // 유효성 검사 조건을 모두 만족하면 true 반환
//        //        return isMaterialCodeValid && isMaterialNameValid && _canExecute.Invoke(selectedMaterial);
//        //    }
//        //    return false; // 선택한 행이 MaterialInfoModel이 아닌 경우에도 버튼 비활성화
//        //}

//        public bool CanExecute(object? parameter)
//        {
//            if (parameter is MaterialInfoModel selectedMaterial)
//            {
//                if (parameter != null)
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//        }

//        //public bool CanExecute(MaterialInfoModel selectedMaterial)
//        //{
//        //    if (selectedMaterial == null)
//        //    {
//        //        return true;
//        //    } else
//        //    {
//        //        return false;
//        //    }
//        //}


//        public void Execute(object? parameter)
//        {
//            _execute.Invoke();
//        }
//    }
//}
