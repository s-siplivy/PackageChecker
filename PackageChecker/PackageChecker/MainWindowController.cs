using PackageChecker.FileSystem;
using PackageChecker.WindowManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PackageChecker
{
	public class MainWindowController
	{
		protected const string savedFiltersPath = "filters-v1.dat";
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

			LoadSavedData();

			ProgressBarController progressController = new ProgressBarController(dataModel);
			filteringManager = new FilteringManager(dataModel.FilteringExpressions);
			filesManager = new FilesManager(filteringManager, progressController, dataModel.FileRecords);

			this.dataModel.CurrentFilteringExpressionHint = filteringManager.GetExpressionPatternHint();

			UpdateFilteringStatus();
			InitializeWindow();
		}

		public void SetZipState(string path)
		{
			SetProgressMode();

			if (dataModel.PathValue != path)
			{
				dataModel.PathValue = path;
			}

			Task task = UpdateFilesList(WindowState.ZipFile);
			if (task != null)
			{
				task.ContinueWith(t => DispatcherInvoke(() => SetPathMode(WindowState.ZipFile)));
			}
		}

		public void SetFolderState(string path)
		{
			SetProgressMode();

			if (dataModel.PathValue != path)
			{
				dataModel.PathValue = path;
			}

			Task task = UpdateFilesList(WindowState.Folder);
			if (task != null)
			{
				task.ContinueWith(t => DispatcherInvoke(() => SetPathMode(WindowState.Folder)));
			}
		}

		public void SetEmptyState()
		{
			dataModel.PathValue = string.Empty;

			ClearFilesList();

			SetChooseMode();
		}

		public void ProcessDragAndDrop(string[] files)
		{
			if (files.Length != 1)
			{
				ShowMessage("Drag-and-Drop support only one record.", "Error");
			}

			string path = files[0];

			if (FilesManager.IsFolder(path))
			{
				SetFolderState(path);
			}
			else if (FilesManager.IsZipFile(path))
			{
				SetZipState(path);
			}
			else
			{
				ShowMessage("File format isn't supported.", "Error");
			}
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

		public void FilteringExpressionInfo()
		{
			string helpMessage = filteringManager.GetHelpMessage();

			ShowMessage(helpMessage, "Help");
		}

		public void ApplyFilesConditions()
		{
			if (windowState == WindowState.Folder || windowState == WindowState.ZipFile)
			{
				filesManager.ApplyFilteting();
			}
			UpdateFilteringStatus();
		}

		public Task UpdateFilesList(WindowState transitionState)
		{
			Task updateTask = null;

			switch (transitionState)
			{
				case WindowState.Folder:
					updateTask = filesManager.ResetFileRecords(dataModel.PathValue, SearchType.Folder);
					break;
				case WindowState.ZipFile:
					updateTask = filesManager.ResetFileRecords(dataModel.PathValue, SearchType.Zip);
					break;
			}

			return updateTask == null ?
				updateTask :
				updateTask.ContinueWith(task => DispatcherInvoke(() => UpdateFilteringStatus()));
		}

		public void ClearFilesList()
		{
			filesManager.Clear();
			UpdateFilteringStatus();
		}

		public void RegisterUncaughtExpectionsHandler(AppDomain domain)
		{
			domain.UnhandledException += new UnhandledExceptionEventHandler(
				(sender, args) =>
				{
					Exception e = (Exception)args.ExceptionObject;
					ShowMessage(e.Message, "Unexpected exception");
					window.Close();
				});
		}

		public void OpenSelectedRecord(FileRecord record)
		{
			if (windowState == WindowState.Folder)
			{
				FilesManager.OpenFileExplorer(dataModel.PathValue, record.FilePath);
			}
			else
			{
				ShowMessage("Open file location is only possible in the Folder mode.", "Error");
			}
		}

		public void SaveDataOnClose()
		{
			Serializer.SaveObjectToFile(dataModel.FilteringExpressions, savedFiltersPath);
		}

		private void UpdateFilteringStatus()
		{
			dataModel.CurrentFilteringStatus = string.Format(CultureInfo.InvariantCulture,
				filteringStatusTemplate, filesManager.FilesShown, filesManager.FilesTotal - filesManager.FilesShown);
		}

		private void LoadSavedData()
		{
			ObservableCollection<string> savedFilters = null;

			try
			{
				savedFilters = Serializer.LoadObjectFromFile(savedFiltersPath) as ObservableCollection<string>;
			}
			catch (SerializationException)
			{
				ShowMessage("Failed to restore previous data. Corrupted files will be overwritten.", "Error");
			}

			if (savedFilters != null)
			{
				dataModel.FilteringExpressions = savedFilters;
			}
		}

		private void InitializeWindow()
		{
			SetChooseMode();
			window.DataContext = dataModel;
		}

		private void SetChooseMode()
		{
			windowState = WindowState.None;

			window.ChoosePanel.Visibility = System.Windows.Visibility.Visible;
			window.PathPanel.Visibility = System.Windows.Visibility.Collapsed;
			window.ProgressPanel.Visibility = System.Windows.Visibility.Collapsed;
		}

		private void SetPathMode(WindowState state)
		{
			windowState = state;

			window.PathPanel.Visibility = System.Windows.Visibility.Visible;
			window.ChoosePanel.Visibility = System.Windows.Visibility.Collapsed;
			window.ProgressPanel.Visibility = System.Windows.Visibility.Collapsed;
		}

		private void SetProgressMode()
		{
			windowState = WindowState.ProgressPanel;

			window.ProgressPanel.Visibility = System.Windows.Visibility.Visible;
			window.PathPanel.Visibility = System.Windows.Visibility.Collapsed;
			window.ChoosePanel.Visibility = System.Windows.Visibility.Collapsed;
		}

		private void ShowMessage(string message, string caption)
		{
			System.Windows.MessageBox.Show(message, caption);
		}

		private void DispatcherInvoke(Action action)
		{
			App.Current.Dispatcher.Invoke(action);
		}
	}
}
