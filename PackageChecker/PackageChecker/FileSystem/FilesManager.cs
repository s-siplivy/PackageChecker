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
		private ProgressBarController progressController;
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

		public FilesManager(FilteringManager filteringManager, ProgressBarController progressController, ObservableCollection<FileRecord> fileRecords)
		{
			this.filteringManager = filteringManager;
			this.progressController = progressController;
			this.fileRecords = fileRecords;
			this.allFileRecords = new List<FileRecord>();
		}

		public Task ResetFileRecords(string path, SearchType type)
		{
			Task task = new Task(() => UpdateFileRecordsAsync(path, type));
			task.Start();
			return task;
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

		private void UpdateFileRecordsAsync(string path, SearchType type)
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

			DispatcherInvoke(() => ApplyFilteting());
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
			UpdateProgressText("Unzipping file");
			IsProgressIndeterminate(false);

			using (ZipArchive zip = ZipFile.OpenRead(zipPath))
			{
				ReadOnlyCollection<ZipArchiveEntry> zipFiles = zip.Entries;

				int currentItem = 0;
				int allItemsCount = zipFiles.Count;

				foreach (ZipArchiveEntry entry in zipFiles)
				{
					if (entry.FullName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ||
						entry.FullName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
					{
						string destinationPath = Path.Combine(dirPath, entry.FullName.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar));

						FileInfo destinationPathFileInfo = new FileInfo(destinationPath);
						if (!destinationPathFileInfo.Directory.Exists)
						{
							Directory.CreateDirectory(destinationPathFileInfo.DirectoryName);
						}

						entry.ExtractToFile(destinationPath, true);
					}

					currentItem++;
					UpdateProgress(100 * currentItem / allItemsCount);
				}
			}
		}

		private List<FileRecord> CollectFilesInfo(List<string> filePaths, string dirPath)
		{
			UpdateProgressText("Collecting files info");
			IsProgressIndeterminate(false);

			List<FileRecord> allFiles = new List<FileRecord>(filePaths.Count);

			int currentItem = 0;
			int allItemsCount = filePaths.Count;

			foreach (string filePath in filePaths)
			{
				FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);

				allFiles.Add(new FileRecord()
				{
					FilePath = GetRelativePath(filePath, dirPath),
					FileVersion = fileVersionInfo.FileVersion,
					ProductVersion = fileVersionInfo.ProductVersion,
				});

				currentItem++;
				UpdateProgress(100 * currentItem / allItemsCount);
			}

			return allFiles;
		}

		private List<string> DirSearch(string dirPath)
		{
			UpdateProgressText("Dirrectory search");
			IsProgressIndeterminate(true);

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
			return referenceUri.MakeRelativeUri(fileUri).ToString().Replace("%20", " ");
		}

		private void UpdateProgress(int progress)
		{
			DispatcherInvoke(() => progressController.Progress = progress);
		}

		private void UpdateProgressText(string progressText)
		{
			DispatcherInvoke(() => progressController.ProgressText = progressText);
		}

		private void IsProgressIndeterminate(bool isIndeterminate)
		{
			DispatcherInvoke(() => progressController.IsIndeterminate = isIndeterminate);
		}

		private void DispatcherInvoke(Action action)
		{
			App.Current.Dispatcher.Invoke(action);
		}
	}
}
