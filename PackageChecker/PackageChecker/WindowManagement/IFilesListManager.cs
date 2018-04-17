using PackageChecker.Files;
using System.Threading.Tasks;

namespace PackageChecker.WindowManagement
{
	internal interface IFilesListManager
	{
		Task UpdateFileRecords(string path, FileSearchType type);
		void ClearList();
	}
}
