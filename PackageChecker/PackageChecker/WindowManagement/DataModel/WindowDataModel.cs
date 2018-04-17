using PackageChecker.Files;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PackageChecker.WindowManagement.DataModel
{
	public class WindowDataModel : INotifyPropertyChanged
	{
		private string pathValue;

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

		public event PropertyChangedEventHandler PropertyChanged;

		public WindowDataModel()
		{
			pathValue = string.Empty;

			PropertyChanged += (s, e) => { };
		}
	}
}
