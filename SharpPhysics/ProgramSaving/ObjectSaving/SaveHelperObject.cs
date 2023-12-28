using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics
{
	public static class SaveHelper
	{
		public static short MaxSaveCount { get; set; } = 10;
		public static void SaveFileStandardWrite(string[] data, string saveName)
		{
			new Thread(() =>
			{
				// last save
				if (!File.Exists($@"{Environment.CurrentDirectory}\Saves\ls.txt"))
				{
					if (ErrorHandler.YesNoQuestion($"Error, External error, Missing ls.txt (required file) in {Environment.CurrentDirectory}\\Saves\\{saveName}, do you want to attempt to fix this?", "?", false))
					{
						try
						{
							File.WriteAllText($@"{Environment.CurrentDirectory}\Saves\ls.txt", "0");
						}
						catch
						{
							ErrorHandler.ThrowError("Error, Internal error, unknown cause.", true);
						}
					}
				}
				// if the save dir does not exist create it
				if (!Directory.Exists($@"{Environment.CurrentDirectory}\Saves")) Directory.CreateDirectory($@"{Environment.CurrentDirectory}\Saves");
				// if the game save dir does not exist create it
				if (!Directory.Exists($@"{Environment.CurrentDirectory}\Saves\{saveName}")) Directory.CreateDirectory($@"{Environment.CurrentDirectory}\Saves\{saveName}");
				try
				{
					for (int i = 0; i < MaxSaveCount; i++)
					{
						if (!File.Exists($@"{Environment.CurrentDirectory}\Saves\{saveName}\{saveName}.{MaxSaveCount}.dat"))
						{
							File.WriteAllLines($@"{Environment.CurrentDirectory}\Saves\{saveName}\{saveName}.{MaxSaveCount}.dat", data);
						}
					}
				}
				catch
				{
					ErrorHandler.ThrowError("Error, Internal error, unknown cause", true);
				}
			});
		}
		public static void SaveObjects(_2dSceneHierarchy hierarchy)
		{
			string[] data = new string[0];
			foreach (_2dSimulatedObject obj in hierarchy.Objects)
			{

			}
		}
	}
}
