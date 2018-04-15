using PackageChecker.WindowManagement.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageChecker.WindowManagement
{
	public class ProgressBarController
	{
		private WindowDataModel dataModel;

		public int Progress
		{
			get
			{
				return dataModel.ProgressBarCurrent;
			}
			set
			{
				if (value < 0 || value > 100)
				{
					throw new ArgumentException(nameof(Progress));
				}

				dataModel.ProgressBarCurrent = value;
			}
		}

		public string ProgressText
		{
			get
			{
				return dataModel.ProgressText;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentException(nameof(ProgressText));
				}

				dataModel.ProgressText = value;
			}
		}

		public bool IsIndeterminate
		{
			get
			{
				return dataModel.IsProgressBarIndeterminate;
			}
			set
			{
				dataModel.IsProgressBarIndeterminate = value;
			}
		}

		public ProgressBarController(WindowDataModel dataModel)
		{
			this.dataModel = dataModel;
		}
	}
}
