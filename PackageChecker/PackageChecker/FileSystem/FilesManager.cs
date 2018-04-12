using PackageChecker.WindowManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageChecker.FileSystem
{
	public class FilesManager
	{
		private FilteringManager filteringManager;
		private ObservableCollection<FileRecord> fileRecords;
		private List<FileRecord> allFileRecords;

		public FilesManager(FilteringManager filteringManager, ObservableCollection<FileRecord> fileRecords)
		{
			this.filteringManager = filteringManager;
			this.fileRecords = fileRecords;
		}

		public void ResetFileRecords(string path, SearchType type)
		{
			switch (type)
			{
				case SearchType.Folder:
					allFileRecords = GetAllFolderRecords(path);
					break;
				default:
					throw new ArgumentException(nameof(type));
			}

			ApplyFilteting();
		}

		private void ApplyFilteting()
		{
			fileRecords.Clear();
			FilteringInfo info = filteringManager.GetFilteringInfo();
			foreach (FileRecord record in allFileRecords)
			{
				if (info.DoFilterFilePath(record.FilePath) ||
					info.DoFilterFileVersion(record.FileVersion) ||
					info.DoFilterProductVersion(record.ProductVersion))
				{
					continue;
				}

				fileRecords.Add(record);
			}
		}

		private List<FileRecord> GetAllFolderRecords(string dirPath)
		{
			List<string> filePaths = DirSearch(dirPath);
			List<FileRecord> allFiles = new List<FileRecord>(filePaths.Count);

			foreach (string filePath in filePaths)
			{
				FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);

				allFiles.Add(new FileRecord()
				{
					FilePath = GetRelativePath(filePath, dirPath),
					FileVersion = fileVersionInfo.FileVersion,
					ProductVersion = fileVersionInfo.ProductVersion,
				});
			}

			return allFiles;
		}

		private List<string> DirSearch(string dirPath)
		{
			List<string> files = new List<string>();

			foreach (string filePath in Directory.GetFiles(dirPath, "*.DLL"))
			{
				files.Add(filePath);
			}
			foreach (string childDirPath in Directory.GetDirectories(dirPath))
			{
				files.AddRange(DirSearch(childDirPath));
			}

			return files;
		}

		private string GetRelativePath(string fullPath, string rootPath)
		{
			var fileUri = new Uri(fullPath);
			var referenceUri = new Uri(rootPath);
			return referenceUri.MakeRelativeUri(fileUri).ToString();
		}
	}
}
