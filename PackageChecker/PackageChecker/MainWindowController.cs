using PackageChecker.WindowManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageChecker
{
	public class MainWindowController : INotifyPropertyChanged
	{
		public WindowState windowState { get; private set; }
		protected MainWindow window;

		public string PathValue { get; set; }
		public event PropertyChangedEventHandler PropertyChanged;

		public MainWindowController(MainWindow window)
		{
			this.window = window;
			InitializeWindow();
		}

		public void SetZipState(string path)
		{
			SetPathMode();

			if (PathValue != path)
			{
				PathValue = path;
				PropertyChanged(this, new PropertyChangedEventArgs("PathValue"));
			}

			windowState = WindowState.ZipFile;
		}

		public void SetFolderState(string path)
		{
			SetPathMode();

			if (PathValue != path)
			{
				PathValue = path;
				PropertyChanged(this, new PropertyChangedEventArgs("PathValue"));
			}

			windowState = WindowState.Folder;
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
