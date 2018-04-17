using PackageChecker.Files;
using PackageChecker.Models;
using PackageChecker.ViewModels;
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

		private FilteringViewModel filteringViewModel;
		WindowViewModel windowViewModel;
		protected MainWindow window;

		public MainWindowController(MainWindow window)
		{
			this.window = window;

			ProgressBarViewModel progressBarViewModel = new ProgressBarViewModel();
			window.ProgressPanel.DataContext = progressBarViewModel;

			filteringViewModel = new FilteringViewModel();
			window.FilterPanel.DataContext = filteringViewModel;

			FilesListViewModel filesListViewModel = new FilesListViewModel(filteringViewModel.GetFilteringManager(), progressBarViewModel.GetProgressBarManager());
			window.FilesPanel.DataContext = filesListViewModel;

			windowViewModel = new WindowViewModel(progressBarViewModel.GetProgressBarManager(), filesListViewModel.GetFilesListManager());
			window.DataContext = windowViewModel;

			LoadSavedData();
		}

		public void ProcessDragAndDrop(string[] files)
		{
			windowViewModel.ProcessDragAndDrop.Execute(files);
		}

		public void RegisterUncaughtExpectionsHandler(AppDomain domain)
		{
			domain.UnhandledException += new UnhandledExceptionEventHandler(
				(sender, args) =>
				{
					Exception e = (Exception)args.ExceptionObject;
					WindowHelper.ShowError(e.Message);
					window.Close();
				});
		}

		public void OpenSelectedRecord(FileRecord record)
		{
			if (windowViewModel.WindowState == MainWindowState.Folder)
			{
				FilesHelper.OpenFileExplorer(windowViewModel.PathValue, record.FilePath);
			}
			else
			{
				WindowHelper.ShowError("Open file location is only possible in the Folder mode.");
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
				WindowHelper.ShowError("Failed to restore previous data. Corrupted files will be overwritten.");
			}

			if (savedFilters != null)
			{
				filteringViewModel.SetState(savedFilters);
			}
		}
	}
}
