using PackageChecker.FileSystem;
using PackageChecker.WindowManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageChecker
{
	public class MainWindowController
	{
		protected const string filteringStatusTemplate = "Files shown: {0}. Files hidden: {1}.";

		public WindowState windowState { get; private set; }
		protected FilteringManager filteringManager;
		protected FilesManager filesManager;
		protected MainWindow window;
		protected WindowDataModel dataModel;

		public MainWindowController(MainWindow window, WindowDataModel dataModel)
		{
			this.window = window;
			this.dataModel = dataModel;

			filteringManager = new FilteringManager(dataModel.FilteringExpressions);
			filesManager = new FilesManager(filteringManager, dataModel.FileRecords);

			UpdateFilteringStatus();
			InitializeWindow();
		}

		public void SetZipState(string path)
		{
			SetPathMode();

			if (dataModel.PathValue != path)
			{
				dataModel.PathValue = path;
				
			}

			windowState = WindowState.ZipFile;
		}

		public void SetFolderState(string path)
		{
			SetPathMode();

			if (dataModel.PathValue != path)
			{
				dataModel.PathValue = path;
			}

			windowState = WindowState.Folder;
		}

		public void SetEmptyState()
		{
			SetChooseMode();

			dataModel.PathValue = string.Empty;

			windowState = WindowState.None;
		}

		public void AddFilteringExpression()
		{
			try
			{
				filteringManager.AddExpression(dataModel.CurrentFilteringExpression);
				dataModel.CurrentFilteringExpression = string.Empty;
				ApplyFilesConditions();
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
				dataModel.CurrentFilteringExpression = filteringManager.EditExpression(index);
				ApplyFilesConditions();
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
				ApplyFilesConditions();
			}
			catch (ArgumentException e)
			{
				ShowMessage(e.Message, "Error");
			}
		}

		public void ApplyFilesConditions()
		{
			if (windowState == WindowState.Folder || windowState == WindowState.ZipFile)
			{
				filesManager.ApplyFilteting();
			}
			UpdateFilteringStatus();
		}

		public void UpdateFilesList()
		{
			switch (windowState)
			{
				case WindowState.Folder:
					filesManager.ResetFileRecords(dataModel.PathValue, SearchType.Folder);
					break;
				case WindowState.ZipFile:
					filesManager.ResetFileRecords(dataModel.PathValue, SearchType.Zip);
					break;
			}
			UpdateFilteringStatus();
		}

		private void UpdateFilteringStatus()
		{
			dataModel.CurrentFilteringStatus = string.Format(CultureInfo.InvariantCulture,
				filteringStatusTemplate, filesManager.FilesShown, filesManager.FilesTotal - filesManager.FilesShown);
		}

		private void InitializeWindow()
		{
			SetChooseMode();
			window.DataContext = dataModel;
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
