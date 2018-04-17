using PackageChecker.Filtering;
using System;

namespace PackageChecker.WindowManagement
{
	internal interface IFilteringManager
	{
		event Action OnFilteringUpdate;
		FilteringInfo GetFilteringInfo();
	}
}
