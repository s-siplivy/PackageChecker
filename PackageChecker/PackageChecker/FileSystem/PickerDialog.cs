using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PackageChecker.FileSystem
{
	public static class PickerDialog
	{
		public static string PickZipDialog()
		{
			using (var fileDialog = new OpenFileDialog())
			{
				fileDialog.Filter = "Zip Archive|*.zip";
				fileDialog.CheckPathExists = true;
				fileDialog.Multiselect = false;
				DialogResult result = fileDialog.ShowDialog();

				if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fileDialog.FileName))
				{
					return fileDialog.FileName;
				}

				return string.Empty;
			}
		}

		public static string PickFolderDialog()
		{
			using (var fileDialog = new FolderBrowserDialog())
			{
				DialogResult result = fileDialog.ShowDialog();

				if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fileDialog.SelectedPath))
				{
					return fileDialog.SelectedPath;
				}

				return string.Empty;
			}
		}
	}
}
