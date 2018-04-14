using PackageChecker.FileSystem;
using PackageChecker.WindowManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
			string fileName = PickerDialog.PickZipDialog();
			if (!string.IsNullOrEmpty(fileName))
			{
				controller.SetZipState(fileName);
			}
		}

		private void FolderChoose_Click(object sender, RoutedEventArgs e)
		{
			string folderPath = PickerDialog.PickFolderDialog();
			if (!string.IsNullOrEmpty(folderPath))
			{
				controller.SetFolderState(folderPath);
			}
		}

		private void SelectionCancel_Click(object sender, RoutedEventArgs e)
		{
			controller.SetEmptyState();
		}

		private void FilterExpression_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				TextBox box = (TextBox)sender;
				dataModel.CurrentFilteringExpression = box.Text;
				controller.AddFilteringExpression();
			}
		}

		private void AddFilter_Click(object sender, RoutedEventArgs e)
		{
			controller.AddFilteringExpression();
		}

		private void EditFilter_Click(object sender, RoutedEventArgs e)
		{
			controller.EditFilteringExpression();
		}

		private void RemoveFilter_Click(object sender, RoutedEventArgs e)
		{
			controller.RemoveFilteringExpression();
		}

		private void Info_Click(object sender, RoutedEventArgs e)
		{
			controller.FilteringExpressionInfo();
		}

		public void OnWindowClosing(object sender, CancelEventArgs e)
		{
			controller.SaveDataOnClose();
		}
	}
}
