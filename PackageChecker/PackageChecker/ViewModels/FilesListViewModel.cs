using PackageChecker.BindingBase;
using PackageChecker.Files;
using PackageChecker.Filtering;
using PackageChecker.Models;
using PackageChecker.WindowManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PackageChecker.ViewModels
{
	internal class FilesListViewModel : BindableBase
	{
		private readonly FilesListModel _model;

		internal FilesListViewModel(IFilteringManager filteringManager, IProgressBarManager progressManager)
		{
			_model = new FilesListModel(filteringManager, progressManager);
			_model.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
		}

		public ReadOnlyObservableCollection<FileRecord> FileRecords => _model.FileRecords;
		public string CurrentFilteringStatus => _model.CurrentFilteringStatus;

		internal IFilesListManager GetFilesListManager()
		{
			return _model;
		}
	}
}
