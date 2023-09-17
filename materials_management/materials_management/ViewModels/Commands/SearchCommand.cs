using System;
using System.Windows.Input;


namespace materials_management.ViewModels.Commands
{
    class SearchCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private Action _execute;

        public SearchCommand(Action execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute.Invoke();
        }
    }
}

