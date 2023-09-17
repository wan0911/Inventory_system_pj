using System;
using System.Windows.Input;

namespace materials_management.ViewModels.Commands
{
    public class DeleteCommand : ICommand
    {
        public DeleteCommand(Action<object> action)
        {
            _action = action;
        }

        private readonly Action<object> _action;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}
