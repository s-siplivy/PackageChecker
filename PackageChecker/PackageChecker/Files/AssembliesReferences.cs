﻿using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;

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
			HashSet<string> additionalAssemblies = GetAdditionalAssemblies(folder, assembly);

			List<string> missingAssemblies = new List<string>();
			foreach (AssemblyName referencedAssembly in assembly.GetReferencedAssemblies())
			{
				string refAssemblyFullName = referencedAssembly.FullName;
				if (!(assembliesInFolder.Contains(refAssemblyFullName) || additionalAssemblies.Contains(refAssemblyFullName)) &&
					!AssemblyManager.IsAssemblyInGAC(referencedAssembly))
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

		private HashSet<string> GetAdditionalAssemblies(string folder, Assembly assembly)
		{
			HashSet<string> additionalAssemblies = new HashSet<string>();

			string assemblyFileName = assembly.ManifestModule.ScopeName;
			if (!".exe".Equals(Path.GetExtension(assemblyFileName), System.StringComparison.OrdinalIgnoreCase))
			{
				return additionalAssemblies;
			}

			string configFilePath = Path.Combine(folder, assemblyFileName + ".config");
			if (!File.Exists(configFilePath))
			{
				return additionalAssemblies;
			}

			List<string> bindingPaths = GetConfigAssemblyBindingPaths(configFilePath);
			foreach (string path in bindingPaths)
			{
				string bindingFolder = string.Empty;
				if (FilesHelper.IsPathAbsolute(path))
				{
					bindingFolder = path;
				}
				else
				{
					bindingFolder = Path.Combine(folder, path);
				}

				if (_assembliesInFolders.ContainsKey(bindingFolder))
				{
					additionalAssemblies.UnionWith(_assembliesInFolders[bindingFolder]);
				}
			}

			return additionalAssemblies;
		}

		private List<string> GetConfigAssemblyBindingPaths(string configPath)
		{
			XmlDocument config = new XmlDocument();
			config.Load(configPath);

			XmlNodeList pathNodes = config.DocumentElement.SelectNodes("//*[local-name() = 'assemblyBinding']/*[local-name() = 'probing']/@privatePath");

			List<string> bindingPaths = new List<string>(pathNodes.Count);
			foreach (XmlAttribute path in pathNodes)
			{
				bindingPaths.Add(path.Value);
			}

			return bindingPaths;
		}
	}
}
