using PackageChecker.FileSystem.DataModel;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PackageChecker.WindowManagement.DataModel
{
	public class WindowDataModel : INotifyPropertyChanged
	{
		private bool isProgressBarIndeterminate;
		private int progressBarCurrent;
		private string progressText;
		private string pathValue;
		private string currentFilteringStatus;
		private ObservableCollection<FileRecord> fileRecords;

		public bool IsProgressBarIndeterminate
		{
			get
			{
				return isProgressBarIndeterminate;
			}
			set
			{
				if (isProgressBarIndeterminate != value)
				{
					isProgressBarIndeterminate = value;
					PropertyChanged(this, new PropertyChangedEventArgs("IsProgressBarIndeterminate"));
				}
			}
		}

		public int ProgressBarCurrent
		{
			get
			{
				return progressBarCurrent;
			}
			set
			{
				if (progressBarCurrent != value)
				{
					progressBarCurrent = value;
					PropertyChanged(this, new PropertyChangedEventArgs("ProgressBarCurrent"));
				}
			}
		}

		public string ProgressText
		{
			get
			{
				return progressText;
			}
			set
			{
				if (progressText != value)
				{
					progressText = value;
					PropertyChanged(this, new PropertyChangedEventArgs("ProgressText"));
				}
			}
		}

		public string PathValue
		{
			get
			{
				return pathValue;
			}
			set
			{
				if (pathValue != value)
				{
					pathValue = value;
					PropertyChanged(this, new PropertyChangedEventArgs("PathValue"));
				}
			}
		}
		public string CurrentFilteringStatus
		{
			get
			{
				return currentFilteringStatus;
			}
			set
			{
				if (currentFilteringStatus != value)
				{
					currentFilteringStatus = value;
					PropertyChanged(this, new PropertyChangedEventArgs("CurrentFilteringStatus"));
				}
			}
		}

		public ObservableCollection<FileRecord> FileRecords
		{
			get
			{
				return fileRecords;
			}
			set
			{
				if (fileRecords != value)
				{
					fileRecords = value;
					PropertyChanged(this, new PropertyChangedEventArgs("FileRecords"));
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public WindowDataModel()
		{
			isProgressBarIndeterminate = false;
			progressBarCurrent = 0;
			progressText = string.Empty;
			pathValue = string.Empty;
			fileRecords = new ObservableCollection<FileRecord>();

			PropertyChanged += (s, e) => { };
		}
	}
}
