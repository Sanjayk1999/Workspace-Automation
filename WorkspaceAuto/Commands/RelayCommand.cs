using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WorkspaceAuto.Commands
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            if (parameter is T value)
            {
                _execute(value);
            }
        }

        public event EventHandler? CanExecuteChanged;
    }
}
