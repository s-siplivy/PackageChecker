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
		private string currentFilteringExpression;
		private string currentFilteringExpressionHint;
		private string currentFilteringStatus;
		private ObservableCollection<FileRecord> fileRecords;
		private ObservableCollection<string> filteringExpressions;

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

		public string CurrentFilteringExpression
		{
			get
			{
				return currentFilteringExpression;
			}
			set
			{
				if (currentFilteringExpression != value)
				{
					currentFilteringExpression = value;
					PropertyChanged(this, new PropertyChangedEventArgs("CurrentFilteringExpression"));
				}
			}
		}

		public string CurrentFilteringExpressionHint
		{
			get
			{
				return currentFilteringExpressionHint;
			}
			set
			{
				if (currentFilteringExpressionHint != value)
				{
					currentFilteringExpressionHint = value;
					PropertyChanged(this, new PropertyChangedEventArgs("CurrentFilteringExpressionHint"));
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

		public ObservableCollection<string> FilteringExpressions
		{
			get
			{
				return filteringExpressions;
			}
			set
			{
				if (filteringExpressions != value)
				{
					filteringExpressions = value;
					PropertyChanged(this, new PropertyChangedEventArgs("FilteringExpressions"));
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
			currentFilteringExpression = string.Empty;
			fileRecords = new ObservableCollection<FileRecord>();
			filteringExpressions = new ObservableCollection<string>();

			PropertyChanged += (s, e) => { };
		}
	}
}
