using System.Windows;

namespace PackageChecker.WindowManagement
{
	internal interface IProgressBarManager
	{
		bool IsIndeterminate { get; set; }
		int Progress { get; set; }
		string ProgressText { get; set; }

		void SetVisibility(Visibility visibility);
	}
}
