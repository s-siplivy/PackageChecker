using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageChecker.WindowManagement
{
	public class FilteringInfo
	{
		public FilteringCondition ProductVersionCondition { get; private set; }
		public FilteringCondition FileVersionCondition { get; private set; }
		public FilteringCondition FilePathCondition { get; private set; }

		public FilteringInfo()
		{
			ProductVersionCondition = new FilteringCondition();
			FileVersionCondition = new FilteringCondition();
			FilePathCondition = new FilteringCondition();
		}

		public bool IsCorrectProductVersion(string productVersion)
		{
			return IsValuePassCondition(ProductVersionCondition, productVersion);
		}

		public bool IsCorrectFileVersion(string fileVersion)
		{
			return IsValuePassCondition(FileVersionCondition, fileVersion);
		}

		public bool IsCorrectFilePath(string filePath)
		{
			return IsValuePassCondition(FilePathCondition, filePath);
		}

		private bool IsValuePassCondition(FilteringCondition condition, string value)
		{
			foreach (string filter in condition.EntityEquals)
			{
				if (!value.Contains(filter))
				{
					return false;
				}
			}

			foreach (string filter in condition.EntityNotEquals)
			{
				if (value.Contains(filter))
				{
					return false;
				}
			}

			return true;
		}
	}
}
