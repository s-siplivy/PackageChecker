using PackageChecker.BindingBase;
using PackageChecker.Models;
using PackageChecker.WindowManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
		public Visibility CurrentVisibility => _model.CurrentVisibility;

		internal IProgressBarManager GetProgressBarManager()
		{
			return _model;
		}
	}
}
