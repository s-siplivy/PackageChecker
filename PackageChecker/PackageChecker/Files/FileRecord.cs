namespace PackageChecker.Files
{
	internal class FileRecord
	{
		internal string Signature { get; set; }

		internal string FilePath { get; set; }

		internal string FileVersion { get; set; }

		internal string ProductVersion { get; set; }

		internal bool DoHighlight { get; set; }
	}
}
