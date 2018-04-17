using PackageChecker.Files;
using PackageChecker.WindowManagement;
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

		public MainWindow()
		{
			InitializeComponent();
			controller = new MainWindowController(this);
			controller.RegisterUncaughtExpectionsHandler(AppDomain.CurrentDomain);
		}

		private void Window_Drop(object sender, DragEventArgs e)
		{
			// Workaround for MVVM drag&drop functionality
			// TODO: Imlement MVVM drag&drop approach
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			controller.ProcessDragAndDrop(files);
		}

		private void FilesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			// Workaround for MVVM double click functionality
			// TODO: Imlement MVVM double click approach
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
