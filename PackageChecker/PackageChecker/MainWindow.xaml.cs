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

		public MainWindow()
		{
			InitializeComponent();

			ProgressBarViewModel progressBarViewModel = new ProgressBarViewModel();
			ProgressPanel.DataContext = progressBarViewModel;

			FilteringViewModel filteringViewModel = new FilteringViewModel();
			LoadFiltersData(filteringViewModel);
			Closing += (s, e) => SaveFiltersData(filteringViewModel);
			FilterPanel.DataContext = filteringViewModel;

			FilesListViewModel filesListViewModel = new FilesListViewModel(filteringViewModel.GetFilteringManager(), progressBarViewModel.GetProgressBarManager());
			FilesPanel.DataContext = filesListViewModel;

			WindowViewModel windowViewModel = new WindowViewModel(progressBarViewModel.GetProgressBarManager(), filesListViewModel.GetFilesListManager());
			this.Drop += (s, e) => ProcessWindowDrop(e, windowViewModel);
			DataContext = windowViewModel;

			FilesList.MouseDoubleClick += (s, e) => FilesList_MouseDoubleClick(s, windowViewModel.WindowState, windowViewModel.PathValue);

			RegisterUncaughtExpectionsHandler(AppDomain.CurrentDomain);
		}

		private void ProcessWindowDrop(DragEventArgs args, WindowViewModel windowViewModel)
		{
			// Workaround for MVVM drag&drop functionality
			// TODO: Imlement MVVM drag&drop approach
			string[] files = (string[])args.Data.GetData(DataFormats.FileDrop);
			windowViewModel.ProcessDragAndDrop.Execute(files);
		}
		private void FilesList_MouseDoubleClick(object sender, MainWindowState currentState, string rootFoler)
		{
			// Workaround for MVVM double click functionality
			// TODO: Imlement MVVM double click approach
			DataGrid listView = (DataGrid)sender;
			FileRecord selectedRecord = (FileRecord)listView.SelectedItem;

			if (selectedRecord != null)
			{
				if (currentState == MainWindowState.Folder)
				{
					FilesHelper.OpenFileExplorer(rootFoler, selectedRecord.FilePath);
				}
				else
				{
					WindowHelper.ShowError("Open file location is only possible in the Folder mode.");
				}
			}
		}

		private void LoadFiltersData(FilteringViewModel filteringViewModel)
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

		private void SaveFiltersData(FilteringViewModel filteringViewModel)
		{
			Serializer.SaveObjectToFile(filteringViewModel.GetState(), savedFiltersPath);
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
