using PackageChecker.FileSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageChecker.WindowManagement
{
	public class WindowDataModel : INotifyPropertyChanged
	{
		private string pathValue;
		private string currentFilteringExpression;
		private string currentFilteringStatus;
		private ObservableCollection<FileRecord> fileRecords;
		private ObservableCollection<string> filteringExpressions;

		public string PathValue
		{
			get { return pathValue; }
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
			get { return currentFilteringExpression; }
			set
			{
				if (currentFilteringExpression != value)
				{
					currentFilteringExpression = value;
					PropertyChanged(this, new PropertyChangedEventArgs("CurrentFilteringExpression"));
				}
			}
		}

		public string CurrentFilteringStatus
		{
			get { return currentFilteringStatus; }
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
			get { return filteringExpressions; }
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
			get { return fileRecords; }
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
			currentFilteringExpression = string.Empty;
			fileRecords = new ObservableCollection<FileRecord>();
			filteringExpressions = new ObservableCollection<string>();

			PropertyChanged += (s,e) => { };
		}
	}
}
