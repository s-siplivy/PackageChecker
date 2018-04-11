using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageChecker
{
	public class MainWindowController
	{
		protected MainWindow window;

		public MainWindowController(MainWindow window)
		{
			this.window = window;
			InitializeWindow();
		}

		private void InitializeWindow()
		{
			SetChooseMode();
		}

		private void SetChooseMode()
		{
			window.ChoosePanel.Visibility = System.Windows.Visibility.Visible;
			window.PathPanel.Visibility = System.Windows.Visibility.Collapsed;
		}

		private void SetPathMode()
		{
			window.PathPanel.Visibility = System.Windows.Visibility.Visible;
			window.ChoosePanel.Visibility = System.Windows.Visibility.Collapsed;
		}
	}
}
