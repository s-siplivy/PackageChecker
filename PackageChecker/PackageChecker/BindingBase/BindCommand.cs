using System;
using System.Windows.Input;

namespace PackageChecker.BindingBase
{
	internal class BindCommand : ICommand
	{
		private Action<object> _action;

		internal BindCommand(Action<object> action)
		{
			_action = action;
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			_action(parameter);
		}
	}
}
