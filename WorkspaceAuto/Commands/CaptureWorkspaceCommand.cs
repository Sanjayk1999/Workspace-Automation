using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WorkspaceAuto.Commands
{
    public class CaptureWorkspaceCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private Action action;

        public CaptureWorkspaceCommand(Action action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            action();
        }
    }
}
