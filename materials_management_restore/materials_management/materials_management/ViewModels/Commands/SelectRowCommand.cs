using materials_management.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace materials_management.ViewModels.Commands
{
    public class SelectRowCommand : ICommand
    {
        private Action<ObservableCollection<MaterialInfoModel>> _execute;

        public SelectRowCommand(Action<ObservableCollection<MaterialInfoModel>> execute)
        {
            _execute = execute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke(parameter as ObservableCollection<MaterialInfoModel>);
        }
    }
}
