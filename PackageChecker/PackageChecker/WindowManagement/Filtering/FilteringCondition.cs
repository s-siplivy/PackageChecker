using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageChecker.WindowManagement.Filtering
{
	public class FilteringCondition
	{
		public List<string> EntityEquals { get; private set; }
		public List<string> EntityNotEquals { get; private set; }
		public List<string> EntityHighlignt { get; private set; }

		public FilteringCondition()
		{
			EntityEquals = new List<string>();
			EntityNotEquals = new List<string>();
			EntityHighlignt = new List<string>();
		}
	}
}
