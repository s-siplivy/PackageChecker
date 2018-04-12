using PackageChecker.FileSystem;
using PackageChecker.WindowManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageChecker
{
	public class MainWindowController : INotifyPropertyChanged
	{
		public WindowState windowState { get; private set; }
		protected FilteringManager filteringManager;
		protected MainWindow window;

		public string PathValue { get; set; }
		public string CurrentFilteringExpression { get; set; }
		public ObservableCollection<string> FilteringExpressions { get; set; }
		public ObservableCollection<FileRecord> FileRecords { get; set; }
		public event PropertyChangedEventHandler PropertyChanged;

		public MainWindowController(MainWindow window)
		{
			this.window = window;

			FileRecords = new ObservableCollection<FileRecord>();
			FilteringExpressions = new ObservableCollection<string>();
			filteringManager = new FilteringManager(FilteringExpressions);

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

		public void SetEmptyState()
		{
			SetChooseMode();

			PathValue = string.Empty;
			PropertyChanged(this, new PropertyChangedEventArgs("PathValue"));

			windowState = WindowState.None;
		}

		public void AddFilteringExpression()
		{
			try
			{
				filteringManager.AddExpression(CurrentFilteringExpression);
				CurrentFilteringExpression = string.Empty;
				PropertyChanged(this, new PropertyChangedEventArgs("CurrentFilteringExpression"));
			}
			catch (ArgumentException e)
			{
				ShowMessage(e.Message, "Error");
			}
		}

		public void EditFilteringExpression()
		{
			try
			{
				int index = window.ListFilterExpressions.SelectedIndex;
				CurrentFilteringExpression = filteringManager.EditExpression(index);
				PropertyChanged(this, new PropertyChangedEventArgs("CurrentFilteringExpression"));
			}
			catch (ArgumentException e)
			{
				ShowMessage(e.Message, "Error");
			}
		}

		public void RemoveFilteringExpression()
		{
			try
			{
				filteringManager.RemoveExpression(window.ListFilterExpressions.SelectedIndex);
			}
			catch (ArgumentException e)
			{
				ShowMessage(e.Message, "Error");
			}
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

		private void ShowMessage(string message, string caption)
		{
			System.Windows.MessageBox.Show(message, caption);
		}
	}
}
