using System.Collections.Generic;

namespace PackageChecker.WindowManagement.Filtering
{
	public class FilteringCondition
	{
		public List<string> EntityInclude { get; private set; }
		public List<string> EntityHighlignt { get; private set; }

		public FilteringCondition()
		{
			EntityInclude = new List<string>();
			EntityHighlignt = new List<string>();
		}
	}
}
