using PackageChecker.WindowManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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

		public int FilesTotal
		{
			get { return allFileRecords.Count; }
		}

		public int FilesShown
		{
			get { return fileRecords.Count; }
		}

		public FilesManager(FilteringManager filteringManager, ObservableCollection<FileRecord> fileRecords)
		{
			this.filteringManager = filteringManager;
			this.fileRecords = fileRecords;
			this.allFileRecords = new List<FileRecord>();
		}

		public void ResetFileRecords(string path, SearchType type)
		{
			switch (type)
			{
				case SearchType.Folder:
					allFileRecords = GetAllFolderRecords(path);
					break;
				case SearchType.Zip:
					allFileRecords = GetAllZipRecords(path);
					break;
				default:
					throw new ArgumentException(nameof(type));
			}

			ApplyFilteting();
		}

		public void Clear()
		{
			allFileRecords.Clear();
			fileRecords.Clear();
			ApplyFilteting();
		}

		public void ApplyFilteting()
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
			return CollectFilesInfo(filePaths, dirPath);
		}

		private List<FileRecord> GetAllZipRecords(string zipPath)
		{
			string temptPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

			Directory.CreateDirectory(temptPath);
			try
			{
				ZipExtract(zipPath, temptPath);
				List<string> filePaths = DirSearch(temptPath);
				return CollectFilesInfo(filePaths, temptPath);
			}
			finally
			{
				Directory.Delete(temptPath, true);
			}
		}

		private void ZipExtract(string zipPath, string dirPath)
		{
			ZipFile.ExtractToDirectory(zipPath, dirPath);
		}

		private List<FileRecord> CollectFilesInfo(List<string> filePaths, string dirPath)
		{
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
			foreach (string filePath in Directory.GetFiles(dirPath, "*.EXE"))
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
			if (!rootPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				rootPath += Path.DirectorySeparatorChar;
			}
			Uri fileUri = new Uri(fullPath);
			Uri referenceUri = new Uri(rootPath);
			return referenceUri.MakeRelativeUri(fileUri).ToString();
		}
	}
}
