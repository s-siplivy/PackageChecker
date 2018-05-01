using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace PackageChecker.Files
{
	internal class AssembliesReferences
	{
		private const string SuccessMessage = "Passed.";
		private const string ErrorMessage = "Failed. Missing assemblies:\n{0}.";

		private Dictionary<string, HashSet<string>> _assembliesInFolders;

		internal AssembliesReferences()
		{
			_assembliesInFolders = new Dictionary<string, HashSet<string>>();
		}

		internal void AddAssembly(string folder, AssemblyName assemblyName)
		{
			if (!_assembliesInFolders.ContainsKey(folder))
			{
				_assembliesInFolders[folder] = new HashSet<string>();
			}

			_assembliesInFolders[folder].Add(assemblyName.FullName);
		}

		internal string CheckAssembly(string folder, Assembly assembly)
		{
			HashSet<string> assembliesInFolder = _assembliesInFolders[folder];

			List<string> missingAssemblies = new List<string>();
			foreach (AssemblyName referencedAssembly in assembly.GetReferencedAssemblies())
			{
				string refAssemblyFullName = referencedAssembly.FullName;
				if (!assembliesInFolder.Contains(refAssemblyFullName) && !AssemblyManager.IsAssemblyInGAC(referencedAssembly))
				{
					missingAssemblies.Add(refAssemblyFullName);
				}
			}

			if (missingAssemblies.Count == 0)
			{
				return SuccessMessage;
			}
			else
			{
				return string.Format(CultureInfo.InvariantCulture, ErrorMessage, string.Join(";\n", missingAssemblies));
			}
		}
	}
}
