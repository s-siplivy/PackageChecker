using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageChecker.WindowManagement
{
	internal static class WindowHelper
	{
		internal static void ShowInfo(string message)
		{
			System.Windows.MessageBox.Show(message, "Info");
		}

		internal static void ShowError(string message)
		{
			System.Windows.MessageBox.Show(message, "Error");
		}

		private static void ShowMessage(string message, string caption)
		{
			System.Windows.MessageBox.Show(message, caption);
		}
	}
}
