using PackageChecker.Files;
using PackageChecker.WindowManagement;
using PackageChecker.WindowManagement.DataModel;
using System;
using System.ComponentModel;
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
		protected MainWindowController controller;
		protected WindowDataModel dataModel;

		public MainWindow()
		{
			InitializeComponent();

			dataModel = new WindowDataModel();
			controller = new MainWindowController(this, dataModel);

			controller.RegisterUncaughtExpectionsHandler(AppDomain.CurrentDomain);
		}

		private void ArchiveChoose_Click(object sender, RoutedEventArgs e)
		{
			string fileName = FilesHelper.PickZipDialog();
			if (!string.IsNullOrEmpty(fileName))
			{
				controller.SetZipState(fileName);
			}
		}

		private void FolderChoose_Click(object sender, RoutedEventArgs e)
		{
			string folderPath = FilesHelper.PickFolderDialog();
			if (!string.IsNullOrEmpty(folderPath))
			{
				controller.SetFolderState(folderPath);
			}
		}

		private void Window_Drop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			controller.ProcessDragAndDrop(files);
		}

		private void SelectionCancel_Click(object sender, RoutedEventArgs e)
		{
			controller.SetEmptyState();
		}

		private void FilesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			ListView listView = (ListView)sender;
			FileRecord selectedRecord = (FileRecord)listView.SelectedItem;

			if (selectedRecord != null)
			{
				controller.OpenSelectedRecord(selectedRecord);
			}
		}

		public void OnWindowClosing(object sender, CancelEventArgs e)
		{
			controller.SaveDataOnClose();
		}
	}
}
