using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageChecker.FileSystem
{
	public class FileRecord
	{
		public string Signature { get; set; }

		public string FilePath { get; set; }

		public string FileVersion { get; set; }

		public string ProductVersion { get; set; }

		public bool DoHighlight { get; set; }
	}
}
