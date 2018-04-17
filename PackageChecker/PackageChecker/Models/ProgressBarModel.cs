using PackageChecker.BindingBase;
using PackageChecker.WindowManagement;
using System;
using System.Windows;

namespace PackageChecker.Models
{
	internal class ProgressBarModel : BindableBase, IProgressBarManager
	{
		#region Private Properties
		private bool _isProgressBarIndeterminate = false;
		private int _progressBarCurrent = 0;
		private string _progressText = string.Empty;
		private Visibility _visibility = Visibility.Collapsed;
		#endregion //Private Properties

		#region Binding Properties
		public bool IsProgressBarIndeterminate
		{
			get
			{
				return _isProgressBarIndeterminate;
			}
			set
			{
				_isProgressBarIndeterminate = value;
				OnPropertyChanged("IsProgressBarIndeterminate");
			}
		}

		public int ProgressBarCurrent
		{
			get
			{
				return _progressBarCurrent;
			}
			set
			{
				_progressBarCurrent = value;
				OnPropertyChanged("ProgressBarCurrent");
			}
		}

		public string ProgressText
		{
			get
			{
				return _progressText;
			}
			set
			{
				_progressText = value;
				OnPropertyChanged("ProgressText");
			}
		}

		public Visibility CurrentVisibility
		{
			get
			{
				return _visibility;
			}
			set
			{
				_visibility = value;
				OnPropertyChanged("CurrentVisibility");
			}
		}
		#endregion //Binding Properties

		#region Interface implementestion
		bool IProgressBarManager.IsIndeterminate
		{
			get
			{
				return IsProgressBarIndeterminate;
			}
			set
			{
				IsProgressBarIndeterminate = value;
			}
		}

		int IProgressBarManager.Progress
		{
			get
			{
				return ProgressBarCurrent;
			}
			set
			{
				if (value < 0 || value > 100)
				{
					throw new ArgumentException(nameof(IProgressBarManager.Progress));
				}

				ProgressBarCurrent = value;
			}
		}

		string IProgressBarManager.ProgressText
		{
			get
			{
				return ProgressText;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentException(nameof(IProgressBarManager.ProgressText));
				}

				ProgressText = value;
			}
		}

		void IProgressBarManager.SetVisibility(Visibility visibility)
		{
			CurrentVisibility = visibility;
		}
		#endregion //Interface implementestion
	}
}
