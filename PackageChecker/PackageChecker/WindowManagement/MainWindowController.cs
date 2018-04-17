using PackageChecker.Files;
using PackageChecker.Models;
using PackageChecker.ViewModels;
using PackageChecker.WindowManagement.DataModel;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PackageChecker.WindowManagement
{
	public class MainWindowController
	{
		protected const string savedFiltersPath = "filters-v2.dat";

		public WindowState windowState { get; private set; }
		private FilteringViewModel filteringViewModel;
		private ProgressBarViewModel progressBarViewModel;
		private FilesListViewModel filesListViewModel;
		private IFilesListManager filesListManager;
		protected MainWindow window;
		protected WindowDataModel dataModel;

		public MainWindowController(MainWindow window, WindowDataModel dataModel)
		{
			this.window = window;
			this.dataModel = dataModel;

			progressBarViewModel = new ProgressBarViewModel();
			window.ProgressPanel.DataContext = progressBarViewModel;

			filteringViewModel = new FilteringViewModel();
			window.FilterPanel.DataContext = filteringViewModel;

			filesListViewModel = new FilesListViewModel(filteringViewModel.GetFilteringManager(), progressBarViewModel.GetFilteringManager());
			window.FilesPanel.DataContext = filesListViewModel;
			filesListManager = filesListViewModel.GetFilesListManager();

			LoadSavedData();
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

			filesListManager.ClearList();

			SetChooseMode();
		}

		public void ProcessDragAndDrop(string[] files)
		{
			if (files.Length != 1)
			{
				ShowMessage("Drag-and-Drop support only one record.", "Error");
			}

			string path = files[0];

			if (FilesHelper.IsFolder(path))
			{
				SetFolderState(path);
			}
			else if (FilesHelper.IsZipFile(path))
			{
				SetZipState(path);
			}
			else
			{
				ShowMessage("File format isn't supported.", "Error");
			}
		}

		public Task UpdateFilesList(WindowState transitionState)
		{
			switch (transitionState)
			{
				case WindowState.Folder:
					return filesListManager.UpdateFileRecords(dataModel.PathValue, FileSearchType.Folder);
				case WindowState.ZipFile:
					return filesListManager.UpdateFileRecords(dataModel.PathValue, FileSearchType.Zip);
				default:
					throw new ArgumentException("Unknown UpdateFilesList argument.");
			}
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
				FilesHelper.OpenFileExplorer(dataModel.PathValue, record.FilePath);
			}
			else
			{
				ShowMessage("Open file location is only possible in the Folder mode.", "Error");
			}
		}

		public void SaveDataOnClose()
		{
			Serializer.SaveObjectToFile(filteringViewModel.GetState(), savedFiltersPath);
		}

		private void LoadSavedData()
		{
			FilteringModel.FilteringState savedFilters = null;

			try
			{
				savedFilters = Serializer.LoadObjectFromFile(savedFiltersPath) as FilteringModel.FilteringState;
			}
			catch (SerializationException)
			{
				ShowMessage("Failed to restore previous data. Corrupted files will be overwritten.", "Error");
			}

			if (savedFilters != null)
			{
				filteringViewModel.SetState(savedFilters);
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
