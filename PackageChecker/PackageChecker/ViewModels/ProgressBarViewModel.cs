using PackageChecker.Models;
using PackageChecker.WindowManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageChecker.ViewModels
{
	internal class ProgressBarViewModel : BindableBase
	{
		private readonly ProgressBarModel _model = new ProgressBarModel();

		internal ProgressBarViewModel()
		{
			_model.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
		}

		public bool IsProgressBarIndeterminate { get { return _model.IsProgressBarIndeterminate; } set { _model.IsProgressBarIndeterminate = value; } }
		public int ProgressBarCurrent { get { return _model.ProgressBarCurrent; } set { _model.ProgressBarCurrent = value; } }
		public string ProgressText => _model.ProgressText;

		internal IProgressBarManager GetFilteringManager()
		{
			return _model;
		}
	}
}
