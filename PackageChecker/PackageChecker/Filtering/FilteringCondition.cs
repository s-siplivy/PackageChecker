using System.Collections.Generic;

namespace PackageChecker.Filtering
{
	internal class FilteringCondition
	{
		internal List<string> EntityInclude { get; private set; }
		internal List<string> EntityHighlignt { get; private set; }

		internal FilteringCondition()
		{
			EntityInclude = new List<string>();
			EntityHighlignt = new List<string>();
		}
	}
}
