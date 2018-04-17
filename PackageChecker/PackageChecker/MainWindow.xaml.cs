using PackageChecker.Files;
using PackageChecker.Models;
using PackageChecker.ViewModels;
using PackageChecker.WindowManagement;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PackageChecker
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const string savedFiltersPath = "filters-v2.dat";

		private FilteringViewModel filteringViewModel;
		WindowViewModel windowViewModel;

		public MainWindow()
		{
			InitializeComponent();

			ProgressBarViewModel progressBarViewModel = new ProgressBarViewModel();
			ProgressPanel.DataContext = progressBarViewModel;

			filteringViewModel = new FilteringViewModel();
			FilterPanel.DataContext = filteringViewModel;

			FilesListViewModel filesListViewModel = new FilesListViewModel(filteringViewModel.GetFilteringManager(), progressBarViewModel.GetProgressBarManager());
			FilesPanel.DataContext = filesListViewModel;

			windowViewModel = new WindowViewModel(progressBarViewModel.GetProgressBarManager(), filesListViewModel.GetFilesListManager());
			DataContext = windowViewModel;

			LoadSavedData();

			RegisterUncaughtExpectionsHandler(AppDomain.CurrentDomain);
		}

		private void Window_Drop(object sender, DragEventArgs e)
		{
			// Workaround for MVVM drag&drop functionality
			// TODO: Imlement MVVM drag&drop approach
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			windowViewModel.ProcessDragAndDrop.Execute(files);
		}

		private void FilesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			// Workaround for MVVM double click functionality
			// TODO: Imlement MVVM double click approach
			ListView listView = (ListView)sender;
			FileRecord selectedRecord = (FileRecord)listView.SelectedItem;

			if (selectedRecord != null)
			{
				if (windowViewModel.WindowState == MainWindowState.Folder)
				{
					FilesHelper.OpenFileExplorer(windowViewModel.PathValue, selectedRecord.FilePath);
				}
				else
				{
					WindowHelper.ShowError("Open file location is only possible in the Folder mode.");
				}
			}
		}

		private void OnWindowClosing(object sender, CancelEventArgs e)
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

		private void RegisterUncaughtExpectionsHandler(AppDomain domain)
		{
			domain.UnhandledException += new UnhandledExceptionEventHandler(
				(sender, args) =>
				{
					Exception e = (Exception)args.ExceptionObject;
					WindowHelper.ShowError(e.Message);
					Close();
				});
		}
	}
}
