using PackageChecker.FileSystem.DataModel;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PackageChecker.WindowManagement.DataModel
{
	public class WindowDataModel : INotifyPropertyChanged
	{
		private string pathValue;
		private string currentFilteringStatus;
		private ObservableCollection<FileRecord> fileRecords;

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
			pathValue = string.Empty;
			fileRecords = new ObservableCollection<FileRecord>();

			PropertyChanged += (s, e) => { };
		}
	}
}
