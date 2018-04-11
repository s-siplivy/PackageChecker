using PackageChecker.FileSystem;
using System;
using System.Collections.Generic;
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

		public MainWindow()
		{
			InitializeComponent();
			controller = new MainWindowController(this);
			DataContext = controller;
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

		private void AddFilter_Click(object sender, RoutedEventArgs e)
		{
			controller.AddFilteringExpression();
		}
	}
}
