using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PackageChecker.FileSystem
{
	public static class Serializer
	{
		public static void SaveObjectToFile(object obj, string fileName)
		{
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}

			using (FileStream stream = File.Create(fileName))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, obj);
			}
		}

		public static object LoadObjectFromFile(string fileName)
		{
			if (!File.Exists(fileName))
			{
				return null;
			}

			using (FileStream stream = File.OpenRead(fileName))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				return formatter.Deserialize(stream);
			}
		}
	}
}
