using Newtonsoft.Json;
using SharpPhysics.Utilities.MISC;

namespace SharpPhysics
{
	public static class SaveHelper
	{
		/// <summary>
		/// Indicates the number of saves that SharpPhysics will attempt to do before over-writing one.
		/// </summary>
		public static short MaxSaveCount { get; set; } = 10;
		public static void SaveFileStandardWrite(string data, string saveName)
		{
			new Thread(() =>
			{
				// if the save dir does not exist create it
				if (!Directory.Exists($@"{Environment.CurrentDirectory}\Saves")) Directory.CreateDirectory($@"{Environment.CurrentDirectory}\Saves");
				// if the game save dir does not exist create it
				if (!Directory.Exists($@"{Environment.CurrentDirectory}\Saves\{saveName}")) Directory.CreateDirectory($@"{Environment.CurrentDirectory}\Saves\{saveName}");
				try
				{
					File.WriteAllText($@"{Environment.CurrentDirectory}\Saves\{saveName}\{saveName}.dat", data);
				}
				catch
				{
					//5
					ErrorHandler.ThrowError(5, true);
				}
			}).Start();
		}
		/// <summary>
		/// Saves objects to a save file with name [name].dat.
		/// Uses JSON
		/// </summary>
		/// <param name="hierarchy"></param>
		public static void SaveObjects(string name)
		{
			new Thread(() =>
			{
				SaveFileStandardWrite(JsonConvert.SerializeObject(_2dWorld.SceneHierarchies[0].Objects), name);
			}).Start();
		}
	}
}
