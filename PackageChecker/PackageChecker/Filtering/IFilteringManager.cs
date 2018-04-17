using System;

namespace PackageChecker.Filtering
{
	internal interface IFilteringManager
	{
		event Action OnFilteringUpdate;
		FilteringInfo GetFilteringInfo();
	}
}
