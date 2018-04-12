using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageChecker.WindowManagement
{
	public class FilteringInfo
	{
		public List<string> ProductVersionFilters { get; private set; }
		public List<string> FileVersionFilters { get; private set; }
		public List<string> FilePathFilters { get; private set; }

		public FilteringInfo()
		{
			ProductVersionFilters = new List<string>();
			FileVersionFilters = new List<string>();
			FilePathFilters = new List<string>();
		}

		public bool DoFilterProductVersion(string productVersion)
		{
			foreach (string filter in ProductVersionFilters)
			{
				if (productVersion.Contains(filter))
				{
					return true;
				}
			}

			return false;
		}

		public bool DoFilterFileVersion(string fileVersion)
		{
			foreach (string filter in FileVersionFilters)
			{
				if (fileVersion.Contains(filter))
				{
					return true;
				}
			}

			return false;
		}

		public bool DoFilterFilePath(string filePath)
		{
			foreach (string filter in FilePathFilters)
			{
				if (filePath.Contains(filter))
				{
					return true;
				}
			}

			return false;
		}
	}
}
