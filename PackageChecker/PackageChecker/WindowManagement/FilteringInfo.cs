using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageChecker.WindowManagement
{
	public class FilteringInfo
	{
		private const string specialSymbol = "*";

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
				if (!ContainsWithPattern(value, filter))
				{
					return false;
				}
			}

			foreach (string filter in condition.EntityNotEquals)
			{
				if (ContainsWithPattern(value, filter))
				{
					return false;
				}
			}

			return true;
		}

		private bool ContainsWithPattern(string source, string value)
		{
			bool isStartsWith = false;
			bool isEndsWith = false;
			string localValue = value;

			if (localValue.StartsWith(specialSymbol))
			{
				isEndsWith = true;
				localValue = localValue.Substring(1, localValue.Length - 1);
			}

			if (localValue.EndsWith(specialSymbol))
			{
				isStartsWith = true;
				localValue = localValue.Substring(0, localValue.Length - 1);
			}

			if (isStartsWith && isEndsWith)
			{
				return source.Contains(localValue);
			}
			else if (isStartsWith)
			{
				return source.StartsWith(localValue);
			}
			else if (isEndsWith)
			{
				return source.EndsWith(localValue);
			}

			return source == localValue;
		}
	}
}
