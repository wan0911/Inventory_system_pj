using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace materials_management.ViewModels.Commands
{
    public class DeleteCommand : ICommand
    {
        private Action _execute;

        public DeleteCommand(Action execute)
        {
            _execute = execute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true; // 여기서 필요한 조건을 설정할 수 있습니다.
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke();
        }
    }
}
