using System;
using System.Windows;
using System.Windows.Input;

namespace PackageChecker.BindingBase
{
	public class CopyToClipboard : ICommand
	{
		public string Name { get { return "Copy"; } }

		public void Execute(object parameter)
		{
			Clipboard.SetText(parameter != null ? parameter.ToString() : "<null>");
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public event EventHandler CanExecuteChanged;
	}
}
