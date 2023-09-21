//using materials_management.Models;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Input;

//namespace materials_management.ViewModels.Commands
//{
//    class DeleteCommand : ICommand
//    {
//        public event EventHandler? CanExecuteChanged;

//        private Action _execute;

//        public DeleteCommand(Action execute)
//        {
//            _execute = execute;
//        }

//        public bool CanExecute(object parameter)
//        {
//            return true;
//        }

//        public void Execute(object parameter)
//        {
//            _execute.Invoke();
//        }
//    }
//}
